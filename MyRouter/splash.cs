using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyRouter
{
    public partial class splash : Form
    {
        public splash()
        {
            InitializeComponent();
        }

        Timer t;
        private void splash_Shown(object sender, EventArgs e)
        {
            t = new Timer();
            t.Interval = 2000;
            t.Start();
            t.Tick += new EventHandler(t_Tick);
        }

        void t_Tick (object sender, EventArgs e)
        {
            t.Stop();
            Form form1 = new Form1();
            form1.Show();
            this.Hide();

        }

        private void splash_Load(object sender, EventArgs e)
        {

        }
    }
}
