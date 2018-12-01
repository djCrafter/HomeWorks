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
                text = File.ReadAllText(path);
            }

            if(text.Length > 0)
            {
                using(var enumerator = fWords.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        if(text.Contains(enumerator.Current))
                        {
                            MessageBox.Show("!!!!!");
                        }
                    }
                }
            }

        }
    }
}
