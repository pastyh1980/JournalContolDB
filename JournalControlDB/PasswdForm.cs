using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JournalControlDB
{
    public partial class PasswdForm : Form
    {
        public string pass;

        public PasswdForm()
        {
            InitializeComponent();
        }

        private void acceptBtn_Click(object sender, EventArgs e)
        {
            if (passTxt.Text != "")
            {
                DialogResult = DialogResult.OK;
                pass = passTxt.Text;
            }
            Close();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
