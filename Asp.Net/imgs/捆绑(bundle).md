# Asp.Net MVC 捆绑（Bundle）

大多数浏览器会对同一域名的请求限制请求数量，一般是在8个以内。每次最多可以同时请求8个，要是资源多于8个，那么剩下的就要排队等待请求了。

所以降低每次请求的数量是提升响应速度的重要方式。

## Bundle

Bundle是Asp.Net 4.5提供的一个特性。可以更加简单的把多个文件合并打包成一个单独的文件。这样在第一次访问页面的时候，因为请求数量的减少，提升了页面的加载性能。

在App_Start文件中新增一个BundleConfig.cs文件。实现RegisterBundles方法。该方法用来创建，注册和配置bundle。

```CSharp
public static void RegisterBundles(BundleCollection bundles)
{
     bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                 "~/Scripts/jquery-{version}.js"));
}
```

该方法是创建了一个名称为~/bundles/jquery的JavaScript bundle。该方法包含了Scripts目录下的jquery文件。在debug文件下该方法提供jquery的完整版文件。在发布模式下该方法提供jquery压缩文件。{version}是提供jquery的最新版本。

* 使用NuGet更新Jquery版本时，不需要更新Bundle的引用。
* 在调试和发布模式下会自动的选择Jquery不压缩和压缩版本。

使用CDN

Bundle对CDN也提供了很好的支持。



```CSharp
public static void RegisterBundles(BundleCollection bundles)
{
    //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
    //            "~/Scripts/jquery-{version}.js"));

    bundles.UseCdn = true;   //enable CDN support

    //add link to jquery on the CDN
    var jqueryCdnPath = "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.1.min.js";

    bundles.Add(new ScriptBundle("~/bundles/jquery",
                jqueryCdnPath).Include(
                "~/Scripts/jquery-{version}.js"));

    // Code removed for clarity.
}
```
在使用CDN时，要应对没有获取到资源的清空

```CSharp
</footer>

        @Scripts.Render("~/bundles/jquery")

        <script type="text/javascript">
            if (typeof jQuery == 'undefined') {
                var e = document.createElement('script');
                e.src = '@Url.Content("~/Scripts/jquery-1.7.1.js")';
                e.type = 'text/javascript';
                document.getElementsByTagName("head")[0].appendChild(e);

            }
        </script> 

        @RenderSection("scripts", required: false)
    </body>
</html>
```

Bundle类的Include方法参数是一个字符串类型的数组，所以可以一个Bundles实例可以添加多个文件。
如

```CSharp
bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
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

但是Bundle类也提供了IncludeDirectory方法，可以添加指定目录下的指定文件。

Bundle实例的Include方法也支持通配符“*”
Include("~/Scripts/Common/*.js")就是添加Common目录下的所有js文件。

但是尽量避免使用通配符

* 通过通配符默认添加脚本以按字母顺序加载它们，这通常不是您想要的。
CSS和JavaScript文件经常需要添加到特定的(非字母顺序)顺序中。
您可以通过添加自定义的iIBundleOrderer实现来减轻这种风险，但是显式地添加每个文件就不那么容易出错。
例如，您可能会在将来添加新的资产到一个文件夹中，这可能要求您修改IBundleOrderer实现。

* 使用通配符加载到目录中的特定文件可以包含在引用该包的所有视图中。如果将视图特定脚本添加到一个包中，您可能会在引用该包的其他视图上得到一个JavaScript错误

* 导入其他文件的CSS文件会导致导入的文件两次加载。例如，下面的代码创建了一个包，其中大多数jQuery UI主题CSS文件都加载了两次。

```CSharp
bundles.Add(new StyleBundle("~/jQueryUI/themes/baseAll")
    .IncludeDirectory("~/Content/themes/base", "*.css"));
```
通配符选择器。css“引入了文件夹中的每个css文件，包括内容主题的jquery.ui.all。css文件。jquery.ui.all。css文件导入其他css文件。
---

## Bundle缓存

当创建Bundle时，Bundle会设置Http Expires超时时间为一年。如果你再次浏览之前浏览过的页面。你会发现浏览器没有请求Bundle的文件。在地址栏输入地址，则不会给服务器发送任何数据。当使用F5刷新页面的时候，浏览器回去Web服务器验证缓存。当然你也可以强制刷新Ctrl+F5.此时会忽略缓存，重新从服务端获取资源。


Bundle在创建包的一年后设置HTTP Expires标题。如果您导航到以前浏览过的页面，Fiddler显示IE没有为该包提供有条件的请求，也就是说，没有来自IE的HTTP GET请求，也没有来自服务器的HTTP 304响应。您可以强制IE使用F5键对每个bundle进行有条件的请求(为每个bundle生成一个HTTP 304响应)。您可以使用F5(为每个bundle生成HTTP 200响应)来强制进行一次完整的刷新。

---

## Bundle注意事项

* 一旦您在一个包中更新一个文件，就会为bundle查询字符串参数生成一个新的令牌，并且当客户端请求包含该包的页面时，必须下载完整的包。在传统标记中，每个资产都是单独列出的，只有修改后的文件才会被下载。频繁变化的资产可能不是捆绑销售的好选择

* 捆绑和缩小主要改进了第一个页面请求加载时间。一旦网页被请求，浏览器就会缓存这些资产(JavaScript、CSS和图片)，所以捆绑和缩小不会提供任何性能提升，只要请求相同的页面，或者同一站点上的页面请求相同的资产。如果您不正确地将expires标题设置在您的资产上，并且您不使用捆绑和缩小，浏览器的新鲜启发式将在几天后标记资产过期，浏览器将需要对每个资产进行验证请求。

* 通过使用CDN，可以减轻每个主机名6个并发连接的浏览器限制。
因为CDN的主机名与您的主机站点不同，CDN上的资产请求不会与您的主机环境的6个并发连接数计数。
CDN还可以提供通用的包缓存和边缘缓存优势

* 捆绑包应该由需要它们的页面进行分区。例如，默认的ASP.Net应用程序的NET MVC模板创建了一个与jQuery分离的jQuery验证包。因为所创建的默认视图没有输入，也没有发布值，所以它们不包括验证包。