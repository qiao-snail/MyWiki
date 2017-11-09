# Asp.Net MVC 使用 Ajax

## Ajax

简单来说Ajax是一个无需重新加载整个网页的情况下，可以局部更新的技术（异步的发送接收数据，不会干扰当前页面）。

### Ajax工作原理

Ajax使浏览器和服务器之间多了一个Ajax引擎作为中间层。通过Ajax请求服务器时，Ajax会自行判断哪些数据是需要提交到服务器，哪些不需要。只有确定需要从服务器读取新数据时，Ajax引擎才会向服务器提交请求。

![Ajax原理](imgs/ajax-vergleich-en.svg)

### Ajax几个特点

1. 不需要刷新更新页面或数据。
2. 与服务器异步通信。
3. Ajax请求不能后退，浏览器没有历史记录。 
4. Ajax请求的页面不能加入到收藏夹。

### Jquery中的Ajax

JQuery 对 Ajax 做了大量的封装，不需要去考虑浏览器兼容性，使用起来也较为方便。

jquery对Ajax一共有三层封装。

* 最底层：$.ajax()。

* 第二层：.load()、$.get()和$.post()。

* 最高层： $.getScript()和$.getJSON()方法。

---

### $.Ajax()

$.Ajax()是所有Ajax方法中最底层的方法，其他的都是基于$.Ajax()方法的封装，该方法只有一个参数-`JQueryAjaxSettings`（功能键值对）。

代码示例：

```javascript
$('button').click(function(){
    $.ajax(
        {
            type:'post',
            url:'test.cshtml',
            data:{
                url:'hello',
            },
            dataType:'json',
            success:function(data,stutas,xhr){
                alert(data);
            },
            error:function(xhr, textStatus, data)){
                alert(data);
            }，
            complete:function(xhr,textStatus){
                alert(textStatus);
            }

        }
    )
});
```

`$.Ajax`参数`JQueryAjaxSettings`介绍：

|参数|类型|说明|
|-----|--------|-----|
|url|String|请求的地址|
|type|String|请求方式：POST 或 GET，默认 GET|
|timeout|Number|设置请求超时的时间（毫秒）|
|data|Object或String|发送到服务器的数据，键值对或字符串|
|dataType|String|从服务器返回的数据类型，比如 html、xml、json 等|
|beforeSend|Function|发送请求前可修改 XMLHttpRequest 对象的函数|
|complete|Function|请求完成后调用的回调函数|
|success|Function|请求成功后调用的回调函数,先执行success再执行complete|
|error|Function|请求失败时调用的回调函数，先执行error再执行complete|
|global|Boolean|默认为 true，表示是否触发全局 Ajax|
|cache|Boolean|设置浏览器缓存响应，默认为 true。如果 dataType类型为 script 或 jsonp 则为 false。|
|content|DOM|指定某个元素为与这个请求相关的所有回调函数的上下文。|
|contentType|String|指 定 请 求 内 容 的 类 型 。 默 认 为application/x-www-form-urlencoded。|
|async|Boolean|是否异步处理。默认为 true，false 为同步处理|
|processData|Boolean|默认为 true，数据被处理为 URL 编码格式。如果为 false，则阻止将传入的数据处理为 URL 编码的格式。|
|dataFilter|Function|用来筛选响应数据的回调函数。|
|ifModified|Boolean|默认为 false，不进行头检测。如果为true，进行头检测，当相应内容与上次请求改变时，请求被认为是成功的。|
|jsonp|String|指定一个查询参数名称来覆盖默认的 jsonp 回调参数名 callback。|
|username|String|在 HTTP 认证请求中使用的用户名|
|password|String|在 HTTP 认证请求中使用的密码|
|scriptCharset|String|当远程和本地内容使用不同的字符集时，用来设置 script 和 jsonp 请求所使用的字符集。|
|xhr|Function|用来提供 XHR 实例自定义实现的回调函数|
|traditional|Boolean|默认为 false，不使用传统风格的参数序列化。如为 true，则使用|


