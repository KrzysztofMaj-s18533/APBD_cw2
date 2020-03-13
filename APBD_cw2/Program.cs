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
            var dPathToCsv = @"C:\Users\peacr\Desktop\dane.csv";
            var dPathToOutput = @"C:\Users\peacr\Desktop\result.xml";
            var dFileFormat = "xml";
            
            var logPath = @"C:\Users\s18533\Desktop\log.txt";   //log, used for errors (ArgumentException()/FileNotFoundException())

            //non-default variables
            var pathToCsv = dPathToCsv;
            var pathToOutput = dPathToOutput;
            var fileFormat = dFileFormat;
            var lines = File.ReadLines(dPathToCsv);

            Console.WriteLine("Provide a csv path: ");
            try
            {
                var input = Console.ReadLine();
                pathToCsv = input;
                lines = File.ReadLines(pathToCsv);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine("Bad csv path " + ex.Message);
            }
            catch (FileNotFoundException exep)
            {
                Console.Error.WriteLine("Csv file not found " + exep.Message);
            }

            Console.WriteLine("Provide an output path: ");
            try
            {
                var input = Console.ReadLine();
                pathToOutput = input;
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine("Bad output path " + ex.Message);
            }
            catch (FileNotFoundException exep)
            {
                Console.Error.WriteLine("Output file not found " + exep.Message);
            }

            var hashStud = new HashSet<Student>(new OwnComparer());
            ICollection<string> currAttributes = new List<string>();
            Student newStud = null;
            string[] splitLine;
            foreach (var line in lines)
            {
                Console.WriteLine("Data: " + line);
                splitLine = line.Split(",");
                currAttributes.Add(splitLine[0]);
                Console.WriteLine("Processed name");
                currAttributes.Add(splitLine[1]);
                Console.WriteLine("Processed surname");
                currAttributes.Add(splitLine[2]);
                Console.WriteLine("Processed faculty");
                currAttributes.Add(splitLine[3]);
                Console.WriteLine("Processed courseType");
                currAttributes.Add(splitLine[4]);
                Console.WriteLine("Processed indNum");
                currAttributes.Add(splitLine[5]);
                Console.WriteLine("Processed date");
                currAttributes.Add(splitLine[6]);
                Console.WriteLine("Processed mail");
                currAttributes.Add(splitLine[7]);
                Console.WriteLine("Processed mName");
                currAttributes.Add(splitLine[8]);
                Console.WriteLine("Processed fName");
                if (splitLine.Length.Equals(9))
                {
                    bool add = true;
                    foreach(string val in splitLine)
                    {
                        if (string.IsNullOrEmpty(val))
                        {
                            add = false;
                        }
                    }
                    if (add)
                    {
                        newStud = new Student()
                        {
                            name = splitLine[0],
                            surname = splitLine[1],
                            faculty = splitLine[2],
                            courseType = splitLine[3],
                            indNum = splitLine[4],
                            date = splitLine[5],
                            mail = splitLine[6],
                            mName = splitLine[7],
                            fName = splitLine[8],
                        };

                        if (!hashStud.Add(newStud))
                        {
                            Console.Error.WriteLine("line bad :< " + line);
                        }
                    }
                }
                else
                {
                    //handle log.txt - missing values
                }

                Console.WriteLine("Successfully created student");
            }
            Console.WriteLine("Number of elems: " + hashStud.Count);
            Console.WriteLine("Exported student to " + fileFormat);

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
