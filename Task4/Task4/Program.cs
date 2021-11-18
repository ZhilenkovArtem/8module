using System;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask
{
    /// <summary>
    /// Студент
    /// </summary>
    [Serializable]
    class Student
    {
        /// <summary>
        /// Имя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Номер группы
        /// </summary>
        public string Group { get; set; }
        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }
    }
    class Program
    {
        static void Main()
        {
            var commonPath = @"C:\Users\Артем Жиленков\Desktop\";
            var filePath = commonPath + "Students.dat";
            var folderPath = commonPath + "Students";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var formatter = new BinaryFormatter();
#pragma warning disable SYSLIB0011
                    var studentsArray = formatter.Deserialize(stream) as Student[];
#pragma warning restore SYSLIB0011
                    var studentsList = studentsArray.Cast<Student>().ToList();
                    var studentGroups = from student in studentsList
                                        group student by student.Group;
                    foreach (var group in studentGroups)
                    {
                        using (StreamWriter sw = File.CreateText(folderPath + $@"\{group.Key}.txt"))
                        {
                            sw.WriteLine("Name\tDateOfBirth");
                            foreach (var student in group)
                            {
                                sw.WriteLine($"{student.Name}\t{student.DateOfBirth}");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Console.ReadKey();
            }
        }
    }
}
