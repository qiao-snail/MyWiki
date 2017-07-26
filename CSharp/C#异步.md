# C#之异步
异步是相对于同步而言。异步和多线程两者不能划为等号。异步简单而言就是一个人两双手可以同时做两件以上不同的事情。多线程是指多个人做不同或相同的事情。

异步跟多线程有什么关系？

异步可以分为CPU异步和IO异步。他们两者的区别就是异步和多线程的区别。异步在CPU操作中是要有线程的。在IO操作中是不需要线程的，硬件直接和内存操作。

为什么要使用异步：
处理更多的服务器请求，通过在等待输入/输出请求返回时，让线程处理更多的请求。
通过在等待i/o请求和将长时间运行的工作转移到其他CPU核心的情况下，可以使UI在UI交互中更有响应性。
许多新的。净api是异步的。
在。NET中编写异步代码是很容易的。

C#实现异步的四种方式：

1. 异步模式BeginXXX,EndXXX
2. 事件异步xxxAsync，xxxCompleted
3. 基于任务`Task`的异步
4. `async`,`await`关键字异步

----
## 异步模式

异步模式是调用`Beginxxx`方法，返回一个`IAsyncResult`类型的值，在回调函数里调用`Endxxxx（IAsyncResult）`获取结果值。

异步模式中最常见的是委托的异步。

声明一个string类型输入参数和string类型返回值的委托。调用委托的BeginInvoke方法，来异步执行该委托。
```CSharp
 Func<string, string> func = (string str) =>
             {
                 Console.WriteLine(str);
                 return str + " end";
             };
            func.BeginInvoke("hello",IAsyncResult ar =>
            {
                Console.WriteLine(func.EndInvoke(ar));
            }, null);
//输出：
//hello
//hello end
```
`BeginInvoke`方法的第一个参数表示委托的输入参数。

第二个参数表示`IAsyncResult`类型输入参数的回调函数，其实也是个委托。

第三个参数是个状态值。


---

## 事件异步

事件异步有一个`xxxAsync`方法，和对应该方法的 `xxxCompleted`事件。

最常用的比如`backgroundworker`和`progressbar`结合

```CSharp

    public partial class MainWindow : Window
    {
        private BackgroundWorker bworker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            //支持报告进度
            bworker.WorkerReportsProgress = true;
            //执行具体的方法
            bworker.DoWork += Bworker_DoWork;
            //进度变化时触发的事件
            bworker.ProgressChanged += Bworker_ProgressChanged;
            //异步结束时触发的事件
            bworker.RunWorkerCompleted += Bworker_RunWorkerCompleted;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //开始异步执行
            bworker.RunWorkerAsync();
        }

        private void Bworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //异步完成时触发的事件
            progressBar.value=100;
        }

        private void Bworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //获取进度值复制给progressBar
            progressBar.Value = e.ProgressPercentage;
        }

        private void Bworker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int j = 0; j <= 100; j++)
            {
                //调用进度变化方法，触发进度变化事件
                bworker.ReportProgress(j);
                Thread.Sleep(100);
            }
        }
    }

```
---

## Task模式的异步
`Task`是在Framework4.0提出来的新概念。`Task`本身就表示一个异步操作（*`Task`默认是运行在线程池里的线程上*）。它比线程更轻量，可以更高效的利用线程。并且任务提供了更多的控制操作。

* 实现了控制任务执行顺序
* 实现父子任务
* 实现了任务的取消操作
* 实现了进度报告
* 实现了返回值
* 实现了随时查看任务状态

任务的执行默认是由任务调度器来实现的(*任务调用器使这些任务并行执行*)。任务的执行和线程不是一一对应的。有可能会是几个任务在同一个线程上运行，充分利用了线程，避免一些短时间的操作单独跑在一个线程里。所以任务更适合CPU密集型操作。

任务是用于实现所谓的并行的承诺模型的构造。简而言之，他们为你提供了一个“承诺”，即工作将在稍后完成，让你用一个干净的API与承诺进行协调。重要的是，将任务作为异步发生的工作的抽象，而不是对线程的抽象

#### Task 启动

任务可以赋值立即运行，也可以先由构造函数赋值，之后再调用。
```CSharp
//启用线程池中的线程异步执行
 Task t1 = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Task启动...");
            });
//启用线程池中的线程异步执行
 Task t2 = Task.Run(() =>
            {
                Console.WriteLine("Task启动...");
            });

 Task t3 = new Task(() =>
            {
                Console.WriteLine("Task启动...");
            });
 t3.Start();//启用线程池中的线程异步执行
 t3.RunSynchronously();//任务同步执行
```

