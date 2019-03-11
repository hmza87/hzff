using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hzff.classes
{
    public class hzShortcut
    {
        public int id { get; set; }
        public string name { get; set; }
        public string title { get; set; }
        public string icon { get; set; }
        public int id_folder { get; set; }
        public string create_date { get; set; }
        public string maj_date { get; set; }

        public string path { get; set; }

        public hzShortcut(string name, string title, string path, string icon, int id_folder, string cdate, string udate)
        {
            this.name = name; this.title = title; this.icon = icon; this.id_folder = id_folder;
            this.maj_date = udate; this.create_date = cdate;
            this.path = path;

        }
        public hzShortcut(string name, string title, string path, string icon, int id_folder)
        {
            this.name = name; this.title = title; this.icon = icon; this.id_folder = id_folder;
            this.maj_date = DateTime.Now.ToShortTimeString();
            this.create_date = DateTime.Now.ToShortTimeString();
            this.path = path;
        }


        public hzShortcut(int id, string name, string title, string path, string icon, int id_folder)
        {
            this.id = id;
            this.name = name; this.title = title; this.icon = icon; this.id_folder = id_folder;
            this.maj_date = DateTime.Now.ToShortTimeString();
            this.create_date = DateTime.Now.ToShortTimeString();
            this.path = path;
        }

        public void create()
        {
            var db = new DB();
            db.create_file(this.id_folder, this.name, this.name, this.path, this.icon);
            
        }

        public bool delete()
        {
            return (new DB()).delete_file(this.id);
        }

    }
}
