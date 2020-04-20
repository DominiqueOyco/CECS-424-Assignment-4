//CECS 424 Assignment 4
//Dominique Oyco - 014605758

using System;                       //contains fundamental classes and base classes 
using System.IO;                    //allows us to work with files and directories in C#
using System.Linq;                  //provides classes and interfaces that support queries that use Language-Integrated Query (LINQ)
using System.Xml.Linq;              //needed to use LINQ's xml builder methods
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
        This recursive function enumerates all files in a given folder recursively including 
        the entire sub-folder hierarchy
        Param: path (string) - the input directory that the user has provided
        Return: file (string) - an iterator that will iterate through the directory when files are found
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
        All the byte size units a file can have. Inspired by the preview shown in lab.
        */
        private enum FileSizes : byte { B, KB, MB, GB, TB, PB, EB, ZB}   //units (bytes) for the sizes of the file

        /*
        This function formats a byte size in human readable form
        Param: byteSize (long) - byte size of a file 
        Return: byte size of a file formatted to two fixed decimal digits and it's corresponding byte unit (string)
        */
        static string FormatByteSize(long byteSize)
        {
            double formattedByteSize = byteSize;
            const double k = 1000;

            FileSizes fileSize = 0;
            //Divide the byte size by 1000 if the file size is >= 1000 and if it's less than the maximum byte unit (ZB)
            //to make sure that the numerical sizes of files are > 1 and < 1000
            for(fileSize = FileSizes.B; formattedByteSize >= k && fileSize < FileSizes.ZB; ++fileSize)
            {
                formattedByteSize /= k;        //
            }
            
            //N2 is equivalent to %.2f in other languages. It prints a double/float in fixed 2 decimal numbers
            return(formattedByteSize.ToString("N2") + " " + fileSize.ToString());    
        }

        /*
        This function constructs the table that will go inside the html report. 
        It uses the input directory from the main to look for the information about the files i.e. file type (.txt, .jpg,
        etc.), number of files of the same type, and the file size. The data is eventually displayed on the table.
        Param: files (IEnumerable<string>) - a string object used to traverse through the folder to looks for files (iterator)
        Return: table (XElement) - the constructed table that displays the data of a directory 
                and it's corresponding sub-directory
         */
        static XElement GetFileInfo(IEnumerable<string> files)
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
        Constructs the html file by building the head, title, style, and body of the html webpage
        Param: files (IEnumerable<string>) - a string object used to traverse through the folder to looks for files (iterator)
        Return: report (XDocument) - the html webpage containing the data from a directory that the user provided
        */
        static XDocument CreateReport(IEnumerable<string> files)
        {
            XDocument report = new XDocument(new XElement("html",
                    new XElement("head",
                        new XElement("title", "File Report"),
                        new XElement("style", "table, th, td {border: 2px solid black;}"),
                        new XElement("body",
                            new XElement("td", GetFileInfo(files))    //call the method above to construct the table
                        )
                    )
            ));
            return report;
        }

        /*
        The main function takes in two command line arguments: 
        1.) The path to the input folder
        2.) The path of the HTML report output file
        
        catch if there is no directory or if it's invalid
        */
        static void Main(string[] args)
        {
            try
            {
                string inputFolder = args[0];     //The location of a directory this program will search for
                string outputFile = args[1];      //The location of the output .html file will be along with name of the .html file
                
                CreateReport(EnumerateFilesRecursively(inputFolder)).Save(outputFile);  //searches for the 
            }
            catch //If an input or output directory does not exist, print the message below
            {
                Console.WriteLine("there's nothing...");            
            }
        }
    }
}