using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestForm
{
    public partial class TestWindow : Form
    {
        public TestWindow()
        {
            InitializeComponent();
        }

        private void parentPanel_MouseMove(object sender, MouseEventArgs e)
        {
            tip1.Text = e.Location.ToString();
        }

        private void childPanel_MouseMove(object sender, MouseEventArgs e)
        {
            tip2.Text = e.Location.ToString();
        }
    }
}
