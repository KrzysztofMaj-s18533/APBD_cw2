using System;
using System.Collections.Generic;
using System.IO;

namespace APBD_cw2
{
    class Program
    {
        static void Main(string[] args)
        {
            //default path variables for data import and export + data format
            var dPathToCsv = @"C:\Users\s18533\Desktop\dane.csv";
            var dPathToXml = @"C:\Users\s18533\Desktop\result.xml";
            var dFormat = "xml";

            var logPath = @"C:\Users\s18533\Desktop\log.txt";   //log, used for errors (ArgumentException()/FileNotFoundException())

            var lines = File.ReadLines(dPathToCsv);
            

            foreach(var line in lines)
            {
                Console.WriteLine(line);
            }

            var today = DateTime.UtcNow;

            Console.WriteLine(today);

            //var hash = new HashSet<Student>(new )
        }
    }
}