$.Ajax的回调函数介绍：

* success

Function( Anything data, String textStatus, jqXHR jqXHR )
请求成功后执行的回调函数，它将在函数处理完之后，并且 HTML 已经被插入完时被调用。回调函数会在每个匹配的元素上被调用一次，并且 this始终指向当前正在处理的 DOM 元素。 
|参数|类型|说明|
|---|---|---|
|data|anything|从服务器返回的数据，并根据dataType参数进线处理后的数据|
|textStatus|string|描述状态的字符串|
|jqxhr|jqXHR|XMLHTTPRequest对象|

* error

Function( jqXHR jqXHR, String textStatus, String errorThrown )
请求失败是执行的回调函数
|参数|类型|说明|
|---|---|---|
|errorThrown|string|HTTP状态的文本部分|
|textStatus|string|描述错误信息的字符串|
|jqxhr|jqXHR|描述发生错误类型的一个字符串 和 捕获的异常对象|

* complete

Function( jqXHR jqXHR, String textStatus )
请求完成后执行的回调函数，不管是成功还是失败都执行。
|参数|类型|说明|
|---|---|---|
|errorThrown|string|HTTP状态的文本部分|
|textStatus|string|描述请求状态的字符串|
|jqxhr|jqXHR|XMLHTTPRequest对象|

---

### $.load()

从服务器获取数据并且将返回的HTML代码并插入至匹配的元素中。

```javascript
jqueryElement.load(url,data,success(responseText,textStatus,XMLHttpRequest))
```

|参数|类型|说明|
|----------|----------|----------|
| url |string|必须 请求地址|
| data |Json或者string|可选 请求数据 如果是json该load方法是post请求，默认是get请求|
| success |function|当请求成功后执行的回调函数|
| responseText |string|获得字符串形式的响应数据|
| textStatus |string|文本方式返回HTTP状态码|
| XMLHttpRequest |Object|xhr对象，有多种属性|

---

### $.get()和$.post()

`.load()`一般在获取静态资源时调用，`$.get()`和`$.post()`方法在需要和服务器交互数据时调用。

`$.get()` 方法通过远程 HTTP GET 请求载入信息。
这是一个简单的 GET 请求功能以取代复杂 `$.ajax` 。请求成功时可调用回调函数。如果需要在出错时执行函数，请使用 `$.ajax`。

```javascript
$(selector).get(url,data,success(response,status,xhr),dataType)

```

`$.post()` 方法通过 HTTP POST 请求从服务器载入数据。

```javascript
jQuery.post(url,data,success(data, textStatus, jqXHR),dataType)
```
`$.get()` `$.post()`方法都是四个参数，前面三个参数和`$.load()`一样，最后一个参数dataType:服务器返回的数据格式：xml、html、script、json、jsonp和text。

`$.get()` `$.post()`都是`$.ajax()`的一个简写封装，都是只能回调success状态，error，和complete不能被回调。但是在jquery1.5版本上，新加了`jqXHR.done()` (表示成功), `jqXHR.fail()` (表示错误), 和 `jqXHR.always()` (表示完成, 无论是成功或错误；在jQuery 1.6 中添加) 事件，可以实现不同状态的回调。

```javascript
$.get('send-ajax-data.php')
    .done(function(data) {
        alert(data);
    })
    .fail(function(data) {
        alert('Error: ' + data);
    })
    .always(function(data){
        alert(data);
    })
    ;
```
---

### 表单序列化

如果我们有一个复杂的表单，一个一个获取表单数据是一个很琐碎的事。jquery提供了一个表单的序列化方法`serialize()`，会智能的获取指定表单内的所有元素（包括单选框，复选框，下拉列表等）把表单内容序列化为字符串。此外`serializeArray()`方法可以把数据整合为键值对的json对象。

*如果我们需要多次调用`$.ajax`方法，并且很多参数都相同，可以使用`$.ajaxSetup()`方法，它会把一些公共的参数预先设置好，不用每次都设置。*

