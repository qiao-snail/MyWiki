# 线程同步

上一篇介绍了如何开启线程，线程间相互传递参数，及线程中本地变量和全局共享变量区别。

本篇主要说明**线程同步**。

如果有多个线程同时访问共享数据的时候，就必须要用线程同步，防止共享数据被破坏。如果多个线程不会同时访问共享数据，可以不用线程同步。

线程同步也会有一些问题存在：
1. 性能损耗。获取，释放锁，线程上下文建切换都是耗性能的。
2. 同步会使线程排队等待执行。
---
线程同步的几种方法：

## 阻塞
当线程调用Sleep，Join，EndInvoke，线程就处于阻塞状态（Sleep使调用线程阻塞，Join、EndInvoke使另外一个线程阻塞），会立即从cpu退出。（阻塞状态的线程不消耗cpu）

<small> *当线程在阻塞和非阻塞状态间切换时会消耗几毫秒时间。*</small>
``` CSharp
//Join
static void Main()
{
  Thread t = new Thread (Go);
  Console.WriteLine ("Main方法已经运行....");  
  t.Start();
  t.Join();//阻塞Main方法
  Console.WriteLine ("Main方法解除阻塞，继续运行...");
}
 
static void Go()
{
  Console.WriteLine ("在t线程上运行Go方法..."); 
}

//Sleep
static void Main()
{
  Console.WriteLine ("Main方法已经运行....");  
  Thread.CurrentThread.Sleep(3000);//阻塞当前线程
  Console.WriteLine ("Main方法解除阻塞，继续运行...");
}
 
 //Task
 static void Main()
{
   Task Task1=Task.Run(() => {  
            Console.WriteLine("task方法执行..."); 
              Thread.Sleep(1000);
            }); 
   Console.WriteLine(Task1.IsCompleted);             
   Task1.Wait();//阻塞主线程 ，等该Task1完成
   Console.WriteLine(Task1.IsCompleted); 
}
```
---
## 加锁（lock）

加锁使多个线程同一时间只有一个线程可以调用该方法，其他线程被阻塞。

**同步对象的选择：**
* 使用**引用类型**，值类型加锁时会装箱，产生一个新的对象。
* 使用**private**修饰，使用public时易产生死锁。*（使用lock（this），lock(typeof(实例))时，该类也应该是private）*。
* string不能作为锁对象。
* 不能在lock中使用`await`关键字

#### 锁是否必须是静态类型？

  如果被锁定的方法是静态的，那么这个锁必须是静态类型。这样就是在全局锁定了该方法，不管该类有多少个实例，都要排队执行。
  
  如果被锁定的方法不是静态的，那么不能使用静态类型的锁，因为被锁定的方法是属于实例的，只要该实例调用锁定方法不产生损坏就可以，不同实例间是不需要锁的。这个锁只锁该实例的方法，而不是锁所有实例的方法.*


``` CSharp
class ThreadSafe
{
 private static object _locker = new object();
 
  void Go()
  {
    lock (_locker)
    {
      ......//共享数据的操作 （Static Method）,使用静态锁确保所有实例排队执行
    }
  }

private object _locker2=new object();
  void GoTo()
  {
    lock(_locker2)
    //共享数据的操作，非静态方法，是用非静态锁，确保同一个实例的方法调用者排队执行
  }
}
```

*同步对象可以兼作它lock的对象*
如：
``` CSharp
class ThreadSafe
{
 private List <string> _list = new List <string>(); 
  void Test()
  {
    lock (_list)
    {
      _list.Add ("Item 1");
    }
  }
}
```
## Monitors
`lock`其实是`Monitors`的简洁写法。
```CSharp
lock (x)  
{  
    DoSomething();  
}  
```
两者其实是一样的。

```CSharp
System.Object obj = (System.Object)x;  
System.Threading.Monitor.Enter(obj);  
try  
{  
    DoSomething();  
}  
finally  
{  
    System.Threading.Monitor.Exit(obj);  
} 
```
---
## 互斥锁（Mutex）
互斥锁是一个互斥的同步对象，同一时间有且仅有一个线程可以获取它。可以实现进程级别上线程的同步。
```CSharp
class Program
    {
      //实例化一个互斥锁
        public static Mutex mutex = new Mutex();

        static void Main(string[] args)
        {
            for (int i = 0; i < 3; i++)
            {
              //在不同的线程中调用受互斥锁保护的方法
                Thread test = new Thread(MutexMethod);
                test.Start();
            }
            Console.Read();
        }

        public static void MutexMethod()
        {
           Console.WriteLine("{0} 请求获取互斥锁", Thread.CurrentThread.Name);
           mut.WaitOne();
           Console.WriteLine("{0} 已获取到互斥锁", Thread.CurrentThread.Name);     
           Thread.Sleep(1000);
           Console.WriteLine("{0} 准备释放互斥锁", Thread.CurrentThread.Name);
            // 释放互斥锁
           mut.ReleaseMutex();
           Console.WriteLine("{0} 已经释放互斥锁", Thread.CurrentThread.Name);
        }
    }
```
**互斥锁可以在不同的进程间实现线程同步**

