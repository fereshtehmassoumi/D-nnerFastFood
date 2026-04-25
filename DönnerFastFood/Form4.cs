using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DönnerFastFood
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Manager" && textBox1 != null)
            { if (textBox2.Text == "111" && textBox2 != null)
                {
                    MessageBox.Show("Erfolgreich");
                }
                else { MessageBox.Show("Geheimnis ist nich richtig");
                    textBox2.Clear();
                }
            }
            else
                { MessageBox.Show("Benutznummer ist nicht richtig");
                textBox1.Clear();
            }


        }
    }
}
    