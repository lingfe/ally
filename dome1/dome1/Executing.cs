using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dome1
{
    public partial class Executing : Form
    {
        public Executing()
        {
            InitializeComponent();
        }

        public Executing(string strName)
        {
            InitializeComponent();
            label1.Text =strName;
        }

        private void Executing_Load(object sender, EventArgs e)
        {

        }

    }
}
