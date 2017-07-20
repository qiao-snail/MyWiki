# C#之异步
C#的异步可以分为三个模式

1. 异步模式BeginXXX,EndXXX
2. 事件异步xxxAsync，xxxCompleted
3. 基于任务Task的异步
4. async,await关键字异步
*异步模式和事件异步的自定义实现比较复杂，没有Task容易实现和理解。所以意义不大，只是简单做一下介绍。*

## 异步模式

异步模式是调用Beginxxx方法，返回一个IAsyncResult类型的值，在回调函数里调用Endxxxx（IAsyncResult）获取结果值。

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
BeginInvoke方法的第一个参数表示委托的输入参数。

第二个参数表示IAsyncResult类型输入参数的回调函数，其实也是个委托。

第三个参数是个状态值。


---

## 事件异步

事件异步有一个xxxAsync方法，和对应该方法的 xxxCompleted事件。

最常用的比如backgroundworker和progressbar结合

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
首先Task就是一个比线程更轻量但更高效的东西。因为任务和线程不是一一对应的。任务跑在线程上。但是也有可能是多个任务在一个线程上跑。这样对于线程来说就减少压力。
Task模式更容易实现异步的调用，和异步操作的取消，进度报告，返回值等功能。Task 对象通常以异步方式执行在线程池线程上而不是以同步方式在主应用程序线程，您可以使用 Status 属性，以及 IsCanceled, ，IsCompleted, ，和 IsFaulted 属性，以确定任务的状态。 
ThreadPool相比Thread来说具备了很多优势，但是ThreadPool却又存在一些使用上的不方便。比如：

1: ThreadPool不支持线程的取消、完成、失败通知等交互性操作；

2: ThreadPool不支持线程执行的先后次序；

任务可以赋值即运行，也可以先由构造函数赋值，之后在调用。

Task 启动
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
Task 等待任务结果，处理结果
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

Task 任务取消
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
Task 返回值
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


## async await 异步

首先要明确一点的就是async await 不会创建线程。

async和await是一对关键字，必须成对的出现，async标记在方法名，表示这是一个异步的方法，方法中的await，会阻塞当前方法的运行，并立即把线程返回给调用者，异步执行await方法，当await方法执行完后，才会执行这个方法内await后面的语句。包含至少一个 await 表达式，该表达式标记一个点，我们可以成为悬挂点，在该点上，直到等待的异步操作完成，之后的方法才能继续执行。 与此同时，该方法将挂起，并将控制权返回到方法的调用方。
如果await没有创建新的线程，那么一个异步操作就是在调用线程的时间片上执行，否则就是在另一个线程上执行。
异步方法返回类型必须是Task<T>类型，如果没有返回值，那么方法返回类型是Task


```CSharp
async Task MethodAsync()
{
    Console.WriteLine("异步执行");
    await Task.Delay(4000); 
    Console.WriteLine("异步执行结束");
}
```
一个异步方法必须有async修饰，且方法名以Async结尾。异步方法体至少包含一个await表达式。且异步方法的返回值必须是Task或者Task<T>。即如果方法没有返回值那就用Task表示，如果有一个string类型的返回值，就用Task<string>修饰。

异步方法执行流程：
1.主线程调用MethodAsync方法，并等待方法执行结束
2.异步方法开始执行，输出“异步执行”；
3.异步方法执行到await关键字，此时MethodAsync方法挂起，等待await表达式执行完毕，同时将控制权返回给调用方主线程，主线程继续执行。
4.执行Task.Delay方法，MethodAsync挂起，等待Task.Delay结束
5.Task.Delay结束，await表达式结束，MehtodAsync执行await表达式之后的语句。

我们可能想当然的认为Task.Delay会阻塞执行线程，就跟Thread.Sleep一样。其实他们是不一样的。Task.Delay创建一个将在设置时间后执行的任务。就相当于一个定时器，多少时间后再执行操作。不会阻塞执行线程。当然如果你在异步方法里调用Thread.Sleep，这时会阻塞调用线程-主线程。同时也表明await并没有创建一个新的线程。

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
上面的异步方法，await表达式在另一个线程中执行，这是因为Task创建了一个线程池线程，而不是await创建了线程。
async await更加简便的创建异步方法。

https://msdn.microsoft.com/zh-cn/library/system.threading.tasks.task(v=vs.110).aspx

https://msdn.microsoft.com/zh-cn/library/dd997423.aspx