---
description: 工作记录日志
---

# 2020日志

2020-03-16

###  TCP TCP是传输层协议

TCP的传输是以字节段的形式发送，字节段的大小，其实取决于接收端的数据处理窗口大小。如果要保证数据的顺序行和不沾包，需要在应用层，自己定制协议，来保证。如HTTP协议。 - Autofac

2020-03-17 - VS插件 1.StyleCol 2.ReSharper.CommandLineTools

2020-03-18 - 

### MVC-Filter 

筛选器主要是特供了在Action执行前和后的添加逻辑的实现方式。筛选器使用了面向切口 \_\(AOP\)\_ 编程的方式,在管道中添加了新的切口，实现操作。 \[参考地址\]\(https://docs.microsoft.com/en-us/previous-versions/aspnet/gg416513\(v=vs.98\)\) --- - Task.Wait\(\) 在UI线程中如果调用如下代码的Task.Wait\(\)会出现死锁 

```csharp

```

但是使用如下代码不会： \`\`\`csharp public async Task RunAsync\(\){ return Task.Run\(\(\)=&gt;{ //异步操作 }\) } \`\`\` 或者是 \`\`\`csharp public async Task RunAsyn\(\){ await Task.Run\(\(\)=&gt;{ //异步操作 }\),ConfigureAwait\(false\); } \`\`\`

2020-03-20 - Action委托 \`\`\`CSharp public MapperConfiguration\(Action configure\); //如果一个委托只调用一个方法，可以使用 new MapperConfiguration\(c=&gt;Method\(c\)\); //如果是需要调用多个可以加花括号来实现 new MapperConfiguration\(c=&gt;{ Method1\(c\); Method2\(c\); Method3\(c\); }\); \`\`\`

2020-03-24 - 方法的形参为父类的集合 当方法的形参为父类时，子类可以作为实参直接传递。 但是 当方法的形参为父类的\*\*集合\*\*时，子类集合不能作为实参传递。 因为子类可以转换为父类，但是子类集合不能转换为父类集合。 建议： 如果需要时，可以使用泛型，使用泛型方法，同时设置泛型类型继承自父类： 如： \`\`\`CSharp public Method DoMethod\(List list\) where T:ParentClass{ } //使用时： DoMethod\(new List\(\) \); \`\`\`

2020-03-26 - MySql索引 &gt;索引可以包含一个或多个列的值，如果索引包含多个列的值，则列的顺序也十分重要，因为MySQL只能高效地使用索引的最左前缀列。 根据数据结构可以分为3种： 1. 哈希表 2. 有序数组 3. 搜索树（重点） \#\#\#\# 哈希索引 哈希表是一种以键-值（key-value）的方式存储数据的结构，我们只要输入待查找的值（即key），就可以找到其对应的值（即Value）。哈希的思路很简单，把值放在数组里，用一个哈希函数把key换算成一个确定的位置，然后把value放在数组的这个位置，即idx = Hash\(key\)。如果出现哈希冲突，就采用拉链法解决。 因为哈希表中存放的数据不是有序的，\*\*因此不适合做区间查询，适用于只有等值查询的场景\*\*。 \#\#\#\# 有序数组 有序数组在等值查询和范围查询场景中的性能都非常优秀。用二分法就可以快速找到（时间复杂度为O\(logN\)）。但是如果要往中间插入一条数据，则必须挪动后面的所有记录，成本较高。因此，\*\*有序数组只适用于静态存储引擎，即数据表一旦建立后不再会修改\*\*。 \#\#\#\# B+树索引（InnoDB） 使用B+树存储数据可以让一个查询尽量少的读磁盘，从而减少查询时磁盘I/O的时间。 在 InnoDB 中，表都是根据主键顺序以索引的形式存放的，这种存储方式的表称为索引组织表。又因为前面我们提到的，InnoDB 使用了 B+ 树索引模型，所以数据都是存储在 B+ 树中的。每一个索引在 InnoDB 里面对应一棵 B+ 树。

2020-03-30 \#\# vscode 使用markdown编写时序图 1. vscode插件： 1.1

2020-04-01 \#\# C\#扩展方法 \* 如果扩展方法与该类型中定义的方法具有相同的签名，则扩展方法永远不会被调用。 \* 在命名空间级别将扩

## 日志

2020-03-16

### TCP

TCP是传输层协议，TCP的传输是以字节段的形式发送，字节段的大小，其实取决于接收端的数据处理窗口大小。如果要保证数据的顺序行和不沾包，需要在应用层，自己定制协议，来保证。如HTTP协议。

2020-03-17

### VS插件

1.StyleCol  
2.ReSharper.CommandLineTools

2020-03-18

### MVC-Filter

> 筛选器主要是特供了在Action执行前和后的添加逻辑的实现方式。筛选器使用了面向切口 _\(AOP\)_ 编程的方式,在管道中添加了新的切口，实现操作。  
> \[参考地址\]\([https://docs.microsoft.com/en-us/previous-versions/aspnet/gg416513\(v=vs.98](https://docs.microsoft.com/en-us/previous-versions/aspnet/gg416513%28v=vs.98)\)\)

* Task.Wait\(\) 在UI线程中如果调用如下代码的Task.Wait\(\)会出现死锁

  ```csharp
  public async Task RunAsync(){
    awiat Task.Run(()=>{
      //异步操作
    })
  }
  ```

  但是使用如下代码不会：

```csharp
  public async Task RunAsync(){
    return Task.Run(()=>{
//异步操作
          })
  }
```

或者是

```csharp
  public async Task RunAsyn(){
    await Task.Run(()=>{
      //异步操作
    }),ConfigureAwait(false);
  }
```

2020-03-20

### Action委托

```csharp
public MapperConfiguration(Action<IMapperConfigurationExpression> configure);

//如果一个委托只调用一个方法，可以使用
new MapperConfiguration(c=>Method(c));
//如果是需要调用多个可以加花括号来实现
new MapperConfiguration(c=>{
Method1(c);
Method2(c);
Method3(c);
});
```

2020-03-24

### 方法的形参为父类的集合

当方法的形参为父类时，子类可以作为实参直接传递。

但是

当方法的形参为父类的**集合**时，子类集合不能作为实参传递。 因为子类可以转换为父类，但是子类集合不能转换为父类集合。

建议：

如果需要时，可以使用泛型，使用泛型方法，同时设置泛型类型继承自父类：

如：

```csharp
public Method<T> DoMethod(List<T> list) where T:ParentClass{

}
//使用时：
DoMethod(new List<ChildClass>() );
```

2020-03-26

### MySql索引

> 索引可以包含一个或多个列的值，如果索引包含多个列的值，则列的顺序也十分重要，因为MySQL只能高效地使用索引的最左前缀列。

根据数据结构可以分为3种：

1. 哈希表
2. 有序数组
3. 搜索树（重点）

**哈希索引**

哈希表是一种以键-值（key-value）的方式存储数据的结构，我们只要输入待查找的值（即key），就可以找到其对应的值（即Value）。哈希的思路很简单，把值放在数组里，用一个哈希函数把key换算成一个确定的位置，然后把value放在数组的这个位置，即idx = Hash\(key\)。如果出现哈希冲突，就采用拉链法解决。

因为哈希表中存放的数据不是有序的，**因此不适合做区间查询，适用于只有等值查询的场景**。

**有序数组**

有序数组在等值查询和范围查询场景中的性能都非常优秀。用二分法就可以快速找到（时间复杂度为O\(logN\)）。但是如果要往中间插入一条数据，则必须挪动后面的所有记录，成本较高。因此，**有序数组只适用于静态存储引擎，即数据表一旦建立后不再会修改**。

**B+树索引（InnoDB）**

使用B+树存储数据可以让一个查询尽量少的读磁盘，从而减少查询时磁盘I/O的时间。

在 InnoDB 中，表都是根据主键顺序以索引的形式存放的，这种存储方式的表称为索引组织表。又因为前面我们提到的，InnoDB 使用了 B+ 树索引模型，所以数据都是存储在 B+ 树中的。每一个索引在 InnoDB 里面对应一棵 B+ 树。

2020-03-30

### vscode 使用markdown编写时序图

1. vscode插件：

   1.1 

2020-04-01

### C\#扩展方法

* 如果扩展方法与该类型中定义的方法具有相同的签名，则扩展方法永远不会被调用。
* 在命名空间级别将扩展方法置于范围中。 例如，如果你在一个名为 Extensions 的命名空间中具有多个包含扩展方法的静态类，则这些扩展方法将全部由 using Extensions; 指令置于范围中。

扩展方法，同样可以扩展接口，这样所有实现该接口的类，自动拥有该方法

```csharp
public interface IInterfaceA{}
public class InterfaceExtension{
  public static ExtensionMethodA(this IInterfaceA,int i){
    console.writeline("ExtensionMethodA:"+i);
  }
}
```

2020-04-10

### Entity Framework查看生成的sql语句

```csharp
context.DataBase.Log=Console.WriteLine;
//*这是一个委托，同理可以做日志输出*
```

[官网地址](https://docs.microsoft.com/en-us/ef/ef6/fundamentals/logging-and-interception)

2020-04-13

### IIS CPU使用过高

可能原因是静态字典变量，在多并发，即多线程的情况下导致。Dictionary类型只有当字典没有更改时，允许多线程并发访问。如果该字典有改动，且多线程访问，会出现排队，CPU升高问题。

解决问题：

1. 使用锁来规避该类问题。
2. 使用ConcurrentDictionary线程安全字典来规避，使用该字典可以降低lock的范围，提升性能。

### lock Tips

lock影响范围应该尽量小，来提升程序性能。

lock读取的变量值，尽量存放到一个局部变量中，且lock的范围只包含读取值，其他的不用加锁。

### MVC异常处理的5种方法

MVC几种可行的方式来处理应用程序异常。

```text
Web.Config 文件中的 <customErrors> 结点
MVC 的 HandleError 特性
Controller.OnException 方法
HttpApplication Application_Error 事件
使用Stackify 的 Retrace 收集异常
```

[来源-简书](https://www.jianshu.com/p/a5204606588a)

2020-04-14

### .NET 事件 内存泄露

首先需要明确一点事件是一种委托，委托由target属性来指向归属。即委托是由哪个对象引用了。当使用操作符+=时，其实了new了一个委托，该事件就和该对象产生了强关联。就会导致，事件对象不会被gc回收。即便已经事件的对象方法已经执行完毕。 只要事件的对象没有被回收，该事件就不会被回收，会一直占用内存。

原则1 **谁创建的对象，谁就有责任释放**

2020-04-22

### Select和SelectMany

Select是返回多个集合

SelectMany是整理后返回一个集合

2020-04-23

### Mysql索引

**聚簇索引**

每个索引都会有个索引树，当索引树包含索引数据时，称之为聚簇索引。（主键索引即为聚簇索引）

**索引回表**

当索引树不包含数据时，会现在当前索引树上找到索引值，再回到主键索引上找到数据（从普通索引树回到主键索引树搜索的过程就叫做回表）

如： select  _from user where name='BB'//name列设置了索引，会现在name这个索引树上找到id（主键）的值。 再在主键索引上找到数据。\(_ 表示查找所有数据，需要回到主键上找到所有数据\)

执行流程：

1. 选择使用 name 索引树；
2. 找到索引树的第一层结点，由于 where 条件中'BB'的值小于第一层结点中关键字'CC'的值，索引进入到关键字'CC'的左子树中查找；
3. 进入到第二层的叶子结点，找到关键字'BB'，由于叶子结点中存放了主键 id 的数据，所以返回'BB'中主键 id 的值 2；
4. 根据主键 id=2，再去主键 id 的索引树中查找，找到 id=2 所对应的数据 R2；
5. 在 name 索引树中继续向后查找，找到'BB'的下一个关键字'CC'，发现'CC'不等于 where 条件中的'BB'，所以结束查找。

**覆盖索引**

select \* from user where name='BB' 需要回表。但是当索引树上包含了查找的数据时，不需要回表操作即为覆盖索引 如： select id from user where name='BB'； name索引树上包含了主键id的值。直接就获取了。这个操作减少了回表操作，会更高效。

**联合索引**

select name,age from user where name='BB'

因为name是有索引的，索引在name索引树上可以直接获取name的信息，但是同时也要获取age信息，此时name索引树上没有name信息，需要回表到主索引树上再获取age信息，进行了一次回表。

如果要避免本次的回表操作。可以使用联合索引。及name和age为同一索引。

但是需要知道联合索引有最左匹配原则：

```sql
select name,age from user where name = 'BB' and age = 33; # 在使用联合索引时，会依次匹配name列和age列。

select name,age from user where name = 'B%' and age = 33; # 在使用联合索引时，当匹配到name这一列的时候，由于name使用了like范围查找，因此后面不会再匹配age这一列了。

select name,age from user where age = 33; # 在使用联合索引时，由于联合索引的最左列为name列，而我们在where条件中匹配的是age列，因此不满足最左匹配原则，所以该条SQL会进行该联合索引的全表扫描。
```

2020-05-11

### string.ToUpper\(\)方法耗时耗资源

* string是引用类型且不可变，每次修改都会创建一个新的string对象。所以当有多个string通过ToUpper方法来比较时，就会创建一倍以上的对象，增加了内存和时间消耗。

  优化方式：

  可以使用string.compare\(string1,string2,StringComparison.OrdinalIgnoreCase\)方法来对比实现。

  该方法是因为使用了ascii编码来对比实现，所以会高效。

2020-05-14

### 协变和逆变

* 当一个集合直接进行协变和逆变时，是不允许的。但是可以通过使用接口和out\(协变\) in\(逆变\) 关键字来实现。

  如 协变：

IEnumerable f=new List\(\); 其中IEnumerable是这样定义的： public interface IEnumerable : System.Collections.IEnumerable

逆变：

[参考](https://blog.csdn.net/u010476739/article/details/106080975)

2020-05-27

### List扩容导致内存增大优化

List如果当前的容量不够时，会成倍的增加容量，导致内存也成倍的增加。即使只是新增了一个元素，也有可能翻倍容量。

所以优化需要手动优化容量 list1.Capacity = list1.Count;

2020-06-09

#### 单例实现的几种方式

1. 双检锁
2. static cctor
3. Lazy

[参考1](https://blog.csdn.net/fd2025/article/details/79711198)

2020-06-10

#### linq 筛选去重操作：

```csharp
var result = myList.GroupBy(test => test.id)
                   .Select(grp => grp.First()).where(item=>item!=null)
                   .ToList();
```

2020-06-11

#### 字符串判断是否相等

```csharp
 bool safeEqual (string a, string b) {
            if (a.Length != b.Length) {
                return false;
            }
            int equal = 0;
            for (int i = 0; i < a.Length; i++) {
                equal |= a[i] ^ b[i];//逻辑异或运算符^:如果a、b两个值不相同，则异或结果为1。如果a、b两个值相同，异或结果为0。
                //按位或运算符“|”是双目运算符。其功能是参与运算的两数各对应的二进位相或。只要对应的二个二进位有一个为1时，结果位就为1。
            }
            return equal == 0;
        }
    }
