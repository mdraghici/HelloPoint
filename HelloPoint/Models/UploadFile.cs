using System;
using System.IO;
using System.Linq;
using System.Web;



namespace HelloPoint.Models
{
    public class UploadFile
    {

        public HttpPostedFileBase Files { get; set; }
        public string UserName { get; set; }
        public int Slideduration { get; set; }
        public string Description { get; set; }
        

        public UploadFile() : base() { }

        public void Upload()
        {
            FileDbEntities _db = new FileDbEntities();

            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Combine the base folder with your specific folder....
            var path = appData + @"\HelloPoint\upfiles";
            // Check if folder exists and if not, create it
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var f = Files;
            try
            {
                string guid = Guid.NewGuid().ToString();
                var newpath = path+ "\\" + System.IO.Path.GetFileName(guid)+ System.IO.Path.GetExtension(f.FileName);

                var maxid = 0;
                if (_db.Files.Count() != 0)
                    maxid = _db.Files.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
                // public File(int id,string u,string o,string s,string t,string d, System.DateTime ad, System.DateTime md)
                _db.Files.Add(new File(maxid, UserName, guid, f.FileName, System.IO.Path.GetExtension(f.FileName), Description, DateTime.Now.ToString("M/dd/yyyy"), DateTime.Now.ToString("M/dd/yyyy")));
                _db.SaveChanges();

                if (System.IO.Path.GetExtension(f.FileName) == ".ppt" || System.IO.Path.GetExtension(f.FileName) == ".pptx")
                {
                    ConvertPowerPoint a = new ConvertPowerPoint();
                    a.GetVideoFromPpt(f,guid, Slideduration);
                }
                else
                {
                    using (System.IO.FileStream output = new System.IO.FileStream(newpath, System.IO.FileMode.Create))
                    {
                        f.InputStream.CopyTo(output);
                    }
                }
            }
            catch (Exception e)
            {
                Controllers.ExceptionController exc = new Controllers.ExceptionController();
                exc.DatabaseError(e.ToString());
            }
           
        }



    }


    public class EditFile
    {


        public string UserName { get; set; }
        public int Slideduration { get; set; }
        public string GuidOriginal { get; set; }

        public string OldDescription { get; set; }
        public string OldFileName { get; set; }
        public string OldType { get; set; }

        public string NewDescription { get; set; }
        public HttpPostedFileBase NewFile { get; set; }

        public EditFile() : base() { }

        public void Edit(EditFile editfile)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Combine the base folder with your specific folder....
            var path = appData + @"\HelloPoint\upfiles";
            // Check if folder exists and if not, create it
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileDbEntities _db = new FileDbEntities();

            if (editfile.NewDescription != editfile.OldDescription)
            {
                try
                {
                    _db.Files.Where(x => x.SavedFileName == editfile.GuidOriginal).First().Description = editfile.NewDescription;
                    _db.SaveChanges();

                }
                catch (Exception e)
                {
                    Controllers.ExceptionController exc = new Controllers.ExceptionController();
                    exc.DatabaseError(e.ToString());
                }
            }

            if (editfile.NewFile != null)
            {
                if (Delete(editfile.GuidOriginal,editfile.OldType))
                {
                    
                    _db.Files.Where(x => x.SavedFileName == editfile.GuidOriginal).First().OrigninalFileName = editfile.NewFile.FileName;
                    _db.Files.Where(x => x.SavedFileName == editfile.GuidOriginal).First().Type = Path.GetExtension(editfile.NewFile.FileName);
                    _db.SaveChanges();
                    string type = System.IO.Path.GetExtension(editfile.NewFile.FileName);
                    if (type == ".ppt" || type == ".pptx")
                    {
                        ConvertPowerPoint a = new ConvertPowerPoint();
                        a.GetVideoFromPpt(editfile.NewFile, editfile.GuidOriginal, editfile.Slideduration);
                    }
                    else
                    {
                        using (System.IO.FileStream output = new System.IO.FileStream(path+"\\"+editfile.GuidOriginal+ type, System.IO.FileMode.Create))
                        {
                            editfile.NewFile.InputStream.CopyTo(output);
                        }
                    }
                }
            }            
        }


        public bool Delete(string guid, string type)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (type == ".ppt" || type == ".pptx") type = ".mp4";
            var path = appData + @"\HelloPoint\upfiles\" + guid + type;
            System.Threading.Thread.Sleep(100);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.Threading.Thread.Sleep(100);
                    System.IO.File.Delete(path);
                    return true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else return false;
        }

        public bool DeletePPT(string guid,string type)
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);   
            var path = appData + @"\HelloPoint\tmp\" + guid +type;
            System.Threading.Thread.Sleep(100);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.Threading.Thread.Sleep(100);
                    System.IO.File.Delete(path);
                    return true;
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }
            }
            else return false;
        }

    }
}