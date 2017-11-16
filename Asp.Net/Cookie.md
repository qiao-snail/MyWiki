# Asp.Net MVC 中的 Cookie（译）

## Cookie

* Cookie是请求服务器或访问Web页面时携带的一个小的文本信息。

Cookie为Web应用程序中提供了一种存储特定用户信息的方法。Cookie的值是字符串类型，且对用户是可见的。

* Cookie随着每次`Request`和`Response`在浏览器和服务器之间交换数据。

如果一个用户请求服务器上的一个页面，服务器除了返回请求的页面,也返回了一个包含日期和时间的Cookie。这个Cookie存储在用户硬盘上的一个文件夹上。稍后，如果用户再次访问服务器，当用户输入URL时，浏览器会在本地硬盘上查看与该URL相关联的Cookie。如果Cookie存在，浏览器会将Cookie随着请求一起发送。然后，服务器可以读取发送过来的Cookie信息，用户上次访问该站点的日期和时间。您可以使用这些信息向用户显示一条消息，或者检查一个过期日期。

* Cookie与特定的站点关联

Cookie与一个Web站点相关联，而不是特定的页面，因此不管用户请求你服务器的什么页面，浏览器和服务器都会交换cookie信息。 浏览器会为每个不同的Web站点分别存储Cookie，保证每个Cookie对应特定的Web站点。

* Cookie保持会话状态

Cookie可以帮助服务器存储访问者的信息。通俗的说，Cookie是保持Web应用程序连续性的一种方式，即会话状态管理。因为Http请求是无状态的，一些列请求中服务器并不知道请求来自哪些用户，所以可以使用Cookie来唯一标识用户，维护会话状态。


Cookie用于许多目的，所有这些都与帮助网站记住用户有关。例如，一个进行投票的站点可能会使用Cookie作为布尔值来指示用户的浏览器是否已经参与投票，这样用户就不能进行两次投票。一个要求用户登录的网站可能会使用Cookie来记录用户已经登录的情况，这样用户就不必继续输入凭证了。

---

## Cookie限制

大多数浏览器支持最多4096字节的Cookie。由于这个限制，cookie最好用于存储少量数据，或者更好的是，像用户ID这样的标识符。

浏览器也会限制Web网站在用户的电脑上可以存储多少Cookie。大多数浏览器只允许每个站点提供20个Cookie。如果你想储存更多，那么之前老的Cookie就会被丢弃。一些浏览器还会限制Cookie的总数量（不区分站点）。

尽管Cookie对Web应用程序非常有用，但是应用程序不应该依赖于Cookie。不要使用Cookie来支持关键敏感数据。

---

## 写Cookie

浏览器负责管理用户电脑上的Cookie。Cookie通过`HttpResonse`对象被发送到浏览器，该对象公开了一个名为Cookies的集合。想要发送到浏览器的任何Cookie都必须添加到这个集合中。在创建Cookie时，指定一个名称和值。每个Cookie都必须有一个惟一的名称，以便浏览器读取时可以识别它。因为Cookie是按名称存储的，因此多个Cookie名称相同时，新的Cookie值会覆盖掉之前的。

可以设置Cookie的日期和过期时间。当用户访问写Cookie的站点时，浏览器会删除过期的Cookie。Cookie的有效期可以是50年。

如果没有设置Cookie的过期时间，就创建了Cookie，那么它并不会存储在用户的硬盘上。这种Cookie是作为用户会话信息的一部分来维护的。当用户关闭浏览器时，cookie就会被丢弃。像这样的非持久cookie对于需要在短时间内存储的信息非常有用，或者出于安全原因，不应该将其写入到客户机计算机上的磁盘上。例如，如果用户正在使用公共计算机，而不希望将cookie写入磁盘，则非持久性cookie非常有用。

您可以通过多种方式将Cookie添加到Cookie集合中。下面的例子展示了两个编写Cookie的方法:

```CSharp
//第一种
Response.Cookies["userName"].Value = "patrick";
Response.Cookies["userName"].Expires = DateTime.Now.AddDays(1);
//第二种
HttpCookie aCookie = new HttpCookie("lastVisit");
aCookie.Value = DateTime.Now.ToString();
aCookie.Expires = DateTime.Now.AddDays(1);
Response.Cookies.Add(aCookie);
```

该示例将两个Cookie添加到Cookie集合中，其中一个名为用户名，另一个为上次访问时间。对于第一种方法，Cookie集合的值可以直接读写。因为Cookie继承自`NameObjectCollectionBase`集合类型，所以可以直接获取Cookie。

对于第二种方法，创建一个HttpCookie对象的实例，设置它的属性，然后通过Add方法将其添加到cookie集合中。Cookie的名称通过构造函数添加。

这两个示例都完成了相同的任务，将Cookie写入浏览器。在这两种方法中，过期值必须为DateTime类型。因为所有的Cookie值都以字符串形式存储，所以日期时间值会转换为字符串。

