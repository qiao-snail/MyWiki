# WPF 属性

依赖项属性是WPF特有的属性，不同于常用的属性。只是封装为我们常用的属性，在语法使用上和普通属性一样。WPF中一些经常用到的功能：动画，属性绑定，样式等都是以依赖项属性为基础的。WPF中元素的属性大部分都是依赖项属性。

依赖项属性和普通属性最主要的区别是：普通属性是通过一个私有的字段来读取。而依赖项属性则是通过继承在`DependencyObject`的GetValue()方法动态的读取属性值。

当你设置一个依赖项属性值时，并不是直接赋值给了一个对象的字段，而是在一个`DependencyObject`的字典中设置Key，Value。Key值为属性的名称，Value值为你要赋值的值。

使用依赖项属性有以下有点：

* 减少内存
   当UI控件的90%以上的属性通常停留在初始值时，为每个属性存储字段会是一个巨大的消耗。依赖项则是只在实例中存储修改的属性，那些默认值只在依赖项属性中存储一次。（如字体属性，并不是每个元素都存储一个字体属性，字体属性只存储一次，其他元素则是继承该字体属性。即：值继承）

* 值继承
    当访问依赖项属性时，该属性值将通过使用一个值解析策略来解决：如果没有设置本地值，则依赖属性将导航到逻辑树，直到找到一个值为止。当您在根元素上设置FontSize时，它将应用到下面的所有文本块，除了覆盖该值。

* 更改通知
    依赖属性有一个内置的变更通知机制。通过在属性元数据中注册一个回调，当属性值被更改时，就会得到通知。这也是数据绑定所使用的。

## 自定义依赖项属性

如果希望原本不支持数据绑定，动画，或其他WPF功能的代码实现这些功能时，就要创建依赖项属性。

### 一、声明依赖项属性

声明一个`DependencyProperty`类型的字段，表示依赖项属性的对象。该属性的值应该始终保持可用，还要保证在多个类之间共享该属性信息。因此，必须将`DependencyProperty`对象定义为与其类关联的静态字段。
（如：Margin属性，所有的类都共享该属性，所以该属性必须是与类本身关联。）

```CSharp
public static readonly DependencyProperty CurrentTimeProperty;
```

### 二、注册依赖项属性

注册需要在使用该属性代码之前完成，因此要在与其管理的静态构造函数中进行。WPF为了确保DependencyProperty对象不能被直接实例化，因为DependencyProperty类没有公有的构造函数。只能使用静态方法`DependencyPropery.Register()`创建DependencyProperty实例。WPF还确保在创建DependencyProperty对象后不能改变该对象，所以所有的DependencyProperty成员都是只读的。他们的值必须作为Register()方法的参数来提供。

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

该注册方法一共有五个参数，其中前三个为必选，后两个为可选参数。

* 第一个参数：表示CLR属性的名称。
* 第二个参数：表示依赖项属性的数据类型
* 第三个参数：表示注册的类的类型
* 第四个参数：表示该依赖项属性的默认值
* 第五个参数：表示该依赖项属性的回调验证

每个依赖属性都提供了更改通知、值强制和验证的回调。这些回调通过FrameworkPropertyMetadata来实现。

如：

```CSharp
new FrameworkPropertyMetadata( DateTime.Now, OnCurrentTimePropertyChanged, OnCoerceCurrentTimeProperty ),OnValidateCurrentTimeProperty );
```

#### 值更改回调

值更改回调是一个静态方法，每当该属性值更改时，都会调用这个回调方法。

```CSharp
private static void OnCurrentTimePropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
{
    MyClockControl control = source as MyClockControl;
    DateTime time = (DateTime)e.NewValue;
}
```

#### 属性值强制回调验证

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

#### 属性验证回调

但依赖项属性的值发生变化时调用该回调方法。该回调方法验证属性值是否合法，如果不合法，返回false，抛出异常。

```CSharp
private static bool OnValidateTimeProperty(object data)
{
    return data is DateTime;
} 
```

### 添加属性包装器

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

**在Visual Studio可以输入`propdp`再点击两次`tab`键，来创建依赖项属性。**

## 附加属性

附加属性也是一种依赖项属性。与其他依赖项属性不同的时，其他的依赖项属性是应用到被注册的类上，而附加属性则是应用到其他的类上。
 
比如：Canvas的类的Top和Left等为附加属性，其他的元素并没有该属性，只有Canvas类有，在使用Canvas.Top时，如果为每个元素都定义一个这样的依赖项属性，那么就会大量的重复性代码，且不可维护更改。如果只在Canvas类型上定义这个依赖项属性，其他的元素只继承使用该属性就好。附加属性就是这样。

附加属性的定义和依赖项属性的定义几乎一样。唯一不同的是注册是通过调用DependencyProperty.GegisterAttached方法来实现,且属性包装器为静态方法。

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

至此我们只需向使用普通的CLR属性一样就可以使用WPF的依赖项属性。