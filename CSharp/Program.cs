using System;
using System.Threading;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DelegateAsync delClass = new DelegateAsync();
           IAsyncResult result= delClass.del.BeginInvoke("hello", null, null);
            while(result.IsCompleted){
                Console.WriteLine("end");
            }
        }
    }
    public class DelegateAsync
    {
        public delegate string MyDelegate(string str);
        public MyDelegate del;
        public DelegateAsync()
        {
            del = Method1;
        }


        public string Method1(string str)
        {
            Thread.Sleep(2000);
            return str;
        }
    }

   
}
