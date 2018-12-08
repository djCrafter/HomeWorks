using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace TextFinder
{
    public class FileWalker
    {
        private string directory;
        private string searchPathern;

        private Core core;
        
        public bool Pause { get; set; }
        public bool Cancel { get; set; }

        public FileWalker(string directory, string searchPathern, Core core)
        {
            Pause = false;
            Cancel = false;
            this.core = core;

            this.directory = directory;
            this.searchPathern = searchPathern;
        }


        public IEnumerable<string> GetFiles()
        {
            return GetFiles(directory, searchPathern);
        }

        private IEnumerable<string> GetFiles(string directory, string searchPathern)
        {
            var list = new List<string>();

            using (var iterator = Directory.EnumerateFiles(directory, searchPathern).GetEnumerator())
            {
                try
                {
                    while (iterator.MoveNext())
                    {
                        if (Pause)
                        {
                            while (Pause)
                            {
                                Thread.Sleep(200);
                            }
                        }
                        if (Cancel)
                        {
                            list.Clear();
                            break;                           
                        }

                        var fileInfo = new FileInfo(iterator.Current);

                        list.Add(iterator.Current);
                    }
                }
                catch (ArgumentException ex)
                {
                    Debug.Print(ex.Message);
                }
                catch (DirectoryNotFoundException ex)
                {
                    Debug.Print(ex.Message);
                }
                catch (IOException ex)
                {
                    Debug.Print(ex.Message);
                }
                catch (UnauthorizedAccessException ex)
                {
                    Debug.Print(ex.Message);
                }
            }

            using (var iterator = Directory.EnumerateDirectories(directory).GetEnumerator())
            {
                while (iterator.MoveNext())
                {
                    try
                    {
                        if (Pause)
                        {
                            while (Pause)
                            {
                                Thread.Sleep(200);
                            }
                        }
                        if (Cancel)
                        {
                            list.Clear();
                            break;
                        }
                       
                        list.AddRange(GetFiles(iterator.Current, searchPathern));
                        TempPathChange(iterator.Current.ToString());

                    }
                    catch (ArgumentException ex)
                    {

                        Debug.Print(ex.Message);
                    }
                    catch (DirectoryNotFoundException ex)
                    {
                        Debug.Print(ex.Message);
                    }
                    catch (IOException ex)
                    {
                        Debug.Print(ex.Message);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        Debug.Print(ex.Message);
                    }
                }
            }

            return list;
        }

        private void TempPathChange(string tempPath)
        {
            core.Temp = tempPath;
        }

    }
}
