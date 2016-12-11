# CSharp的线程
## 线程同步
### 同步要素

到目前为止，我们已经描述了如何在一个线程上开始一项任务、 配置一个线程，和在两个方向传递数据。
我们还介绍了如何本地变量都是私有的线程和如何在线程之间共享引用允许他们通过公共字段进行通信。   
下一步是同步︰ 协调操作的线程可预测的结果。同步是尤为重要的当线程访问相同的数据;它是很容易被搁浅在这一领域。  
 同步构造可分为四类︰
### Simple blocking methods
  等待另一个线程结束，或者在一个时间段内等待另一个线程。Sleep,Join,Task.Wait是一些简单的阻塞方法。

### Locking Constructs
这些限制，可以执行一些活动或一次执行一段代码的线程的数。
是最常见的独占锁定构造 — — 这些在一段时间，允许仅在一个线程中的，并且允许竞争线程访问共同数据而不会互相干扰。
标准的独占锁定构造是锁 (Monitor.Enter/Monitor.Exit)、 互斥体和自旋锁。非独占锁定构造是信号灯，SemaphoreSlim 和读取器/编写器锁。

### Signaling constructs
这些允许暂停直到从另一个，避免出现效率低下的轮询需要接收通知的线程。
有两种常用的信号设备︰ 事件等待句柄和监视器的等待/脉冲的方法。框架 4.0 引入了 CountdownEvent 和屏障的类。

### Nonblocking synchronization constructs
这些处理器基元呼吁保护对常见的字段的访问。
CLR 和 C# 提供了以下的非阻塞构造︰ Thread.MemoryBarrier、 Thread.VolatileRead、 Thread.VolatileWrite、 volatile 关键字和 Interlocked 类。

### 阻塞对于除最后一个类别之外的所有类别都是必需的让我们简单地研究一下这个概念

##闭塞
当一个线程调用Sleeping或者等待另一个线程Join或Endinvoke，那么该线程就会处于阻塞状态。处于阻塞状态的线程会立即从当前CPU时间片里退出，
并不会消耗CPU时间。当该线程不被阻塞时，就会再次得到CPU的时间片。通过ThreadState属性可以查询线程状态。
```Csahrp
bool blocked =(someThread.ThreadState & ThreadState.WaitSleepJoin)!=0;
```
（考虑到线程会在测试时状态改变然后再获取信息，所以该语句只有在调试时有效）
当线程阻塞，或解锁时，操作系统会切换上下文，这会导致几毫秒的开销。

有四种解锁的方式
1. 满足阻塞条件时
2. 超时
3. 调用Thread.Interrupt（打断）
4. 调用Thread.Abort (终止)
（线程如果时被Suspend方法暂停，不会处于阻塞状态。）

## Blocking Versus Spinning
有时候一个线程的条件不被满足时，必须处于暂停状态，等待该条件。Signaling和Locking Constructs可以通过阻塞来实现。
但还有一种更简单的方式，线程可以再while循环中等该该条件。
``` CSharp
while(!proceed);
```
or
``` CSharp
while(DateTime.Now<nextStartTime)；
```
（一般情况下，这是非常浪费处理器时间︰ CLR 和操作系统而言，该线程正在执行重要的计算，并因此获取相应地分配资源 ！）
有时会同时使用这个方法：
``` CSharp
while (!proceed) Thread.Sleep (10);
```
虽然不雅，但是比直接使用Spinning更有效，由于对并发proceed标志。正确使用锁和信号 避免了这一点。
（纺纱很简单，当你想到一个条件被满足很快（或许在几微秒），因为它避免了上下文切换的开销和延迟可能是有效的。.NET Framework提供了特殊的方法和类协助-这是覆盖 在并行编程部分。）

