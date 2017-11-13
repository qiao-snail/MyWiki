# Asp.Net MVC 中的 Cookie

## Cookie
Cookie是请求服务器，或访问Web页面时携带的一个小的文本信息。Cookie包含了任何时候用户访问站点时的可读信息。

cookie为Web应用程序中提供了一种存储特定于用户的信息方法。例如，当用户访问您的站点时，您可以使用cookie来存储用户首选项或其他信息。当用户再次访问您的Web站点时，应用程序可以检索它先前存储的信息。


Background

例如,如果一个用户请求你网站的一个页面，你的Web网站除了返回请求的页面,也返回了一个包含日期和时间的Cookie,当用户的浏览器得到请求页面的时候，也获取到了携带的Cookie信息。,这个Cookie存储在用户的硬盘上的一个文件夹上。

稍后，如果用户再次向您的站点请求页面，当用户输入URL时，浏览器会在本地硬盘上查看与该URL相关联的cookie。
如果cookie存在，浏览器会将cookie发送到您的站点，并与页面请求一起发送。
然后，您的应用程序可以确定用户上次访问该站点的日期和时间。
您可以使用这些信息向用户显示一条消息，或者检查一个过期日期。

cookie与一个Web站点相关联，而不是特定的页面，因此不管用户请求你Web程序的什么页面，浏览器和服务器都会交换cookie信息。 浏览器会为每个不同的站点分别存储Cookie，保证每个Cookie都对应特定的站点。

Cookies可以帮助网站存储访问者的信息。通俗的说，cookie是保持Web应用程序连续性的一种方式，即执行状态管理。除了在实际交换信息的短暂时间之外，浏览器和Web服务器断开连接。因为用户对Web服务器的每个请求都是独立于任何其他请求处理的（每个请求都是独立，一系列的请求并没有连续关联起来）。但是，很多时候，Web服务器在用户请求页面时识别用户是很有用的。例如，购物网站上的Web服务器会跟踪单个购物者，这样站点就可以管理购物车和其他特定于用户的信息。此时，cookie充当一种调用卡，提供相关的标识，帮助应用程序知道如何处理。

（因为Web请求每个都是独立的，就是说用户的当前请求，并不知道用户上个请求做了些什么。对于服务端来说就是，不知道是哪个用户做了这个事情。但是有时候Web站点需要知道每个用户都请求了哪些。我们就可以用Cookie来对用户做唯一标识，每次请求都会携带用户唯一标识的Cookie。这样我们就知道了哪个用户做了这个请求）

ookie用于许多目的，所有这些都与帮助网站记住用户有关。例如，一个进行投票的站点可能会使用cookie作为布尔值来指示用户的浏览器是否已经参与投票，这样用户就不能进行两次投票。一个要求用户登录的网站可能会使用cookie来记录用户已经登录的情况，这样用户就不必继续输入凭证了。

Cookie Limitations

大多数浏览器支持最多4096字节的cookie。由于这个限制，cookie最好用于存储少量数据，或者更好的是，像用户ID这样的标识符。用户ID可以用来标识用户，并从数据库或其他数据存储中读取用户信息。

浏览器也会限制你的网站可以在用户的电脑上存储多少cookie。大多数浏览器只允许每个站点提供20个cookie;如果你想储存更多，最古老的Cookie就会被丢弃。一些浏览器还会对他们从所有站点所接受的cookie的数量设置绝对限制，通常是300。

您可能会遇到的一个cookie限制是用户可以设置他们的浏览器来拒绝cookie。如果您定义了P3P隐私策略并将其放到Web站点的根目录中，那么更多的浏览器将接受来自您站点的cookie。但是，您可能必须完全避免cookie，并使用不同的机制来存储特定于用户的信息。存储用户信息的常用方法是会话状态，但会话状态取决于cookie，如“cookie和会话状态”小节中所解释的那样。

尽管cookie在您的应用程序中非常有用，但是应用程序不应该依赖于能够存储cookie。不要使用cookie来支持关键特性。如果您的应用程序必须依赖于cookie，那么您可以测试浏览器是否接受cookie。请参阅“检查浏览器是否接受cookie”一节的内容。