### 多个值的Cookie

Cookie可以存储一个值，比如用户名或最后一次访问。也可以在一个Cookie中存储多个键值对。键值对被称为子键。(子键的布局非常类似于URL中的查询字符串。)例如，您可以创建一个名为userInfo的cookie，它具有子健userName和lastVisit，而不是创建两个单独的Cookie。

子健可以将相关的或类似的信息放入一个Cookie中。如果所有信息都在一个Cookie中，Cookie属性如过期将应用于所有信息。(相反，如果您想为不同类型的信息分配不同的过期日期，则应该将信息存储在单独的cookie中。)

带有子键的Cookie也可以帮助限制Cookie文件的大小。之前提到，Cookie通常被限制为4096字节，并且每个站点能存储20多个Cookie。通过使用一个带有子键的Cookie，可以降低站点Cookie数量的限制。此外，一个Cookie自身就占用了大约50个字符(过期信息，等等)，加上您存储在其中的值的长度，所有这些值都指向4096字节的限制。如果您存储5个子键而不是5个单独的cookie，您可以节省单个Cookie的开销，并且还可以节省大约200个字节。

创建有子键的Cookie：

```Csharp
//第一种
Response.Cookies["userInfo"]["userName"] = "patrick";
Response.Cookies["userInfo"]["lastVisit"] = DateTime.Now.ToString();
Response.Cookies["userInfo"].Expires = DateTime.Now.AddDays(1);
//第二种
HttpCookie aCookie = new HttpCookie("userInfo");
aCookie.Values["userName"] = "patrick";
aCookie.Values["lastVisit"] = DateTime.Now.ToString();
aCookie.Expires = DateTime.Now.AddDays(1);
Response.Cookies.Add(aCookie);
```

---

## Cookie作用域

默认情况下，一个站点的有过期时间的所有Cookie都存储在客户硬盘上，请求该网站时所有的Cookie都被发送到服务器。换句话说，网站上的每一个页面都能获得该站点的所有Cookie。但是，您可以通过两种方式来限制Cookie的范围:

* 将Cookie的范围限制在服务器上的一个文件夹中，这允许您将Cookie限制在站点上的一个应用程序中。

* 将范围设置为域，允许您指定域中的哪些子域可以访问cookie。

要将cookie限制在服务器上的一个文件夹中，可以设置cookie的路径属性，如下面的例子:

```CSharp
HttpCookie appCookie = new HttpCookie("AppCookie");
appCookie.Value = "written " + DateTime.Now.ToString();
appCookie.Expires = DateTime.Now.AddDays(1);
appCookie.Path = "/Application1";
Response.Cookies.Add(appCookie);
```

路径既可以是站点根下的物理路径，也可以是虚拟路径。这样做的效果是，Cookie只可用于Application1文件夹或虚拟根目录中的页面。例如,如果你的网站是www.contoso.com,在前面的示例中创建的Cookie可以与路径`http://www.contoso.com/Application1/`页面,该文件夹下任何页面。然而,Cookie不会在其他应用程序中可用的页面,比如`http://www.contoso.com/Application2/`或者`http://www.contoso.com/`。

### Cookie的限制和作用域

默认情况下，Cookie与特定的域相关联。
例如，如果您的站点是www.contoso.com，那么当用户请求该站点的任何页面时，您所写的Cookie将被发送到服务器。
(这可能不包括带有特定路径值的cookie。)
如果你的站点有子域名，例如，contoso.com，sales.contoso.com，以及support.contoso.com，那么你就可以把Cookies与特定的子域名联系起来。

代码示例：
```CSharp
Response.Cookies["domain"].Value = DateTime.Now.ToString();
Response.Cookies["domain"].Expires = DateTime.Now.AddDays(1);
Response.Cookies["domain"].Domain = "support.contoso.com";
```

当域以这种方式设置时，Cookie将只在指定子域中的页面可用。您还可以使用域属性来创建一个可以在多个子域中共享的Cookie，如下面的示例所示:

```CSharp
Response.Cookies["domain"].Value = DateTime.Now.ToString();
Response.Cookies["domain"].Expires = DateTime.Now.AddDays(1);
Response.Cookies["domain"].Domain = "contoso.com";
```

此时Cookie既可以主域，也可以用于sales.contoso.com和support.contoso.com等子域。

---

## 读取Cookie

当浏览器向服务器发出请求时，Cookie会随着请求一起发送到服务器上。在你的应用程序，您可以使用HttpRequest来读取cookie。

代码示例：

```Csharp
if(Request.Cookies["userName"] != null)
{
    HttpCookie aCookie = Request.Cookies["userName"];
    var cookiText = Server.HtmlEncode(aCookie.Value);
}
```

