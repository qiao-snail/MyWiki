# Asp.Net MVC 使用 Ajax

## Ajax

简单来说Ajax是一个无需重新加载整个网页的情况下，可以局部更新的技术（异步的发送接收数据，不会干扰当前页面）。

### Ajax工作原理

Ajax使浏览器和服务器之间多了一个Ajax引擎作为中间层。通过Ajax请求服务器时，Ajax会自行判断哪些数据是需要提交到服务器，哪些不需要。只有确定需要从服务器读取新数据时，Ajax引擎才会向服务器提交请求。

![Ajax原理](imgs/ajax-vergleich-en.svg)

### Ajax

1. 不需要刷新更新页面或数据。

2. 与服务器异步通信。

3. Ajax请求不能后退，浏览器没有历史记录。
 
4. Ajax请求的页面不能加入到收藏夹。


### Jquery中的Ajax

jQuery 对 Ajax 做了大量的封装，我们使用起来也较为方便，不需要去考虑浏览器兼容性。

jquery对Ajax一共有三层封装。

最底层：$.ajax()。

第二层：.load()、$.get()和$.post()。

最高层： $.getScript()和$.getJSON()方法。

**$.Ajax()**

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

|参数|类型|说明|
|-----|----|-----|
|url|String|请求的地址|
|type|String|请求方式：POST 或 GET，默认 GET|
|timeout|Number|设置请求超时的时间（毫秒）|
|data|Object 或String|发送到服务器的数据，键值对字符串或对象|
|dataType|String|从服务器返回你期望的数据类型，比如 html、xml、json 等|
|beforeSend|Function|发送请求前可修改 XMLHttpRequest 对象的函数|
|complete|Function|请求完成后调用的回调函数|
|success|Function|请求成功后调用的回调函数|
|error|Function|请求失败时调用的回调函数请求失败时调用此函数|
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


回调函数：

success
Function( Anything data, String textStatus, jqXHR jqXHR )
请求成功后执行的回调函数
|参数|类型|说明|
|---|---|---|
|data|anything|从服务器返回的数据，并根据dataType参数进线处理后的数据|
|textStatus|string|描述状态的字符串|
|jqxhr|jqXHR|XMLHTTPRequest对象|

error
Function( jqXHR jqXHR, String textStatus, String errorThrown )
请求失败是执行的回调函数
|参数|类型|说明|
|---|---|---|
|errorThrown|string|HTTP状态的文本部分|
|textStatus|string|描述错误信息的字符串|
|jqxhr|jqXHR|描述发生错误类型的一个字符串 和 捕获的异常对象|

complete
Function( jqXHR jqXHR, String textStatus )
请求完成后执行的回调函数，不管是成功还是失败都执行。
|参数|类型|说明|
|---|---|---|
|errorThrown|string|HTTP状态的文本部分|
|textStatus|string|描述请求状态的字符串|
|jqxhr|jqXHR|XMLHTTPRequest对象|


从jQuery 1.5开始，所有jQuery的Ajax方法都返回一个XMLHTTPRequest对象的超集。返回的jQuery XHR对象，或“jqXHR，”实现了 Promise 接口，使它拥有 Promise 的所有属性，方法和行为。如：jqXHR.done() (表示成功), jqXHR.fail() (表示错误), 和 jqXHR.always() (表示完成, 无论是成功或错误；在jQuery 1.6 中添加) 方法接受一个函数参数，用来请求终止时被调用。

### .load()

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

* 如果选择器没有匹配的元素——在这种情况下，如果document不包含id = "result" 的元素- 这个Ajax请求将不会被发送出去。

* Callback Function
如果提供了 "success" 回调函数，它将在函数处理完之后，并且 HTML 已经被插入完时被调用。回调函数会在每个匹配的元素上被调用一次，并且 this始终指向当前正在处理的 DOM 元素。 

* Request Method
默认使用 GET 方式 ， 如果data参数提供一个对象，那么使用 POST 方式。

**$.get()和$.post()**

.load()方法适合做静态文件的异步获取，$.get()和$.post()方法适合需要提交数据到服务器。

$.get()使用一个Http GET请求从服务器加载数据。

该方法有四个参数，前面三个参数和.load()一样，多了一个第四参数type，即服务器返回的内容格式：包括xml、html、script、json、jsonp和text。第一个参数为必选参数，后面三个为可选参数

或者使用ajax其他事件来get请求

```javascript
$.get('send-ajax-data.php')
    .done(function(data) {
        alert(data);
    })
    .fail(function(data) {
        alert('Error: ' + data);
    });
```

### 表单序列化

使用表单序列化方法.serialize()，会智能的获取指定表单内的所有元素。这样，在面对大量表单元素时，会把表单元素内容序列化为字符串，然后再使用Ajax请求。
序列化表单内的元素：data : $('form').serialize()，其余部分相同。
除此之外还可以直接获取单选框、复选框和下拉列表框等内容
除了.serialize()方法，还有一个可以返回 JSON 数据的方法：.serializeArray()。这个方法可以直接把数据整合成键值对的 JSON 对象。
使用方法相同$('form').serializeArray().
有时，我们可能会在同一个程序中多次调用$.ajax()方法。而它们很多参数都相同，这
个时候我们课时使用 jQuery 提供的$.ajaxSetup()请求默认值来初始化参数。

```javascript
$('form input[type=button]').click(function () {
    $.ajaxSetup({
        type : 'POST',
        url : 'test.php',
        data : $('form').serialize()
    });
    $.ajax({
        success : function (response, status, xhr) {
                    alert(response);
    });
    });
```
在使用 data 属性传递的时候，如果是以对象形式传递键值对，可以使用$.param()方法将对象转换为字符串键值对格式。
主要是针对无法直接使用表单序列化方法.serialize()的情况，且传递参数为对象，建议使用该方法进行解析后再进行传递。

## MVC中的Ajax使用

需要引用ajax.js包

### 部分渲染

要部分加载的页面必须是partialPage吗？不是的，它也可以是一个完整的View页面，只是在被当作部分页面加载的时候使用PartialView()方法。
对于Controller来说，部分渲染和完整渲染的区别是Return View()还是Return PartialView()。当return View()时，渲染html页面是带所有资源（css、js）和母板页的。但是使用PartialView()是只渲染了当前页面的Html。

绝大部分情况下，Asp.Net MVC会把部分渲染和其他请求同等对待---请求被路由到指定控制器，控制器执行特定的业务逻辑。我们可以在控制器中使用Request.IsAjax来区别是否是ajax请求。





### Ajax.BeginForm

### Ajax数据验证

ajax翻页


在Asp.Net MVC中还包含一组Ajax辅助方法。可以用来创建表单和指向控制器操作的链接，不同的是他们时异步进行的。当使用这个辅助方法时，不用编写任何脚本代码来实现程序的异步。该辅助方法依赖于非侵入式mvc的jquery扩展。如果使用这些辅助方法时，需要引入脚本jquery.unbotrusive-ajax.js

在razor试图中，Ajax辅助方法，可以通过Ajax属性访问。
```CSharp
@Ajax.ActionLink("Click here","ActionName",null,new AjaxOptions{UpdateTargetId="",InsertionModel=InsertionModel.Replace,HttpMethod="get“}，new{@class="btn"})
```