# Asp.Net MVC 身份验证-Forms

在MVC中对于需要登陆才可以访问的页面，只需要在对应的Controller或Action上添加特性`[Authorize]`就可以限制非登录用户访问该页面。那么如果实现登录？

## Form登录

HTTP协议是无状态的。所以上一次请求和下一次请求并不能相互关联起来，就是说这些请求并不能确定是哪个用户和用户的状态。但是对于登录来说，我们就需要准确的知道用户的状态及是哪个用户。

通常有两种情况来记录用户状态。

* 一种在服务端通过Session来标识。

* 一种通过Cookie在客户端标识用户。Form身份验证就是使用Cookie来标识用户。

Form登录验证流程：

用户访问受保护页面，

### Form登录实现

#### 一

在`Web.Config`配置文件中指定验证的方式为`Form`，并设置跳转的登录地址和Cookie的名称，及超时时间等。

```xml
<system.web>
    <authentication mode="Forms">
      <forms loginUrl="/Home/Login" timeout="200" cookieless="UseCookies" name="LoginCookieName"></forms>
    </authentication>  
  </system.web>
```

[Form特性的详细说明](https://msdn.microsoft.com/zh-cn/library/1d3t3c61(v=vs.110).aspx)


此时当未登录的用户访问有[Authorize]特性的Action操作时，会被跳转到Login页面，同时Login页面的URL后面会添加一个加密的ReturnUrl地址，该地址指向之前访问的有[Authorize]特性的Action地址。

#### 二

之前提到Form认证其实就是生成了一个Cookie，存放到用户的浏览器中。`FormAuthenication.SetAuthCookie(userName,true);`设置验证登录的Cookie。再通过页面跳转将Cookie响应给客户端。

```CSharp
[HttpPost]
[ValidateAntiForgeryToken]
public ActionResult Login(LoginViewModel vm,string returnUrl)
{
    if (ModelState.IsValid)
    {
        //用户名，密码验证

        FormsAuthentication.SetAuthCookie(vm.UserName, true); //登录,后一个 参数为是否创建持久Cookie。及true为可以在用户浏览器上保存的。false为不在浏览器上保存。
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Detail");
    }
    else
        return View(vm);
}
```

此时当我们登录以后会在浏览器中生成一个跟配置文件中名称相同的Cookie

如：

![](imgs/cookie.png)

该Cookie就是我们已经登录，通过Form验证的凭证。

#### 注销

删除对应的Cookie即可实现注销，代码如下：

```CSharp
[Authorize]
public ActionResult LogOut()
{
    FormsAuthentication.SignOut();//通过Forms验证来删除Cookie
    return View();
}

```
---

至此最简单的Form身份验证实现了。但是该Cookie只包含了用户名，没有其他信息。如果要包含其他信息，可以通过扩展用户的身份标识（HttpContext.User实例）来实现。

HttpContext.User是IPrincipal类型，所以实现一个自定义的继承自IPrincipal的类就可以。
如：

```CSharp
public class MyFormsPrincipal<TUserData> : IPrincipal
    where TUserData : class, new()
{
    private IIdentity _identity;
    private TUserData _userData;

    public MyFormsPrincipal(FormsAuthenticationTicket ticket, TUserData userData)
    {
        if( ticket == null )
            throw new ArgumentNullException("ticket");
        if( userData == null )
            throw new ArgumentNullException("userData");

        _identity = new FormsIdentity(ticket);
        _userData = userData;
    }
    
    public TUserData UserData
    {
        get { return _userData; }
    }

    public IIdentity Identity
    {
        get { return _identity; }
    }

    public bool IsInRole(string role)
    {
        // 把判断用户组的操作留给UserData去实现。

        IPrincipal principal = _userData as IPrincipal;
        if( principal == null )
            throw new NotImplementedException();
        else
            return principal.IsInRole(role);
    }

```

这个方法的核心是（分为二个子过程）：
1. 在登录时，创建自定义的FormsAuthenticationTicket对象，它包含了用户信息。
2. 加密FormsAuthenticationTicket对象。
3. 创建登录Cookie，它将包含FormsAuthenticationTicket对象加密后的结果。
4. 在管线的早期阶段，读取登录Cookie，如果有，则解密。
5. 从解密后的FormsAuthenticationTicket对象中还原我们保存的用户信息。
6. 设置HttpContext.User为我们自定义的对象。

---

```CSharp
var authTicket = new FormsAuthenticationTicket(
                    1,                             // version
                    model.Email,                      // user name
                    DateTime.Now,                  // created
                    DateTime.Now.AddMinutes(20),   // expires
                    model.RememberMe,                    // persistent?
                    "Moderator;Admin"                        // can be used to store roles
);

string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
    authCookie.HttpOnly = true;//客户端脚本不能访问
    authCookie.Secure = FormsAuthentication.RequireSSL;//是否仅用https传递cookie
    authCookie.Domain = FormsAuthentication.CookieDomain;//与cookie关联的域
    authCookie.Path = FormsAuthentication.FormsCookiePath;//cookie关联的虚拟路径
    Response.Cookies.Add(authCookie);
if (Url.IsLocalUrl(returnUrl))
{
    return Redirect(returnUrl);
}
```




