using System;
using System.Collections.Generic;
using System.Text;

namespace MovieLib
{
    public class ErrorManager
    {
        public static void PrintConnectionError(Exception e)
        {
            Console.WriteLine("Connection problem");

            if (e != null)
                Console.WriteLine(e.Message);
        }
    }
}