使用互斥锁实现一个一次只能启动一个应用程序的功能。
```CSharp
    public static class SingleInstance
    {
        private static Mutex m;

        public static bool IsSingleInstance()
        {
            //是否需要创建一个应用
            Boolean isCreateNew = false;
            try
            {
               m = new Mutex(initiallyOwned: true, name: "SingleInstanceMutex", createdNew: out isCreateNew);
            }
            catch (Exception ex)
            {
               
            }
            return isCreateNew;
        }
    }
```
互斥锁的带有三个参数的构造函数

1. initiallyOwned: 如果initiallyOwned为true，互斥锁的初始状态就是被所实例化的线程所获取，否则实例化的线程处于未获取状态。

2. name:该互斥锁的名字，在操作系统中只有一个命名为name的互斥锁mutex，如果一个线程得到这个name的互斥锁，其他线程就无法得到这个互斥锁了，必须等待那个线程对这个线程释放

3. createNew:如果指定名称的互斥体已经存在就返回false，否则返回true
---
## 信号和句柄
`lock`和`mutex`可以实现线程同步，确保一次只有一个线程执行。但是线程间的通信就不能实现。如果线程需要相互通信的话就要使用`AutoResetEvent`,`ManualResetEvent`，通过信号来相互通信。它们都有两个状态，终止状态和非终止状态。只有处于非终止状态时，线程才可以阻塞。

#### `AutoResetEvent`：

`AutoResetEvent` 构造函数可以传入一个bool类型的参数，`false`表示将`AutoResetEvent`对象的初始状态设置为非终止。如果为`true`标识终止状态，那么`WaitOne`方法就不会再阻塞线程了。但是因为该类会自动的将终止状态修改为非终止，所以，之后再调用`WaitOne`方法就会被阻塞。

`WaitOne` 方法如果`AutoResetEvent`对象状态非终止，则阻塞调用该方法的线程。可以指定时间，若没有获取到信号，返回false

`set` 方法释放被阻塞的线程。但是一次只可以释放一个被阻塞的线程。
``` CSharp
class ThreadSafe 
{  
    static AutoResetEvent autoEvent;  

    static void Main()  
    {  
        //使AutoResetEvent处于非终止状态
        autoEvent = new AutoResetEvent(false);  

        Console.WriteLine("主线程运行...");  
        Thread t = new Thread(DoWork);  
        t.Start();  

        Console.WriteLine("主线程sleep 1秒...");  
        Thread.Sleep(1000);  

        Console.WriteLine("主线程释放信号...");  
        autoEvent.Set();  
    }  

     static void DoWork()  
    {  
        Console.WriteLine("  t线程运行DoWork方法，阻塞自己等待main线程信号...");  
        autoEvent.WaitOne();  
        Console.WriteLine("  t线程DoWork方法获取到main线程信号，继续执行...");  
    }  

}  

//输出
//主线程运行...
//主线程sleep 1秒...
//  t线程运行DoWork方法，阻塞自己等待main线程信号...
//主线程释放信号...
//  t线程DoWork方法获取到main线程信号，继续执行...

```
#### ManualResetEvent
`ManualResetEvent`和`AutoResetEvent`用法类似。

`AutoResetEvent`在调用了`Set`方法后，会自动的将信号由释放（终止）改为阻塞（非终止），一次只有一个线程会得到释放信号。而`ManualResetEvent`在调用`Set`方法后不会自动的将信号由释放（终止）改为阻塞（非终止），而是一直保持释放信号，使得一次有多个被阻塞线程运行，只能手动的调用`Reset`方法，将信号由释放（终止）改为阻塞（非终止）,之后的再调用Wait.One方法的线程才会被再次阻塞。
```CSharp
public class ThreadSafe
{
    //创建一个处于非终止状态的ManualResetEvent
    private static ManualResetEvent mre = new ManualResetEvent(false);

    static void Main()
    {
        for(int i = 0; i <= 2; i++)
        {
            Thread t = new Thread(ThreadProc);
            t.Name = "Thread_" + i;
            t.Start();
        }

        Thread.Sleep(500);
        Console.WriteLine("\n新线程的方法已经启动，且被阻塞，调用Set释放阻塞线程");

        mre.Set();

        Thread.Sleep(500);
        Console.WriteLine("\n当ManualResetEvent处于终止状态时，调用由Wait.One方法的多线程，不会被阻塞。");

        for(int i = 3; i <= 4; i++)
        {
            Thread t = new Thread(ThreadProc);
            t.Name = "Thread_" + i;
            t.Start();
        }

        Thread.Sleep(500);
        Console.WriteLine("\n调用Reset方法，ManualResetEvent处于非阻塞状态，此时调用Wait.One方法的线程再次被阻塞");
  

        mre.Reset();

        Thread t5 = new Thread(ThreadProc);
        t5.Name = "Thread_5";
        t5.Start();

        Thread.Sleep(500);
        Console.WriteLine("\n调用Set方法，释放阻塞线程");

        mre.Set();
    }


    private static void ThreadProc()
    {
        string name = Thread.CurrentThread.Name;

        Console.WriteLine(name + " 运行并调用WaitOne()");

        mre.WaitOne();

        Console.WriteLine(name + " 结束");
    }
}


//Thread_2 运行并调用WaitOne()
//Thread_1 运行并调用WaitOne()
//Thread_0 运行并调用WaitOne()

//新线程的方法已经启动，且被阻塞，调用Set释放阻塞线程

//Thread_2 结束
//Thread_1 结束
//Thread_0 结束

//当ManualResetEvent处于终止状态时，调用由Wait.One方法的多线程，不会被阻塞。

//Thread_3 运行并调用WaitOne()
//Thread_4 运行并调用WaitOne()

//Thread_4 结束
//Thread_3 结束

///调用Reset方法，ManualResetEvent处于非阻塞状态，此时调用Wait.One方法的线程再次被阻塞

//Thread_5 运行并调用WaitOne()
//调用Set方法，释放阻塞线程
//Thread_5 结束

```
---
## Interlocked
如果一个变量被多个线程修改，读取。可以用`Interlocked`。

