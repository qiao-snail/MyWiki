# Asp.Net MVC 路由

>使用URL请求应用程序时，该请求最终是通过Handler来完成，Asp.Net MVC 是通过一个自定义的HttpHandler--MVCHandler来实现对Controller的激活和Action执行。但是在这之前对Controller和Action的名称的解析是通过Asp.Net的URL路由系统来完成，整个路由系统是通过一个自定义的HttpModule--UrlRoutingModule来是实现的。

即： 路由是对URL到Controller和Action的映射及URL的输出。

## Route Table

当创建一个MVC应用程序的时候，在Global.asax文件中的Application_Start()方法会创建一个route table（Global.asax包含了Asp.Net应用程序生命周期的handler事件）。

当MVC应用程序第一次启动时，会调用Application_Start()方法。`RouteConfig.RegisterRoutes(RouteTable.Routes)`方法创建一个Route Table。

```CSharp
public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);//路由注册到应用程序
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }

```

在App_Start文件下，RouteConfig.cs文件里是路由的配置信息。通过RouteCollection.MapRoute()方法配置路由信息。

```CSharp
public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");//忽略该模式的URL

            routes.MapRoute(
                name: "Default",//路由名称
                url: "{controller}/{action}/{id}",//路由模板
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }//路由默认值，参数id可以为空
            );
        }
    }
```

* name:为该路由名称

* url：为路由模板，`{}`是占位符。

* defaults:为路由默认值

上述代码在路由表里创建了一个路由名为Default的路由。该Default路由由controller，action，id三部分组成，id为可选参数。

所以该路由配置如下url

* xxx.com/home/index/1
* xxx.com/home/index
* xxx.com/home
* xxx.com/

这些URL都会映射到如下Action：

```CSharp
public class HomeController :Controller
{
    public ActionResult Index()
    {
        return View();
    }
}
```

或

```CSharp
//在路由中id参数是可为空的，所以对于值类型的参数必须是可空的值类型。
public class HomeController :Controller
{
    public ActionResult Index(int? id)
    {
        return View();
    }
}
```
并且该action的参数名称需要和route中的参数（id）一直。即也是id。才可以匹配`xxx.com/home/index/1`否则只能通过url传参匹配`xxx.com/home/index?myparam=1`

如：如果定义的Action如下

```CSharp
public class HomeController :Controller
{
    public ActionResult Index(string str)
    {
        return View();
    }
}
```
输入`xxx.com/home/index/1`时，会认为参数为空，即str并没有被赋值，但是依然会调用index方法，只不过是认为str为空。但是当你通过url传参时，是可以匹配的`xxx.com/home/index?str=hello`


#### 在同一个Controller下是不允许有Action重载的

如：
```CSharp
public class HomeController :Controller
{
    public ActionResult Index(int? id)
    {
        return View();
    }
    public ActionResult Index()
    {
        return View();
    }
}
```
`在请求时提示错误：在对控制器类型“HomeController”的操作Index的请求方法不明确。`

---

### 路由顺序和优先级

路由引擎在定位路由时，会遍历路由集合中的所有路由。只要发现了一个匹配的路由，会立即停止搜索。所以定义路由一定要注意路由的先后循序。一般是越是精确的放在前面。

如：有一个如下的路由配置
```CSharp
routes.MapRoute{
    "one",
    "{site}",
    new{controller="MyControllerOne",action="Index"}
}
routes.MapRoute{
    "two",
    "Admin",
    new {controller="Admin",action="Index"}
}
```
第一个路由有一个{site}占位符。默认的控制器为MyControllerOne,第二个路由是一个常量Admin,默认的控制器为Admin。这两个都是正确的路由配置。但是当我们输入url`xxx.com/admin`时，我们是请求AdminController下的Index操作方法。但是根据上面的路由映射，该url匹配第一个路由，然后就停止了路由查找。此时出发的Controller为MyControllerOne。

---

### 路由约束

之前的路由配置，都没有url的参数的类型信息。如果我们的Action是一个Int类型，但是url中的参数是个字符串，这样就会导致错误。所以如果有url的类型约束可以规避这个错误的发生。

在Asp.Net MVC中我们可以通过正则表达式来约束路由。

如：

```CSharp
routes.MapRoute{
    "Default",
    "{controller}/{action}/{id}",
    new{controller="Home",action="Index",id=UrlParameter.Optional},
    new{id="\d+"}//该id为整数
}
```

除了使用正则表达式来约束路由，我们还可以通过继承IRouteConstraint接口自定义约束规则

如：
```CSharp
 public class MyRouteConstraint : IRouteConstraint
{
    public bool Match(HttpContextBase httpContext, Route route, string parameterName, RouteValueDictionary values, RouteDirection routeDirection)
    {
        //获取id的值
        var id = values[parameterName];

        //id验证方法

        return true;
    }
}
```

更新路由配置

```CSharp
routes.MapRoute{
    "Default",
    "{controller}/{action}/{id}",
    new{controller="Home",action="Index",id=UrlParameter.Optional},
    new{id=new MyRouteConstraint()}
}
```

That's it

---
