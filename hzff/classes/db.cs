using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hzff.classes
{
    class DB
    {
        public string db_path = "db.db";
        SQLiteConnection cnx;
        public DB()
        {
            init();
        }

        private void init()
        {
            var cr = false;
            if (!File.Exists(db_path))
            {
                SQLiteConnection.CreateFile(db_path);
                cr = true;

            }
            cnx = new SQLiteConnection(String.Format("Data Source={0};Version=3;", db_path));
            if (cr)
            {
                cnx.Open();
                string sql = hzff.Properties.Resources.db;
                SQLiteCommand command = new SQLiteCommand(sql, cnx);
                command.ExecuteNonQuery();
                cnx.Close();
            }
           
        }

        public List<hzFolder> get_folders()
        {
            var data = new List<hzFolder>();

            cnx.Open();
            string sql = String.Format("select * from folders order by name asc");
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                data.Add(new hzFolder(reader["name"].ToString(), reader["title"].ToString(), reader["icon"].ToString(), int.Parse(reader["id"].ToString()), reader["create_date"].ToString(), reader["maj_date"].ToString(), int.Parse(reader["height"].ToString() ?? "0"), int.Parse(reader["width"].ToString() ?? "0")));
            }


            return data;
        }

        public List<hzShortcut> get_files(int id_folder)
        {
            var data = new List<hzShortcut>();

            cnx.Open();
            string sql = String.Format("select * from files where id_folder = '{0}' AND deleted = 0 order by name asc", id_folder);
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            SQLiteDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                //var b = new Bitmap(hzDesktopFolders.Properties.Resources.Twitter);
                data.Add(new hzShortcut(int.Parse(reader["id"].ToString()) , reader["name"].ToString(), reader["title"].ToString(), reader["path"].ToString(), reader["icon"].ToString(), id_folder));
            }


            return data;
        }
        public hzFolder get_folder(int id)
        {
            

            cnx.Open();
            string sql = String.Format("select * from folders where id = '{0}'", id);
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            SQLiteDataReader reader = command.ExecuteReader();
            hzFolder folder = null;

            while (reader.Read())
            {
                folder = new hzFolder(reader["name"].ToString(), reader["title"].ToString(), reader["icon"].ToString(), id, reader["create_date"].ToString(), reader["maj_date"].ToString(), int.Parse(reader["height"].ToString()), int.Parse(reader["with"].ToString()));
            }

            cnx.Close();

            return folder;
        }

        public hzFolder create_folder(string name, string title, string icon)
        {
            cnx.Open();
            string sql = String.Format("insert into folders (name, title, create_date, icon) values ('{0}', '{1}', '{2}', '{3}')", name, title, DateTime.Now.ToShortTimeString(), icon);
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            command.ExecuteNonQuery();
            sql = String.Format("select last_insert_rowid() as id");
            command = new SQLiteCommand(sql, cnx);
            SQLiteDataReader reader = command.ExecuteReader();
            hzFolder folder = null;

            while (reader.Read())
            {
                folder = new hzFolder(int.Parse(reader["id"].ToString()));
            }

            cnx.Close();
            return folder;
        }

        public bool delete_file(int id)
        {
            cnx.Open();
            string sql = String.Format("update files set deleted = 1 WHERE id = {0}", id.ToString());
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            command.ExecuteNonQuery();


            cnx.Close();
            return true;
        }
        public hzShortcut create_file(int id_folder, string name, string title, string path, string icon)
        {
            cnx.Open();
            string sql = String.Format("insert into files (name, title, create_date, icon, path, id_folder) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", name, title, DateTime.Now.ToShortTimeString(),icon, path, id_folder);
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            command.ExecuteNonQuery();

            
            cnx.Close();
            return new hzShortcut(name, title, path, icon, id_folder);
        }

        public void nonQuery(string sql_commande)
        {
            cnx.Open();
            string sql = sql_commande;
            SQLiteCommand command = new SQLiteCommand(sql, cnx);
            command.ExecuteNonQuery();
            cnx.Close();
           
        }
    }
}
