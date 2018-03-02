# C#事件总线

## 一、概述

事件总线是事件发送者将事件消息发送到一个中心，事件订阅者在该中心订阅，接收事件，再处理事件。

事件总线可以分为三个部分，事件发送者，事件接收者，事件中心。

* 事件发送者： 事件的起点，
* 事件接收者： 接收事件，处理事件
* 事件中心： 负责关联事件发送者和事件接收者，及触发事件。一个事件发送者可以关联多个事件接收者。

如图：

在一些无需立即获取事件结果，并且耗时的操作，使用事件总线提高性能。

事件总线的核心就是降低耦合，将消息的发送和接收分开，实现异步处理消息事件。事件的发送者并不关心是谁订阅了事件，要做怎么样的处理，同样事件的接收者也并不关心是谁触发了事件，只要事件被触发我就执行。

比如：在系统中产生了一个订单（标记为事件发送者），此时需要给用户发送邮件（事件接收者），发送短信通知（事件接收者），微信通知（事件接收者）等等一些列关于订单的操作。

如果不适用事件总线的话，怎么实现呢？

通常我们会使用委托来调用这些事件方法。（观察者模式）

1. 在订单类里定义一个订单通知的委托
2. 各个操作订阅这个委托。
3. 在订单类中调用这个委托。

```CSharp
/// <summary>
/// 订单类
/// </summary>
public class Order
{
    /// <summary>
    /// 通知委托
    /// </summary>
    public Action<object> NotifyOrder;
    /// <summary>
    /// 执行通知
    /// </summary>
    public void DoNotify()
    {
        NotifyOrder?.Invoke(this);
    }
}

public class Email
{
    public void EmailNotify(object o)
    {
        Console.WriteLine("Email !");

    }
}
public class Txt
{
    public void TxtNotify(object o)
    {
        Console.WriteLine("Txt !");

    }
}
public class WeChart
{
    public void WeChartNotify(object o)
    {
        Console.WriteLine("WeChart !");

    }
}

//在控制台中调用：

var order = new Order();
    order.NotifyOrder += new Email().EmailNotify;
    order.NotifyOrder += new Txt().TxtNotify;
    order.NotifyOrder += new WeChart().WeChartNotify;
    order.DoNotify();

//输出：
//Email !
//Txt !
//"WeChart !
```

这种使用方式可以简单概括为A=>B即：A调用B。

该方式存在的问题就是A和B存在强耦合，即订单和通知事件强关联。订单事件的订阅及订单的调用都在订单类里完成了。并不符合SOLID原则。

一个类只做一件事件，且该类要做到对扩展开发，对修改关闭。订单类应该只关注订单，并不应该关注消息的发送。且消息的发送最好不要在订单类里触发。


由于一些因素我们要去掉微信通知，再过一段时间要去掉短信通知，再过一段时间又要恢复这些通知。由于订单和这些订单关联的操作的强耦合性，使得扩展订单事件并不太方便。



那么使用事件总线怎么处理这个订单呢？

```CSharp
//伪代码
public class EventBus
{
    //关联订单事件和订单的处理事件（短信，邮件等事件）
    public Register<T>(EventHandler handler)
    {}
    //发布订单事件，触发订阅的和订单关联的处理事件
    public Publish(Event eventItem)
    {}
}
//在订单产生之前的任何地方都可以注册事件
EventBus.Instance.Register<OrderEvent>(new EmailEventHandler());
EventBus.Instance.Register<OrderEvent>(new TXTEventHandler());
EventBus.Instance.Register<OrderEvent>(new WeChartEventHandler());
//在订单产生之后的任何地方都可以调用事件通知方法
EventBus.Instance.Publish(new OrderEvent(){});
```

如果要对订单处理的事件增加和删除的话，只用在这里修改。事件的发送者和事件的接收者通过事件总线（EventBus）解耦。

调用方式为：A=>EventBus=>B

---

## 事件总线详述

事件总线大体可以分为三个部分

1. 事件产生者
2. 事件执行者
3. 事件管理者

定义一个事件产生者的接口。所有事件的产生者都需要继承该接口。

```CSharp
/// <summary>
/// 事件源接口（领域接口）
/// </summary>
public interface IEvent
{
}
```

定义一个事件执行者的泛型接口。其中Excute()方法为执行的方法。参数为事件产生者。

```CSharp
/// <summary>
/// 执行事件接口（领域事件）
/// </summary>
public interface IEventHandler<TEvent> where TEvent : IEvent
{
    /// <summary>
    /// 执行事件
    /// </summary>
    /// <param name="eve">事件源</param>
    void Excute(TEvent eve);
}
```

通常一个事件总线的实现有多种方式：

1. 内存模型
2. 队列模型
3. 持久化模型

所以定义一个接口，任何模型的实现都要依赖该接口

