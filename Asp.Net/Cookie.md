# Asp.Net MVC 中的 Cookie

## Cookie
Cookie是请求服务器或访问Web页面时携带的一个小的文本信息。

Cookie为Web应用程序中提供了一种存储特定用户的信息方法。例如，当用户访问您的站点时，您可以使用cookie来存储用户首选项或其他信息。当用户再次访问您的Web站点时，应用程序可以检索它先前存储的信息。

## Cookie使用场景

如果一个用户请求你网站的一个页面，你的Web网站除了返回请求的页面,也返回了一个包含日期和时间的Cookie,当用户的浏览器得到请求页面的时候，也获取到了携带的Cookie信息。,这个Cookie存储在用户的硬盘上的一个文件夹上。

稍后，如果用户再次向您的站点请求页面，当用户输入URL时，浏览器会在本地硬盘上查看与该URL相关联的cookie。
如果cookie存在，浏览器会将cookie发送到您的站点，并与页面请求一起发送。
然后，您的应用程序可以确定用户上次访问该站点的日期和时间。
您可以使用这些信息向用户显示一条消息，或者检查一个过期日期。

Cookie与一个Web站点相关联，而不是特定的页面，因此不管用户请求你Web程序的什么页面，浏览器和服务器都会交换cookie信息。 浏览器会为每个不同的站点分别存储Cookie，保证每个Cookie都对应特定的站点。

Cookie可以帮助网站存储访问者的信息。通俗的说，cookie是保持Web应用程序连续性的一种方式，即执行状态管理。除了在实际交换信息的短暂时间之外，浏览器和Web服务器断开连接。因为用户对Web服务器的每个请求都是独立于任何其他请求处理的（每个请求都是独立，一系列的请求并没有连续关联起来）。但是，很多时候，Web服务器在用户请求页面时识别用户是很有用的。例如，购物网站上的Web服务器会跟踪单个购物者，这样站点就可以管理购物车和其他特定于用户的信息。此时，cookie充当一种调用卡，提供相关的标识，帮助应用程序知道如何处理。

（因为Web请求每个都是独立的，就是说用户的当前请求，并不知道用户上个请求做了些什么。对于服务端来说就是，不知道是哪个用户做了这个事情。但是有时候Web站点需要知道每个用户都请求了哪些。我们就可以用Cookie来对用户做唯一标识，每次请求都会携带用户唯一标识的Cookie。这样我们就知道了哪个用户做了这个请求）

cookie用于许多目的，所有这些都与帮助网站记住用户有关。例如，一个进行投票的站点可能会使用cookie作为布尔值来指示用户的浏览器是否已经参与投票，这样用户就不能进行两次投票。一个要求用户登录的网站可能会使用cookie来记录用户已经登录的情况，这样用户就不必继续输入凭证了。

## Cookie限制

大多数浏览器支持最多4096字节的cookie。由于这个限制，cookie最好用于存储少量数据，或者更好的是，像用户ID这样的标识符。用户ID可以用来标识用户，并从数据库或其他数据存储中读取用户信息。

浏览器也会限制你的网站可以在用户的电脑上存储多少cookie。大多数浏览器只允许每个站点提供20个cookie;如果你想储存更多，最古老的Cookie就会被丢弃。一些浏览器还会对他们从所有站点所接受的cookie的数量设置绝对限制，通常是300。

您可能会遇到的一个cookie限制是用户可以设置他们的浏览器来拒绝cookie。如果您定义了P3P隐私策略并将其放到Web站点的根目录中，那么更多的浏览器将接受来自您站点的cookie。但是，您可能必须完全避免cookie，并使用不同的机制来存储特定于用户的信息。存储用户信息的常用方法是会话状态，但会话状态取决于cookie，如“cookie和会话状态”小节中所解释的那样。

尽管cookie在您的应用程序中非常有用，但是应用程序不应该依赖于能够存储cookie。不要使用cookie来支持关键特性。如果您的应用程序必须依赖于cookie，那么您可以测试浏览器是否接受cookie。请参阅“检查浏览器是否接受cookie”一节的内容。

---

## 写Cookie

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

## Cookie作用域

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

### Cookie的限制和作用域

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

## 读取Cookie

当浏览器向服务器发出请求时，cookie会随着请求一起发送到服务器上。
在你的ASP.NET应用程序，您可以使用HttpRequest来读取cookie。
对象的结构本质上和那个HttpResponse对象一样，所以你可以从该对象中读取cookie，就像你在HttpResponse对象中写Cookie一样。


