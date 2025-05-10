using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Runtime.InteropServices;
using Microsoft.Office.Core;
using PowerPoint = Microsoft.Office.Interop.PowerPoint;
using System.IO;
using System.Web;

namespace HelloPoint
{
    public class ConvertPowerPoint
    {

        //Converts a PowerPoint file into a mp4 video with variable 
        //slide time
        public void GetVideoFromPpt(HttpPostedFileBase f,string filename, int SlideTime)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Combine the base folder with your specific folder....
            var path = appData + @"\HelloPoint\tmp\";
            // Check if folder exists and if not, create it
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var newpath = path + filename+".ppt";
            using (System.IO.FileStream output = new System.IO.FileStream(newpath, System.IO.FileMode.Create))
            {
                f.InputStream.CopyTo(output);
            }

            var resolution = 720;
            var fps = 24;

            var app = new PowerPoint.Application();
            var presentation = app.Presentations.Open(newpath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);

            var mp4file = filename + ".mp4";
            var fullPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HelloPoint\upfiles";

            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            try
            {
                presentation.CreateVideo(fullPath + "\\" + mp4file, true, SlideTime, resolution, fps, 85);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                app.Quit();
            }       
        }        
    }


}
