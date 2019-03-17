using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Div
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ClearLabels();

            if (IsPostBack)           
                Calc();
            

        }

        private void ClearLabels()
        {
            label1.Text = string.Empty;
            label2.Text = string.Empty;
            label3.Text = string.Empty;
          
        }

        private void Calc()
        {
            double a, b;

            double.TryParse(textBox1.Text, out a);
            double.TryParse(textBox2.Text, out b);

            label3.Text = (a / b).ToString();            
        }     
    }
}