代码示例：

```Csharp
if(Request.Cookies["userName"] != null)
{
    HttpCookie aCookie = Request.Cookies["userName"];
    var cookiText = Server.HtmlEncode(aCookie.Value);
}
```

在尝试获取cookie的值之前，您应该确保cookie存在;如果cookie不存在，您将得到NullReferenceException异常。还要注意，HtmlEncode方法被调用来对cookie的内容进行编码。这确保了恶意用户没有将可执行脚本添加到cookie中。


在cookie中读取子键的值也类似于设置它。下面的代码示例展示了获取子键值的一种方法

```CSharp
if(Request.Cookies["userInfo"] != null)
{
    var name = 
        Server.HtmlEncode(Request.Cookies["userInfo"]["userName"]);

    var visit =
        Server.HtmlEncode(Request.Cookies["userInfo"]["lastVisit"]);
}
```

在前面的示例中，代码读取键值对lastVisit的值，该值之前被设置为DateTime值的字符串表示。cookie将值存储为字符串，因此如果您想使用lastVisit的值作为日期，则必须将其转换为适当的类型，如本例中所做的那样:

```Csharp
DateTime dt= DateTime.Parse(Request.Cookies["userInfo"]["lastVisit"]);
```

cookie中的子键被键入为一个NameValueCollection类型。因此，获取子键的另一种方法是获取子键集合，然后按名称提取子键值，如下面的示例所示:


```CSharp
if(Request.Cookies["userInfo"] != null)
{
    System.Collections.Specialized.NameValueCollection  UserInfoCookieCollection;
    UserInfoCookieCollection = Request.Cookies["userInfo"].Values;
   var name = 
        Server.HtmlEncode(UserInfoCookieCollection["userName"]);
    var visit =
        Server.HtmlEncode(UserInfoCookieCollection["lastVisit"]);
}
```

修改Cookie的过期时间

浏览器负责管理cookie，cookie的过期时间和日期帮助浏览器管理其存储的cookie。因此，虽然您可以读取cookie的名称和值，但是您不能读取cookie的过期日期和时间。当浏览器向服务器发送cookie信息时，浏览器不包含过期信息。(cookie的过期属性总是返回一个日期时间值为0。)如果您担心cookie的过期日期，您必须重新设置它。


### 读取Cookie集合

获取所有的Cookie

代码示例；

```CSharp
System.Text.StringBuilder output = new System.Text.StringBuilder();
HttpCookie aCookie;
for(int i=0; i<Request.Cookies.Count; i++)
{
    aCookie = Request.Cookies[i];
    output.Append("Cookie name = " + Server.HtmlEncode(aCookie.Name) 
        + "<br />");
    output.Append("Cookie value = " + Server.HtmlEncode(aCookie.Value)
        + "<br /><br />");
}
```

上一个示例的有一个不足的地方，如果cookie有子键，会将子键作为单个name/value字符串显示。
您可以读取cookie的haskey属性，以确定cookie是否有子键。
如果有，您可以读取子键集合以获得单独的子键名和值。
您可以通过索引值直接从值集合中读取子键值。
相应的子键名可以在值集合的AllKeys成员中找到，它返回一个字符串数组。
您还可以使用值集合的键值。
但是，AllKeys属性在第一次被访问时被缓存。
相反，键属性在每次访问时都构建一个数组。
由于这个原因，在相同页面请求的上下文中，AllKeys属性要快得多。

下面的例子显示了前面的示例的修改。它使用HasKeys属性来测试子键，如果检测到子键，那么这个示例将从值集合中获得子键:

```CSharp
for(int i=0; i<Request.Cookies.Count; i++)
{
    aCookie = Request.Cookies[i];
    output.Append("Name = " + aCookie.Name + "<br />");
    if(aCookie.HasKeys)
    {
        for(int j=0; j<aCookie.Values.Count; j++)
        {
            subkeyName = Server.HtmlEncode(aCookie.Values.AllKeys[j]);
            subkeyValue = Server.HtmlEncode(aCookie.Values[j]);
            output.Append("Subkey name = " + subkeyName + "<br />");
            output.Append("Subkey value = " + subkeyValue + 
                "<br /><br />");
        }
    }
    else
    {
        output.Append("Value = " + Server.HtmlEncode(aCookie.Value) +
            "<br /><br />");
    }
}
```

或者，您可以将子键提取为NameValueCollection对象，如下面的例子所示:

