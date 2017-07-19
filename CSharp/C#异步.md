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
声明一个string类型输入参数和string类型返回值的委托。调用委托的BeginInvoke方法，来异步执行该委托。

BeginInvoke方法的第一个参数表示委托的输入参数，

第二个参数表示IAsyncResult类型输入参数的回调函数。

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
https://msdn.microsoft.com/zh-cn/library/system.threading.tasks.task(v=vs.110).aspx

https://msdn.microsoft.com/zh-cn/library/dd997423.aspx