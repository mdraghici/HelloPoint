using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using HelloPoint.Models;
using System.Net;

namespace HelloPoint.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    {
                        return RedirectToAction("UserManagement", "UserManagement");
                    }//return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }

   
        //
        // GET: /Account/Register
        //[AllowAnonymous]
        [Authorize(Roles = "Admin")]
        public ActionResult Register()
        {
            UsernameRoleEntities _roles = new UsernameRoleEntities();
            //RoleEntities _roles = new RoleEntities();
            ViewBag.AllRoles = _roles.AspNetRoles.ToList();
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        // [AllowAnonymous]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.UserName, Email = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, model.Role);
                    

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("AdminPage", "Account");
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

  
        
        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        // Get: 
        [HttpGet]
        public ActionResult Upload(string UserName)
        {
            UploadFile _upfile = new UploadFile();
            _upfile.UserName = UserName;
            return View("Upload", _upfile);
        }

        // Post: 
        [HttpPost]
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        [OutputCache(NoStore = true, Duration = 0)]
        public ActionResult Upload(UploadFile upfile)
        {
            upfile.Upload();
            return RedirectToAction("UserManagement", "UserManagement");
        }

    
        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion

        [Authorize(Roles = "Admin")]
        public ActionResult ChangePassPage(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangePassword(string username, string newPassword)
        {
            UsernameRoleEntities _roles = new UsernameRoleEntities();
            string userId= "";
            foreach (var user in _roles.AspNetUsers)
            {
                if (user.UserName == username)
                {
                    userId = user.Id;
                    break;
                }

            }
          
             string resetToken = await UserManager.GeneratePasswordResetTokenAsync(userId);
             var result = await UserManager.ResetPasswordAsync(userId, resetToken, newPassword);
            
            if (result.Succeeded)
            {
                return Content("The password has been changed succesfully!");
            }
            
            return RedirectToAction("AdminPage", "Account");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRolePage(string username, string role)
        {
            ViewBag.Username = username;
            UsernameRoleEntities _roles = new UsernameRoleEntities();
            ViewBag.AllRoles = _roles.AspNetRoles.ToList();
           
            return View();
        }


        [Authorize(Roles = "Admin")]
        public String ChangeRole(string username, string roleid)
        {
            UsernameRoleEntities _roles = new UsernameRoleEntities();
            UserHasRoleEntities _changerole = new UserHasRoleEntities();
            UsernameRoleEntities _db = new UsernameRoleEntities();

            string userid = _roles.AspNetUsers.Where(x=>x.UserName == username).First().Id.ToString();
            string oldroleid = _changerole.AspNetUserRoles.Where(x => x.UserId == userid).First().RoleId.ToString();
            string oldrole = _roles.AspNetRoles.Where(x => x.Id == oldroleid).First().Name.ToString();
            string newrole = _db.AspNetRoles.Where(x => x.Id == roleid).First().Name.ToString();


                UserManager.RemoveFromRoles(userid, oldrole);
                UserManager.AddToRole(userid, newrole);
              
      
            return "The role has been changed successfully !";
        }


    [Authorize(Roles = "Admin")]
        public ActionResult AdminPage()
        {
           
            UserAndRole myUserRole = new UserAndRole();
            UsernameRoleEntities _db = new UsernameRoleEntities();
            UserHasRoleEntities userHasRole = new UserHasRoleEntities();
            
           
            foreach(var user in _db.AspNetUsers)
            {
                myUserRole.UserName.Add(user.UserName) ;
                string idRole="" ;
                string idUsername = user.Id;
                    foreach(var idrol in userHasRole.AspNetUserRoles)
                {
                    if (idUsername == idrol.UserId)
                    { 
                         idRole = idrol.RoleId;
                    }
                }
               foreach(var rol in _db.AspNetRoles)
                {
                    if(rol.Id == idRole)
                    {
                        if (rol.Name == "N")
                            rol.Name = "UNDEFINED";
                        myUserRole.Role.Add(rol.Name);
                        break;
                        
                    }
                }
                

            }
            return View(myUserRole);
        }
       
        public void DeleteAllFiles(string Username)
        {
            FileDbEntities _db = new FileDbEntities();
            foreach (var f in _db.Files.Where(x => x.UserName == Username))
            {
                string type;
                if (f.Type == ".ppt" || f.Type == ".pptx") type = ".mp4";
                else type = f.Type;

                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                var path = appData + @"\HelloPoint\upfiles\" + f.SavedFileName + type;
                // the sleeps are there because if they miss somethimes the program doesent have enouf time to delete from the hard disk !!!
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
                        ExceptionController ex = new ExceptionController();
                        ex.DiscError(e.Message);
                    }
                }
                var t = _db.Files.Where(x => x.UserName == Username && x.SavedFileName == f.SavedFileName).First();
                 _db.Files.Remove(t);  
            }
            _db.SaveChanges();
        }

        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteUser(string username, string rolename)
        {
            DeleteAllFiles(username);
            UsernameRoleEntities _users = new UsernameRoleEntities();
            string id= null;
            foreach(var userToGetId in _users.AspNetUsers)
            {
                if(userToGetId.UserName == username)
                {
                    id = userToGetId.Id;
                        break;
                }
            }

            // Check for for both ID and Role and exit if not found
            if (id == null || rolename == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Look for user in the UserStore
            var user = UserManager.Users.SingleOrDefault(u => u.Id == id);

            // If not found, exit
            if (user == null)
            {
                return HttpNotFound();
            }

            // Remove user from role first!
            var remFromRole = await UserManager.RemoveFromRoleAsync(id, rolename);

            // If successful
            if (remFromRole.Succeeded)
            {
                // Remove user from UserStore
                var results = await UserManager.DeleteAsync(user);

                // If successful
                if (results.Succeeded)
                {
                    // Redirect to Users page
                    return Content("");
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

        }
    }
}

