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
锁定以这种方式的缺点是，你不封装锁定逻辑，因此它越来越难以防止死锁和过度阻塞。锁定类型也可能渗入应用程序域边界 （在相同的进程）。

## 什么时候加锁
最基本的就是所有共享可写的字段。即使是最简单的例子，单个字段的赋值，必须要考虑同步。
``` Csharp
class ThreadUnsafe
{
  static int _x;
  static void Increment() { _x++; }
  static void Assign()    { _x = 123; }
} 
```
``` CSharp
class ThreadSafe
{
  static readonly object _locker = new object();
  static int _x;
 
  static void Increment() { lock (_locker) _x++; }
  static void Assign()    { lock (_locker) _x = 123; }
}
```

### 锁定和原子性
如果一组变量总是读取和写在同一个锁内，可以说这些变量是读写原子变量。
假设 字段x,y总是在一个锁内读写
``` CSharp
lock(locker){if(x!=0)y/=x}
```
x,y都是原子操作，因为这个代码块不能分割，或者被另一个线程抢占，这样会改变xy的值，或者是非法值。如果把xy放在一个锁里就可以避免非法值。

如果在lock代码块内发生异常，那么就违反了原子提供的锁。例如：
``` CSharp
decimal _savingsBalance, _checkBalance;

void Transfer (decimal amount)
{
  lock (_locker)
  {
    _savingsBalance += amount;
    _checkBalance -= amount + GetBankFee();
  }
}
```
如果调用GetBankFee()出现异常，那么bank就会丢失内存。在这里，我们可以早一点调用。另一个解决方法是在catch或者finally块里实现回滚逻辑。

## 嵌套锁
一个线程可以反复锁定同一对象中
``` CSharp
lock (locker)
  lock (locker)
    lock (locker)
    {
       // Do something...
    }
```
or
``` CSharp
Monitor.Enter (locker); Monitor.Enter (locker);  Monitor.Enter (locker); 
// Do something...
Monitor.Exit (locker);  Monitor.Exit (locker);   Monitor.Exit (locker);
```
在这种情况下，只有最外层lock退出了，这个对象才会解锁.或者就是Monitor.Exit匹配个数。
当一个方法在一个锁内调用另一个锁时会很有用。
``` CSharp
static readonly object _locker = new object();
 
static void Main()
{
  lock (_locker)
  {
     AnotherMethod();
     // We still have the lock - because locks are reentrant.
  }
}
 
static void AnotherMethod()
{
  lock (_locker) { Console.WriteLine ("Another method"); }
}
```
线程只会在最外层的锁被阻塞。
## 死锁
当两个线程都在等待对方释放同一个资源，那么就会死锁。解释死锁最简单的方法是用两个锁
``` CSharp
object locker1 = new object();
object locker2 = new object();
 
new Thread (() => {
                    lock (locker1)
                    {
                      Thread.Sleep (1000);
                      lock (locker2);      // Deadlock
                    }
                  }).Start();
lock (locker2)
{
  Thread.Sleep (1000);
  lock (locker1);                          // Deadlock
}
```
死锁是在最困难的问题之一多线程 — — 尤其是当有很多相互关联的对象。从根本上说，困难的问题是，你无法确定您调用方已采取了什么锁。
所以你可能直接在x类的锁定a字段，没有意识到调用者也在y类里锁定了b字段。同时另一个线程做了反向处理。那么就产生了死锁。且只有在运行时才有可能发现这个问题。
最受欢迎的建议是“按照一定的顺序锁定对象。”

另一种死锁情况时，调用 Dispatcher.Invoke （在 WPF 应用程序中） 或 Control.Invoke （在 Windows 窗体应用程序中） 同时持有的锁。
如果UI正在执行一个等待同一个锁的方法，那么就会死锁。最简单的方法使用BeginInvoke。或者可以在调用Invoke前释放锁。

## 互斥（Mutex）
Mutex像是C#的lock,但是它可以实现跨进程。换句话说，Mutex既可以整个应用程序域，也可以跨电脑。

