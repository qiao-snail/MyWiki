# CSharp的线程
## 第一部分（共四部分）
## 简单使用线程
### 概念介绍
 C# 支持通过多线程并行执行代码。线程是独立的执行路径，能够与其它线程同时运行。
 C#客户端由CLR或操作系统自动创建的单线程中启动。用多线程创建其他线程。
 ``` CSharp
class ThreadTest
{
    static void Main(){
        Thread t=new Thread(WriteY);
        t.Start();
        for(int i=0;i<1000;i++){
            Console.Write("X");
        }
    }
    static void WriteY(){
        for(int i=0;i<1000;i++)
            Console.Write("Y");
    }
}
 ```
 输出： xxxxxxxyyyyxxxxyyyyxxxyyyyy

 主线程创建了个新的线程t，线程t执行输出y的方法，同时主线程上也在执行输出x的方法。
  
一旦开始，线程的 IsAlive 属性返回 true，直到该线程的结束。当传递给线程构造函数的委托完成时该线程结束。一旦结束，线程无法重新启动。
  
  CLR 把自身内存堆栈分配给每个线程，以便本地变量都单独存放。
  如下示例中，我们定义的本地变量，方法，然后在主线程和一个新创建的线程同时调用方法︰
  ``` CSharp 
static void Main() 
{
  new Thread (Go).Start();      // 在新的线程上执行Go()
  Go();                         // 在主线程上执行Go()
}
 
static void Go()
{
  // Declare and use a local variable - 'cycles'
  for (int cycles = 0; cycles < 5; cycles++) Console.Write ('?');
}
  ```
  会输出10个问号。因为在每个线程的内存堆栈上创建了循环变量的单独副本。

  线程共享数据，如果他们是有共同的引用相同的对象实例
  如：
  ``` Csharp 
  class ThreadTest
{
  bool done;
 
  static void Main()
  {
    ThreadTest tt = new ThreadTest();   // 创建共同的实例
    new Thread (tt.Go).Start();
    tt.Go();
  }
 
  // 该实例的方法
  void Go() 
  {
     if (!done) { done = true; Console.WriteLine ("Done"); }
  }
}
  
  ```
  输出：Done 

  因为两个线程都调用了同一个实例的Go()方法，他们共享同一个字段。所以只输出一个Done

  跟上面的方法不一样的是，该方法的实例化实在一个线程中，而另一个是在自己单独的线程中。

  静态字段也可以实现线程的数据共享

  ``` CSharp 
class ThreadTest 
{
  static bool done;    // Static fields are shared between all threads
 
  static void Main()
  {
    new Thread (Go).Start();
    Go();
  }
 
  static void Go()
  {
    if (!done) { done = true; Console.WriteLine ("Done"); }
  }
}
  ```

  这两个例子说明了另一个关键概念︰ 线程安全 。输出实际上不确定︰ 它有可能输出两次“Done”。如果我们改变方法中语句的顺序，那么输出两次“Done”的几率会更大的提升。
``` CSharp
static void Go()
{
  if (!done) { Console.WriteLine ("Done"); done = true; }
}
```
出现这个情况是因为，一个线程在没执行到done=true的时候，另一个线程执行完了if(!done)语句。

解决方法：同时读取和写入的公共字段获取排它锁。C# 为此提供 lock 语句︰
``` Csharp
class ThreadSafe 
{
  static bool done;
  static readonly object locker = new object();
 
  static void Main()
  {
    new Thread (Go).Start();
    Go();
  }
 
  static void Go()
  {
    lock (locker)
    {
      if (!done) { Console.WriteLine ("Done"); done = true; }
    }
  }
}
```

当两个线程同时竞争一个锁时。一个线程会处于wait或block，直到这个锁可用。在这种情况下，它可以确保只有一个线程可以在一段时间内进入代码的临界区。
这就是线程安全。

共享的数据是多线程复杂性和产生错误的主要原因。

