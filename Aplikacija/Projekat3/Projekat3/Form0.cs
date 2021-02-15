using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projekat3
{
    public partial class Form0 : Form
    {
        public Form0()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 forma = new Form1();
            forma.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Treneri forma = new Treneri();
            forma.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Kosarkasi forma = new Kosarkasi();
            forma.ShowDialog();
        }
    }
}
