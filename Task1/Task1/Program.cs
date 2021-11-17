using System;
using System.IO;
/// <summary>
/// Первое задание
/// </summary>
namespace Task1
{
    /// <summary>
    /// Основной класс программы
    /// </summary>
    class Program
    {
        /// <summary>
        /// Точка входа
        /// </summary>
        static void Main()
        {
            var directory = new DirectoryInfo(System.IO.Directory.GetCurrentDirectory());
            var taskDirectory = directory.Parent.Parent.Parent.Parent;
            directory = new DirectoryInfo(String.Concat(taskDirectory.FullName, @"\TestFolder"));
            if (directory.Exists)
            {
                try
                {
                    DeleteOldFiles(directory);
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
        static bool DeleteOldFiles(DirectoryInfo directory)
        {
            try
            {
                var files = directory.GetFiles();
                foreach (var file in files)
                {
                    if (CheckLastAccessOver30min(file.LastAccessTime))
                    {
                        Console.WriteLine($"{file.Name} удален");
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
                    if (DeleteOldFiles(dir))
                    {
                        Console.WriteLine($"{dir.Name} удален");
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
    }
}