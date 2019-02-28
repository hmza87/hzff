using hzff.classes;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hzff
{
    static class Program
    {
        public static Form1 manager;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            var MainForm = new Form1();
            manager = MainForm;
            SingleInstanceApplication.Run(MainForm, NewInstanceHandler);
        }

        public static void NewInstanceHandler(object sender, StartupNextInstanceEventArgs e)
        {
            //You can add a method on your Form1 class to notify it has been started again  
            //and perhaps pass parameters to it. That is if you need to know for instance   
            //the startup parameters.  

            //MainForm.NewInstance(e);  
            //MessageBox.Show(String.Join("|", Environment.GetCommandLineArgs()));
            //Cursor currsor = new Cursor(Cursor.Current.Handle);

            string str = "";
            foreach (var s in e.CommandLine)
            {
                str += "|" + s;
            }
            var d = str.Split('|');

            handleComs(d);
            e.BringToForeground = false;
        }

        static void handleComs(string[] args)
        {
            
            for (var i = 0; i < (args.Length - 1); i++)
            {
                if (args[i] == "-f" && args[i + 1] != "")
                {
                    var id = args[i + 1];
                    var toAdd = args.Length > (i + 2);

                    if (toAdd)
                    {
                        var folder = manager.getLazyFolder(int.Parse(id));
                        if (folder != null)
                        {
                            for (var n = 0; n < args.Length; n++)
                            {
                                if (n > (i + 1))
                                {
                                    folder.addFileByPath(args[n]);
                                }
                            }
                        }
                    }

                    if (!toAdd)
                    {
                        var position = new Point(Cursor.Position.X, Cursor.Position.Y);
                        manager.launchFolder(int.Parse(id), position);
                    }
                    

                    //var folder = new hzFolder(int.Parse(args[i + 1]));

                    //folder.show(position);

                    //var folder = center.folders.Where(e => e.id_folder == int.Parse(args[i + 1])).ToList<hzFolder>();
                    //if (folder.Count > 0) folder[0].show(position);

                }
            }

        }

        public class SingleInstanceApplication : WindowsFormsApplicationBase
        {
            private SingleInstanceApplication()
            {
                base.IsSingleInstance = true;
            }

            public static void Run(Form f, StartupNextInstanceEventHandler startupHandler)
            {
             
                SingleInstanceApplication app = new SingleInstanceApplication();
                settings.init();
                var r = new System.Collections.ObjectModel.ReadOnlyCollection<string>(Environment.GetCommandLineArgs().ToList());
                
                app.StartupNextInstance += startupHandler;
                app.MainForm = f;
                app.Run(Environment.GetCommandLineArgs());



            }
        }  
    }
}