```CSharp
System.Text.StringBuilder output = new System.Text.StringBuilder();
HttpCookie aCookie;
string subkeyName;
string subkeyValue;

for (int i = 0; i < Request.Cookies.Count; i++)
{
    aCookie = Request.Cookies[i];
    output.Append("Name = " + aCookie.Name + "<br />");
    if (aCookie.HasKeys)
    {
        System.Collections.Specialized.NameValueCollection CookieValues = 
            aCookie.Values;
        string[] CookieValueNames = CookieValues.AllKeys;
        for (int j = 0; j < CookieValues.Count; j++)
        {
            subkeyName = Server.HtmlEncode(CookieValueNames[j]);
            subkeyValue = Server.HtmlEncode(CookieValues[j]);
            output.Append("Subkey name = " + subkeyName + "<br />");
            output.Append("Subkey value = " + subkeyValue + 
                "<br /><br />");
        }
    }
    else
    {
        output.Append("Value = " + Server.HtmlEncode(aCookie.Value) +
            "<br /><br />");
    }
}
```

---

## Cookie的删除和修改

不能直接在获取到cookie后就修改cookie的值（Request.Cookies["Key"].Value="some"，在这里是不起作用的)，必须在Response.Cookies["Key"].Value="SomeNew"中修改才可以。其实就是程序中设置的Cookie新值，覆盖了用户浏览地上的Cookie的旧值。

代码示例：

```CSharp
int counter;
if (Request.Cookies["counter"] == null)
    counter = 0;
else
{
    counter = int.Parse(Request.Cookies["counter"].Value);
}
counter++;

Response.Cookies["counter"].Value = counter.ToString();
Response.Cookies["counter"].Expires = DateTime.Now.AddDays(1);

```

删除Cookie

将cookie从用户的硬磁盘上删除，和修改cookie原理是一样的。不能直接删除cookie，因为cookie在用户的计算机上。但是，您可以让浏览器删除cookie。
该方法是创建一个新cookie，其名称与要删除的cookie相同，但要将cookie的过期时间设置为比今天更早的日期。当浏览器检查cookie的过期时，浏览器将丢弃已经过时的cookie。

代码示例：

```CSharp
HttpCookie aCookie;
string cookieName;
int limit = Request.Cookies.Count;
for (int i=0; i<limit; i++)
{
    cookieName = Request.Cookies[i].Name;
    aCookie = new HttpCookie(cookieName);
    aCookie.Expires = DateTime.Now.AddDays(-1);
    Response.Cookies.Add(aCookie);
}
```

### 子键的修改和删除

修改一个单独的子键与创建它是一样的

如:

```CSharp
Response.Cookies["userInfo"]["lastVisit"] = DateTime.Now.ToString();
Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(1);
```

要删除一个单独的子键，您可以操作cookie的值集合，该集合包含子键。
首先从cookie集合中获取一个指定的cookie，然后调用该cookie值集合的remove方法，参数即为要删除的子键名称，再把该cookie添加到集合中。

如下代码示例：

```CSharp
string subkeyName;
subkeyName = "userName";
HttpCookie aCookie = Request.Cookies["userInfo"];
aCookie.Values.Remove(subkeyName);
aCookie.Expires = DateTime.Now.AddDays(1);
Response.Cookies.Add(aCookie);
```

---

## Cookies安全性

cookie的安全性问题类似于从客户端获取数据。对于应用程序来说，可以把Cookie看作是另一种形式的用户输入。因为cookie是保存在用户自己的硬盘上，所以cookie对用户来说是可见的，也是可以修改的。

因为Cookie对用户是可见的所以不能在cookie中存储敏感数据，比如用户名、密码、信用卡号等等。

因为Cookie也是可修改的，所以对程序来说从Cookie获取的信息，也需要验证和判断。
不能简单的认为Cookie中的数据就和我们预期的数据一样。
<!-- 使用与cookie值相同的保护措施，您可以使用用户输入到Web页面的数据。
前面的例子显示了html编码了一个cookie的内容，然后显示了一个页面的值，就像在显示用户获得的任何信息之前一样。 -->

cookie是在浏览器和服务器之间是作为纯文本发送的，任何能够拦截您的Web流量的人都可以拦截cookie。但是可以设置cookie只有在使用安全套接字层(SSL)的情况下才会传输cookie，这样在传输时会对cookie加密。但是在用户的硬盘上，cookie就不受SSL保护了。

---

## 确定浏览器是否接受cookie

用户可以设置浏览器拒绝cookie。如果不能写入cookie，也不会抛出错误。同样，浏览器也不会向服务器发送关于当前cookie设置的任何信息。

