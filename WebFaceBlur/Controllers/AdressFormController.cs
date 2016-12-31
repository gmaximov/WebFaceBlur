using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFaceBlur.Controllers
{
    public class AdressFormController : Controller
    {
        [HttpPost]
        public ActionResult Index(FormCollection formCollection)
        {
            string path = formCollection["path"].ToString();          
            if ( path == null )
            {
                return Redirect("~/");
            }
            path = HttpUtility.UrlEncode(path);
            return Redirect("~/?path=" + path);
        }
    }
}