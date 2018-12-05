using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;


namespace WordsFinder
{
    /// <summary>
    /// Класс содержащий основную логику программы
    /// </summary>
    public class Core
    {
        public string Words { get; set; }

        private string drive;

        FileWalker fileWalker;
        TextWalker textWalker;

        List<string> allPath = new List<string>();

        CancellationTokenSource cancelToken = new CancellationTokenSource();
     
        /// <summary>
        ///  Инициализирует новый экземпляр класса Core.
        /// </summary>
        public Core(string drive)
        {
            this.drive = drive;
            fileWalker = new FileWalker(cancelToken, this.drive);
            textWalker = new TextWalker(cancelToken);
            CreateDirs();
        }

        public string OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return "";

            string fileName = openFileDialog.FileName;
           
            return File.ReadAllText(fileName);
        }      

        public async Task<List<string>> LoadAllPath()
        {
           return await Task.Run(() =>
            {
                allPath.Clear();
                


                ParallelOptions paropt = new ParallelOptions();
                paropt.CancellationToken = cancelToken.Token;
                paropt.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

                var result = fileWalker.GetFiles();

             
                try
                {
                    foreach (var item in result)
                    {                     
                        allPath.Add(item);
                    }                
               }
                catch (OperationCanceledException ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //StringBuilder sb = new StringBuilder();


                //foreach (string item in allPath)
                //{
                //    sb.Append(item + "\n");
                //}
                //MessageBox.Show(sb.ToString() );

                if (cancelToken.IsCancellationRequested)
                    return null;


                string[] str = Words.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in allPath)
                {
                    textWalker.ScanFile(item, str);
                }


                return allPath;
            });           
        }

        public async Task<bool> TextWalk(string path)
        {
            return await Task.Run(() =>
            {
                MessageBox.Show(Thread.CurrentThread.ManagedThreadId.ToString());

                string[] str = Words.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string item in allPath)
                {
                    textWalker.ScanFile(item, str);
                    if (cancelToken.IsCancellationRequested)
                    {
                        return false;
                    }
                }
                return true;
            });
        }

        public void CancelOperation()
        {
            cancelToken.Cancel();
        }
        
        private void CreateDirs()
        {
            string path = @".\CopyTxt";           
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
          
        }
        
    }
}