```

#### 字符串驻留池

使用驻留池可以使字符串相同的内容，共用一个托管堆地址。减少内存。当字符串相同时在编译时就已经默认实现了驻留池。但是当在运行时，默认是不会使用驻留池的。需要我们手动添加`string.Intern`来实现。

2020-06-16

#### foreach赋值异常

foreach迭代中，不可对集合进行增删，但是其实对于值类型来说也是不允许修改的。但是对于引用类型是可以的。

#### async await 异常

> [Async void methods have different error-handling semantics. When an exception is thrown out of an async Task or async Task method, that exception is captured and placed on the Task object. With async void methods, there is no Task object, so any exceptions thrown out of an async void method will be raised directly on the SynchronizationContext that was active when the async void method started. Figure 2 illustrates that exceptions thrown from async void methods can’t be caught naturally.](https://stackoverflow.com/questions/19865523/why-cant-i-catch-an-exception-from-async-code)

在async方法中是可以再次catch到里面await的 async方法。但是在同步方法中是不能再次catch到await的async方法。

#### 索引失效

1. mysql 中索引列参与了函数运算会导致索引失效。需要特别注意的是隐式函数如类型转换，varchar列使用int数据查询（varcharcolumn1=1233\);
2. mysql 中使用了索引排序有可能会导致索引失效。
3. 对索引列使用了计算（也算是函数）

**覆盖索引**

覆盖索引（covering index）指一个查询语句的执行只用从索引中就能够取得，不必从数据表中读取。也可以称之为实现了索引覆盖。 如果一个索引包含了（或覆盖了）满足查询语句中字段与条件的数据就叫做覆盖索引。

#### DDD

数据实体分为贫血模型和充血模型。 1. 贫血模型，即实体中只包含了对应的属性，而不包含对应的行为，对应的行为放到了service层。 2. 充血模型，即service层很薄，几乎没有，实体不仅包含了对应的属性，相关的行为也放到了实体中，但是这样的后果就是实体会很复杂，有可能不同的实体会相互掺杂，且不耦合，不易扩展。

数据仓库也可分为读取操作在一个仓库完成，或者是增删改在一仓库中完成，查询相对来说会较多，较复杂，可单独出离出来在service层实现。这样也很好。

**聚合根，实体对象和值对象**

聚合根：领域中主要的对象。如电商中主要对象是订单。

实体对象：包含了一些行为的对象

值对象：简单对象，且一旦实现了就不再更改。如已付订单中的地址对象，就是一个值对象，订单一旦生成，就不可再更改地址。

通常情况下，对于聚合对象，使用工厂模式来创建。因为聚合对象通常交复杂，需要初始化许多辅助对象，辅助信息，所以这些封装后，不易出错，且简单可用。

_至于仓储应该放在基础层还是领域层，还是领域层只定义，基础层负责实现。一不影响性能，二不影响扩展。我认为都可以。_

2020-06-24

#### C\# 读取文件

可以使用FileStream流直接读取

```csharp
 var streamRead = new FileStream(filepath, FileMode.Open);
 byte[] pdfByte = new byte[streamRead.Length];
 streamRead.Read(pdfByte, 0, (int)streamRead.Length);
 //字节数组转换为base64字符串
 string base64String = Convert.ToBase64String(pdfByte, 0, pdfByte.Length, Base64FormattingOptions.None);
 streamRead.Close();
