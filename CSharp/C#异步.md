# C#之异步
C#的异步可以分为三个模式

1. 异步模式BeginXXX,EndXXX
2. 事件异步xxxAsync，xxxCompleted
3. 基于任务Task的异步
4. async,await关键字异步
*异步模式和事件异步的自定义实现比较复杂，没有Task容易实现和理解。所以意义不大，只是简单做一下介绍。*

## 异步模式

异步模式是调用Beginxxx方法，返回一个IAsyncResult类型的值，在回调函数里调用Endxxxx（IAsyncResult）获取结果值。使用也很简单。
委托是异步尝试中最常见的。
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
---

## 事件异步

事件异步有一个xxxAsync， xxxCompleted方法，最常用的比如backgroundworker和progressbar结合
```CSharp

    public partial class MainWindow : Window
    {
        private BackgroundWorker bworker = new BackgroundWorker();
        public MainWindow()
        {
            InitializeComponent();
            bworker.WorkerReportsProgress = true;
            bworker.DoWork += Bworker_DoWork;
            bworker.ProgressChanged += Bworker_ProgressChanged;
            bworker.RunWorkerCompleted += Bworker_RunWorkerCompleted;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            bworker.RunWorkerAsync();
        }

        private void Bworker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void Bworker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pb.Value = e.ProgressPercentage;
        }

        private void Bworker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int j = 0; j <= 100; j++)
            {
                bworker.ReportProgress(j);

                Thread.Sleep(100);
            }
        }
    }

```
---

## Task模式的异步
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          