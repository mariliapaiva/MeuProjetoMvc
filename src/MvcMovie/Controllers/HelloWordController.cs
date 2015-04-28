using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcMovie.Controllers
{
    public class HelloWordController : Controller
    {  
        // GET: HelloWord
        public ActionResult Index()
        {
            return View();
        }

        //[Route("helloword/welcome/{name}/{numtimes?}")]
        ////[Route("helloword/welcome/{name}")]
        //public string Welcome(string name, int numTimes = 1)
        //{
        //    return HttpUtility.HtmlEncode("Hello "+ name + ", NumTime is :"+ numTimes);
        //}
    }


}