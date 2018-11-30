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
        FileWalker fileWalker = new FileWalker();

        List<string> allPath = new List<string>();

        CancellationTokenSource cancelToken = new CancellationTokenSource();


        /// <summary>
        ///  Инициализирует новый экземпляр класса Core.
        /// </summary>
        public Core()
        {
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

        public async Task<bool> LoadAllPath()
        {
           return await Task.Run(() =>
            {
                allPath.Clear();

                ParallelOptions paropt = new ParallelOptions();
                paropt.CancellationToken = cancelToken.Token;
                paropt.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

                var result = fileWalker.GetFiles();

                Thread.Sleep(3000);
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
                //MessageBox.Show(sb.ToString());

                return true;
            });           
        }

        public void CancelOperation()
        {
            cancelToken.Cancel();
        }
        
    }
}
