using hzff.classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hzff.views
{
    public partial class createFolder : Form
    {
        public createFolder()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new DB();
            var pic = fname.Text + DateTime.Now.Ticks.ToString();
            pic = hzFolder.createEmptyIcon(pic);
            
            var f = db.create_folder(fname.Text, fname.Text, pic);
            f.createShortcut();
            DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void createFolder_Load(object sender, EventArgs e)
        {

        }
    }
}