使用Mutex，调用WaitOne方法加锁，ReleaseMutex解锁，自动关闭，释放。就像lock。Mutex只能在开启的线程上释放掉。

最想用的互斥锁就是确保一次只有一个程序在运行。
``` CSharp
class OneAtATimePlease
{
  static void Main()
  {
    // Naming a Mutex makes it available computer-wide. Use a name that's
    // unique to your company and application (e.g., include your URL).
 
    using (var mutex = new Mutex (false, "oreilly.com OneAtATimeDemo"))
    {
      // Wait a few seconds if contended, in case another instance
      // of the program is still in the process of shutting down.
 
      if (!mutex.WaitOne (TimeSpan.FromSeconds (3), false))
      {
        Console.WriteLine ("Another app instance is running. Bye!");
        return;
      }
      RunProgram();
    }
  }
 
  static void RunProgram()
  {
    Console.WriteLine ("Running. Press Enter to exit");
    Console.ReadLine();
  }
}
```

## 信号（Semaphore）
信号就像是夜店，它有一定的容量，由保镖强制执行。如果满了，谁也不能进。在外面建立一个队列。然后一旦有人离开，那么队列首位可以进去。
这个构造函数需要两个参数，当前夜总会的人数，和夜店的承载人数。
信号在功能上和Mutex，lock一样。但是它没有owner，是线程不可知者。任何线程都可以释放信号。Mutex,lock只能在调用线程上释放。

信号量可以有助于限制并发 — — 防止过多的线程同时执行特定的代码。在以下示例中，五个线程试图进入一次允许只有三个线程在夜总会︰
``` CSharp
class TheClub      // No door lists!
{
  static SemaphoreSlim _sem = new SemaphoreSlim (3);    // Capacity of 3
 
  static void Main()
  {
    for (int i = 1; i <= 5; i++) new Thread (Enter).Start (i);
  }
 
  static void Enter (object id)
  {
    Console.WriteLine (id + " wants to enter");
    _sem.Wait();
    Console.WriteLine (id + " is in!");           // Only three threads
    Thread.Sleep (1000 * (int) id);               // can be here at
    Console.WriteLine (id + " is leaving");       // a time.
    _sem.Release();
  }
}
```
如果把Sleep替换为磁盘IO，信号量会通过限制过度并发硬驱活动提高总体性能。

 一个信号量，如果命名，可以像互斥锁跨越进程访问。

 ## 线程安全
 如果程序或方法没有面临多线程环境，或者加锁，减少线程间的交互，可以说线程是安全的。
 通常的类型不是线程安全的类型主要有以下原因：
  1. 如果一个类型有很多字段，那么会有性能影响。
  2. 性能影响。
  3. 类型安全，不一定程序就是安全的。

  线程安全所以就是哪里需要哪里实现。
  然而，还是有几个方法可以是一个大而复杂的类实现线程安全。一个是通过牺牲粒度包装大段代码，甚至是整个对象的访问权限。
  在一个单一的排他锁内，是序列化访问权限最高。事实上，如果你想使用安全的使用线程不安全的第三方代码，这是必不可少的。
  诀窍就是简单地使用相同的排它锁来保护对所有属性、 方法和字段的线程不安全对象上的访问。这个方法如果所有的对象的方法都可以快速执行，那就可以实现线程安全，否则就会产生大量的阻塞。

  另一种方式是尽量通过最小化共享的数据以最小化线程相互作用。这是非常好的方法，。由于多个客户端请求可以同时到达时，他们调用服务器方法必须是线程安全的。无状态的设计（流行的可扩展性的原因）本质上限制了互动的可能性，因为类不保留请求之间的数据，线程交互只限于一个静态字段，用于这种目的作为缓存常用的数据在内存中，在提供基础设施服务，如身份验证和审核。

  在执行线程安全的最终途径是使用自动锁定政权。.NET 框架也正是这一点，如果子类 ContextBoundObject 和将同步属性应用于类。任何时候然后调用方法或属性上这样的对象，对象范围内锁定自动采取整体执行的方法或属性。虽然这会减少线程安全负担，它将创建它自己的问题︰ 死锁，否则不会发生，贫困的并发性和意外可重入性。出于这些原因，手动锁定通常是更好的选择 — — 至少直到一个较简单的自动锁定政权变得可用。

  ##  .NET Framework Types
  Locking可以把线程不安全的代码转换为线程安全的代码。几乎所有的非基本类型都是线程不安全的。但是通过locking却可以在多线程中实例化并访问。
  ``` CSharp
  class ThreadSafe
{
  static List <string> _list = new List <string>();
 
  static void Main()
  {
    new Thread (AddItem).Start();
    new Thread (AddItem).Start();
  }
 
  static void AddItem()
  {
    lock (_list) _list.Add ("Item " + _list.Count);
 
    string[] items;
    lock (_list) items = _list.ToArray();
    foreach (string s in items) Console.WriteLine (s);
  }

  ```
  相关的操作都要锁定在一个锁内。
  假设如下的List是线程安全的。
  ``` CSharp
  if (!_list.Contains (newItem)) _list.Add (newItem);
  ```
  不管是不是线程安全的，这样声明就是不对的。整个if语句必须要包裹在一个锁内，防止验证是否存在和添加方法之间抢占锁。同样这个锁要被用在任何对这个List的操作。

  ## 静态字段
  把整个对象都包装在一个自定义锁里，只有所有的并发线程都意识和使用这个锁，那么锁才有效。如果对象是广泛的，则可能不是这种情况。
  最坏的情况是带有公共类型中的静态成员。例如，想象一下如果 DateTime 结构不是线程安全的，两个并发调用会导致乱码的输出或异常。
  唯一能解决这个外部锁就是锁定自身类型，在调用DateTime.Now之前。

  通常认为静态字段是线程安全的，实力字段是不安全的。

  ## Read-only thread safety
  用read onlys使类型并发线程安全。因为它让消费者避免过多的Locking。