```CSharp
/// <summary>
/// 事件管理接口
/// </summary>
public interface IEventStore
{
    /// <summary>
    /// 注册一个事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="act"></param>
    void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : class, IEvent;
    /// <summary>
    /// 注销领域
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    void UnRegisterEvent<TEvent>() where TEvent : IEvent;

    /// <summary>
    /// 注销领域事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="act"></param>
    void UnRegisterHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent;

    /// <summary>
    /// 发布事件
    /// </summary>
    /// <typeparam name="Tevent"></typeparam>
    /// <param name="eventItem"></param>
    void Publish<Tevent>(Tevent eventItem) where Tevent : class, IEvent;
    /// <summary>
    /// 异步发布事件
    /// </summary>
    /// <typeparam name="TEvent"></typeparam>
    /// <param name="eventItem"></param>
    void PublishAsync<TEvent>(TEvent eventItem) where TEvent : IEvent;
}
```

这里只实现内存模型的事件总线，

```CSharp

/// <summary>
/// 内存事件管理
/// </summary>
public class InMemoryEventStore : IEventStore
{
    /// <summary>
    /// 源和事件的字典
    /// </summary>
    private readonly Dictionary<Type, List<object>> _registeredEventsDic = new Dictionary<Type, List<object>>();

    public void Publish<TEvent>(TEvent tevent) where TEvent : class, IEvent
    {
        if (_registeredEventsDic.ContainsKey(typeof(TEvent)))
        {
            var list = _registeredEventsDic[typeof(TEvent)];
            foreach (var item in list)
            {
                var s = item as IEventHandler<TEvent>;
                s.Excute(tevent);
            }
        }
    }
    private int _timeOut = 4000;
    public void PublishAsync<TEvent>(TEvent eventItem) where TEvent : IEvent
    {
        List<Task> taskList = new List<Task>();
        if (_registeredEventsDic.ContainsKey(typeof(TEvent)))
        {
            var events = _registeredEventsDic[typeof(TEvent)];
            //foreach (var item in events)
            //{
            //    var s = item as IEventHandler<TEvent>;
            //    taskList.Add(Task.Run(() => s.Excute(eventItem)));
            //}

            //events.ForEach((e) => taskList.Add(Task.Run(() =>
            //      (e as IEventHandler<TEvent>).Excute(eventItem))));


            Parallel.ForEach(events, e => taskList.Add(Task.Run(() =>
            {
                (e as IEventHandler<TEvent>).Excute(eventItem);
            })));


            if (_timeOut > 0)
                Task.WaitAll(taskList.ToArray(), _timeOut);
            else
                Task.WaitAll(taskList.ToArray());
        }
    }

    public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : class, IEvent
    {
        if (handler == null)
            throw new ArgumentNullException(nameof(handler));
        if (!_registeredEventsDic.ContainsKey(typeof(TEvent)))
        {
            _registeredEventsDic.Add(typeof(TEvent), new List<object>() { handler });
        }
        else
        {
            _registeredEventsDic[typeof(TEvent)].Add(handler);
        }
    }

    public void UnRegister<TEvent>() where TEvent : IEvent
    {
        throw new NotImplementedException();
    }

    public void UnRegisterEvent<TEvent>() where TEvent : IEvent
    {
        if (_registeredEventsDic.ContainsKey(typeof(TEvent)))
        {
            _registeredEventsDic.Remove(typeof(TEvent));
        }

    }

    public void UnRegisterHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
    {
        if (_registeredEventsDic.ContainsKey(typeof(TEvent)))
        {
            var list = _registeredEventsDic[typeof(TEvent)];
            var item = list.FirstOrDefault(x => x.ToString() == handler.ToString());
            list.Remove(item);
        }
    }
}
```

不管是何种类型的事件总线，对外都提供一种使用方式，

```CSharp
public sealed class EventBus
{

    private EventBus() { }
    private IEventStore _store;

    private static EventBus _instance;
    public static EventBus Instance
    {
        get
        {
            return _instance ?? (_instance = new EventBus()
            {
                _store = new InMemoryEventStore()
            });
        }
    }

    public void Register<TEvent>(IEventHandler<TEvent> handler) where TEvent : class, IEvent
    {
        _store.Register<TEvent>(handler);
    }

    public void UnRegisterHandler<TEvent>(IEventHandler<TEvent> handler) where TEvent : IEvent
    {
        _store.UnRegisterHandler<TEvent>(handler);
    }

    public void UnRegisterEvent<TEvent>() where TEvent : IEvent
    {
        _store.UnRegisterEvent<TEvent>();
    }

    public void Publish<TEvent>(TEvent tevent) where TEvent : class, IEvent
    {
        _store.Publish<TEvent>(tevent);
    }
}

```

