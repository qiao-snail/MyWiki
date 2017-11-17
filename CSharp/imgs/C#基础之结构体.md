# C#基础之结构体

## 什么是结构体

`struct`关键字用来创建结构体。一个结构可以包含变量、方法、静态构造函数、参数化构造函数、操作符、索引器、事件和属性。

声明一个结构体

```CSharp
struct Struct_Name  
{  
    //Structure members  
} 
```

下面的创建的结构体包含了变量，方法，属性。结构体的字段可以是静态的和非静态。

```CSharp
using System;  
  
namespace Tutpoint  
{  
    class Program  
    {  
        struct Student  
        {  
            //Variables  
            int Roll_no;  
            string Name;  
            string Mobile;  
  
            //Enum  
            enum course { BTech, MBA, BPharma, MA, BSc }  
  
            // Static method  
            public static void marks()  
            {  
                Console.WriteLine("Marks");  
            }  
  
            // Method  
            public void Grades()  
            {  
                Console.WriteLine("Grades");  
            }  
  
            //Property  
            public int Serial_No { get; set; }  
  
        }  
  
        static void Main(string[] args)  
        {  
            Console.ReadKey();  
        }  
    }  
}
```

### 结构体不能继承任何一个结构体或类

如下代码所示当结构体Syllabus继承结构体Student时，编译器会提示错误。同样当结构体继承一个类时，也会提示同样的错误。

```CSharp

using System;  
  
namespace Tutpoint  
{  
    class Program  
    {  
        struct Student  
        {  
            //Variables  
            int Roll_no;  
            string Name;  
            string Mobile;  
  
            //Enum  
            enum course { BTech, MBA, BPharma, MA, BSc }  
  
            // Static method  
            public static void marks()  
            {  
                Console.WriteLine("Marks");  
            }  
  
            // Method  
            public void Grades()  
            {  
                Console.WriteLine("Grades");  
            }  
  
            //Property  
            public int Serial_No { get; set; }  
  
        }  
  
        // When Struct 'Syllabus' try to inherit from Struct 'Student' then compiler will produce an error  
        // Error as "Type 'Program.Student' in interface list is not an interface"  
        struct Syllabus : Student  
        {  
            int Roll_no;  
            int status;  
        }  
  
        static void Main(string[] args)  
        {  
            Console.WriteLine("December ");  
            Console.ReadKey();  
        }  
    }  
}
```
### 结构体可以实现接口的任何一个成员。

如下代码结构体Student实现了两个接口IStudent和IRecords。IRecords有一个要实现的方法。

```CSharp
using System;  
  
namespace Tutpoint  
{  
    class Program  
    {  
        interface IStudent  
        {  
        }  
        interface IRecords  
        {  
            void Records();  
        }  
        //struct implements 2 interfaces 'IRecords' and 'IStudent'  
        struct Student : IStudent, IRecords  
        {  
            public int Roll_no;  
            public string Name;  
            public string Mobile;  
            //Method from IRecords interface is defined  
            public void Records()  
            {  
                Console.WriteLine("Records");  
            }  
  
        }  
        static void Main(string[] args)  
        {  
            //struct object is creating  
            Student student = new Student();  
            student.Records();  
            Console.ReadKey();  
        }  
    }  
}

//output
//records
```

### 构造函数

我们不能定义结构体的默认构造函数，但是可以定义静态和参数化构造函数。如下代码所以，静态构造函数会比其他构造函数更早执行。

在Main方法中，我们创建了一个结构体的实例，跟创建类实力一样。然后我们通过是使用结构体实例“.”出来字段，属性等来访问数据。

```CSharp
using System;  
  
namespace Tutpoint  
{  
    class Program  
    {  
        struct Student  
        {  
            //Variables  
            public int Roll_no;  
            public string Name;  
            public string Mobile;  
  
            //Enum  
            public enum course { BTech, MBA, BPharma, MA, BSc }  
  
            // Static Constructor, valid  
            static Student()  
            {  
                Console.WriteLine("Static Constructor");  
            }  
  
            // Struct does not contain Default Constructor  
            // It will produce an error as "Structs cannot contain explicit parameterless constructor"  
            public Student()  
            {  
                Console.WriteLine("Default Constructor");  
            }  
  
            // Parameterised Constructor  
            // Valid, This constructor should return all the values of the struct members and must contain all the arguments  
            public Student(int roll, string name, string mobile, int serial)  
            {  
                Roll_no = roll;  
                Name = name;  
                Mobile = mobile;  
                Serial_No = serial;  
            }  
  
            //Property  
            public int Serial_No { get; set; }  
  
        }  
  
  
        static void Main(string[] args)  
        {  
            // Creating object of struct 'Student' by passing parametres to it  
            Student student = new Student(3, "Shubham", "9821705378", 1);  
            //Struct value are get with struct object followed by (.) and struct member name  
            Console.WriteLine("Roll no: " + student.Roll_no + " Name: " + student.Name + " Mobile no: " + student.Mobile + " Serial no: " + student.Serial_No);  
            Console.ReadKey();  
        }  
    }  
}

//output
//Static Constructor
//Roll no:3 Name:Shubhanm Mobile no:9821705278 Serial no:1
```

