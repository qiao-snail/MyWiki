# C#异常几点建议
## 1. 保持堆栈跟踪
当你第一次开始编程的时候。你可能会认为你捕获了一个异常，尝试处理，但如果你无法处理，那就重新抛出这个异常。或者，您可能已经定义了自己的异常类。
它可能看起来像下面的代码。

在这两种情况下，您将发现，与异常一起提供的堆栈跟踪信息截至到PartialStackTrace方法，不会有更深层的堆栈信息。
```CSharp
private double PartialStackTrace()
{
  try
  {
       return DivideANumber(1, 0);
  }
  catch (Exception ex)
  {
       throw ex;
  }
}

OR

private double PartialStackTrace()
{
  try
  {
       return DivideANumber(1, 0);
  }
  catch (Exception ex)
  {
       throw new MyAppException(“Ooops!”);
  }
}
```
因为每次throw都会重新开始记录堆栈信息。如果您获得一个完整的堆栈跟踪信息，这个信息揭示了这个问题存在于DivideANumber(或者在调用堆栈的后面)。

**如果您只是重新抛出异常使用抛出，那么完整的堆栈跟踪将被保留。**

**如果您定义了自己的异常类，那么在构造函数中提供补货的异常作为内部异常再重新抛出。**
如：
```CSharp
private double FullStackTrace()
{
  try
  {
       return DivideANumber(1, 0);
  }
  catch (Exception ex)
  {
       throw;
  }
}

OR

private double FullStackTrace()
{
  try
  {
       return DivideANumber(1, 0);
  }
  catch (Exception ex)
  {
       throw new MyAppException(“Ooops!”, ex);
  }
}
```
---

## 2. 异常任他异常

这听起来有点老套，但有时您不需要异常，因为您知道有时会收到数据，或者出现异常情况，您知道如何处理它。

例如，在尝试访问一个值之前，验证一个键是否存在于字典中是很简单的，并且在try.catch块中保存字典访问。
如：
```CSharp
var alphabet = new Dictionary<int, string>() { { 1, "A" }, { 2, "B" } };

// throws KeyNotFoundException

try
{
    Console.WriteLine($"27th letter of alphabet {alphabet[27]}");
}
catch (KeyNotFoundException kex)
{
    Console.WriteLine("27th letter of alphabet : <not present>");
}

// check first, no try ... catch required
var letter = alphabet.ContainsKey(27) ? alphabet[27] : "<not present>";
Console.WriteLine($"27th letter of alphabet : { letter }");k
```
类似地，在尝试复制新文件之前，您可能需要检查具有相同名称的文件是否存在。
---

## 3. 避免通用的、全面的异常处理
您不仅应该为预期会定期发生的情况抛出异常，您还应该始终致力于捕获特定的异常，而不是一般的异常。
在C#6.0中，这已经变得更细粒度了，因为它支持使用catch...when进行过滤。
``` CSharp
// handle specific exception differently

try
{
    double result = DivideByZeroCheckForSpecificException();
    Console.WriteLine($"Divide by zero results : {result}\n");
}
catch (DivideByZeroException dex)
{
    Console.WriteLine("Divide by zero results : <cannot divide by 0>\n");
}
catch (Exception ex)
{
    Console.WriteLine("Unknown exception : \n\n" + ex.ToString() + "\n");
}

// handle exception using a catch ... when predicate 

try
{
    double result = DivideByZeroCheckExceptionPredicate();
    Console.WriteLine($"Divide by zero results : {result}");
}
catch (Exception ex) when (ex.InnerException != null)
{
    Console.WriteLine("An InnerException exists : \n\n" + ex.ToString() + "\n");
}
```
---
## 4. 异步异常的处理
async为我们提供了一些关于异常的思考。第一种情况是，您可能会比以前更频繁地处理这种异常情况。这个包装器可以包装好几个异常，或者只是一个异常。您必须检查InnerException属性，以查看是否存在您可以处理的潜在异常

还应该注意的是，用async的方法不会抛出异常，除非您在await它们。在用mock和stubs创建单元测试时很容易忽略这一点，并且由于异常不像预期的那样抛出和处理异常而导致测试突然失败，这可能会导致很多挫折。

```CSharp
internal static async Task ThrowAnException()
{
    throw new Exception("Throwing from an async decorated method.");
}
private static async Task TestAsyncExceptions()
{

  // No exception will be thrown, we omitted await

  try
  {
    ThrowAnException();
    Console.WriteLine("No exception thrown.");
  }
  catch (Exception ex)
  {
    Console.WriteLine("Exception thrown : \n\n" + ex.ToString() + "\n");
  }

  // Exception will be thrown because we have used await

  try
  {
    await ThrowAnException();
    Console.WriteLine("No exception thrown.");
  }
  catch (Exception ex)
  {
    Console.WriteLine("Exception thrown : \n\n" + ex.ToString() + "\n");
  } 
}
```

---
## 5. 异常消息有局限性，最好不依赖它
即使在特定的异常类型捕获时，您可能会发现异常是不明确的，并没有真正地通知您潜在的问题。检查一个特定的异常消息来确认异常是您认为可以处理的异常，这是很有诱惑力的。
```CSharp
try
{  
  getDocument = DocStore.SingleOrDefault(d => d.SelfLink = doc.SelfLink); 
}
catch (System.InvalidOperationException ex)
{    
  if (ex.Message == “Sequence contains more than one matching element”)  
  {     
    CleanUpDuplicates(doc.SelfLink);  
  }  
  else  
  {    
   throw;  
  }
}
```
---
[原文](https://blogs.technet.microsoft.com/uktechnet/2017/07/25/top-5-net-exceptions/)