```

展方法置于范围中。 例如，如果你在一个名为 Extensions 的命名空间中具有多个包含扩展方法的静态类，则这些扩展方法将全部由 using Extensions; 指令置于范围中。 扩展方法，同样可以扩展接口，这样所有实现该接口的类，自动拥有该方法 \`\`\`csharp public interface IInterfaceA{} public class InterfaceExtension{ public static ExtensionMethodA\(this IInterfaceA,int i\){ console.writeline\("ExtensionMethodA:"+i\); } } \`\`\`

2020-04-10 \#\# Entity Framework查看生成的sql语句 \`\`\`csharp context.DataBase.Log=Console.WriteLine; //\*这是一个委托，同理可以做日志输出\* \`\`\` \[官网地址\]\(https://docs.microsoft.com/en-us/ef/ef6/fundamentals/logging-and-interception\)

2020-04-13 \#\# IIS CPU使用过高 可能原因是静态字典变量，在多并发，即多线程的情况下导致。Dictionary类型只有当字典没有更改时，允许多线程并发访问。如果该字典有改动，且多线程访问，会出现排队，CPU升高问题。 解决问题： 1. 使用锁来规避该类问题。 2. 使用ConcurrentDictionary线程安全字典来规避，使用该字典可以降低lock的范围，提升性能。 \#\# lock Tips lock影响范围应该尽量小，来提升程序性能。 lock读取的变量值，尽量存放到一个局部变量中，且lock的范围只包含读取值，其他的不用加锁。 \#\# MVC异常处理的5种方法 MVC几种可行的方式来处理应用程序异常。 Web.Config 文件中的 结点 MVC 的 HandleError 特性 Controller.OnException 方法 HttpApplication Application\_Error 事件 使用Stackify 的 Retrace 收集异常 \[来源-简书\]\(https://www.jianshu.com/p/a5204606588a\)

2020-04-14 \#\# .NET 事件 内存泄露 首先需要明确一点事件是一种委托，委托由target属性来指向归属。即委托是由哪个对象引用了。当使用操作符+=时，其实了new了一个委托，该事件就和该对象产生了强关联。就会导致，事件对象不会被gc回收。即便已经事件的对象方法已经执行完毕。 只要事件的对象没有被回收，该事件就不会被回收，会一直占用内存。 原则1 \*\*谁创建的对象，谁就有责任释放\*\*

2020-04-22 \#\# Select和SelectMany Select是返回多个集合 SelectMany是整理后返回一个集合

2020-04-23 \#\# Mysql索引 \#\#\#\# 聚簇索引 每个索引都会有个索引树，当索引树包含索引数据时，称之为聚簇索引。（主键索引即为聚簇索引） \#\#\#\# 索引回表 当索引树不包含数据时，会现在当前索引树上找到索引值，再回到主键索引上找到数据（从普通索引树回到主键索引树搜索的过程就叫做回表） 如： select \* from user where name='BB'//name列设置了索引，会现在name这个索引树上找到id（主键）的值。 再在主键索引上找到数据。\(\* 表示查找所有数据，需要回到主键上找到所有数据\) 执行流程： 1. 选择使用 name 索引树； 2. 找到索引树的第一层结点，由于 where 条件中'BB'的值小于第一层结点中关键字'CC'的值，索引进入到关键字'CC'的左子树中查找； 3. 进入到第二层的叶子结点，找到关键字'BB'，由于叶子结点中存放了主键 id 的数据，所以返回'BB'中主键 id 的值 2； 4. 根据主键 id=2，再去主键 id 的索引树中查找，找到 id=2 所对应的数据 R2； 5. 在 name 索引树中继续向后查找，找到'BB'的下一个关键字'CC'，发现'CC'不等于 where 条件中的'BB'，所以结束查找。 \#\#\#\# 覆盖索引 select \* from user where name='BB' 需要回表。但是当索引树上包含了查找的数据时，不需要回表操作即为覆盖索引 如： select id from user where name='BB'； name索引树上包含了主键id的值。直接就获取了。这个操作减少了回表操作，会更高效。 \#\#\#\# 联合索引 select name,age from user where name='BB' 因为name是有索引的，索引在name索引树上可以直接获取name的信息，但是同时也要获取age信息，此时name索引树上没有name信息，需要回表到主索引树上再获取age信息，进行了一次回表。 如果要避免本次的回表操作。可以使用联合索引。及name和age为同一索引。 但是需要知道联合索引有最左匹配原则： \`\`\`sql select name,age from user where name = 'BB' and age = 33; \# 在使用联合索引时，会依次匹配name列和age列。 select name,age from user where name = 'B%' and age = 33; \# 在使用联合索引时，当匹配到name这一列的时候，由于name使用了like范围查找，因此后面不会再匹配age这一列了。 select name,age from user where age = 33; \# 在使用联合索引时，由于联合索引的最左列为name列，而我们在where条件中匹配的是age列，因此不满足最左匹配原则，所以该条SQL会进行该联合索引的全表扫描。 \`\`\`

2020-05-11 \#\# string.ToUpper\(\)方法耗时耗资源 \* string是引用类型且不可变，每次修改都会创建一个新的string对象。所以当有多个string通过ToUpper方法来比较时，就会创建一倍以上的对象，增加了内存和时间消耗。 优化方式： 可以使用string.compare\(string1,string2,StringComparison.OrdinalIgnoreCase\)方法来对比实现。 该方法是因为使用了ascii编码来对比实现，所以会高效。

2020-05-14 \#\# 协变和逆变 \* 当一个集合直接进行协变和逆变时，是不允许的。但是可以通过使用接口和out\(协变\) in\(逆变\) 关键字来实现。 如 协变： IEnumerable f=new List\(\); 其中IEnumerable是这样定义的： public interface IEnumerable : System.Collections.IEnumerable 逆变： \[参考\]\(https://blog.csdn.net/u010476739/article/details/106080975\)

2020-05-27

## List扩容导致内存增大优化

List如果当前的容量不够时，会成倍的增加容量，导致内存也成倍的增加。即使只是新增了一个元素，也有可能翻倍容量。

所以优化需要手动优化容量 list1.Capacity = list1.Count;

2020-06-09

### 单例实现的几种方式

1. 双检锁
2. static cctor
3. Lazy

[参考1](https://blog.csdn.net/fd2025/article/details/79711198)

2020-06-10

### linq 筛选去重操作：

```csharp
var result = myList.GroupBy(test => test.id)
                   .Select(grp => grp.First()).where(item=>item!=null)
                   .ToList();