### 客户端验证
最简单的是使用JS来判断

```javascript
  if (navigator.cookieEnabled) {
        alert("Cookie 可用");
    }
    else {
        alert("Cookie 禁用");
    }
```

### 服务端验证

*cookie属性并不表示是否启用了cookie。它只表明当前浏览器本身是否支持cookie。*



在服务端判断Cookie是否被接受的一种方法是尝试写一个Cookie，然后再试着读取该Cookie。如果不能读取所编写的cookie，那么就可以假定浏览器关闭了cookie。


<!-- 下面的代码示例展示了如何测试cookie是否被接受。这个示例由两个页面组成。第一个页面写出一个cookie，然后将浏览器重定向到第二个页面。第二个页面尝试读取cookie。它会将浏览器重定向到第一个页面，并在URL中添加一个带有测试结果的查询字符串变量。

第一个页面的代码看起来像下面的例子:

```CSharp
protected void Page_Load(object sender, EventArgs e)
{
    if (!Page.IsPostBack)
    {
        if (Request.QueryString["AcceptsCookies"] == null)
        {
            Response.Cookies["TestCookie"].Value = "ok";
            Response.Cookies["TestCookie"].Expires =
                DateTime.Now.AddMinutes(1);
            Response.Redirect("TestForCookies.aspx?redirect=" +
                Server.UrlEncode(Request.Url.ToString()));
        }
        else
        {
            Label1.Text = "Accept cookies = " +
                Server.UrlEncode(
                Request.QueryString["AcceptsCookies"]);
        }
    }
}
```

该页面首先测试这是否为postback，如果没有，则页面将查找包含测试结果的查询字符串变量名称AcceptsCookies。如果没有查询字符串变量，那么测试还没有完成，因此代码会写出一个名为TestCookie的cookie。在编写完cookie后，示例调用重定向到测试页面TestForCookies.aspx。附加到测试页面的URL是一个名为重定向的查询字符串变量，它包含当前页面的URL;这将允许您在执行测试之后重定向到这个页面。


测试页面可以完全由代码组成;它不需要包含控件。下面的例子演示了测试页面

```CSharp
protected void Page_Load(object sender, EventArgs e)
{
    string redirect = Request.QueryString["redirect"];
    string acceptsCookies;
    if(Request.Cookies["TestCookie"] ==null)
        acceptsCookies = "no";
    else
    {
        acceptsCookies = "yes";
        // Delete test cookie.
        Response.Cookies["TestCookie"].Expires = 
            DateTime.Now.AddDays(-1);
    }
    Response.Redirect(redirect + "?AcceptsCookies=" + acceptsCookies,
    true);
}
```

读取重定向查询字符串变量后，代码尝试读取cookie。出于家务的目的，如果cookie存在，它会立即被删除。当测试完成后，代码将从重定向查询字符串变量中传递给它的URL构造一个新的URL。新的URL还包括一个包含测试结果的查询字符串变量。最后一步是使用新的URL将浏览器重定向到原来的页面。
示例的改进是将cookie测试结果保存在一个持久化存储中，比如数据库，这样测试就不必再重复了。 -->

## Cookies 和 Session State

当用户导航到您的站点时，服务器将为该用户建立一个独特的会话，该用户将持续访问用户的访问时间。
对于每一个会话,ASP。
NET维护会话状态信息，其中应用程序可以存储特定于用户的信息。
要了解更多信息，请参阅ASP。
NET会话状态概述主题。

ASP。
NET必须为每个用户跟踪一个会话ID，以便它能够将用户映射到服务器上的会话状态信息。
默认情况下,ASP。
NET使用一个非持久性的cookie来存储会话状态。
但是，如果用户在浏览器上禁用了cookie，会话状态信息就不能存储在cookie中。

ASP。
NET提供了一种无cookie会话的替代方法。
您可以配置应用程序来存储会话id，而不是在cookie中，而是在站点的页面url中。
如果您的应用程序依赖于会话状态，那么您可能会考虑将其配置为使用无cookie会话。
但是，在某些有限的情况下，如果用户与其他人共享URL，或者在用户会话仍然活跃的情况下将URL发送给同事，那么这两个用户都可以共享同一个会话，结果不可预测。
有关如何配置应用程序以使用无cookie会话的更多信息，请参阅ASP。
净状态管理概述主题。


---

* [MSDN-Cookie](https://msdn.microsoft.com/en-us/library/ms178194.aspx)