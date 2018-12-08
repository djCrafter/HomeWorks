using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Windows.Threading;
using System.Threading;


namespace TextFinder
{
    public class Core
    {     
        Progress progress = null;
        FileWalker fileWalker;
        TextWalker textWalker;

        List<string> allPath = new List<string>();
        string[] words = null;

        string tempLabel = null;
        string storePath = null;

        public bool Pause { get; set; }
        public bool Cancel { get; set; }
        public string Drive { get; set; }     
        public string Temp
        {          
            set
            {
                if (progress != null)
                    progress.label.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        progress.label.Content = value.ToString();
                    }));
            }
        }
        

        public Core(string drive, string words)
        {
            fileWalker = new FileWalker(drive, "*.txt", this);
            textWalker = new TextWalker();

            Pause = false;
            Cancel = false;

            Drive = drive;
            this.words = words.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            textWalker.DeleteAllFiles();
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

                var result = fileWalker.GetFiles();

                foreach(var item in result)
                {
                    allPath.Add(item);
                }


                //StringBuilder sb = new StringBuilder();


                //foreach (string item in allPath)
                //{
                //    sb.Append(item + "\n");
                //}
                //MessageBox.Show(sb.ToString());

                return allPath;
            });
        }


         
        public async Task TextWalk(List<string> allPath)
        {
            await Task.Run(() =>
            {
                storePath = textWalker.CreateStore(Drive);

                foreach (string item in allPath)
                {
                    progress.label.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        progress.label.Content = item;
                    }));

                    progress.progressBar.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
                    {
                        progress.progressBar.Value++;
                    }));


                    if (Pause)
                    {
                        while (Pause)
                        {
                            Thread.Sleep(200);
                        }
                    }
                    if (Cancel)
                    {
                        return;
                    }
                   
                    textWalker.ScanFileAndCopy(item, words, Drive);                    
                }
            });
        }
      

        public void CancelSearch()
        {
            fileWalker.Cancel = true;
            Cancel = true;
        }
       
        public void PauseSearch()
        {
            if (Pause)
            {
                progress.labelStatic.Content = tempLabel;
                fileWalker.Pause = false;
                Pause = false;
            }
            else
            {
                tempLabel = progress.labelStatic.Content.ToString();
                progress.labelStatic.Content = "Процесс приостановлен...(Пауза).";
                fileWalker.Pause = true;
                Pause = true;
            }
        }

        public void SetProgressWindow(Progress progress)
        {
            this.progress = progress;
        }

    }
}