```

2020-06-11

### 字符串判断是否相等

```csharp
 bool safeEqual (string a, string b) {
            if (a.Length != b.Length) {
                return false;
            }
            int equal = 0;
            for (int i = 0; i < a.Length; i++) {
                equal |= a[i] ^ b[i];//逻辑异或运算符^:如果a、b两个值不相同，则异或结果为1。如果a、b两个值相同，异或结果为0。
                //按位或运算符“|”是双目运算符。其功能是参与运算的两数各对应的二进位相或。只要对应的二个二进位有一个为1时，结果位就为1。
            }
            return equal == 0;
        }
    }
```

### 字符串驻留池

使用驻留池可以使字符串相同的内容，共用一个托管堆地址。减少内存。当字符串相同时在编译时就已经默认实现了驻留池。但是当在运行时，默认是不会使用驻留池的。需要我们手动添加`string.Intern`来实现。

2020-06-16

### foreach赋值异常

foreach迭代中，不可对集合进行增删，但是其实对于值类型来说也是不允许修改的。但是对于引用类型是可以的。

### async await 异常

> [Async void methods have different error-handling semantics. When an exception is thrown out of an async Task or async Task method, that exception is captured and placed on the Task object. With async void methods, there is no Task object, so any exceptions thrown out of an async void method will be raised directly on the SynchronizationContext that was active when the async void method started. Figure 2 illustrates that exceptions thrown from async void methods can’t be caught naturally.](https://stackoverflow.com/questions/19865523/why-cant-i-catch-an-exception-from-async-code)

在async方法中是可以再次catch到里面await的 async方法。但是在同步方法中是不能再次catch到await的async方法。

### 索引失效

1. mysql 中索引列参与了函数运算会导致索引失效。需要特别注意的是隐式函数如类型转换，varchar列使用int数据查询（varcharcolumn1=1233\);
2. mysql 中使用了索引排序有可能会导致索引失效。
3. 对索引列使用了计算（也算是函数）

**覆盖索引**

覆盖索引（covering index）指一个查询语句的执行只用从索引中就能够取得，不必从数据表中读取。也可以称之为实现了索引覆盖。 如果一个索引包含了（或覆盖了）满足查询语句中字段与条件的数据就叫做覆盖索引。

### DDD

数据实体分为贫血模型和充血模型。 1. 贫血模型，即实体中只包含了对应的属性，而不包含对应的行为，对应的行为放到了service层。 2. 充血模型，即service层很薄，几乎没有，实体不仅包含了对应的属性，相关的行为也放到了实体中，但是这样的后果就是实体会很复杂，有可能不同的实体会相互掺杂，且不耦合，不易扩展。

数据仓库也可分为读取操作在一个仓库完成，或者是增删改在一仓库中完成，查询相对来说会较多，较复杂，可单独出离出来在service层实现。这样也很好。

**聚合根，实体对象和值对象**

聚合根：领域中主要的对象。如电商中主要对象是订单。

实体对象：包含了一些行为的对象

值对象：简单对象，且一旦实现了就不再更改。如已付订单中的地址对象，就是一个值对象，订单一旦生成，就不可再更改地址。

通常情况下，对于聚合对象，使用工厂模式来创建。因为聚合对象通常交复杂，需要初始化许多辅助对象，辅助信息，所以这些封装后，不易出错，且简单可用。

_至于仓储应该放在基础层还是领域层，还是领域层只定义，基础层负责实现。一不影响性能，二不影响扩展。我认为都可以。_

2020-06-24

### C\# 读取文件

可以使用FileStream流直接读取

```csharp
 var streamRead = new FileStream(filepath, FileMode.Open);
 byte[] pdfByte = new byte[streamRead.Length];
 streamRead.Read(pdfByte, 0, (int)streamRead.Length);
 //字节数组转换为base64字符串
 string base64String = Convert.ToBase64String(pdfByte, 0, pdfByte.Length, Base64FormattingOptions.None);
 streamRead.Close();
```

