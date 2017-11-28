# Asp.Net MVC 捆绑（Bundle）

大多数浏览器会对同一域名的请求限制请求数量，一般是在8个以内。每次最多可以同时请求8个，要是资源多于8个，那么剩下的就要排队等待请求了。所以为了提高首次加载页面的速度。提高请求并发请求数量，降低请求次数就是一个很重要的点。

## Bundle

Asp.Net MVC4和.NET Framework4.5提供了支持捆绑和压缩的新类库System.Web.Optimization。

该类库提供了如下特性：

* 捆绑-将多个资源文件（javascript,css）合并成一个单独的文件，但是合并成的单独文件必须是相同类型，要么都是JavaScript要么都是CSS。
* 压缩资源文件-清理空格，换行等。
* 自动清理缓存-服务端更新资源时，客户端不再使用缓存资源，而是重新从服务端缓存。

---

### 1. 定义Bundle

在`App_Start`文件中新增一个`BundleConfig.cs`文件。实现静态`RegisterBundles`方法。该方法用来创建，注册和配置bundle。（*在该目录下代码最好把他们的命名空间去掉 ".App_Start"，保持一个统一的高等级的命名空间*）。

* 调用`BundleCollection.Add()`方法添加捆绑资源，该方法参数为`ScriptBundle`或`StyleBundle`。

* `ScriptBundle`和`StyleBundle`需要传递一个虚拟路径给构造函数。该虚拟路径其实就是该捆绑的名称或者标识符。所以该虚拟路径可以任意设置，并不需要匹配物料路径。`Bundle`的`Include`方法包含一个或者多个脚本。

* 通过引用该虚拟路径就可以使用这些捆绑的资源`@Script.Render("~/bundles/jquery")`。

* Debug模式下默认没有开启捆绑和压缩，发布模式下默认是开启的。

```CSharp
public static void RegisterBundles(BundleCollection bundles)
{
     //该值为true，在任何模式下都使用捆绑和压缩。
     //BundleTable.EnableOptimizations = true;
     
     //添加名称为“~/bundles/jquery”脚本捆绑
     bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
     //添加名称为“~/Content/css”样式捆绑
     bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"))
}
```

使用`{version}`占位符可以在使用NuGet更新Jquery版本时，不需要更新Bundle的引用，自动使用最新的Jquery版本。

`ScriptBundle`和`StyleBundle`的`Include`方法参数是一个字符串类型的数组，所以一个Bundles实例可以添加多个文件。
如

```CSharp
bundles.Add(new StyleBundle("~/Content/css").Include(
	"~/Content/themes/base/jquery.ui.core.css",
	"~/Content/themes/base/jquery.ui.resizable.css",
	"~/Content/themes/base/jquery.ui.selectable.css",
	"~/Content/themes/base/jquery.ui.accordion.css",
	"~/Content/themes/base/jquery.ui.autocomplete.css",
	"~/Content/themes/base/jquery.ui.button.css",
	"~/Content/themes/base/jquery.ui.dialog.css",
	"~/Content/themes/base/jquery.ui.slider.css",
	"~/Content/themes/base/jquery.ui.tabs.css",
	"~/Content/themes/base/jquery.ui.datepicker.css",
	"~/Content/themes/base/jquery.ui.progressbar.css",
	"~/Content/themes/base/jquery.ui.theme.css"));
```

但是Bundle类也提供了`IncludeDirectory`方法，可以添加指定目录下的指定文件。

```CSharp
//添加Content/themes/base目录下的所有css文件
bundles.Add(new StyleBundle("~/Content/css"").IncludeDirectory("~/Content/themes/base", "*.css"));
```

使用通配符要注意：

* 使用通配符添加资源时。这些资源文件是按照名称来排序的。

---

### 2. 启用Bundle

在Global.asax的Appliaction_Start方法中调用之前的定义的方法，`BundleConfig.RegisterBundles(BundleTable.Bundles)`
启用Bundle。

