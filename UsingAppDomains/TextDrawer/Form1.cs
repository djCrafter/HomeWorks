﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TextDrawer
{
    public partial class Form1 : Form
    {
        string SourceText = "No text was added!";
        Font DrawingFont;

        public Form1()
        {
            InitializeComponent();

            DrawingFont = new Font("Arial", 45);
            panel1.Paint += Panel1_Paint;
            this.Paint += Form1_Paint;
        }
 
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Panel1_Paint(panel1, new PaintEventArgs(panel1.CreateGraphics(), panel1.ClientRectangle));
        }

        private void Panel1_Paint(object sender, PaintEventArgs e)
        {
           if(SourceText.Length > 0)
            {
                Image img = new Bitmap(panel1.ClientRectangle.Width, panel1.ClientRectangle.Height);
                Graphics imgDC = Graphics.FromImage(img);
                imgDC.Clear(BackColor);
                imgDC.DrawString(SourceText, DrawingFont, Brushes.Brown, ClientRectangle, new StringFormat(StringFormatFlags.NoFontFallback));
                e.Graphics.DrawImage(img, 0, 0);
            }
        }

        private void FontToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            FontDialog dlg = new FontDialog();

            dlg.Font = DrawingFont;

            if (dlg.ShowDialog() == DialogResult.OK)
                DrawingFont = dlg.Font;

            Panel1_Paint(panel1, new PaintEventArgs(panel1.CreateGraphics(), panel1.ClientRectangle));
        }

        public void SetText(string text)
        {
            SourceText = text;

            Panel1_Paint(panel1, new PaintEventArgs(panel1.CreateGraphics(), panel1.ClientRectangle));
        }

        public void Move(Point newLocation, int width)
        {
            this.Location = newLocation;

            this.Width = width;
        }
    }
}
