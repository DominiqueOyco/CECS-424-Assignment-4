//CECS 424 Assignment 4
//Dominique Oyco - 014605758

using System;                       //contains fundamental classes and base classes 
using System.IO;                    //allows us to work with files and directories in C#
using System.Linq;                  //provides classes and interfaces that support queries that use Language-Integrated Query (LINQ)
using System.Xml.Linq;              //needed to use linq's xml builder methods
using System.Collections.Generic;   //needed to access C#'s data structures

//TERMINAL COMMAND TO RUN: dotnet run /Users/rain/Desktop/lol /Users/rain/Desktop/lol/lol.html

/*
This program uses C# and LINQ to iterate files, to query, group, and order data, 
and to create an XML document based on that data
*/
namespace lab4tester424
{
    internal static class FileReport
    {
        /*
        This function enumerates all files in a given folder recursively including 
        the entire sub-folder hierarchy
        */
        static IEnumerable<string> EnumerateFilesRecursively(string path) 
        {
            //Using EnumerateFiles from System.IO.Directory to locate files in a folder. 
            //SearchOption.AllDirectories allows us to continue the search through a folder's subdirectories
            foreach (string file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories)) 
            {
                yield return file;                    //generate the iterator
            }
            EnumerateFilesRecursively(path);         //recursive call
        }


        /*
        All the byte size units a file can have. Similar to the preview shown in lab.
        */
        private enum FileSizes : byte { B, KB, MB, GB, TB, PB, EB, ZB, YB }                 //units (bytes) for the sizes of the file

        /*
        This function formats a byte size in human readable form
        */
        static string FormatByteSize(long byteSize)
        {
            double formattedByteSize = byteSize;
            const double kiloValue = 1000;

            FileSizes fileSize = 0;
            //
            for(fileSize = FileSizes.B; formattedByteSize >= kiloValue && fileSize < FileSizes.YB; ++fileSize)
            {
                formattedByteSize /= kiloValue;
            }
            
            return(formattedByteSize.ToString("N2") + " " + fileSize.ToString());    //N2 is equivalent to %.2f in other languages. It prints a double/float in fixed 2 decimal numbers
        }

        public static XElement GetFileInfo(IEnumerable<string> files)
        {
            XElement table = new XElement("table",
                new XAttribute("style", "width : 50%"),
                new XAttribute("border", 1),
                new XElement("thread",
                    new XElement("tr",
                        new XElement("th", "file type"),
                        new XElement("th", "count"),
                        new XElement("th", "size")
                    )
                ),
                new XElement("tbody",
                    from f in files
                    group f by Path.GetExtension(f).ToLower() into fileGroup
                    let size = fileGroup.Select(file => new FileInfo(file).Length).Sum()
                    orderby size descending 
                    select new XElement("tr",
                        new XElement("td", fileGroup.Key),
                        new XElement("td", fileGroup.Count()),
                        new XElement("td", FormatByteSize(size))
                    )
                )
            );
            return table;
        }
        
        /*
        Creates the necessary html report file containing a table with three 
        columns: "Type", "Count", & "Size" for the file name extension, the 
        number of files with this type, and the total size of all files with 
        the type respectively
        */
        private static XDocument CreateReport(IEnumerable<string> files)
        {
            XDocument report = new XDocument(new XElement("html",
                    new XElement("head",
                        new XElement("title", "File Report"),
                        new XElement("style", "table, th, td {border: 2px solid black;}"),
                        new XElement("body",
                            new XElement("td", GetFileInfo(files))
                        )
                    )
            ));
            return report;
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
                string inputFolder = args[0];     //The location of a directory this program will search for
                string outputFile = args[1];      //The location of the output .html file will be along with name of the .html file
                
                CreateReport(EnumerateFilesRecursively(inputFolder)).Save(outputFile);  //searches for the 
            }
            catch
            {
                Console.WriteLine("there's nothing...");                
            }
        }
    }
}