# 使用.NET和Blazor开发基于浏览器的APP（译）

今天我很高兴的宣布由一个来自Asp.Net团队的新的实现项目--Blazor.Blazor是一个基于C#,Razor和HTML的通过WebAssembly运行在浏览器的一个Web UI 框架。Blazor可以更快，更友好的创建一个在浏览器中运行的单页应用程序。正是通过这个，它让开发人员可以依靠web标准来编写运行在浏览器的webapp 客户端。

如果你已经开发过.NET程序，你已经完成了这个图片：除了现有的服务器和基于云的服务、原生移动/桌面应用和游戏之外，你还可以使用你的技能进行基于浏览器的开发。如果你还没有使用.NET，我们的希望产品的生产效率和简单性的这些特点足以让你去尝试它。

## 为什么使用.NET 开发浏览器APP

Web开发在很多方面都有了改进，但是构建现代Web应用程序仍然面临挑战。在浏览器中使用.NET有许多优势，可以使web开发变得更简单、更高效:

* **稳定性和一致性:** .NET提供了标准的API、工具和构建基础设施。网络平台稳定，功能丰富，易于使用。
* **现代创新语言：** C＃和F＃等.NET语言使得编程变得愉快，并通过创新的语言功能不断改进。
* **行业领先的工具：** Visual Studio产品系列在Windows，Linux和MacOS上提供了一个很好的.NET开发体验。
* **快速且可扩展性：** .NET在服务器上的Web开发方面具有悠久的性能，可靠性和安全性。使用.NET作为全栈解决方案可以更轻松地构建快速，可靠和安全的应用程序。

---

**Browser+Razor=Blazor!**

Blazor基于现有的Web技术，如HTML和CSS，但是使用C＃和Razor语法代替JavaScript来构建可组合的Web UI。请注意，这不是在浏览器中部署现有UWP或Xamarin移动应用程序的方式。可以在[Steve Sanderson’s prototype demo at NDC Oslo](https://www.youtube.com/watch?v=MiLAE6HMr10&feature=youtu.be&t=31m45s)中查看运行起来的效果，或者在[simple Blazor app running in Azure]https://blazor-demo.azurewebsites.net/)中尝试构建。

Blazor将具有现代Web框架的所有功能，包括：

* 构建可组合UI的组件模型
* 路由
* 布局
* 形式和验证
* 依赖注入
* JavaScript交互操作
* 开发期间在浏览器中重新加载
* 服务器端呈现
* 在浏览器和IDE中进行网络调试
* 丰富的智能提示和工具
* 能够通过asm.js在较老的(非webassmbly)浏览器上运行
* 发布和应用程序大小调整

## WebAssembly改变Web

在浏览器中运行.NET可以通过WebAssembly来实现，WebAssembly是一种新的Web标准，用于“适合编译到Web的便携式，大小和加载时间效率高的格式”.WebAssembly使编写Web应用程序的基本新方法成为可能。 编译为WebAssembly的代码可以在任何浏览器中以本地速度运行。 这是构建可以在浏览器中运行的.NET运行时所需的基础部分。 没有插件或transpilation需要。 您使用基于WebAssembly的.NET运行时在浏览器中运行正常的.NET程序集。

去年八月，微软Xamarin团队的朋友宣布他们计划使用WebAssembly将一个.NET运行时（Mono）带到Web上，并且一直在稳步前进。 Blazor项目建立在他们的工作之上，以创建一个用.NET编写的富客户端单页面应用程序框架。

## 新的实验

虽然我们对Blazor的承诺感到兴奋，但这是一个实验性的项目，而不是一个承诺的产品。 在这个实验阶段，我们期望与Blazor的早期采用者深入沟通，听取您的意见和建议。 这一次，我们可以解决与在浏览器中运行.NET相关的技术问题，并确保我们可以构建开发人员喜欢并且可以高效运行的东西。

## 发生的地方

Blazor仓库现在是公开的，你可以找到所有的行动。这是一个完全开源的项目：你可以看到所有的开发工作，并在公共回购问题跟踪。

请注意，我们在这个项目中很早。 目前还没有安装程序或项目模板，许多计划功能尚未实现。 即使已经实施的部件尚未针对最小有效载荷大小进行优化。 如果你热衷，你可以克隆回购，建立它，并运行测试，但只有最勇敢的开拓者今天试图编写应用程序代码。 如果你是那个勇敢的先驱，请深入挖掘资料来源。 反馈和建议可以通过Blazor回购问题追踪器提供。 在未来的几个月里，我们希望发布pre-alpha项目模板和工具，让更多的听众来试用。

请查看BlazorFAQ了解更多。

---

[原文](https://blogs.msdn.microsoft.com/webdev/2018/02/06/blazor-experimental-project/)