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