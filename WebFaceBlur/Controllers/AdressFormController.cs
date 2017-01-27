using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebFaceBlur.Controllers
{
    public class AdressFormController : Controller
    {
        public ActionResult GetForm()
        {
            return View("Form");
        }

        [HttpPost]
        public ActionResult Post(FormCollection formCollection)
        {
            string path = formCollection["path"].ToString();
            if ( path == null )
            {
                return Redirect("~/");
            }
            path = HttpUtility.UrlEncode(path);
            return Redirect(string.Format("~/?path={0}", path));
        }
    }
}