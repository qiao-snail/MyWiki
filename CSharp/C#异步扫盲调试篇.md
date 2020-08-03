> 在上一篇文章中，我们探讨了异步代码的基础知识，为什么它很重要，以及如何用c#编写它。然而，虽然它可以提高您的程序的总体吞吐量，异步代码仍然不能避免错误!当潜在的死锁、模糊的错误消息和查找导致错误的任务混杂在一起时，编写异步代码会使调试变得更加困难。幸运的是，Visual Studio可以帮你更容易实现异步调试!

### 如何看到程序的所有任务?

当您在异步代码中遇到错误时，您可能希望标识所有的任务，并确定是哪些任务导致了错误。如果您曾经调试过多线程应用程序，那么您可能对Threads窗口很熟悉。同样，对于任务也有相同的窗口!Tasks窗口允许您查看所有任务，显示它们的id、当前位置、最初传递给每个任务的方法以及它们在中断时的当前状态(活动的、计划的、阻塞的或死锁的)。如果您的程序是多线程的，此窗口还将显示运行每个任务的线程。这可以帮助找到可能导致问题的特定线程。

你可以通过`Debug > Windows > Task` 或者使用快捷键 `CTRL+SHIFT+D,K`, 查看Task窗体

![](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2020/06/TasksWindow.png)

### 如何在异步代码中找到一个抛出异常的起源?

在调试异步代码时，确定抛出的异常的原始位置是令人沮丧的。当一个异常被多次抛出时，Visual Studio通常会返回最近通过异常帮助器抛出异常的调用堆栈。不幸的是，这并不总是有助于异步调试。为了解决这个问题，我们在16.4中实现了重新抛出异常。使用此更新，异常帮助器将在重新抛出异常时显示原始调用堆栈。要了解更多关于这个新特性的信息，请查看[Andy Sterland的博客](https://devblogs.microsoft.com/visualstudio/exception-helper-rethrown-exceptions/)。

![](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2020/06/RethrownException-Copy.png)

### 有没有更好地可视化任务和异步代码流的方法?

为了对异步代码的执行进行图形化描述，Visual Studio有一个用于线程和任务的`并行堆栈窗口(Parallel Stacks)`。任务的并行堆栈窗口(或并行任务窗口)可视化地显示活动的、等待的和计划的任务以及它们之间的关系。双击一个活动的或等待的任务会在“调用堆栈”窗口中显示异步调用堆栈。要了解哪个线程正在运行特定任务，可以在并行线程和并行任务窗口之间进行交换。您可以通过右键单击并选择上下文菜单中的`Go To Thread`来做到这一点。要了解更多关于16.6中并行任务的新更新，请关注即将发布的博客文章……

![](https://devblogs.microsoft.com/visualstudio/wp-content/uploads/sites/4/2020/06/ParallelTask_TourExample-Copy.png)

[原文](https://devblogs.microsoft.com/visualstudio/how-do-i-debug-async-code-in-visual-studio/)