using System;
using System.Collections.Generic;
using System.IO;

namespace APBD_cw2
{
    class Program
    {
        static void Main(string[] args)
        {
            var fFormat;
            //default path variables for data import and export + data format
            var dPathToCsv = @"C:\Users\s18533\Desktop\dane.csv";
            var dPathToXml = @"C:\Users\s18533\Desktop\result.xml";
            var dFileFormat = "xml";
            
            var logPath = @"C:\Users\s18533\Desktop\log.txt";   //log, used for errors (ArgumentException()/FileNotFoundException())

            var lines = File.ReadLines(dPathToCsv);

            var hashStud = new HashSet<Student>(new OwnComparer());
            ICollection<string> currAttributes = new List<string>();

            foreach (var line in lines)
            {
                Console.WriteLine("Data: " + line);
                currAttributes.Add()
                Console.WriteLine("Processed name");
                Console.WriteLine("Processed surname");
                Console.WriteLine("Processed faculty");
                Console.WriteLine("Processed courseType");
                Console.WriteLine("Processed indNum");
                Console.WriteLine("Processed date");
                Console.WriteLine("Processed mail");
                Console.WriteLine("Processed mName");
                Console.WriteLine("Processed fName");
                Console.WriteLine("Successfully created student");
                Console.WriteLine("Exported student to " + fFormat)
            }

            var today = DateTime.UtcNow;
            var parsedDate = DateTime.Parse("2020-03-09");
            Console.WriteLine(today);
        }
    }

    public class Student
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string faculty { get; set; }
        public string courseType { get; set; }
        public string indNum { get; set; }
        public string date { get; set; }
        public string mail { get; set; }
        public string mName { get; set; }
        public string fName { get; set; }

        public Student()
        {
            name = "";
            surname = "";
            faculty = "";
            courseType = "";
            indNum = "";
            date = "";
            mail = "";
            mName = "";
            fName = "";
        }


    }

    class OwnComparer : IEqualityComparer<Student>
    {
        public bool Equals(Student x, Student y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals($"{x.name} {x.surname} {x.indNum}", 
                $"{y.name} {y.surname} {y.indNum}");
        }

        public int GetHashCode(Student obj)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode($"{obj.name} {obj.surname} {obj.indNum}");
        }
    }

}
