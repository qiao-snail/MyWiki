# C#事件总线

## 一、概述

事件总线是事件发送者将事件消息发送到一个中心，事件订阅者在该中心订阅，接收事件，再处理事件。

事件总线可以分为三个部分，事件发送者，事件接收者，事件中心。

* 事件发送者： 事件的起点，
* 事件接收者： 接收事件，处理事件
* 事件中心： 负责关联事件发送者和事件接收者，及触发事件。一个事件发送者可以关联多个事件接收者。

如图：

事件总线的核心就是降低耦合，将消息的发送和接收分开，实现异步处理消息事件。

比如：在系统中产生了一个订单（标记为事件发送者），此时需要给用户发送邮件（事件接收者），发送短信通知（事件接收者），微信通知（事件接收者）等等一些列关于订单的操作。

如果不适用事件总线的话，怎么实现呢？

```CSharp
//伪代码-订单
protected void OrderOK()
{
    Task.Run(()=>SendEmail());
    Task.Run(()=>SendTxt());
    Task.Run(()=>SendWeChart());
    //其他操作
}
```
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

EventBus.Instance.Register<OrderEvent>(new EmailEventHandler());
EventBus.Instance.Register<OrderEvent>(new TXTEventHandler());
EventBus.Instance.Register<OrderEvent>(new WeChartEventHandler());

EventBus.Instance.Publish(new OrderEvent(){});
```
如果要对订单处理的事件增加和删除的话，只用在这里修改。事件的发送者和事件的接收者通过事件总线（EventBus）解耦。
