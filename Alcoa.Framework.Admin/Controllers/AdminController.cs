using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Presentation.Web.Mvc;
using Alcoa.Framework.Log;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Alcoa.Framework.Admin.Controllers
{
    public class AdminController : BaseController
    {
        //private readonly ISso _exampleService;

        public AdminController()
        {
            //_exampleService = new ExampleService();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}