---

Writing Cookies

浏览器负责管理用户系统上的cookie。cookie被发送到浏览器，通过名为“cookie”的对象，该对象公开了一个名为cookie的集合。您可以将该对象作为页面类的响应属性来访问。您想要发送到浏览器的任何cookie都必须添加到这个集合中。在创建cookie时，指定一个名称和值。每个cookie都必须有一个惟一的名称，以便在从浏览器读取时可以识别它。因为cookie是按名称存储的，因此命名两个cookie会导致一个被覆盖。

您还可以设置cookie的日期和时间过期。
当用户访问写cookie的站点时，浏览器会删除过期的cookie。
只要您的应用程序认为cookie值是有效的，就应该设置cookie的过期时间。
对于一个有效的cookie，您可以将过期日期设置为50年以后。

如果您没有设置cookie的过期时间，那么就创建了cookie，但是它并没有存储在用户的硬盘上。相反，cookie是作为用户会话信息的一部分来维护的。当用户关闭浏览器时，cookie就会被丢弃。像这样的非持久cookie对于需要在短时间内存储的信息非常有用，或者出于安全原因，不应该将其写入到客户机计算机上的磁盘上。例如，如果用户正在使用公共计算机，而不希望将cookie写入磁盘，则非持久性cookie非常有用。

您可以通过多种方式将cookie添加到cookie集合中。下面的例子展示了两个编写cookie的方法:

```CSharp
Response.Cookies["userName"].Value = "patrick";
Response.Cookies["userName"].Expires = DateTime.Now.AddDays(1);

HttpCookie aCookie = new HttpCookie("lastVisit");
aCookie.Value = DateTime.Now.ToString();
aCookie.Expires = DateTime.Now.AddDays(1);
Response.Cookies.Add(aCookie);
```

该示例将两个cookie添加到cookie集合中，其中一个名为用户名，另一个命名为last参拜。对于第一个cookie，cookie集合的值是直接设置的。您可以添加值以这种方式收集因为饼干来源于专业NameObjectCollectionBase类型的集合。

对于第二个cookie，代码创建一个HttpCookie对象的实例，设置它的属性，然后通过Add方法将其添加到cookie集合中。当您实例化一个HttpCookie对象时，您必须将cookie名作为构造函数的一部分传递。

这两个示例都完成了相同的任务，将cookie写入浏览器。在这两种方法中，过期值必须为DateTime类型。但是，last参拜值也是一个日期时间值。因为所有的cookie值都以字符串形式存储，所以日期时间值必须转换为字符串。

Cookies With More Than One Value

您可以在cookie中存储一个值，比如用户名和最后一次访问。您还可以在一个cookie中存储多个名称-值对。名称-值对被称为子键。(子键的布局非常类似于URL中的查询字符串。)例如，您可以创建一个名为userInfo的cookie，它具有subkeys用户名和last参拜，而不是创建两个单独的cookie。

出于几个原因，您可能会使用子键。首先，将相关的或类似的信息放入一个cookie中是很方便的。此外，由于所有信息都在一个cookie中，cookie属性如过期将应用于所有信息。(相反，如果您想为不同类型的信息分配不同的过期日期，则应该将信息存储在单独的cookie中。)

带有子键的cookie也可以帮助您限制cookie文件的大小。
正如前面提到的“Cookie限制”一节中提到的，Cookie通常被限制为4096字节，并且每个站点不能存储20多个Cookie。
通过使用一个带有子键的cookie，您的站点所分配的20个cookie就少了。
此外，一个cookie占用了大约50个字符(过期信息，等等)，加上您存储在其中的值的长度，所有这些值都指向4096字节的限制。
如果您存储5个子键而不是5个单独的cookie，您可以节省单独的cookie的开销，并且可以节省大约200个字节。

要使用子键创建一个cookie，您可以使用语法的变体来编写一个cookie。以下示例显示了两种编写同一个cookie的方法，每个方法都有两个子项：

