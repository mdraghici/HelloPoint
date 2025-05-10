using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloPoint.Controllers
{
    public class ExceptionController : Controller
    {
        // GET: Exception
        public String DatabaseError(string e)
        {
            return "Exception:"+e+ "  -  Database error! pleases try again later";
        }

        public String DiscError(string e)
        {
            return "Exception:" + e + "  -   pleases try again later";
        }
    }
}