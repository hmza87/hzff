using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hzff.classes 
{
    static class settings
    {
        public static string ICON_DIR = @"icons";
        public static string FOLDERS_ICON_DIR = @"icons/folders";
        public static void init() {

            if (!Directory.Exists(ICON_DIR)) Directory.CreateDirectory(ICON_DIR);
            if (!Directory.Exists(FOLDERS_ICON_DIR)) Directory.CreateDirectory(FOLDERS_ICON_DIR); 
            

        }
        

    }

    
}