```Csharp
Response.Cookies["userInfo"]["userName"] = "patrick";
Response.Cookies["userInfo"]["lastVisit"] = DateTime.Now.ToString();
Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(1);

HttpCookie aCookie = new HttpCookie("userInfo");
aCookie.Values["userName"] = "patrick";
aCookie.Values["lastVisit"] = DateTime.Now.ToString();
aCookie.Expires = DateTime.Now.AddDays(1);
Response.Cookies.Add(aCookie);
```

---

Controlling Cookie Scope

默认情况下，一个站点的所有cookie都存储在客户机上，所有的cookie都被发送到服务器，并向该站点发送任何请求。换句话说，网站上的每一个页面都能获得该站点的所有cookie。但是，您可以通过两种方式来设置cookie的范围:

* 将cookie的范围限制在服务器上的一个文件夹中，这允许您将cookie限制在站点上的一个应用程序中。

* 将范围设置为域，允许您指定域中的哪些子域可以访问cookie。

要将cookie限制在服务器上的一个文件夹中，可以设置cookie的路径属性，如下面的例子:

```CSharp
HttpCookie appCookie = new HttpCookie("AppCookie");
appCookie.Value = "written " + DateTime.Now.ToString();
appCookie.Expires = DateTime.Now.AddDays(1);
appCookie.Path = "/Application1";
Response.Cookies.Add(appCookie);
```

路径既可以是站点根下的物理路径，也可以是虚拟根。这样做的效果是，cookie只可用于Application1文件夹或虚拟根目录中的页面。例如,如果你的网站是www.contoso.com,在前面的示例中创建的饼干可以与路径`http://www.contoso.com/Application1/`页面,该文件夹下任何页面。然而,饼干不会在其他应用程序中可用的页面,比如`http://www.contoso.com/Application2/`或者`http://www.contoso.com/`。

### Limiting Cookie Domain Scope

默认情况下，cookie与特定的域相关联。
例如，如果您的站点是www.contoso.com，那么当用户请求该站点的任何页面时，您所写的cookie将被发送到服务器。
(这可能不包括带有特定路径值的cookie。)
如果你的站点有子域名，例如，contoso.com，sales.contoso.com，以及support.contoso.com，那么你就可以把cookies与特定的子域名联系起来。
为此，设置cookie的域属性，就像本例中所做的那样:

```CSharp
Response.Cookies["domain"].Value = DateTime.Now.ToString();
Response.Cookies["domain"].Expires = DateTime.Now.AddDays(1);
Response.Cookies["domain"].Domain = "support.contoso.com";
```

当域以这种方式设置时，cookie将只在指定子域中的页面可用。您还可以使用域属性来创建一个可以在多个子域中共享的cookie，如下面的示例所示:

```CSharp
Response.Cookies["domain"].Value = DateTime.Now.ToString();
Response.Cookies["domain"].Expires = DateTime.Now.AddDays(1);
Response.Cookies["domain"].Domain = "contoso.com";
```

此时Cookie既可以用户主域，也可以用于sales.contoso.com和support.contoso.com等子域。

---

## Reading Cookies

当浏览器向服务器发出请求时，它会连同请求一起发送该服务器的cookie。
在你的ASP.NET应用程序，您可以使用该对象来读取cookie，该对象作为页面类的请求属性可用。
对象的结构本质上和那个“海绵”对象一样，所以你可以从对象中读取cookie，就像你在“海绵”对象中写饼干一样。
下面的代码示例展示了获得一个名为用户名的cookie的两种方法，并在标签控件中显示其值

```Csharp
if(Request.Cookies["userName"] != null)
    Label1.Text = Server.HtmlEncode(Request.Cookies["userName"].Value);

if(Request.Cookies["userName"] != null)
{
    HttpCookie aCookie = Request.Cookies["userName"];
    Label1.Text = Server.HtmlEncode(aCookie.Value);
}
```

在尝试获取cookie的值之前，您应该确保cookie存在;如果cookie不存在，您将得到NullReferenceException异常。还要注意，HtmlEncode方法被调用来对cookie的内容进行编码，然后将其显示在页面中。这确保了恶意用户没有将可执行脚本添加到cookie中。有关cookie安全性的更多信息，请参阅“cookie和安全性”一节。

---

* [MSDN-Cookie](https://msdn.microsoft.com/en-us/library/ms178194.aspx)