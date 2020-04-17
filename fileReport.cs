using System;

namespace labAssignment4
{
    class fileReport
    {
        static void Main(string[] args)
        {
            try
            {
                //command line args for folder location and file output
                string folder = args[0];
                string reportfile = args[1];
            }
            catch
            {
                Console.WriteLine("Wrong file type report"); //if there is an error, catch it!
            }
        }
    }
}