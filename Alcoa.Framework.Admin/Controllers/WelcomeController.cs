using Alcoa.Framework.Common.Presentation.Web.Mvc;
using System.Web.Mvc;

namespace Alcoa.Framework.Admin.Controllers
{
    public class WelcomeController : BaseController
    {
        public ActionResult Index()
        {
            return View("Welcome");
        }
    }
}