## ThreadState
 通过访问ThreadState，返回一个ThreadState枚举值，来获取线程状态。线程状态主要分为三个。多余的，未使用的，过时的。
 线程最有用的四个状态：Unstarted, Running, WaitSleepJoin, and Stopped:
 现在状态只有在调试时是有效的，在线程同步时是不可取的，现在在线程获取状态到显示状态信息之间，线程会改变状态。

 ## Locking
 独占锁用来保证一次只有一个线程执行。Lock,Mutex。前者可以简单高效的实现，后者可以跨域不同计算机的独占锁。
 ``` CSharp
 class ThreadUnsafe
{
  static int _val1 = 1, _val2 = 1;
 
  static void Go()
  {
    if (_val2 != 0) Console.WriteLine (_val1 / _val2);
    _val2 = 0;
  }
}
 ```
 这是个线程不安全的例子，如果GO方法被两个线程同时调用，有可能返回一个除数为0的错误，因为，在一个线程设置_val2=0的时候，另一个线程可能正好执行到if语句。
 
 加锁可以解决这个问题
 ``` CSharp
class ThreadSafe
{
  static readonly object _locker = new object();
  static int _val1, _val2;
 
  static void Go()
  {
    lock (_locker)
    {
      if (_val2 != 0) Console.WriteLine (_val1 / _val2);
      _val2 = 0;
    }
  }
} 
 ```
 在同一时间只有一个线程可以锁定同步对象（_locker），其他的线程被阻塞，直到锁释放。如果有多个线程访问，那么就会添加到一个线程队列。

 线程在等待锁，如果锁的状态为WaitSleepJoin,那么该线程将被阻塞。调用Interrupt和Abort，可以强制另一个线程释放锁。

 Locking 方法对比

| 方法 | 描述 | 跨进程 | 耗时 |
|----|---|----|---|
|lock (Monitor.Enter / Monitor.Exit) | 确保一次只有一个线程访问一个资源或代码| No| 20ns|
|Mutex | 确保一次只有一个线程访问一个资源或代码|Yes|1000ns|
|SemaphoreSlim (introduced in Framework 4.0) | 确保不大于指定个数的访问资源或代码段的并发线程|No|200ns|
|Semaphore|确保不大于指定个数的访问资源或代码段的并发线程|Yes|1000ns|
|ReaderWriterLockSlim (introduced in Framework 3.5)|允许一个writter和多个readder|No|40ns|
|ReaderWriterLock (effectively deprecated)|允许一个writter和多个readder|No|40ns|

## Monitor.Enter and Monitor.Exit
C#的lock实际上是对Monitor.Enter and Monitor.Exit的try catch的语法简洁方式上面的Go方法实际如下：
``` CSharp
Monitor.Enter (_locker);
try
{
  if (_val2 != 0) Console.WriteLine (_val1 / _val2);
  _val2 = 0;
}
finally { Monitor.Exit (_locker); 
}
```
lockTaken 重载
我们刚刚展示的代码也正是C＃1.0，​​2.0和3.0编译器在编译lock时的声明。
但是这样是有问题的，如果在调用Monitor.Enter或者在Monitor.Enter和try之间的代码出现异常，
那么时不会执行finally语句的。
为了避免这个问题，CLR4.0 实现了如下的Monitor.Enter重载
``` Csahrp
public static void Enter (object obj, ref bool lockTaken);
```
在Enter方法里，如果Enter方法抛出异常且，没有加锁，那么lockTaken就是false
在C# 4.0里lock声明如下：
``` CSharp
bool lockTaken = false;
try
{
  Monitor.Enter (_locker, ref lockTaken);
  // Do your stuff...
}
finally { if (lockTaken) Monitor.Exit (_locker); }
```
### TryEnter
Monitor还提供了一种TryEnter方法，该方法允许指定一个超时，可以是毫秒或TimeSpan。
然后如果得到一个锁，该方法返回true，如果没有得到锁，因为该方法超时，返回false。TryEnter也可以不带参数，如果没有获得锁，会立即超时，
作为与Enter方法，它的重载在CLR 4.0接受lockTaken 参数。

Enter方法，在CLR4.0中接受有lockTaken参数的重载

## 选择同步对象
对每个共享线程可访问的任何对象都可以用作同步对象，一个重要规则：必须是引用类型，同步对象必须是private（因为这有助于将锁定逻辑封装），通常是一个实例或者是静态字段。同步对象可以兼作它保护的对象
如下_list字段为同步对象
``` CSharp
class ThreadSafe
{
  List <string> _list = new List <string>();
 
  void Test()
  {
    lock (_list)
    {
      _list.Add ("Item 1");
    }
  }
}
```

专门为locking的字段（例如_locker,前面的例子）可以更精确的控制锁的范围和粒度。
this(包含对象)，甚至是其类型也可以作为同步对象。
``` CSharp
lock(this){...}
```
or
``` CSharp
lock(typeof(Widget)){...}//
```