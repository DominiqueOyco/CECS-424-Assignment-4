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
    internal static class FileReport
    {
        /*
        This function enumerates all files in a given folder recursively including 
        the entire sub-folder hierarchy
        */
        static IEnumerable<string> EnumerateFilesRecursively(string path)
        {
            foreach (string directory in Directory.EnumerateDirectories(path))              //iterate through the folder
            {
                foreach (string file in Directory.EnumerateFiles(directory))                //iterate through the files in the folder
                {
                    yield return file;                                                      //generate the iterator
                }
                EnumerateFilesRecursively(directory);
            }
        }

        
        /*
        All the byte size units a file can have. Inspired by the preview shown in lab.
        */
        private enum FileSizes : byte { B, KB, MB, GB, TB, PB, EB, ZB, YB }                 //units (bytes) for the sizes of the file

        /*
        This function formats a byte size in human readable form
        */
        static string FormatByteSize(long byteSize)
        {
            //TODO: Implement this function
            double formatByteSize = byteSize;
            const double kilo = 1000;

            FileSizes fileSize = 0;
            for(fileSize = FileSizes.B; formatByteSize >= kilo && fileSize < FileSizes.YB; ++fileSize)
            {
                formatByteSize /= kilo;
            }
            
            return(formatByteSize.ToString());
        }

        /*
        Creates the necessary html report file containing a table with three 
        columns: "Type", "Count", & "Size" for the file name extension, the 
        number of files with this type, and the total size of all files with 
        the type respectively
        */
//        static XDocument CreateReport(IEnumerable<string> files)
//        { 
//            //TODO: Implement this function
//        }

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