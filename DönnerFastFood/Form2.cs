using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace DönnerFastFood
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, 30, 30), 180, 90);
            path.AddArc(new Rectangle(this.Width - 30, 0, 30, 30), 270, 90);
            path.AddArc(new Rectangle(this.Width - 30, this.Height - 30, 30, 30), 0, 90);
            path.AddArc(new Rectangle(0, this.Height - 30, 30, 30), 90, 90);
            path.CloseFigure();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