```javascript
$('form input[type=button]').click(function () {
    $.ajaxSetup({
        type : 'POST',
        url : 'test',
        data : $('form').serialize()
    });
    $.ajax({
        success : function (response, status, xhr) {
                    alert(response);
    });
    });
```

在使用 `data` 属性传递数据的时候，如果是以对象形式传递键值对，可以使用`$.param()`方法将对象转换为字符串键值对格式。
(*主要是针对无法直接使用表单序列化方法`.serialize()`，且传递参数为对象的情况*)

---

## MVC中的Ajax使用

Asp.Net MVC中包含了一组Ajax辅助方法。可以用来创建表单和指向控制器操作的链接，不同的是他们时异步进行的。当使用这个辅助方法时，不用编写任何脚本代码来实现程序的异步。该辅助方法依赖于**非侵入式mvc的jquery扩展**。如果使用这些辅助方法时，需要引入脚本`jquery.unbotrusive-ajax.js`

### 分部渲染

Asp.Net MVC中的分部页面可以是`partialPage`也可以是含有布局（`layout`）的完整页面。只是在return的时候返回类型是`PartialView`。

不管是分部页面还是完整页面，绝大部分情况下，部分页面的请求和完整页面的请求是一样的流程-*请求被路由到指定控制器，控制器执行特定的业务逻辑，返回给对应的试图。* 但是我们可以在控制器中使用`Request.IsAjax`来区别是否是ajax请求,是否是要返回分部试图，还是完整试图。分部试图（return PartialView)是render和返回了该页面的html。但是完整试图（return View)是返回了包括页面资源（css，js）和布局的所有html。

### Ajax.Load()

异步加载一个分布页面

* 定义一个ViewModel

```CSharp
//Model
[Bind(Exclude = "PersonID")]
public class PersonViewModel
{
    [ScaffoldColumn(false)]
    public int PersonID { get; set; }

    [Display(Name = "姓名")]
    [Required(ErrorMessage = "不能为空")]
    public string Name { get; set; }

    [Display(Name = "手机号")]
    [Required(ErrorMessage = "不能为空")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNum { get; set; }
}
```

* 定义主页面View

```CSharp
//Main View:
@{
    ViewBag.Title = "主页面";
}
<h2>主页面</h2>
<p>列表详细信息</p>
<div id="partialDiv"></div>
<script>
    $('#partialDiv').load('@Url.Action("ListPage", "MyAjax")')
</script>
```

定义分部页面View

```CSharp
//分部页面
@{
    ViewBag.Title = "ListPage";
}
@model IList<WebApp.Models.PersonViewModel>
<h2>分布页</h2>
<table class="table table-striped">
    <thead>
        @{ WebApp.Models.PersonViewModel p = null;}
        <tr>
            <th>@Html.LabelFor(m => @p.Email)</th>
            <th>@Html.LabelFor(m => @p.Name)</th>
            <th>@Html.LabelFor(m => @p.Home)</th>
            <th>@Html.LabelFor(m => @p.IsMarried)</th>
            <th>@Html.LabelFor(m => @p.Height)</th>
            <th>@Html.LabelFor(m => @p.PhoneNum)</th>
            @*也可以使用DisplayNameFor来显示表头*@
            @*<th>@Html.DisplayNameFor(m => Model[0].Email)</th>
            <th>@Html.DisplayNameFor(m => Model[0].Name)</th>
            <th>@Html.DisplayNameFor(m => Model[0].IsMarried)</th>
            <th>@Html.DisplayNameFor(m => Model[0].Height)</th>
            <th>@Html.DisplayNameFor(m => Model[0].PhoneNum)</th>*@
        </tr>

    </thead>
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>@item.Email</td>
                <td>@item.Name</td>
                <td>@item.Home</td>
                <td>@item.IsMarried</td>
                <td>@item.Height</td>
                <td>@item.PhoneNum</td>
            </tr>
        }
    </tbody>
</table>
```

定义一个Action

