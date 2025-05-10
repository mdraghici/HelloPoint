using System;
using System.Linq;
using System.Web.Mvc;
using HelloPoint.Models;
using System.Web.Script.Serialization;
using Microsoft.AspNet.Identity;
using System.IO;
using HelloPoint.Communication;

namespace HelloPoint.Controllers
{
    public class UserManagementController : Controller
    {
        FileDbEntities _db = new FileDbEntities();


        [Authorize(Roles = "Admin,User")]
        public ActionResult UserManagement(string UserName)
        {
            string Username = User.Identity.Name;
            UserManagementModel myfile = new UserManagementModel();

            myfile.UserName = Username;

            foreach (var f in _db.Files.Where(x => x.UserName == Username))
            {
                myfile.OriginalFileName.Add(f.OrigninalFileName);
                myfile.SavedFileName.Add(f.SavedFileName);
                myfile.Type.Add(f.Type);
                myfile.Description.Add(f.Description);
            }

            return View("UserManagement", myfile);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(string guid,string type)
        {
            _db.Files.Remove(_db.Files.Where(x => x.SavedFileName == guid && x.UserName == User.Identity.Name).First());
            _db.SaveChanges();
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            // Combine the base folder with your specific folder....
            if (type == ".ppt" || type == ".pptx") type = ".mp4";
            var path = appData + @"\HelloPoint\upfiles\"+guid+type;
            System.Threading.Thread.Sleep(100);
            if (System.IO.File.Exists(path))
            {
                try
                {
                    System.Threading.Thread.Sleep(100);
                    System.IO.File.Delete(path);
                }
                catch (System.IO.IOException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            
            return RedirectToAction("UserManagement", "UserManagement", new { UserName = User.Identity.Name });
        }
        [Authorize(Roles = "Admin,User")]
        public ActionResult GetPlayList()
        {
            PlayListModel _playlist = new PlayListModel();            
            string json = _playlist.GetPlayList().Playlist.ToJSON();
            return Content(json);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult AddElementToPlayList(string guid,string type, string description)
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.AddElement(guid,type,description).Playlist.ToJSON();
            return Content(json);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Play(int id)
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.Play(id).Playlist.ToJSON();
            return Content(json);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult Pause()
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.Pause().Playlist.ToJSON();
            return Content(json);
        }
        [Authorize(Roles = "Admin,User")]
        public ActionResult Stop()
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.Stop().Playlist.ToJSON();
            return Content(json);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult ClearPlaylist()
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.ClearAll().Playlist.ToJSON();
            return Content(json);
        }


        [Authorize(Roles = "Admin,User")]
        public ActionResult MoveUp(int id)
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.MoveUp(id).Playlist.ToJSON();
            return Content(json);
        }

        [Authorize(Roles = "Admin,User")]
        public ActionResult MoveDown(int id)
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.MoveDown(id).Playlist.ToJSON();
            return Content(json);
        }
        [Authorize(Roles = "Admin,User")]
        public ActionResult RemoveFromPlaylist(int id)
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.Remove(id).Playlist.ToJSON();
            return Content(json);
        }

        public ActionResult ModifyRepeatNumber(int id ,int repeatnumber)
        {
            PlayListModel _playlist = new PlayListModel();
            string json = _playlist.ModifyRepeatNumber(id, repeatnumber).Playlist.ToJSON();
            return Content(json);
        }

        public String VerifyElementIsDone(string guid,string type)
        {
            long length = new System.IO.FileInfo(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\HelloPoint\upfiles\" + guid + ".mp4").Length;
            if (length > 0)
            {   
                EditFile delete = new EditFile();
                delete.DeletePPT(guid,type);
            }       
            return length.ToString();
        }



        [HttpGet]
        public ActionResult EditUploadedFile(string guid)
        {
            FileDbEntities _db = new FileDbEntities();

            var data =_db.Files.Where(x => x.SavedFileName == guid).First();

            EditFile _fileToEdit = new EditFile();
            _fileToEdit.OldDescription = data.Description;
            _fileToEdit.OldFileName = data.OrigninalFileName;
            _fileToEdit.GuidOriginal = guid;
            _fileToEdit.OldType = data.Type;
            return View(_fileToEdit);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPost]
        public ActionResult EditUploadedFile(EditFile editfile)
        {
            editfile.Edit(editfile);
            return RedirectToAction("UserManagement", "UserManagement");
        }


        [HttpGet]
        public ActionResult Configuration()
        {
            return View();
        }

        public ActionResult GetMonitorData(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.GetMonitorData(monitorindex).ToJSON();
            return Content(json);
        }

        public ActionResult MoveUpMonitor(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.MoveUp(monitorindex).ToJSON();
            return Content(json);
        }

        public ActionResult MoveDownMonitor(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.MoveDown(monitorindex).ToJSON();
            return Content(json);
        }

        public ActionResult MoveLeftMonitor(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.MoveLeft(monitorindex).ToJSON();
            return Content(json);
        }

        public ActionResult MoveRightMonitor(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.MoveRight(monitorindex).ToJSON();
            return Content(json);
        }

        public ActionResult ScaleUpMonitor(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.ScaleUp(monitorindex).ToJSON();
            return Content(json);
        }

        public ActionResult ScaleDownMonitor(int monitorindex)
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.ScaleDown(monitorindex).ToJSON();
            return Content(json);
        }
        
        public ActionResult ResetMonitor()
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.Reset().ToJSON();
            return Content(json);
        }

        public ActionResult RestartWfp()
        {
            StartWpfApp start = new StartWpfApp();
            return Content(start.RestartWpf().ToString());
        }


        public ActionResult TornMonitorOn()
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.TurnOn().ToJSON();
            return Content(json);
        }

        public ActionResult TornMonitorOff()
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.TurnOff().ToJSON();
            return Content(json);
        }

        public ActionResult GetMonitorState()
        {
            ConfigurationCommandModel _monitordata = new ConfigurationCommandModel();
            string json = _monitordata.GetMonitorState().ToJSON();
            return Content(json);
        }
    }



    public static class JSONHelper
    {
        public static string ToJSON(this object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

    }
}