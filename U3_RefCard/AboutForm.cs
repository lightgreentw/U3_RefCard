using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace U3_RefCard
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();

            Assembly assembly = Assembly.GetExecutingAssembly();
            Version assemblyVersion = assembly.GetName().Version;
            
            lab_Version.Text = "Ver " + assemblyVersion;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