```CSharp
//Controller
 public class MyAjaxController : Controller
 {
    //主页面
    public ActionResult MainPage()
    {

        return View();
    }
    //分部页面
    public ActionResult ListPage()
    {
        IList<PersonViewModel> persons = new List<PersonViewModel>();
        for (int i = 0; i < 10; i++)
        {
            persons.Add(new PersonViewModel() { Email = "email" + i, Name = "name", IsMarried = false, PhoneNum = "1234" + i, Home = CityEnum.BJ, Height = i });
        }
        if (Request.IsAjaxRequest())
        {
            return PartialView(persons);
        }

        return View(persons);
    }
 }
```

当请求主页面的时候，会把分布页面异步加载到主页面的`<div id="partialDiv"></div>`里

---

### Ajax.ActionLink()

Ajax.ActionLink()辅助方法，可以异步请求加载页面。

```CSharp
//Main view 主页面
@{
    ViewBag.Title = "MainPage";
}
<h2>主页面</h2>
<p>列表详细信息</p>
@Ajax.ActionLink("加载详细列表", "ListPage", new AjaxOptions { UpdateTargetId = "partialDiv", InsertionMode = InsertionMode.Replace, HttpMethod = "Get" })
<div id="partialDiv"></div>
```

Asp.Net MVC 提供了多个`AjaxOptions`的属性，方法给我们使用，免去了不少js代码。

如
|名称|说明|
|----|----|
|Confirm|  获取或设置在提交请求之前显示在确认窗口中的消息。|
|HttpMethod| 获取或设置 HTTP 请求方法（“Get”或“Post”）。 |
|InsertionModel|获取或设置指定如何将响应插入目标 DOM 元素的模式。插入模式（“InsertAfter”、“InsertBefore”或“Replace”）。 默认值为“Replace”。   |
|LoadingElementDuration|获取或设置一个值（以毫秒为单位），该值控制在显示或隐藏加载元素时的动画持续时间。  |
|LoadingElementId| 获取或设置在加载 Ajax 函数时要显示的 HTML 元素的 id 特性。 |
|OnBegin| 获取或设置要在更新页面之前立即调用的 JavaScript 函数的名称 |
|OnComplete|获取或设置在实例化响应数据之后但在更新页面之前，要调用的 JavaScript 函数。  |
|OnFailure| 获取或设置在页面更新失败时要调用的 JavaScript 函数。 |
|OnSuccess|  获取或设置在成功更新页面之后要调用的 JavaScript 函数。|
|UpdateTargetId| 获取或设置要使用服务器响应来更新的 DOM 元素的 ID。 |
|Url|  获取或设置要向其发送请求的 URL。|

---

### Ajax表单提交

当我们使用jquery的ajax提交表单时，需要在click事件中添加`e.preventDefault()`或者把`<input type="submit" value="提交" />`改为`<input type="button" value="提交" />`。否则会刷新页面。如下代码所示，

```CSharp
<form class="form-horizontal" role="form" method="post" id="myform">
    <div>
        <label for="i1">第一</label>
        <input type="text" name="i1" id="i1" />
    </div>
    <div>
        <label for="i2">第二</label>
        <input type="text" name="i2" id="i2" />
    </div>
    <div>
        <label for="i3">第三</label>
        <input type="text" name="i3" id="i3" />
    </div>
    //或者使用<input type="button" value="提交" />，不必再阻止事件的传递了。
    <input type="submit" value="提交" />
</form>
<script>
   $("input[type=submit]").click(function (e) {
        e.preventDefault();//阻止事件传递
        $.post("@Url.Action("CheckNameByAjax")", $("#myform").serialize(), function (result) {
            alert(result);
        });
    });
</script>
```

Asp.Net MVC提供了Ajax的表单辅助方法，可以更简单快速的实现表单的ajax提交。

