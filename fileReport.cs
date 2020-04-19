using System;                       //contains fundamental classes and base classes 
using System.IO;                    //allows us to work with files and directories in C#
using System.Linq;                  //provides classes and interfaces that support queries that use Language-Integrated Query (LINQ)
using System.Xml.Linq;              //needed to use linq's xml builder methods
using System.Collections.Generic;   //needed to access C#'s data structures

/*
This program uses C# and LINQ to iterate files, to query, group, and order data, 
and to create an XML document based on that data
*/
namespace labAssignment4
{
    class fileReport
    {
        /*
        This function enumerates all files in a given folder recursively including 
        the entire sub-folder hierarchy
        */
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            try{
                foreach (string directory in Directory.EnumerateDirectories(path))          //iterate through the folder
                {
                    foreach (string file in Directory.EnumerateFiles(d))                    //iterate through the files in the folder
                    {
                        yield return file;                                                  //generate the iterator
                    }
                    EnumerateFilesRecursively(d);
                }     
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }

        /*
        All the byte size units a file can have. Similar to the preview shown in lab.
        */
        private enum FileSizes : byte 
        {
            B, kB, MB, GB, TB, PB, EB, ZB, YB;
        }

        /*
        This function formats a byte size in human readable form
        */
        static string FormatByteSize(long byteSize){
            //TODO: Implement this function
            const double kilo = (long)Math.Pow(10, 3);
        }

        /*
        Creates the necessary html report file containing a table with three 
        columns: "Type", "Count", & "Size" for the file name extension, the 
        number of files with this type, and the total size of all files with 
        the type respectively
        */
        static XDocument CreateReport(IEnumerable<string> files)
        { 
            //TODO: Implement this function
        }

        /*
        The main function takes in two command line arguments: 
        1.) The path to the input folder
        2.) The path of the HTML report output file
        */
        static void Main(string[] args)
        {
            try
            {
                //command line args for folder location and file output
                string folder = args[0];
                string reportfile = args[1];
                //TODO: call CreateReport() and pass in the folder the file output as parameters
            }
            catch
            {
                Console.WriteLine("Wrong file type report"); //if there is an error, catch it!
            }
        }
    }
}