```CSharp
public class MvcApplication : System.Web.HttpApplication
{
    protected void Application_Start()
    {
        BundleConfig.RegisterBundles(BundleTable.Bundles);
    }
}
```
---

### 3. 使用Bundle

如果我们需要在页面中使用这些资源时。可以通过Styles和Scripts来引入。如果要使用捆绑的Style，可以在页面中添加`@Styles.Render("~/Content/css")`。如果要使用捆绑的Script，可以在页面中添加`@Script.Render("~/bundles/jquery")`。

```html
<!DOCTYPE html>
<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>@ViewBag.Title - 我的 ASP.NET 应用程序</title>
    //引入样式捆绑
    @Styles.Render("~/Content/css")    
</head>
<body>    
    <div class="container body-content">
        @RenderBody()
        <hr />
        <footer>
            <p>&copy; @DateTime.Now.Year - 我的 ASP.NET 应用程序</p>
        </footer>
    </div>
    //引入js捆绑
    @Scripts.Render("~/bundles/jquery")
    @RenderSection("scripts", required: false)
</body>
</html>

```

*可以把CSS样式文件置顶，JavaScript文件置底，来优化网页。但是`modernizr.js`文件要放在页面顶部，因为有些样式文件需要。*

---

## 使用CDN

Bundle对CDN也提供了很好的支持。

```CSharp
public static void RegisterBundles(BundleCollection bundles)
{
    //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
    //            "~/Scripts/jquery-{version}.js"));

    bundles.UseCdn = true;   //启用cdn
    //添加地址
    var jqueryCdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js";
    bundles.Add(new ScriptBundle("~/bundles/jquery",jqueryCdnPath).Include("~/Scripts/jquery-{version}.js"));
}
```
在使用CDN时，要应对没有获取到资源的情况。

```CSharp

        @Scripts.Render("~/bundles/jquery")
        <script type="text/javascript">
            if (typeof jQuery == 'undefined') {
                var e = document.createElement('script');
                e.src = '@Url.Content("~/Scripts/jquery-1.7.1.js")';
                e.type = 'text/javascript';
                document.getElementsByTagName("head")[0].appendChild(e);

            }
        </script>   
```

---

## Bundle缓存

浏览器是根据URL来缓存数据的。浏览器无论何时请求资源，都会根据URL来检查缓存里是否包含了该资源文件。如果包含了，浏览器就不会再去请求，而是使用缓存的文件，来渲染。

Bundle机制使我们每次修改资源文件时都会在URL后自动添加一个哈希值,从而避免浏览器缓存，不能及时更新资源的情况。

![缓存](imgs/BoundleCache.png)

v=******，后面的值就是哈希值。`Bundle` 在Debug模式下默认是没有开启的。在发布模式下才会开启。但是我们可以在BundleConfig下添加`BundleTable.EnableOptimizations = true;`开启捆绑模式。

---

## Bundle注意事项

* 一个`Bundle`一般包含多个文件，如果我们只是修改了其中的一个文件，那么`Bundle`的哈希值也会改变，就会更新`Bundle`的所有文件。

* 捆绑和缩小主要降低了第一次访问页面时加载的时间。此时静态资源就会被缓存起来（js,css，图片）。当访问其他页面，且该页面的资源地址和第一次访问的地址相同时，就会从缓存里获取，不再向服务端获取。

* 如果资源过多，使用CDN，比使用`Bundle`更有效。当然`Bundle`也可以结合CDN使用。 通过使用CDN，可以减轻每个主机名8个并发连接的浏览器限制。因为CDN的主机名与您的主机站点不同，CDN上的资产请求不会与您的主机环境的8个并发连接数计数。

* `Bundle`最好按照功能来区分捆绑。例如，默认的ASP.Net应用程序的NET MVC模板创建了一个与jQuery分离的jQuery验证包。因为所创建的默认视图输入输出值，所以它们需要验证包。

---

如有不对，请多多指教。

---

参考：

* [Bundleing and Minification](https://docs.microsoft.com/en-us/aspnet/mvc/overview/performance/bundling-and-minification)
* Asp.Net MVC 4 Web 编程