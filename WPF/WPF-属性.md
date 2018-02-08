# 浅谈WPF依赖项属性

## 0. 引言

依赖项属性虽然在使用上和CLR属性一样，但是它是WPF特有的，不同于CLR属性。只是封装为我们常用CLR的属性，在语法使用上和CLR属性一样。WPF中一些功能：动画，属性绑定，样式等都是以依赖项属性为基础的。WPF中元素的属性大部分都是依赖项属性。

依赖项属性和CLR属性最主要的区别是：CLR属性是通过一个私有的字段来读取。而依赖项属性则是通过继承在`DependencyObject`的`GetValue()`和`SetValue()`方法动态的读取属性值。

就是说当设置一个依赖项属性值时，并不是直接给一个对象的字段赋值，而是在一个`DependencyObject`对象的字典中设置键值对。`Key`值为属性的名称，`Value`值为你要赋值的值。

为什么使用依赖项属性：

* 减少内存
   当UI控件的90%以上的属性通常停留在初始值时，为每个属性存储字段会是一个巨大的消耗。依赖项属性则是只在实例中存储修改的属性，那些默认值只在依赖项属性中存储一次。（如字体属性，并不是每个元素都存储一个字体属性，字体属性只存储一次，其他元素则是继承该字体属性。即：值继承）

* 值继承
    当访问依赖项属性时，该属性值将通过使用一个值解析策略来解决：如果没有设置本地值，则依赖属性将导航到逻辑树，直到找到一个值为止。当在根元素上设置FontSize时，它将应用该根元素的所有子元素的所有文本块，除非在子元素中重新设置了FontSize值。

* 更改通知
    依赖属性有一个内置的变更通知机制。通过在属性元数据中注册一个回调，当属性值被更改时，就会得到通知。这也是数据绑定所使用的。

---

## 1. 自定义依赖项属性

如果希望原本不支持数据绑定，动画，或其他WPF功能的代码实现这些功能时，就要创建依赖项属性。

### 1.1 声明依赖项属性

声明一个`DependencyProperty`类型的字段，表示依赖项属性的对象。该属性的值应该始终保持可用，同时为了保证在多个类之间共享该属性信息，必须将`DependencyProperty`对象定义为与其类关联的静态字段，WPF为了确保在创建`DependencyProperty`对象后不能修改该对象，所以所有的`DependencyProperty`成员都是只读的。
（如：Margin属性，所有的类都共享该属性，所以该属性必须是与类本身关联。）

```CSharp
//声明一个当前时间的依赖项属性
public static readonly DependencyProperty CurrentTimeProperty;
```

### 1.2 注册依赖项属性

在使用该属性代码之前要先完成注册（即该属性和被注册的类和数据类型，默认值，及一些事件关联起来）完成，因此要在与其关联的静态构造函数中进行。WPF为了确保`DependencyProperty`对象不能被直接实例化，使`DependencyProperty`类没有公有的构造函数。只能使用静态方法`DependencyPropery.Register()`创建`DependencyProperty`实例。这些注册的值必须作为`Register()`方法的参数来提供。

```CSharp
public class MyClockControl1
{
    CurrentTimeProperty = DependencyProperty.Register( "CurrentTime", typeof(DateTime),
     typeof(MyClockControl), new FrameworkPropertyMetadata(DateTime.Now),validateValueCallback);

 
    static bool validateValueCallback(object data)
    {
        //自定义验证
        return true；
    }
}
```

`Register()`注册方法一共有五个参数，其中前三个为必选，后两个为可选参数。

* 第一个参数：表示CLR属性的名称。
* 第二个参数：表示依赖项属性的数据类型
* 第三个参数：表示注册的类的类型
* 第四个参数：表示该依赖项属性的默认值
* 第五个参数：表示该依赖项属性的回调验证

每个依赖属性都提供了更改通知、值强制和验证的回调。这些回调可以通过`FrameworkPropertyMetadata`来实现。

如：

```CSharp
new FrameworkPropertyMetadata( DateTime.Now, OnCurrentTimePropertyChanged, OnCoerceCurrentTimeProperty ),OnValidateCurrentTimeProperty );
```

