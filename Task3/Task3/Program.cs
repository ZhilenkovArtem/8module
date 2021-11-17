using System;
using System.IO;

namespace Task3
{
    class Program
    {
        // <summary>
        /// Точка входа
        /// </summary>
        static void Main()
        {
            var directory = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            directory = directory.Parent.Parent.Parent.Parent;
            directory = new DirectoryInfo(String.Concat(directory.FullName, @"\Test"));
            int deletedFilesCount = 0;
            long initialSize = 0, deletedSize = 0, newSize = 0;
            if (directory.Exists)
            {
                try
                {
                    DetermineFileSize(directory, ref initialSize);
                    Console.WriteLine($"Исходный размер папки: {ShowResult(initialSize)}");

                    DeleteOldFiles(directory, ref deletedFilesCount, ref deletedSize);
                    Console.WriteLine($"Удалено: {deletedFilesCount} файлов.\nОсвобождено: {ShowResult(deletedSize)}");

                    DetermineFileSize(directory, ref newSize);
                    Console.WriteLine($"Текущий размер папки: {ShowResult(newSize)}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Во время выполнения программы обнаружена ошибка:\n{ex}");
                }
            }
            else
            {
                Console.WriteLine($"Выбранная директория по пути \"{directory.FullName}\" не существует");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Удалить неиспользующиеся более 30 минут файлы и папки
        /// </summary>
        /// <param name="directory">рассматриваемая директория</param>
        /// <returns>необходимо удалить неиспользуемую директорию</returns>
        static bool DeleteOldFiles(DirectoryInfo directory, ref int deletedFilesCount, ref long deletedSize)
        {
            try
            {
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    if (CheckLastAccessOver30min(file.LastAccessTime))
                    {
                        deletedFilesCount++;
                        deletedSize += file.Length;
                        file.Delete();
                    }
                }
            }
            catch
            {
                throw new Exception($"Нет доступа к файлам директории {directory}");
            }
            try
            {
                var directories = directory.GetDirectories();
                foreach (var dir in directories)
                {
                    if (DeleteOldFiles(dir, ref deletedFilesCount, ref deletedSize))
                    {
                        dir.Delete();
                    }
                }
            }
            catch
            {
                throw new Exception($"Нет доступа к директориям папки {directory}");
            }
            var remainingFilesCount = directory.GetDirectories().Length + directory.GetFiles().Length;
            return remainingFilesCount == 0 ? true : false;
        }
        /// <summary>
        /// Определить, были ли файл изменен более 30 минут назад
        /// </summary>
        /// <param name="lastAccessTime">последнее время изменения</param>
        /// <returns>был изменен более 30 минут назад</returns>
        static bool CheckLastAccessOver30min(DateTime lastAccessTime)
        {
            var dateTimeInterval = DateTime.Now.Subtract(lastAccessTime);
            return dateTimeInterval > TimeSpan.FromMinutes(30) ? true : false;
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
        static string ShowResult(long size)
        {
            string newStr = "";
            long newSize = size;
            if ((newSize = size / (long)Math.Pow(1024, 3)) > 10)
                newStr = $"{newSize} ГБ";
            else if ((newSize = size / (long)Math.Pow(1024, 2)) > 10)
                newStr = $"{newSize} МБ";
            else if ((newSize = size / 1024) > 10)
                newStr = $"{newSize} КБ";
            else
                newStr = $"{newSize} байт";
            return newStr;
        }
    }
}
