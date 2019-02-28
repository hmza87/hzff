using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using hzff.classes;
using System.Diagnostics;

namespace hzff
{
     
    public partial class nFolder : DevComponents.DotNetBar.Metro.MetroForm
    {
        public hzFolder afolder;
        public nFolder()
        {

            InitializeComponent();
            this.ResizeEnd += folder_ResizeEnd;

            this.AllowDrop = true;
            this.KeyPreview = true;
            lst.AllowDrop = true;
            this.DragEnter += folder_DragEnter;
            this.DragOver += folder_DragOver;
            this.DragDrop += folder_DragDrop;
            lst.DragDrop += folder_DragDrop;
            lst.DragOver += folder_DragOver;
            lst.DragEnter += folder_DragEnter;
            this.Deactivate += wFolder_Leave;

        }

        private void nFolder_Load(object sender, EventArgs e)
        {
            if (afolder == null) Close();
            this.Text = afolder.title;
            initItems();
        }

        void folder_DragDrop(object sender, DragEventArgs e)
        {
            if (this.AccessibleDescription == "dragging")
            {
                MessageBox.Show(((ButtonX)this.Tag).Text);
            }
            else
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                {
                    afolder.addFileByPath(file);
                }

                initItems();
            }



        }

        void folder_DragOver(object sender, DragEventArgs e)
        {
            //throw new NotImplementedException();
        }

        void folder_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        void folder_ResizeEnd(object sender, EventArgs e)
        {
            initItems();
        }

        void initItems()
        {
            lst.Controls.Clear();
            var point = new Point(-100, 0);
            foreach (var item in afolder._files)
            {
                point.X = point.X + 100;
                if ((point.X + 96) > lst.Width)
                {
                    point.X = 0;
                    point.Y = point.Y + 100;
                }
                var btn = new ButtonX();
                lst.Controls.Add(btn);
                if (item.icon != null && item.icon != "" && System.IO.File.Exists(item.icon))
                {
                    btn.Image = Image.FromFile(item.icon);
                }
                else
                {
                    btn.Image = hzff.Properties.Resources.Folder_1;
                }

                btn.ImageFixedSize = new Size(32, 32);
                btn.ImagePosition = eImagePosition.Top;
                RoundRectangleShapeDescriptor shape = new RoundRectangleShapeDescriptor(3);
                btn.Shape = shape;
                btn.Size = new Size(96, 96);
                btn.Left = point.X;
                btn.Top = point.Y;
                btn.Text = wrapName(item.title);
                btn.FocusCuesEnabled = false;
                btn.ThemeAware = false;
                btn.BackColor = Color.Transparent;
                btn.AutoCheckOnClick = false;
                btn.Checked = false;
                btn.ColorTable = eButtonColor.Flat;
                btn.ContextMenuStrip = fileMenu;
                btn.Tag = item;

                btn.MouseDown += (object s, MouseEventArgs e) =>
                {
                    if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    {
                        fileMenu.Tag = btn;
                    }
                    else if (e.Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        btn.AccessibleDescription = "down";
                    }

                };
                btn.MouseUp += (object s, MouseEventArgs e) =>
                {
                    btn.AccessibleDescription = "up";
                    if (btn.AccessibleDescription == "drag")
                    {

                        this.AccessibleDescription = "";
                    }
                };
                btn.MouseMove += (object s, MouseEventArgs e) =>
                {
                    if (btn.AccessibleDescription == "down")
                    {
                        btn.AccessibleDescription = "drag";
                        this.AccessibleDescription = "dragging";
                        this.Tag = btn;
                        string[] files = { item.path };
                        var data = new DataObject(DataFormats.FileDrop, files);
                        this.DoDragDrop(data, DragDropEffects.Copy);
                    }
                };



                btn.Click += (object s, EventArgs e) =>
                {
                    Process.Start(item.path);
                    this.Close();
                };



            }


            if (afolder._files.Count == 0)
            {
                lst.Text = "Folder is empty";
            }
            else
            {
                lst.Text = "";
            }

            this.Focus();
        }
        public string wrapName(string name)
        {
            var wrapped = name;

            if (name.Length > 14) wrapped = name.Substring(0, 14) + "..";


            return wrapped;
        }

        private void wFolder_Leave(object sender, EventArgs e)
        {
            Close();
        }

        private void supprimerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void lst_Click(object sender, EventArgs e)
        {

        }

        private void createDesktopShortcutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            afolder.createShortcut();
        }

        private void nFolder_SettingsButtonClick(object sender, EventArgs e)
        {
            settings.Show((Control) sender, new Point(0,0));
        }
    }
}