一个线程阻塞时, 不消耗 CPU 资源。

Join Sleep
 当调用Join方法时，可以实现等待另一个线程结束时再执行。
 ``` CSharp
 static void Main()
{
  Thread t = new Thread (Go);
  t.Start();
  t.Join();
  Console.WriteLine ("Thread t has ended!");
}
 
static void Go()
{
  for (int i = 0; i < 1000; i++) Console.Write ("y")
 ```
 输出： 先输出Y，再输出Thread t has ended
 可以给Join设置超时时间，当超时返回false。
 Thread.Sleep 可以暂停当前线程
 ``` CSharp
 Thread.Sleep (TimeSpan.FromHours (1));  // sleep for 1 hour
Thread.Sleep (500);                     // sleep for 500 milliseconds
 ```
 Thread.Sleep(0) 立即释放掉该线程的当前时间。把CPU资源交给其他线程。（Framework 4.0’的 new Thread.Yield() 方法一样效果）

 ### 线程如何工作
 多线程处理是由线程调度程序内部管理，CLR 通常委托给操作系统函数管理。线程调度程序确保所有活动线程分配适当的执行时间，并且线程正在等待或阻止 （例如，在一个独占锁或用户输入） 不会消耗 CPU 时间。

在单处理器计算机上，线程调度程序是执行时间切片 — — 迅速切换每个活动线程。在 Windows 中下, 一个时间片是通常数十毫秒为单位的区域 — — 相比来说 线程间相互切换比CPU更消耗资源。

在多处理器计算机上，多线程用一种混合的时间切片和真正的并发性来实现，在不同的线程在不同的 Cpu运行代码。仍然会有一些时间切片，由于操作系统的需要服务自己的线程 — — 以及那些其他应用程序。

一个线程会被外部因素打断，例如：时间切片。一般来说这种被抢占，打断线程是不可控的。

### 线程 VS 进程

线程就像应用程序运行在操作系统的进程上。就像在一台计算机上并行运行进程，线程在单个进程内并行运行。进程是完全相互隔离的，线程在一定程度上也是相互隔离的。
尤其是，线程与其他运行在同一程序中的线程共享内存。这样一个线程可以处理数据，另一个线程负责展示数据，所以多线程会更有效率。

### 线程的正确，错误用法

### 维护响应式用户界面
在"Worker"线程上运行耗的任务，主 UI 线程可以不被阻塞地继续处理键盘和鼠标事件。
### 更效率的使用或阻止CPU
可以使用一个线程等待来自另一个计算机或硬件设备的响应。当一个线程被阻止执行任务时，其他线程可以继续正常有效的执行。
### 投机的执行
在多核机器上，你可以提前来执行要做的东西来提高性能。LINQPad使用这种技术来加快新的查询。当你不知道哪个算法更有效的时候，可以并行执行所有的算法，来选择。
### 允许同时处理请求
在服务器上，来自客户端的请求有可能同一时间有好几个，这时候并行处理这些请求，可以更有效。

线程会增加程序的复杂性，线程本身是并没有太多的复杂性，但是线程间的通信和数据共享会增加复杂性。

一个好的策略是将多线程逻辑封装到可重用的类，可以独立地进行检查和测试。框架本身提供了许多更高级别的线程构造，我们稍后讨论。

