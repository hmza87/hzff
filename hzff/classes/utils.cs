using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using Shell32;
using System.Diagnostics;
using System.Windows.Forms;

namespace hzff.classes
{

    static class utils
    {

        [System.Runtime.InteropServices.DllImport("Shell32.dll")]
        private static extern int SHChangeNotify(int eventId, int flags, IntPtr item1, IntPtr item2);
        public static void refreshDesktop()
        {
            SHChangeNotify(0x8000000, 0x1000, IntPtr.Zero, IntPtr.Zero);
        }

        public static bool isImage(string filename)
        {
            var fi = new FileInfo(filename);
            string[] extentions = {"jpeg", "jpg", "png", "gif", "bmp"};
            return extentions.Where(e => e == fi.Extension.ToLower().Replace(".", "")).ToList().Count > 0;
        }

        public static void openFile(string filename, string arguments)
        {
            if (File.Exists(filename))
            {
                if(arguments != null) {
                    Process.Start(filename, arguments);
                }else{
                    Process.Start(filename);
                }
            }
            else
            {
                MessageBox.Show(null, "The file was not found!", "404 File not found :(", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void openFile(string filename)
        {
            openFile(filename, null);
        }

        public static Image getFileIcon(string filename)
        {
            var isImage = utils.isImage(filename);
            if (isImage)
            {
                return (Bitmap.FromFile(filename)).GetThumbnailImage(120, 120, null, new IntPtr());
            }
            else
            {
                return utils.extract_icon(filename).ToBitmap();
            }
        }
        public static Icon extract_icon(string filename)
        {
            return Icon.ExtractAssociatedIcon(filename);
            if(filename == "") return null;
            var file = new FileInfo(filename);
            Icon icon = null;
            if (file.Extension.ToLower() == "exe")
            {
                icon = IconExtractor.ExtractIconLarge(filename);
            }
            else if (file.Extension.ToLower() == "lnk")
            {
                return extract_icon(GetShortcutTargetFile(filename));
            }
            else
            {


            }



            return icon;
        }

        private static Random random = new Random();
        public static string randomID(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static string GetShortcutTargetFile(string shortcutFilename)
        {
            string pathOnly = System.IO.Path.GetDirectoryName(shortcutFilename);
            string filenameOnly = System.IO.Path.GetFileName(shortcutFilename);

            Shell shell = new Shell();
            Folder folder = shell.NameSpace(pathOnly);
            FolderItem folderItem = folder.ParseName(filenameOnly);
            if (folderItem != null)
            {
                Shell32.ShellLinkObject link = (Shell32.ShellLinkObject)folderItem.GetLink;
                return link.Path;
            }

            return string.Empty;
        }
       

        public static void createIcon(Bitmap img, string fn){


            string destFileName = fn;

            // Create a Bitmap object from an image file.
            Bitmap bmp = img;

            // Get an Hicon for myBitmap. 
            IntPtr Hicon = bmp.GetHicon();

            // Create a new icon from the handle. 
            Icon newIcon = Icon.FromHandle(Hicon);

            //Write Icon to File Stream
            System.IO.FileStream fs = new System.IO.FileStream(destFileName, System.IO.FileMode.OpenOrCreate);
            newIcon.Save(fs);
            fs.Close();
            //DestroyIcon(Hicon);

            //DestroyIcon( hIcon);
            
        }


        
    }




    public class IconExtractor
    {
        [DllImport("shell32.dll", EntryPoint = "ExtractIconEx")]
        private static extern int ExtractIconExA(string lpszFile, int nIconIndex, ref IntPtr phiconLarge, ref IntPtr phiconSmall, int nIcons);

        [DllImport("user32")]
        private static extern int DestroyIcon(IntPtr hIcon);

        //Attempts to extract the small-version of the applicaiton's icon
        public static Icon ExtractIconSmall(string path)
        {
            IntPtr largeIcon = IntPtr.Zero;
            IntPtr smallIcon = IntPtr.Zero;
            ExtractIconExA(path, 0, ref largeIcon, ref smallIcon, 1);

            //Transform the bits into the icon image
            Icon returnIcon = null;
            if (smallIcon != IntPtr.Zero)
                returnIcon = Icon.FromHandle(smallIcon);

            //clean up
            DestroyIcon(largeIcon);

            return returnIcon;
        }

        //Attempts to extract the large-version of the application's icon
        public static Icon ExtractIconLarge(string path)
        {
            IntPtr largeIcon = IntPtr.Zero;
            IntPtr smallIcon = IntPtr.Zero;
            ExtractIconExA(path, 0, ref largeIcon, ref smallIcon, 1);

            //Transform the bits into the icon image
            Icon returnIcon = null;
            if (largeIcon != IntPtr.Zero)
                returnIcon = Icon.FromHandle(largeIcon);

            //clean up
            DestroyIcon(smallIcon);

            return returnIcon;
        }

        //Returns the large icon if found, if not the small icon
        public static Icon ExtractIcon(string path)
        {
            Icon largeIcon = ExtractIconLarge(path);

            if (largeIcon == null)
                return ExtractIconSmall(path);
            else
                return largeIcon;
        }
    }
}


