using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace APBD_cw2
{
    class Program
    {
        static void Main(string[] args)
        {
            //default path variables for data import and export + data format and Files
            var dPathToCsv = @"C:\Users\peacr\Desktop\dane.csv";
            var dPathToOutput = @"C:\Users\peacr\Desktop\result.xml";
            var dFileFormat = "xml";
            
            var logPath = @"C:\Users\peacr\Desktop\log.txt";   //log, used for errors (ArgumentException()/FileNotFoundException())
            if (!File.Exists(logPath))
            {
                using FileStream fs = File.Create(logPath);
            }
            using FileStream ws = File.OpenWrite(logPath);


            //non-default variables
            var pathToCsv = dPathToCsv;
            var pathToOutput = dPathToOutput;
            var fileFormat = dFileFormat;
            var lines = File.ReadLines(dPathToCsv);

            Console.WriteLine("Verifying the csv path: ");
            try
            {
                if (!string.IsNullOrEmpty(args[0]))
                {
                    pathToCsv = args[0];
                    lines = File.ReadLines(pathToCsv);
                }
            }
            catch (ArgumentException ex)
            {
                var data = ex.Message + (" Podana ścieżka jest niepoprawna " + args[0] +"\n");
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                ws.Write(bytes, 0, bytes.Length);
                Console.Error.WriteLine("Bad csv path " + ex.Message);
            }
            catch (FileNotFoundException exep)
            {
                var data = exep.Message + (" Podany plik " + args[0] + " nie istnieje" + "\n");
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                ws.Write(bytes, 0, bytes.Length);
                Console.Error.WriteLine("Csv file not found " + exep.Message);
            }

            Console.WriteLine("Verifying the output path: ");
            try
            {
                if (!string.IsNullOrEmpty(args[1]))
                {
                    pathToOutput = args[1];
                }
            }
            catch (ArgumentException ex)
            {
                var data = ex.Message + (" Podana ścieżka jest niepoprawna " + args[1] + "\n");
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                ws.Write(bytes, 0, bytes.Length);
                Console.Error.WriteLine("Bad output path " + ex.Message);
            }
            catch (FileNotFoundException exep)
            {
                var data = exep.Message + (" Podany plik " + args[1] + " nie istnieje" + "\n");
                byte[] bytes = Encoding.UTF8.GetBytes(data);

                ws.Write(bytes, 0, bytes.Length);
                Console.Error.WriteLine("Output file not found " + exep.Message);
            }

            Console.WriteLine("Verifying the data format: ");
            try
            {
                if (!string.IsNullOrEmpty(args[2]))
                {
                    if(args[2] == "xml" || args[2] == "json")
                    {
                        fileFormat = args[2];
                    }
                    else
                    {
                        throw new ArgumentException("Zły format pliku");
                    }
                }
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine("Bad file format " + ex.Message);
            }

            var hashStud = new HashSet<Student>(new OwnComparer());
            var hashFaculties = new HashSet<Faculty>(new FacultyComparer());
            List<string> faculties = new List<string>();
            List<int> facultyCount = new List<int>();
            
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
                        Faculty newFac = new Faculty();
                        newFac.fac = splitLine[2];
                        newFac.mode = splitLine[3];

                        if (!hashStud.Add(newStud))
                        {
                            Console.Error.WriteLine("line bad :< " + line);
                        }
                        else
                        {
                            if (faculties.Contains(newFac.fac))
                            {
                                facultyCount[faculties.IndexOf(newFac.fac)]++;
                            }
                            else
                            {
                                faculties.Add(newFac.fac);
                                facultyCount.Add(1);
                            }
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

            switch (fileFormat)
            {
                case "xml":
                    int facInd = 0;
                    XDocument doc = new XDocument(new XElement("university",
                            new XAttribute("createdAt", today),
                            new XAttribute("author", "Krzysztof Maj"),
                            new XElement("studenci",
                                from student in hashStud
                                select new XElement("student",
                                    new XAttribute("indexNumber", student.indNum),
                                    new XElement("name", student.name),
                                    new XElement("secondName", student.surname),
                                    new XElement("birthdate", student.date),
                                    new XElement("email", student.mail),
                                    new XElement("mothersName", student.mName),
                                    new XElement("fathersName", student.fName),
                                    new XElement("studies",
                                        new XElement("name", student.faculty),
                                        new XElement("mode", student.courseType)
                                    )))
                            ,
                            new XElement("activeStudies",
                                from faculty in faculties
                                select new XElement("studies",
                                    new XAttribute("name", faculty),
                                    new XAttribute("numberOfStudents", facultyCount[facInd++])
                                )
                            )
                            ));
                    doc.Save(pathToOutput);
                    Console.WriteLine("XML it is.");
                    break;
                case "json":
                    Console.WriteLine("JSon it is.");
                    break;
                default:
                    Console.Error.WriteLine("Bad file format! Unexpected value in switch");
                    break;
            }



            foreach(Student stud in hashStud)
            {

            }
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

    public class Faculty
    {
        public string fac { get; set; }
        public string mode { get; set; }
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

    class FacultyComparer : IEqualityComparer<Faculty>
    {
        public bool Equals(Faculty x, Faculty y)
        {
            return StringComparer.InvariantCultureIgnoreCase.Equals($"{x.fac} {x.mode}",
                $"{y.fac} {y.mode}");
        }

        public int GetHashCode(Faculty obj)
        {
            return StringComparer.CurrentCultureIgnoreCase.GetHashCode($"{obj.fac} {obj.mode}");
        }
    }

}