## 创建，启动线程
正如我们看到在前面提到的，使用线程类的构造函数，传递在一个 ThreadStart 委托，它表示创建的线程。这里是 ThreadStart 委托如何定义︰
``` Csharp
public delegate void ThreadStart();
```
调用线程的Start（），线程开始执行，运行线程上的方法，直到线程结束。
``` CSharp
class ThreadTest
{
  static void Main() 
  {
    Thread t = new Thread (new ThreadStart (Go));
 
    t.Start();   // Run Go() on the new thread.
    Go();        // Simultaneously run Go() in the main thread.
  }
 
  static void Go()
  {
    Console.WriteLine ("hello!");
  }
}
```
线程t执行Go（），同时主线程也执行Go（）方法。
可以直接在构造函数里调用方法
``` CSharp
Thread t = new Thread (Go);
```
也可以使用lambda
``` CSharp
static void Main()
{
  Thread t = new Thread ( () => Console.WriteLine ("Hello!") );
  t.Start();
}
```
## 给线程传递数据（带参数的线程）
最简单的就是在lambda中的调用方法里传递参数
``` CSharp
static void Main()
{
  Thread t = new Thread ( () => Print ("Hello from t!") );
  t.Start();
}
 
static void Print (string message) 
{
  Console.WriteLine (message);
}
```
这种方法，还可以向方法传递多个参数，还可以在多语句 lambda 包裹整个执行。
``` CSharp
new Thread (() =>
{
  Console.WriteLine ("I'm running on another thread!");
  Console.WriteLine ("This is so easy!");
}).Start();
```
使用委托来实现
``` CSharp
new Thread (delegate()
{
  ...
}).Start();
```
2 给Start（）传递参数
``` CSharp
static void Main()
{
  Thread t = new Thread (Print);
  t.Start ("Hello from t!");
}
 
static void Print (object messageObj)
{
  string message = (string) messageObj;   // We need to cast here
  Console.WriteLine (message);
}
```
### Lambda表达式和被不捕获的变量
正如我们所看到的 lambda 表达式是最有力的将数据传递给线程的方式。然而，你必须要小心启动线程后修改捕获的变量，因为这些变量共享的。例如，考虑以下方面︰
``` CSharp
for (int i = 0; i < 10; i++)
  new Thread (() => Console.Write (i)).Start();
```
输出：0223557799
原因是变量i在这个循环的生命周期中都指向同一个内存地址,因此 每个线程调用Console.Write时，变量i会改变。
解决方法: 使用个临时变量
``` CSharp
for (int i = 0; i < 10; i++)
{
  int temp = i;
  new Thread (() => Console.Write (temp)).Start();
}
```
变量temp是每个循环里的本地变量（本次循环变量），因此，每个线程捕获不同的内存位置。举个例子：
``` CSharp
string text = "t1";
Thread t1 = new Thread ( () => Console.WriteLine (text) );
 
text = "t2";
Thread t2 = new Thread ( () => Console.WriteLine (text) );
 
t1.Start();
t2.Start();
```
输出： t2 t2 因为两个lambda都捕获了相同的text变量

## 线程命名
线程有个Name属性来设置线程的名字，这样有利于调试，同时名字只能设置一次，之后更改就会报错。
静态方法Thread.CurrentThread获取线程名字
``` CSharp
class ThreadNaming
{
  static void Main()
  {
    Thread.CurrentThread.Name = "main";
    Thread worker = new Thread (Go);
    worker.Name = "worker";
    worker.Start();
    Go();
  }
 
  static void Go()
  {
    Console.WriteLine ("Hello from " + Thread.CurrentThread.Name);
  }
}
```
## 前台，后台线程
默认情况下，创建的线程都是前台线程。前台线程当线程没有执行完，主程序就不能结束。后台线程则相反，主程序结束，后台线程立即释放。
通过IsBackground来设置线程是否是前台，后台。
``` CSharp
class PriorityTest
{
  static void Main (string[] args)
  {
    Thread worker = new Thread ( () => Console.ReadLine() );
    if (args.Length > 0) worker.IsBackground = true;
    worker.Start();
  }
}
```
如果不带参数调用此程序，工作线程假定前景状态，并将在用户按 enter 键的 ReadLine 语句等。同时，主线程退出，但应用程序保持运行，因为前台线程是还活着。

如果在Finally或Using中处理一些清理工作，如：释放资源，删除临时数据，避免这些，显式的等待后台应用退出应用程序
两种方法：
如果是自己创建的线程，可以对该线程调用Join
如果是在线程池上，可以使用wait handle