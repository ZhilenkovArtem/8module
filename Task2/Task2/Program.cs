using System;
using System.IO;

namespace Task2
{
    class Program
    {
        /// <summary>
        /// Точка входа
        /// </summary>
        static void Main()
        {
            Console.WriteLine("Введите путь:");
            string path = Console.ReadLine();
            var directory = new DirectoryInfo(path);
            long size = 0;
            if (directory.Exists)
            {
                try
                {
                    DetermineFileSize(directory, ref size);
                    ShowResult(size);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            else
            {
                Console.WriteLine($"Выбранная директория по пути \"{directory.FullName}\" не существует");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Определить размер директории
        /// </summary>
        /// <param name="directory">директория</param>
        /// <param name="size">полученный размер</param>
        static void DetermineFileSize(DirectoryInfo directory, ref long size)
        {
            try
            {
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    size += file.Length;
                }
            }
            catch
            {
                throw new Exception($"Нет доступа к файлам папки {directory}");
            }
            try
            {
                var directories = directory.GetDirectories();
                foreach (var dir in directories)
                {
                    DetermineFileSize(dir, ref size);
                }
            }
            catch
            {
                throw new Exception($"Нет доступа к директориям папки {directory}");
            }
        }
        /// <summary>
        /// Вывести результат в ГБ, МБ, КБ или байтах
        /// </summary>
        /// <param name="size">размер в байтах</param>
        static void ShowResult(long size)
        {
            try
            {
                long newSize = size;
                if ((newSize = size / (long)Math.Pow(1024, 3)) > 1)
                    Console.WriteLine($"Размер папки {newSize} ГБ");
                else if ((newSize = size / (long)Math.Pow(1024, 2)) > 1)
                    Console.WriteLine($"Размер папки {newSize} МБ");
                else if ((newSize = size / 1024) > 1)
                    Console.WriteLine($"Размер папки {newSize} КБ");
                else
                    Console.WriteLine($"Размер папки {newSize} Б");
            }
            catch
            {
                Console.WriteLine("Не получилось вывести результат");
            }
        }
    }
}