## Thread Safety in Application Servers
  应用程序服务器需要多线程来处理并发客户端请求。WCF，ASP.NET 和 Web 服务应用程序是隐式多线程;为远程处理服务器应用程序使用 TCP 或 HTTP 等网络渠道也是如此。这意味着，编写代码时在服务器端，您必须考虑线程安全，如果有任何可能性在处理客户端请求的线程之间的交互。幸运的是，这种可能性是罕见的;一个典型的服务器类，要么是无国籍 （没有字段） 或已为每个客户端或每个请求创建一个单独的对象实例的激活模型。交互通常出现只有通过静态字段，有时用于在数据库的内存部分缓存以提高性能。

  ``` CSharp
  static class UserCache
{
  static Dictionary <int, User> _users = new Dictionary <int, User>();
 
  internal static User GetUser (int id)
  {
    User u = null;
 
    lock (_users)
      if (_users.TryGetValue (id, out u))
        return u;
 
    u = RetrieveUser (id);   // Method to retrieve user from database
    lock (_users) _users [id] = u;
    return u;
  }
}
  ```

  我们必须在最低限度，以读取和更新字典来确保线程安全的锁。在此示例中，我们选择简洁和性能在锁定之间切实可行的妥协方案。我们的设计实际上会创建很小的潜在效率低下︰ 如果两个线程同时调用此方法具有相同的以前未检索 id，RetrieveUser 方法将被调用两次 — — 和字典会不必要地更新。在整个方法一次锁定会防止这种情况，但会造成严重的效率低下︰ 整个缓存将被锁定为调用的 RetrieveUser，在这段时间其他线程将阻塞在检索任何用户的持续时间。

## Rich Client Applications and Thread Affinity
  Windows WPF 和 Windows 窗体库遵循基于线程关联的模型。虽然各有一个单独的实现，但他们都在它们的功能非常相似。