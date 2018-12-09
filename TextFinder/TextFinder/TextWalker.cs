using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace TextFinder
{
    public class TextWalker
    {               
        public TextWalker()
        {                  
        }

        public void ScanFileAndCopy(string path, IEnumerable<string> fWords, string drive)
        {
            string text = null;

            if (File.Exists(path))
            {
                try
                {
                    Stream fs = new FileStream(path, FileMode.Open);
                    using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                        text = sr.ReadToEnd();
                }
                catch(UnauthorizedAccessException e)
                {
                    Debug.Print(e.Message);
                }
                catch(Exception e)
                {
                    Debug.Print(e.Message);
                }            
            }
         
            if (text != null)
            {
                using (var enumerator = fWords.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if (text.Contains(enumerator.Current))
                        {                              
                            string newPath = @"CopyTxt\Drive_" + drive + @"\" + Path.GetFileName(path); 
                            FileInfo fileInf = new FileInfo(path);
                            if (fileInf.Exists)
                            {
                                try
                                {
                                    if (File.Exists(newPath))
                                    {                                    
                                        break;
                                    }
                                    else
                                    {
                                        File.Copy(path, newPath, true);                                      
                                    }                                
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show(e.Message);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void DeleteAllFiles()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(@"CopyTxt\");

                foreach (FileInfo item in di.GetFiles())
                {
                    item.Delete();
                }
            }
            catch(DirectoryNotFoundException e)
            {
                Debug.Print(e.Message);
            }        
        }

        
        public string CreateStore(string drive)
        {           
            string dirPath = @"CopyTxt\";
            string subpath = @"Drive_" + drive[0];

            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            dirInfo.CreateSubdirectory(subpath);

            return dirPath + subpath;
        }

        public void Replace(string drive, string[] words) 
        {
            string path = @"CopyTxt\Drive_" + drive;

            DirectoryInfo df = new DirectoryInfo(path);

            df.CreateSubdirectory("RepFiles");

            foreach(FileInfo file in df.GetFiles())
            {
               string text = File.ReadAllText(file.FullName, Encoding.Default);

                foreach(string word in words)
                {                  
                    text = text.Replace(word, new string('*', word.Length));
                }

                string newPath = path + @"\RepFiles\" + Path.GetFileNameWithoutExtension(file.FullName) + "_copy.txt";

                File.WriteAllText(newPath, text, Encoding.Default);
            }
        }
    }
}