#### 1.2.1 值更改回调

值更改回调是一个静态方法，每当该属性值更改时，都会调用这个回调方法。

```CSharp
private static void OnCurrentTimePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
{
    MyClockControl control = source as MyClockControl;
    DateTime time = (DateTime)e.NewValue;
}
```

#### 1.2.2 属性值强制回调验证

在执行属性验证回调前先执行该方法，尝试纠正不合法的属性值。

```CSharp
private static object OnCoerceTimeProperty( DependencyObject sender, object data )
{
    if ((DateTime)data > DateTime.Now )
    {
        data = DateTime.Now;
    }
    return data;
}
```

#### 1.2.3 属性验证回调

但依赖项属性的值发生变化时调用该回调方法。该回调方法验证属性值是否合法，如果不合法，返回false，抛出异常。

```CSharp
private static bool OnValidateTimeProperty(object data)
{
    return data is DateTime;
} 
```

### 1.3 添加属性包装器

依赖项属性的包装器，是通过DependencyObject的GetValue()和SetValue()方法来包装。

```CSharp
public DateTime CurrentTime
{
    get { return (DateTime)GetValue(CurrentTimeProperty); }
    set { SetValue(CurrentTimeProperty, value); }
}
```

完整的代码如下：

```CSharp
// Dependency Property
public static readonly DependencyProperty CurrentTimeProperty = 
     DependencyProperty.Register( "CurrentTime", typeof(DateTime),
     typeof(MyClockControl), new FrameworkPropertyMetadata(DateTime.Now));
 
// .NET Property wrapper
public DateTime CurrentTime
{
    get { return (DateTime)GetValue(CurrentTimeProperty); }
    set { SetValue(CurrentTimeProperty, value); }
}

static bool validateValueCallback(object data)
{
    //自定义验证
    return true；
}
```

自定义一个依赖项属性，需要挺多的套路，当然这些套路不需要你一个一个代码来敲，**在Visual Studio可以输入`propdp`再点击两次`tab`键，就可以创建依赖项属性模板。** 只需修改模板的属性名称和类型即可。

模板代码如下：

```CSharp
public int MyProperty
{
    get { return (int)GetValue(MyPropertyProperty); }
    set { SetValue(MyPropertyProperty, value); }
}

// Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
public static readonly DependencyProperty MyPropertyProperty =
    DependencyProperty.Register("MyProperty", typeof(int), typeof(ownerclass), new PropertyMetadata(0));

```

---

## 2. 附加属性

附加属性也是一种依赖项属性。与其他依赖项属性不同的时，其他的依赖项属性是应用到被注册的类上，而附加属性则是应用到其他的类上。
 
比如：`Canvas`的类的`Top`和`Left`等为附加属性，其他的元素并没有该属性，只有`Canvas`类有，在使用`Canvas.Top`时，如果为每个元素都定义一个这样的依赖项属性，那么就会大量的重复性代码，且不可维护更改。如果只在`Canvas`类型上定义这个依赖项属性，其他的元素只继承使用该属性就好。附加属性就是这样。

附加属性的定义和依赖项属性的定义几乎一样。唯一不同的是注册是通过调用`DependencyProperty.GegisterAttached()`方法来实现,且属性包装器为静态方法。

如：

```CSharp
public static readonly DependencyProperty TopProperty =
    DependencyProperty.RegisterAttached("Top", 
    typeof(double), typeof(Canvas),
    new FrameworkPropertyMetadata(0d,
        FrameworkPropertyMetadataOptions.Inherits));
 
public static void SetTop(UIElement element, double value)
{
    element.SetValue(TopProperty, value);
}
 
public static double GetTop(UIElement element)
{
    return (double)element.GetValue(TopProperty);
}
```

至此就可以像使用普通的CLR属性一样使用WPF的依赖项属性。

---

如有不对，请多多指教！

---

参考列表：

* [WPF Tutorial](http://www.wpftutorial.net/DependencyProperties.html)