```CSharp
@using (Ajax.BeginForm("AjaxForm", "MyAjax", new AjaxOptions { HttpMethod = "Post", OnComplete = "foo", OnSuccess = "succ", OnFailure = "fail" }, new { role = "form" }))
{
    <div>
        <label for="i1">第一</label>
        <input type="text" name="i1" id="i1" />
    </div>
    <div>
        <label for="i2">第二</label>
        <input type="text" name="i2" id="i2" />
    </div>
    <div>
        <label for="i3">第三</label>
        <input type="text" name="i3" id="i3" />
    </div>
    <input type="submit" value="提交" />
}
```
---

### Ajax数据验证

在注册有时需要保证用户名或者邮箱唯一或者是否合法，这个验证又必须放在服务端完成。可以使用ajax异步请求，在用户添加完用户名或者邮箱的时候立即在服务端验证并告知用户结果，而不用填完整个表单，再去验证唯一合法性。

* 定义一个ViewModel

```CSharp
//Model
[Bind(Exclude = "PersonID")]
public class PersonViewModel
{
    [ScaffoldColumn(false)]
    public int PersonID { get; set; }

    [Display(Name = "姓名")]
    [Required(ErrorMessage = "不能为空")]
    public string Name { get; set; }

    [Display(Name = "手机号")]
    [Required(ErrorMessage = "不能为空")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNum { get; set; }
}
```

* 定义试图View

```CSharp
//view
@model NameSpace.PersonViewModel
<form class="form-horizontal" role="form" method="post" id="myform">
    <div>
        <div class="form-group">
            @Html.LabelFor(m => m.Name, new { @class = "control-label col-md-3" })
            <div class="col-md-9">
                @Html.TextBoxFor(m => m.Name, new { @class = "form-control" })
                @Html.ValidationMessageFor(m => m.Name, "", new { @class = "text-danger" })
            </div>
        </div>
        <div>
            <input type="submit" value="提交" class="btn btn-success" id="sure" />
        </div>
    </div>
</form>
<script>
    $("#Name").change(function () {
        $.ajax({
            url: "@Url.Action("CheckUserName")",
            type: "post",
            data: { Name: $("#Name").val() },
            dataType: "JSON",
            success: function (response, stutas, xhr) {
                alert(response+status + xhr.statusText);
            },
            error: function (xhr, stutas, response) {
                alert(response + status + xhr.statusText);
            },
            complete: function (data) {
                alert(data.status+data);
            },
        });
    });
    </script>
```

* 定义一个Action校验用户名的唯一和合法性

```CSharp
[HttpPost]
//参数一定要和ViewModel的属性名称一致
public JsonResult CheckUserName(string Name)
{
    bool result = true;
    if (Name == "admin")
    {
        result = false;
    }
    return Json(result);
}
```

至此我们实现了Ajax的用户名唯一性和合法性的校验。**但是** Asp.Net MVC 提供了一个更简单的方法，可以用更少的代码实现一样的功能

* 在属性上添加`[Remote("CheckUserName", "DataValidation")]`特性

该特性允许客户端调用服务端的方法。

修改Model

```CSharp
   [Bind(Exclude = "PersonID")]
    public class PersonViewModel
    {
        [ScaffoldColumn(false)]
        public int PersonID { get; set; }

        [Display(Name = "姓名")]
        //添加Remote特性
        [Remote("CheckUserName", "ControllerName",ErrorMessage="用户名已存在")]
        [Required(ErrorMessage = "不能为空")]
        public string Name { get; set; }

        [Display(Name = "手机号")]
        [Required(ErrorMessage = "不能为空")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNum { get; set; }
    }
```

我们只需添加一个Remote特性就可以实现用户名的服务端验证。节省了js的代码。

* 修改Action

Asp.Net MVC默认是不允许Get请求Json（防止Json被劫持）。所以如果你需要Get请求Json。必须添加`JsonRequestBehavior.AllowGet`。且该数据不那么重要。

```CSharp
//Action
public JsonResult CheckUserName(string Name)
{
    //参数一定要和属性名称一致
    bool result = true;
    if (Name == "admin")
    {
        result = false;
    }
    //添加JsonRequestBehavior.AllowGet
    return Json(result, JsonRequestBehavior.AllowGet);
}
```
---
