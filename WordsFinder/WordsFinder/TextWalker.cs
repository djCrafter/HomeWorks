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
    public class TextWalker
    {
        CancellationTokenSource cancellationToken;

        public TextWalker(CancellationTokenSource cancellationToken)
        {
            this.cancellationToken = cancellationToken;
        }
         
        public void ScanFile(string path, IEnumerable<string> fWords)
        {
            string text = null;
             
            if (File.Exists(path))
            {
                Stream fs = new FileStream(path, FileMode.Open);
                using (StreamReader sr = new StreamReader(fs, Encoding.Default))
                    text = sr.ReadToEnd();

                //text = File.ReadAllText(path, Encoding.Default); //Альтернатива
            }


            StringBuilder sb = new StringBuilder();

            if(text.Length > 0)
            {
                using(var enumerator = fWords.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if(text.Contains(enumerator.Current))
                        {
                            //if (text.Length > 100)
                            //    sb.Append(text, 0, 100);
                            //else
                            //    sb.Append(text, 0, text.Length);

                            //MessageBox.Show(sb.ToString());


                            string newPath = @"CopyTxt\" + Path.GetFileName(path);
                            FileInfo fileInf = new FileInfo(path);
                            if (fileInf.Exists)
                            {
                                try
                                {
                                    fileInf.CopyTo(newPath, true);
                                    // альтернатива с помощью класса File
                                    // File.Copy(path, newPath, true);
                                }
                                catch(Exception e)
                                {
                                   
                                }
                            }
                        }
                    }
                }
            }

        }
    }
}
