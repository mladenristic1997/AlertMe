using System;

namespace AlertMe.Domain
{
    public class IdProvider
    {
        static long Number;

        static IdProvider()
        {
            Number = DateTime.Now.Ticks;
        }

        public static string GetId() => $"{Number++}";
    }
}