在尝试获取Cookie的值之前，应该确保Cookie存在;如果Cookie不存在，将得到NullReferenceException异常。还要注意，HtmlEncode方法被调用来对Cookie的内容进行编码。这确保了恶意用户没有将可执行脚本添加到Cookie中。


在Cookie中读取子键的值也类似于设置它。下面的代码示例展示了获取子键值的一种方法

```CSharp
if(Request.Cookies["userInfo"] != null)
{
    var name = 
        Server.HtmlEncode(Request.Cookies["userInfo"]["userName"]);

    var visit =
        Server.HtmlEncode(Request.Cookies["userInfo"]["lastVisit"]);
}
```

读取键值对lastVisit的值，该值之前被设置为DateTime类型。但是Cookie将值存储为字符串，因此如果您想使用lastVisit的值作为日期，则必须将其转换为适当的类型。

如

```Csharp
DateTime dt= DateTime.Parse(Request.Cookies["userInfo"]["lastVisit"]);
```

Cookie中的子键是NameValueCollection类型。因此，获取子键的另一种方法是获取子键集合，然后按名称提取子键值，如下面的示例所示:


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

### Cookie的过期时间

浏览器负责管理Cookie，Cookie的过期时间和日期帮助浏览器管理其存储的Cookie。因此，虽然您可以读取Cookie的名称和值，但是您不能读取cookie的过期日期和时间。当浏览器向服务器发送Cookie信息时，浏览器不包含过期信息。(Cookie的过期属性总是返回一个日期时间值为0。)如果您担心cookie的过期日期，您必须重新设置它。


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

上一个示例的有一个不足的地方，如果cookie有子键，会将子键作为单个name/value字符串显示。您可以读取cookie的haskey属性，以确定cookie是否有子键。如果有，您可以读取子键集合以获得单独的子键名和值。
您也可以通过索引值直接从值集合中读取子键值。相应的子键名可以在值集合的AllKeys成员中找到，它返回一个字符串数组。您还可以使用值集合的键值。但是，AllKeys属性在第一次被访问时被缓存。相反，键属性在每次访问时都构建一个数组。由于这个原因，在相同页面请求的上下文中，AllKeys属性要快得多。

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

### 修改Cookie

不能直接在获取到Cookie后就修改Cookie的值,因为Cookie存储在用户的硬盘上（Request.Cookies["Key"].Value="some"，在这里是不起作用的)，必须在Response.Cookies["Key"].Value="SomeNew"中修改才可以。其实就是服务器中设置的Cookie新值，覆盖了用户浏览地上的Cookie的旧值。

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

### 删除Cookie

将Cookie从用户的硬磁盘上删除，和修改Cookie原理是一样的。不能直接删除Cookie，因为Cookie在用户的硬盘上。但是，可以创建一个新Cookie，其名称与要删除的Cookie相同，但要将Cookie的过期时间设置为比今天更早的日期。当浏览器检查Cookie的过期时间，浏览器将丢弃已经过时的Cookie。

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

要删除一个**单独的子键**，您可以操作Cookie的值集合，该集合包含子键。
首先从Cookie集合中获取一个指定的Cookie，然后调用该cookie值集合的remove方法，参数即为要删除的子键名称，再把该Cookie添加到集合中。

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

Cookie的安全性问题类似于从客户端获取数据。对于应用程序来说，可以把Cookie看作是另一种形式的用户输入。因为Cookie是保存在用户自己的硬盘上，所以Cookie对用户来说是可见的，也是可以修改的。

因为Cookie对用户是可见的所以不能在Cookie中存储敏感数据，比如用户名、密码、信用卡号等等。

因为Cookie也是可修改的，所以对程序来说从Cookie获取的信息，也需要验证和判断。不能简单的认为Cookie中的数据就和我们预期的数据一样。
<!-- 使用与cookie值相同的保护措施，您可以使用用户输入到Web页面的数据。
前面的例子显示了html编码了一个cookie的内容，然后显示了一个页面的值，就像在显示用户获得的任何信息之前一样。 -->

Cookie是在浏览器和服务器之间是作为纯文本发送的，任何能够拦截Web流量的人都可以拦截Cookie。但是可以设置Cookie只有在使用安全套接字层(SSL)的情况下才会传输Cookie，这样在传输时会对Cookie加密。但是在用户的硬盘上，Cookie就不受SSL保护了。

---

## 确定浏览器是否接受cookie

用户可以设置浏览器拒绝Cookie。如果不能写入Cookie，也不会抛出错误。同样，浏览器也不会向服务器发送关于当前Cookie设置的任何信息。

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



在服务端判断Cookie是否被接受的一种方法是尝试写一个Cookie，然后再试着读取该Cookie。如果不能读取所编写的Cookie，那么就可以假定浏览器关闭了Cookie。


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

---

* [MSDN-Cookie](https://msdn.microsoft.com/en-us/library/ms178194.aspx)