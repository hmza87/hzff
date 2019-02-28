using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IWshRuntimeLibrary;

using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Drawing.Imaging;
using hzff.views;

namespace hzff.classes {  
    public class hzFolder
    {
        public string name { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public int id_folder { get; set; }
        public string create_date { get; set; }
        public string maj_date { get; set; }

        public List<hzShortcut> _files;


        public static string createEmptyIcon(string filename)
        {
            var fn = String.Format(Application.StartupPath + @"\icons\folders\{0}.png", filename);
            var bitmap = new Bitmap(120, 120);
            var g = Graphics.FromImage(bitmap);
            g.DrawImage((Image) hzff.Properties.Resources.fb_1, new Rectangle(0, 0, 120, 120));
            var icon = classes.HelperIcon.PngIconFromImage(bitmap, 120);

            using (FileStream fs = new FileStream(fn, FileMode.Create))
            {
                icon.Save(fs);
            }
            return fn;
        }

        public void updateIcon()
        {
            var fn = this.icon;
            var bitmap = new Bitmap(120, 120);
            bitmap.SetResolution(320, 320);
            var g = Graphics.FromImage(bitmap);
            g.DrawImage(hzff.Properties.Resources.fb_1, new Rectangle(0, 0, 120, 120));

            var index = 1;
            foreach (var file in this._files)
            {
                Rectangle rec;
                if (index == 1)
                    rec = new Rectangle(5,5, 50, 50);
                else if (index == 2)
                    rec = new Rectangle(65, 5, 50, 50);
                else if (index == 3)
                    rec = new Rectangle(5, 65, 50, 50);
                else if (index == 4)
                    rec = new Rectangle(65, 65, 50, 50);
                else
                    break;

                if (System.IO.File.Exists(file.icon))
                {
                    var img = Image.FromFile(file.icon);
                    g.DrawImage(img, rec);
                    index++;
                }
                
            }


            var icon = classes.HelperIcon.PngIconFromImage(bitmap, 120);

            using (FileStream fs = new FileStream(fn, FileMode.Create))
            {
                icon.Save(fs);
            }
            utils.refreshDesktop();
            
        }

        public hzFolder(string name, string title, string icon, int id, string cdate, string udate)
        {
            this.name = name; this.title = title; this.icon = icon; this.id_folder = id;
            this.maj_date = udate; this.create_date = cdate;
            _files = get_files();
        }
        public hzFolder(string name, string title)
        {
            this.name = name; this.title = title;
            this.maj_date = DateTime.Now.ToShortTimeString();
            this.create_date = DateTime.Now.ToShortTimeString();
            _files = get_files();
        }

        public hzFolder(int id)
        {
            var f = (new DB()).get_folder(id);
            if (f == null) throw new Exception("No folder with this id");
            this.name = f.name;
            this.title = f.title;
            this.id_folder = f.id_folder;
            this.icon = f.icon;
            this.create_date = f.create_date;
            this.maj_date = f.maj_date;
            this.icon = f.icon;
            _files = get_files();
        }

        public void create()
        {
            var db = new DB();
            db.create_folder(this.name, this.title, this.icon);
        }

        public List<hzShortcut> get_files()
        {
            var db = new DB();
            return db.get_files(this.id_folder) ?? new List<hzShortcut>();
        }

        public void createShortcut()
        {
            //hzDesktopFolders.Properties.Resources.folder_blue.Save(this.name, System.Drawing.Imaging.ImageFormat.Icon);
            var dir = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            //var iconpath = dir + @"\icons\folder_blue.ico";

            

            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\"+this.name+".lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = this.name;
            shortcut.IconLocation = this.icon;
            shortcut.Hotkey = "Ctrl+Shift+N";
            shortcut.WorkingDirectory = dir;
            shortcut.Arguments = "-f " + this.id_folder;
            shortcut.TargetPath = Application.ExecutablePath;
            shortcut.Save();

        }

        public bool show()
        {
            var f = new nFolder();
            f.afolder = this;
            f.Show();
            f.Focus();
            return true;
        }
        public bool show(Point cursorPosition)
        {
            var f = new nFolder();
            f.afolder = this;
            f.Show();
            f.Focus();

            var left = cursorPosition.X - (f.Width / 2);
            var top = cursorPosition.Y - (f.Height / 2);
            if (top < 0) top = 0;
            else if (top > (Screen.PrimaryScreen.Bounds.Height - 50)) top = (Screen.PrimaryScreen.Bounds.Height - 50) - f.Height;

            if (left < 0) left = 0;
            if (left > Screen.PrimaryScreen.Bounds.Width) left = Screen.PrimaryScreen.Bounds.Left - f.Width;

            f.Top = top; f.Left = left;
            return true;
        }

        public void addFileByPath(string filepath)
        {
            if (System.IO.File.Exists(filepath))
            {
            var fi = new System.IO.FileInfo(filepath);
  
            var iconpath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\icons\" + utils.randomID(20) + ".png";
            utils.getFileIcon(filepath).Save(iconpath);

            var shortcut = new hzShortcut(fi.Name, fi.Name, filepath, iconpath, this.id_folder);
            shortcut.create();
            this._files = this.get_files();
            this.updateIcon();
            }
            
        }

    } }

  