### 析构函数

结构体没有析构函数。如果我们定义一个析构函数的时候，编译器会提示错误。

```CSharp
using System;  
  
namespace Tutpoint  
{  
    class Program  
    {  
        struct Student  
        {  
            //Variables  
            public int Roll_no;  
            public string Name;  
            public string Mobile;  
  
  
            // Destructor  
            // Struct cannot contain destructors. It will produce a compile error as "Only class types can contain destructors"  
            ~Student()  
            {  
                Console.WriteLine("Destructor");  
            }  
  
        }  
        static void Main(string[] args)  
        {  
            Console.ReadKey();  
        }  
    }  
}  
```

### 不能初始化结构中的struct成员

在结构体中，我们不能用一个值初始化struct成员。这将产生一个编译错误。Student不能在结构中拥有实例属性或字段初始化器

```CSharp
using System;  
  
namespace Tutpoint  
{  
    class Program  
    {  
        struct Student  
        {  
            //Variables 'Roll_no', 'Name' and 'Mobile' are initialised with value which is not possible in struct.  
            //It will produce a compile error as "Program.Student cannot have instance property or field initializers in structs"  
            public int Roll_no = 11;  
            public string Name = "Shubham";  
            public string Mobile = "9821705378";  
  
        }  
        static void Main(string[] args)  
        {  
            Console.ReadKey();  
        }  
    }  
} 
```

### 什么时候使用结构体，结构体和类有什么区别？

结构体和类之间有许多不同。我们一一列举：

1. 当你使用值类型集合且该集合很小的时候，结构体会有更好的性能。

2. 当所有的字段成员都是值类型的时候使用结构体，如果成员中有一个是引用类型就使用类。

3. 在C#中，使用值类型比引用类型在托管堆上的创建的对象更少，所以垃圾收集器(GC)的负载更少，GC周期更少，从而导致更好的性能。然而，值类型也有其缺点。传递一个大的结构体肯定比传递引用要更耗性能。

4. 类是引用类型。当创建类的对象时，对象被赋值的变量只包含对该对象内存的引用。当对象引用被分配给一个新变量时，新变量引用了原始对象。通过一个变量进行的更改反映在另一个变量中，因为它们都引用相同的数据。struct是一种值类型。当一个结构体被创建时，这个结构被赋值的变量保存了结构体的实际数据。当将struct分配给一个新变量时，它将被复制。因此，新变量和原始变量包含两个单独的相同数据副本。对一个副本的更改不会影响另一个副本。通常，类用于建模更复杂的行为或数据，这些行为或数据是在创建类对象后被修改的。结构体最适合于小的数据结构，这些结构中包含的数据是在创建结构之后不打算修改的。

5. 结构体是值类型。如果把结构体赋值给一个新的变量，那么这个新的变量的值就是该结构体的一个副本。

```CSharp
public struct IntStruct  
{  
    public int Value { get; set; }  
}  
```

对比如下5个结构体对象变量

```CSharp
var struct1 = new IntStruct() { Value = 0 }; // original  
var struct2 = struct1; // A copy is made  
var struct3 = struct2; // A copy is made  
var struct4 = struct3; // A copy is made  
var struct5 = struct4; // A copy is made  
  
// NOTE: A "copy" will occur when you pass a struct into a method parameter.  
// To avoid the "copy", use the ref keyword.  
  
// Although structs are designed to use less system resources  
// than classes. If used incorrectly, they could use significantly more.  
```

类是引用类型，当给一个类的实例赋值给新的变量，新变量是指向类的实例的地址。

```CSharp
public class IntClass  
{  
    public int Value { get; set; }  
}  
```

执行如下代码，对比5个实例的值

```CSharp
var class1 = new IntClass() { Value = 0 };  
var class2 = class1; // A reference is made to class1  
var class3 = class2; // A reference is made to class1  
var class4 = class3; // A reference is made to class1  
var class5 = class4; // A reference is made to class1  
```

结构体可能会增加代码错误的可能性。如果一个值对象被当作一个可变的引用对象对待，那么开发人员可能会惊讶于所做的更改会出乎意料地丢失。

```CSharp
var struct1 = new IntStruct() { Value = 0 };  
var struct2 = struct1;  
struct2.Value = 1;  
// At this point, a developer may be surprised when  
// struct1.Value is 0 and not 1
```

### 总结

结构体提供了更好的性能，因为它是值类型。使用值类型将导致托管堆上的对象减少，这将导致垃圾收集器(GC)的负载更少，GC周期更少，从而导致更好的性能。然而，值类型也有其缺点。传递一个大的结构体肯定比传递引用要昂贵，这是一个明显的问题。我希望这篇文章能帮助您更好地理解结构。