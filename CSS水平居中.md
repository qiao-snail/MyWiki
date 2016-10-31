 
# HTML水平布局居中

## Css元素分为块级和行内两种，故居中也要分情况讨论。

### 行内元素

*（行内元素包括：常见的有：a，strong，br, img, map,span, button, input, label, select, textarea。及： b, big, i, small, tt
abbr, acronym, cite, code, dfn, em, kbd,  samp, var, bdo,  object, q, script, sub, sup）*

这种类型的元素可以通过给父元素设置text-align:center来实现。

```HTML
    <body>
         <div class="txtCenter">
            水平居中！
        </div>        
    </body>
```
```CSS
    <style> div.txtCenter{
        text-align:center;
        }
    </style>
```
<p data-height="265" data-theme-id="dark" data-slug-hash="RGvJBJ" data-default-tab="html,result" data-user="joesnail" data-embed-version="2" data-pen-title="RGvJBJ" class="codepen">See the Pen <a href="http://codepen.io/joesnail/pen/RGvJBJ/">RGvJBJ</a> by joe (<a href="http://codepen.io/joesnail">@joesnail</a>) on <a href="http://codepen.io">CodePen</a>.</p>
<script async src="https://production-assets.codepen.io/assets/embed/ei.js"></script>

***
### 块级元素
*（块级元素：常见的块级元素：div，table，ul，dl，form，h1，p。及：address，article，aside，audio，blockquote，canvas，dd
div，dl，fieldset，figcaption，figure，figcaption，footer，form,h1,h2,h3,h4,h5,h6,header，hgroup，
hr，noscript，ol，output，p，pre，section,table,tfoot,ul,video)*

根据应用场景可分为定宽块级和不定款块级。

### 定宽块级元素
* 满足定宽，块级的元素可以通过设置margin的左右值为auto来实现。*
```HTML
<body>
    <div>
    块级元素水平居中！
    </div>
</body>
```
```CSS
<style>
div{
    border:1px solid red;
    width:500px;
    margin:20px; auto;
}
</style>
```
[CodePen](http://codepen.io/joesnail/pen/XjOYOp?editors=1010)
***
### 不定宽块级元素
*常见的情景如：分页导航。*
此种情况可以说使用如下三种方式：
* 加入table标签
* 设置display:inline
* 设置position：relative和left:50%
#### 加入table标签
1. 为需要设置的居中的元素父级元素加入一个table标签
2. 为这个table设置margin：auto
```HTML
<div>
    <table>
        <tbody>
            <tr>
                <td> 
                    <ul>
                        <li> <a href="#">1</a></li>
                        <li> <a href="#">2</a></li>
                        <li> <a href="#">3</a></li>
                    </ul>
                </td>
            </tr>
        </tbody>
    </table>
</div>
```
```CSS
<style>
 table{
     margin:0 auto;
 }
 ul{ list-style:none;marign:0px;padding:0px;}
 li{float:left;display:inline;margin-right:8px;}
</style>
```
[CodePen](http://codepen.io/joesnail/pen/kkVpmk)
***
#### 设置display:inline 方法
*设置块级元素的display为inline，然后使用text-align:center*
```HTML
<body>
    <div class="container">
        <ul>
            <li><a href="#">1</a></li>
            <li><a href="#">2</a></li>
            <li><a href="#">3</a></li>
        </ul>
    </div>
</body>
```
```CSS
<style>
  .container{
    text-align:center;
  }
  .container ul{
    list-style:none;
    margin:0;
    padding:0;
    display:inline;
  }
  .container li{
    margin-right:8px;
    display:inline;
  }
</style>
```
[CodePen](http://codepen.io/joesnail/pen/kkVpdk)
***
#### 设置position：relative和left:50%
*通过相对位置和left来设置*
```HTML
<body>
  <div class="container">
    <ul>
      <li><a href="#">1</a></li>
      <li><a href="#">2</a></li>
      <li><a href="#">3</a></li>
    </ul>
  </div>
</body>
```
```Css
<style>
  .container{
    float:left;
    position:relative;
    left:50%
  }
  .container ul{
    list-style:none;
    margin:0;
    padding:0;
    position:relative;
    left:-50%;
  }
  .container li{float:left;display:inline;margin-right:8px;}
</style>
```
[CodePen](http://codepen.io/joesnail/pen/xErJKP)


 <script async src="http://assets.codepen.io/assets/embed/ei.js"></script>