#### Task 等待任务结果，处理结果
```CSharp
 Task t1 = Task.Run(() =>
            {
                Console.WriteLine("Task启动...");
            });
 Task t2 = Task.Run(() =>
            {
                Console.WriteLine("Task启动...");
            });

 //调用WaitAll()等待任务执行完成 ,Waitxxx会阻塞调用线程，这时异步也没有意义了          
 Task.WaitAll(new Task[] { t1, t2 });
 Console.WriteLine("Task完成...");

 //调用ContinueWith，等待任务完成，触发下一个任务，这个任务可当作任务完成时触发的回调函数。
 //为了获取结果，同时不阻塞调用线程，建议使用ContinueWith，在任务完成后，接着执行一个处理结果的任务。
t1.ContinueWith((t) =>
{
    Console.WriteLine("Task完成...");
});
t2.ContinueWith((t) =>
{
    Console.WriteLine("Task完成...");
});

//调用GetAwaiter()方法，获取任务的等待者，调用OnCompleted事件，当任务完成时触发
//调用OnCompleted事件也不会阻塞线程
t1.GetAwaiter().OnCompleted(() =>
{
    Console.WriteLine("Task完成...");
});
t2.GetAwaiter().OnCompleted(() =>
{
    Console.WriteLine("Task完成...");
});

```

#### Task 任务取消
```CSharp
//实例化一个取消实例
var source = new CancellationTokenSource();
var token = source.Token;

Task t1 = Task.Run(() =>
{
    Thread.Sleep(2000);
    //判断是否任务取消
    if (token.IsCancellationRequested)
    {
        //token.ThrowIfCancellationRequested();
        Console.WriteLine("任务已取消");
    }
    Thread.Sleep(500);
    //token传递给任务
}, token);

Thread.Sleep(1000);
Console.WriteLine(t1.Status);
//取消该任务
source.Cancel();
Console.WriteLine(t1.Status);
            
```
#### Task 返回值
```CSharp
Task<string> t1 = Task.Run(() => TaskMethod("hello"));
t1.Wait();
Console.WriteLine(t1.Result);

public string TaskMethod(string str)
{
    return str + " from task method";
}

```
**Task异步操作，需要注意的一点就是调用Waitxxx方法，会阻塞调用线程。**

----
## async await 异步

首先要明确一点的就是`async` `await` 不会创建线程。并且他们是一对关键字，必须成对的出现。

如果`await`的表达式没有创建新的线程，那么一个异步操作就是在调用线程的时间片上执行，否则就是在另一个线程上执行。

```CSharp
async Task MethodAsync()
{
    Console.WriteLine("异步执行");
    await Task.Delay(4000); 
    Console.WriteLine("异步执行结束");
}
```
一个异步方法必须有`async`修饰，且方法名以Async结尾。异步方法体至少包含一个`await`表达式。`await` 可以看作是一个挂起异步方法的一个点，且同时把控制权返回给调用者。异步方法的返回值必须是`Task`或者`Task<T>`。即如果方法没有返回值那就用Task表示，如果有一个string类型的返回值，就用`Task<string>`修饰。

异步方法执行流程：
1.主线程调用MethodAsync方法，并等待方法执行结束
2.异步方法开始执行，输出“异步执行”
3.异步方法执行到await关键字，此时MethodAsync方法挂起，等待await表达式执行完毕，同时将控制权返回给调用方主线程，主线程继续执行。
4.执行Task.Delay方法，MethodAsync挂起，等待`Task.Delay`结束
5.`Task.Delay`结束，`await`表达式结束，MehtodAsync执行await表达式之后的语句。

我们可能想当然的认为`Task.Delay`会阻塞执行线程，就跟`Thread.Sleep`一样。其实他们是不一样的。`Task.Delay`创建一个将在设置时间后执行的任务。就相当于一个定时器，多少时间后再执行操作。不会阻塞执行线程。当然如果你在异步方法里调用`Thread.Sleep`，这时会阻塞调用线程-主线程。同时也表明`await`并没有创建一个新的线程。

```CSharp
async Task Method2Async()
{
    Console.WriteLine("await执行前..."+Thread.CurrentThread.ManagedThreadId);
    await Task.Run(() =>
    {
        Console.WriteLine("await执行..." + Thread.CurrentThread.ManagedThreadId);
        Thread.Sleep(5000);
    });
    Console.WriteLine("await执行结束..."+ Thread.CurrentThread.ManagedThreadId);
}

//输出：
//await执行前...9
//await执行...12
//await执行结束...9
```
上面的异步方法，`await`表达式在另一个线程中执行，这是因为`Task`创建了一个线程池线程，而不是`await`创建了线程。
`async` `await`更加简便的创建异步方法。

https://msdn.microsoft.com/zh-cn/library/system.threading.tasks.task(v=vs.110).aspx

https://msdn.microsoft.com/zh-cn/library/dd997423.aspx

https://docs.microsoft.com/en-us/dotnet/standard/async-in-depth

https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/task-based-asynchronous-programming