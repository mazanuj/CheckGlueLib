using System;
using System.Collections.Generic;
using CheckGlueLib;

namespace Test
{
    internal static class Program
    {
        private static void Main()
        {
            var result = Check.CheckIsGlue(new List<string> {"mail.ru", "mail.ua", "goo.gl"}).Result;

            foreach (var x in result)
                Console.WriteLine($"{x.DomainName} => {x.IsGlue}");

            Console.WriteLine("Done!!!");
            Console.ReadKey();
        }
    }
}