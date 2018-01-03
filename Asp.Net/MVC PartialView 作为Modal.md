# MVC使用Controller.PartialView 作为Modal

## Controller.PartialView

该PartialView只会返回该页面的html数据，并不会返回包含有html代码的数据。

## Modal

该Modal为Bootstrap的modal。

---

Home页面有一个按钮，单击该按钮，弹出modal

```html
<div>
   <input type="button" value="弹出" class="btn btn-default">
</div>
```
同时该页面有一个div来存放modal，但是modal的内容放在另一个页面

```html
<div class="modal fade" id="myModal" tabindex="-1" role="dialog" aria-hidden="true">
    <div class="modal-dialog" style="margin-top:15%">
        <div class="modal-content" id="modal-container">
        </div>
    </div>
</div>
```
定义js
通过jquery的get请求获取到partialview的html数据，填充到modal-content里。设置title，展示modal。
```javascript
 $('#btn-add').on('click', function (data) {
            $.get('@Url.Action("BomAdd")', function (data) {
                $('.modal-content').html(data);//填充请求到的数据
                $('#myModalLabel').html("添加物料");//通过id设置title
                $('#myModal').modal('show');//展示modal
                $.validator.unobtrusive.parse("#bom-form");
            });
        });
```

其中`$.validator.unobtrusive.parse("#bom-form")`用来提供partialview页面的客户端验证。

## partialview

在该页面定义modal的内容，该内容是一个表单

```html
<div class="modal-header">
    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
    <h4 class="modal-title" id="myModalLabel">修改物料</h4>
</div>
@using (Ajax.BeginForm("BomAddOrModify", "WarehouseManage", new { type = @ViewBag.Type }, new AjaxOptions { HttpMethod = "POST" }, new { @class = "form -horizontal", id = "bom-form" }))
{
    <div class="modal-body">
        @Html.AntiForgeryToken()
        @Html.ValidationSummary(true)
        <div class="form-group">
            @Html.LabelFor(m => m.BomName, new { @class = "control-label col-sm-2" })
            <div class="col-sm-10">
                @Html.TextBoxFor(m => m.BomName, new
                {
                    @class = "form-control"
                })
                @Html.ValidationMessageFor(m => m.BomName, "", new { @class = "text-danger" })

            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(m => m.BomCode, new { @class = "control-label col-sm-2" })
            <div class="col-sm-10">
                @Html.TextBoxFor(m => m.BomCode, new
                {
                    @class = "form-control"
                })
                @Html.ValidationMessageFor(m => m.BomCode, "", new { @class = "text-danger" })

            </div>
        </div>
        <div class="form-group">
            @Html.LabelFor(m => m.BomID, new { @class = "control-label col-sm-2" })
            <div class="col-sm-10">
                @Html.TextBoxFor(m => m.BomID, new
                {
                    @class = "form-control"
                })
                @Html.ValidationMessageFor(m => m.BomID, "", new { @class = "text-danger" })

            </div>
        </div>
    </div>
    <div class="modal-footer">
        <button type="button" class="btn btn-default pull-left" data-dismiss="modal">关闭</button>
        <button type="submit" id="btn-submit" class="btn btn-primary">提交更改</button>
    </div>
}
```
                                                                                                                                                          