计算机上不能保证对一个数据的增删是原子性的，因为对数据的操作也是分步骤的：

1. 将实例变量中的值加载到寄存器中。
2. 增加或减少该值。
3. 在实例变量中存储该值。

`Interlocked`为多线程共享的变量提供原子操作。
`Interlocked`提供了需要原子操作的方法：
* public static int Add (ref int location1, int value); 	 两个参数相加，且把结果和赋值该第一个参数。
* public static int Increment (ref int location); 自增。
* public static int CompareExchange (ref int location1, int value, int comparand);

      location1 和comparand比较，被value替换.

      value 如果第一个参数和第三个参数相等，那么就把value赋值给第一个参数。

      comparand 和第一个参数对比。
---

## ReaderWriterLock
如果要确保一个资源或数据在被访问之前是最新的。那么就可以使用`ReaderWriterLock`.该锁确保在对资源获取赋值或更新时，只有它自己可以访问这些资源，其他线程都不可以访问。即排它锁。但用改锁读取这些数据时，不能实现排它锁。

`lock`允许同一时间只有一个线程执行。而`ReaderWriterLock`允许同一时间有多个线程可以执行**读操作**，或者只有一个有排它锁的线程执行**写操作**。
```CSharp
    class Program
    {
        // 创建一个对象
        public static ReaderWriterLock readerwritelock = new ReaderWriterLock();
        static void Main(string[] args)
        {
            //创建一个线程读取数据
            Thread t1 = new Thread(Write);
           // t1.Start(1);
            Thread t2 = new Thread(Write);
            //t2.Start(2);
            // 创建10个线程读取数据
            for (int i = 3; i < 6; i++)
            {
                Thread t = new Thread(Read);
              //  t.Start(i);
            }

            Console.Read();

        }

        // 写入方法
        public static void Write(object i)
        {
            // 获取写入锁，20毫秒超时。
            Console.WriteLine("线程：" + i + "准备写...");
            readerwritelock.AcquireWriterLock(Timeout.Infinite);
            Console.WriteLine("线程：" + i + " 写操作" + DateTime.Now);
            // 释放写入锁
            Console.WriteLine("线程：" + i + "写结束...");
            Thread.Sleep(1000);
            readerwritelock.ReleaseWriterLock();

        }

        // 读取方法
        public static void Read(object i)
        {
            Console.WriteLine("线程：" + i + "准备读...");

            // 获取读取锁，20毫秒超时
            readerwritelock.AcquireReaderLock(Timeout.Infinite);
            Console.WriteLine("线程：" + i + " 读操作" + DateTime.Now);
            // 释放读取锁
            Console.WriteLine("线程：" + i + "读结束...");
            Thread.Sleep(1000);

            readerwritelock.ReleaseReaderLock();

        }
    }
//分别屏蔽writer和reader方法。可以更清晰的看到 writer被阻塞了。而reader没有被阻塞。

//屏蔽reader方法
//线程：1准备写...
//线程：1 写操作2017/7/5 17:50:01
//线程：1写结束...
//线程：2准备写...
//线程：2 写操作2017/7/5 17:50:02
//线程：2写结束...

//屏蔽writer方法
//线程：3准备读...
//线程：5准备读...
//线程：4准备读...
//线程：5 读操作2017/7/5 17:50:54
//线程：5读结束...
//线程：3 读操作2017/7/5 17:50:54
//线程：3读结束...
//线程：4 读操作2017/7/5 17:50:54
//线程：4读结束...
```
---
参考：
* MSDN
* 《CLR via C#》