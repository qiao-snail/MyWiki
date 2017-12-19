
# Yield介绍

当我们使用`yield`关键字时，就表示我们有一个可以被遍历的方法，操作，或者`get`访问器。使用`yield`不再需要实现`IEnumerable`和`IEnumerator`接口的额外的类就可以实现遍历（即使用`yield`可以更简单创建遍历对象）。

使用`yield`的两种实例：

```CSharp
yield return <expression>;  //返回当前项，记录当前项位置
yield break;  //结束遍历
```

## 提示

使用`yield return`语句每次只会返回一项。

可以通过`Linq`查询，`foreach`语句来使用迭代。foreach迭代中的每一项都会调用这个迭代方法。当迭代方法执行到`yield return`语句时，表达式被返回，并且当前位置被记录。当下一次调用遍历方法时，会从这个位置重新开始执行。

使用`yield break`结束这个迭代。

---

## 遍历方法和get访问器


声明一个遍历，必须符合如下要求：

* 返回类型必须是`IEnumerable`,`IEnumerable<T>`,`IEnumerator`或者`IEnumerator<T>`

* 不能有`ref`或`out`修饰参数

迭代器中`yield`返回的`IEnumerable`或`IEnumerator`都是`object`。如果迭代器返回类型是个泛型`IEnumerable<T>`或`IEnumerator<T>`。一定是有一个隐式转换，从表达式返回语句的表达式类型转换为泛型类型参数。

在如下情况中不能使用`yield return` `yield break`

* 匿名方法

* 方法中包换不安全块（unsafe blocks）

---

## 异常处理

* `yield return`不能出现在try-catch块中。但是可以在try-finally的try块中出现。

* `yiled break`可以在try或catch块中出现，但是不能在finally块中。

* 如果foreach主体（迭代器方法之外）抛出异常，则执行iterator方法中的finally块。

---

## 实现

如下代码返回`IEnumerable<string>`。然后遍历该集合

```CSharp

IEnumerable<string> elements = MyIteratorMethod();  
foreach (string element in elements)  
{  
   ...  
}

```

调用MyIteratorMethod方法时，并没有执行该方法。而是在遍历该方法的返回值时，才执行(延迟执行)。

在`foreach`循环的每次迭代中，都调用了`MoveNext`方法，都调用执行`MyIteratorMethod`方法，直到执行`yield return` 语句返回结果。`yield return`表达式不仅决定了循环体的元素变量的值，还决定了元素的当前属性，这是一个可枚举的字符串。

在foreach循环的每个后续迭代中，迭代器主体（MyIteratorMethod）的执行继续从它停止的地方继续执行，当它到达`yield return` 语句时再次停止。当迭代器方法的结束或执行`yield break`语句时，`foreach`循环就完成了。


## 实例

for循环中有一个`yield return`。在main方法的foreach循环中，每次都会调用Power方法，且都会从上一次`yield return`位置处执行下一项。（即，不是从Power中获取所有的数据，再foreach中遍历结果，而是每次遍历项都从Power方法中执行，）


```CSharp
public class PowersOf2
{
    static void Main()
    {
        // Display powers of 2 up to the exponent of 8:
        foreach (int i in Power(2, 8))
        {
            Console.Write("{0} ", i);
        }
    }

    public static System.Collections.Generic.IEnumerable<int> Power(int number, int exponent)
    {
        int result = 1;

        for (int i = 0; i < exponent; i++)
        {
            result = result * number;
            yield return result;
        }
    }

    // Output: 2 4 8 16 32 64 128 256
}

```

get属性访问器

```CSharp
public static class GalaxyClass
{
    public static void ShowGalaxies()
    {
        var theGalaxies = new Galaxies();
        foreach (Galaxy theGalaxy in theGalaxies.NextGalaxy)
        {
            Debug.WriteLine(theGalaxy.Name + " " + theGalaxy.MegaLightYears.ToString());
        }
    }

    public class Galaxies
    {

        public System.Collections.Generic.IEnumerable<Galaxy> NextGalaxy
        {
            get
            {
                yield return new Galaxy { Name = "Tadpole", MegaLightYears = 400 };
                yield return new Galaxy { Name = "Pinwheel", MegaLightYears = 25 };
                yield return new Galaxy { Name = "Milky Way", MegaLightYears = 0 };
                yield return new Galaxy { Name = "Andromeda", MegaLightYears = 3 };
            }
        }

    }

    public class Galaxy
    {
        public String Name { get; set; }
        public int MegaLightYears { get; set; }
    }
}

```
---
