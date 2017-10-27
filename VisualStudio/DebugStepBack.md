# Visual Studio 2017 调试时后退智能跟踪
在Visual Studio 2017(15.5Preview)版本中添加了一些新的功能。其中最大的更新就是实现了调试的“后退”。现在在新版Visual Studio2017你就能体验到调试时智能跟踪的“后退”与“前进”。Visual Studio在每一个断点处都保存了一个快照，使得能在后退时获得上一部数据的状态。

![](imgs/step-backward-and-forward-debugging.jpg)

您可以在诊断工具中查看调试期间捕获的快照事件，并通过选择事件导航到指定的断点。

![](imgs/debugging-events.jpg)

当您“后退”或“前进”时，快照将为每个状态保存调试状态数据。您可以使用监视（Watch)或自动窗口(Autos Window)查看不同状态的数据。

![](imgs/debugging-state-values.jpg)

要启用这个功能，选择“工具”>“选项”>“调试”>“常规”>"智能跟踪事件和快照"

![](imgs/enable-step-back-debugging.jpg)