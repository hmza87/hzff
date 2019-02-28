using hzff.classes;
using hzff.views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hzff
{
    public partial class Form1 : Form
    {
        public List<hzFolder> list;
        public Form1()
        {
            InitializeComponent();
            Timer t = new Timer();
            t.Interval = 1000;
            t.Tick += (object s, EventArgs ee) =>
            {
                init(t);
            };

            t.Start();



            
        }

        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var bitmap = new Bitmap(120, 120);
            var g = Graphics.FromImage(bitmap);
            g.DrawImage(hzff.Properties.Resources.folder_back, new Rectangle(0, 0, 120, 120));
            g.DrawImage(hzff.Properties.Resources.Folder_1, new Rectangle(0, 0, 60, 60));
            var f = new Font("Arial", 20);
            g.DrawString("haha", f, Brushes.Red, new PointF(0, 0));

            using (var o = new SaveFileDialog())
            {
                if (o.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    bitmap.Save(o.FileName);
                }
            }
            
        }

        void init(Timer t)
        {
            t.Stop();
            createNotif();
            var db = new DB();
            list = db.get_folders();
            foreach (var folder in list)
            {
                var item = new ToolStripMenuItem();
                item.Text = folder.name;
                //item.Image = Image.FromFile(folder.icon);
                menu.Items.Insert(0, item);
                item.Click += (object o, EventArgs e) =>
                {
                    if(Control.ModifierKeys == Keys.Shift)
                        utils.openFile(Application.ExecutablePath, "-f " + folder.id_folder);
                };
                foreach (var file in folder._files)
                {
                    var itm = new ToolStripMenuItem();
                    itm.Text = file.title;
                    itm.Image = Image.FromFile(file.icon);
                    item.DropDownItems.Add(itm);
                    itm.Click += (object o, EventArgs e) =>
                    {
                        utils.openFile(file.path);
                    };
                }

            }


        }

        public hzFolder getLazyFolder(int id)
        {
            foreach (var folder in list)
            {
                if (folder.id_folder == id)
                {
                    return folder;
                }
            }
            return null;
        }

        public bool launchFolder(int id,  Point position)
        {
            var folder = getLazyFolder(id);
            return folder != null ? (position == null ? folder.show() : folder.show(position)) : false;
            
        }



        void createNotif()
        {
            var n = new NotifyIcon();
            n.Icon = Icon.FromHandle(hzff.Properties.Resources.Folder_1.GetHicon());
            n.Text = "Fancy Folder";
            n.BalloonTipText = "Now running in the background";
            n.ShowBalloonTip(600);
            n.Visible = true;
            n.ContextMenuStrip = menu;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void createNewFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var dialog = new createFolder())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    list = (new DB()).get_folders();
                }
            }
        }


    }
}
