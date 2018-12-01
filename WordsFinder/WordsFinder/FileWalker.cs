using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace WordsFinder
{
    /// <summary>
    /// Предоставляет методы для поиска файлов в директории, включая вложенные.
    /// </summary>
    internal class FileWalker
    {
        #region Ctor

        CancellationTokenSource cancellationToken;

        private bool isCancel = false;

        /// <summary>
        ///  Инициализирует новый экземпляр класса FileWalker.
        /// </summary>
        public FileWalker()
        {          
            IncludePath = new List<string>() { @"d:\" };
            ExcludePath = new List<string>() { "" };
            IncludeExtension = new List<string>() { "*.txt" };
            ExcludeExtension = new List<string>() { "" };
        }

        public FileWalker(CancellationTokenSource cancellationToken)
        {
            IncludePath = new List<string>() { @"e:\" };
            ExcludePath = new List<string>() { "" };
            IncludeExtension = new List<string>() { "*.txt" };
            ExcludeExtension = new List<string>() { "" };
            this.cancellationToken = cancellationToken;
        }



        /// <summary>
        /// Инициализирует новый экземпляр класса FileWalker.
        /// </summary>
        /// <param name="includePath">Список включаемых путей.</param>
        /// <param name="excludePath">Список исключаемых путей.</param>
        /// <param name="includeExtension">Список включаемых расширений.</param>
        /// <param name="excludeExtension">Список исключаемых расширений.</param>
        public FileWalker(IEnumerable<String> includePath, IEnumerable<String> excludePath, IEnumerable<String> includeExtension, IEnumerable<String> excludeExtension)
        {
            IncludePath = includePath;
            ExcludePath = excludePath;
            IncludeExtension = includeExtension;
            ExcludeExtension = excludeExtension;
        }

#if DEBUG
        /// <summary>
        /// Полезен для обеспечения того, чтобы объекты должным образом были обработаны сборщиком мусора.
        /// </summary>
        ~FileWalker()
        {
            Debug.WriteLine(string.Format("{0} ({1}) Finalized", GetType().Name, GetHashCode()));
        }
#endif

        #endregion

        #region Properties

        /// <summary>
        /// Возвращает или задает список включаемых путей.
        /// </summary>
        private IEnumerable<String> IncludePath { get; set; }

        /// <summary>
        /// Возвращает или задает список исключаемых путей.
        /// </summary>
        private IEnumerable<String> ExcludePath { get; set; }

        /// <summary>
        /// Возвращает или задает список включаемых расширений.
        /// </summary>
        private IEnumerable<String> IncludeExtension { get; set; }

        /// <summary>
        /// Возвращает или задает список исключаемых расширений.
        /// </summary>
        private IEnumerable<String> ExcludeExtension { get; set; }

        #endregion

        #region Public Members

        /// <summary>
        /// Возвращает имена файлов (включая пути) из указанного каталога.
        /// </summary>
        /// <returns>Массив String содержит имена файлов из указанного каталога.Имена файлов включают полный путь к файлу.</returns>
        public IEnumerable<String> GetFiles()
        {
            // Создаем список
            var list = new List<String>();
            // Проверяем список включаемых папок
            if (IncludePath != null)
            {
                using (var enumerator = IncludePath.GetEnumerator())
                {
                    // Проходим все пути
                    while (enumerator.MoveNext())
                    {
                        // Проверяем список строк поиска
                        if (IncludeExtension != null)
                        {
                            using (var iterator = IncludeExtension.GetEnumerator())
                            {
                                // Проходим все строки поиска
                                while (iterator.MoveNext())
                                {
                                    // Проверяем папку в исключенных
                                    if (!ExcludePath.Contains(enumerator.Current, StringComparer.OrdinalIgnoreCase))
                                    {
                                        // Получаем файлы
                                        list.AddRange(GetFiles(enumerator.Current, iterator.Current));
                                        if (isCancel)
                                        {
                                            list.Clear();
                                            return list;
                                        }
                                            
                                    }
                                }
                            }
                        }
                    }
                }
            }
            // Возвращаем результат
            return list;
        }

        #endregion

        #region Private Members

        /// <summary>
        /// Возвращает имена файлов (включая пути) из указанного каталога, отвечающие условиям заданного шаблона поиска.
        /// </summary>
        /// <param name="directory">Каталог, в котором необходимо выполнить поиск.</param>
        /// <param name="searchPattern">Строка поиска, которую необходимо сравнить с именами файлов, на которые указывает путь</param>
        /// <returns>Массив String содержит имена файлов из указанного каталога, отвечающие условиям заданного шаблона поиска.Имена файлов включают полный путь к файлу.</returns>
        private IEnumerable<String> GetFiles(String directory, String searchPattern)
        {
            // Создаем список
            var list = new List<String>();
            // Итератор по файлам

            ParallelOptions parallelOptions = new ParallelOptions();
            parallelOptions.CancellationToken = cancellationToken.Token;
            parallelOptions.MaxDegreeOfParallelism = System.Environment.ProcessorCount;
            
           

            using (var iterator = Directory.EnumerateFiles(directory, searchPattern).GetEnumerator())
            {
                try
                {
                    while (iterator.MoveNext())
                    {
                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                        // Получаем расширение файла и проверяем его
                        var fileInfo = new FileInfo(iterator.Current);
                        if (!ExcludeExtension.Contains(String.Concat("*", fileInfo.Extension)))
                        {
                         
                            // Добавляем в список
                            list.Add(iterator.Current);
                        }
                    }
                }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show(ex.Message);
                    isCancel = true;
                    list.Clear();
                    return list;
                }
                catch (ArgumentException ex)
                {
                    // Путь является строкой нулевой длины, содержит только пробелы или содержит недопустимые символы.
                    // Параметр поиска не является допустимым значением.
                    Debug.Print(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    // Путь недопустим, например, ссылается на неподключенный диск.
                    Debug.Print(ex.Message);
                }
                catch (IOException ex)
                {
                    // Путь является именем файла.
                    // Длина указанного пути, имени файла или обоих параметров превышает установленный системой предел.
                    Debug.Print(ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    // Исключение, возникающее в случае запрета доступа операционной системой 
                    // из-за ошибки ввода-вывода или особого типа ошибки безопасности.
                    Debug.Print(ex.Message);
                }
               
            }
            // Итератор по директориям
            using (var iterator = Directory.EnumerateDirectories(directory).GetEnumerator())
            {                
                while (iterator.MoveNext())
                {                   
                    try
                    {
                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                        // Проверяем папку в исключенных
                        if (!ExcludePath.Contains(iterator.Current, StringComparer.OrdinalIgnoreCase))
                        {
                           
                            list.AddRange(GetFiles(iterator.Current, searchPattern));
                            if (isCancel)
                            {
                                list.Clear();
                                return list;
                            }
                        }
                    }
                    catch (OperationCanceledException ex)
                    {
                        MessageBox.Show(ex.Message);
                        isCancel = true;
                        list.Clear();
                        return list;
                    }
                    catch (ArgumentException ex)
                    {
                        // Путь является строкой нулевой длины, содержит только пробелы или содержит недопустимые символы.
                        // Параметр поиска не является допустимым значением.
                        Debug.Print(ex.Message);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        // Путь недопустим, например, ссылается на неподключенный диск.
                        Debug.Print(ex.Message);
                    }
                    catch (IOException ex)
                    {
                        // Путь является именем файла.
                        // Длина указанного пути, имени файла или обоих параметров превышает установленный системой предел.
                        Debug.Print(ex.Message);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        // Исключение, возникающее в случае запрета доступа операционной системой 
                        // из-за ошибки ввода-вывода или особого типа ошибки безопасности.
                        Debug.Print(ex.Message);
                    }
                }
            }
            // Возвращаем результат
            return list;
        }

        #endregion
    }
}
