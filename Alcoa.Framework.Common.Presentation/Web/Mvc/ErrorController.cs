using System;
using System.Web.Mvc;

namespace Alcoa.Framework.Common.Presentation.Web.Mvc
{
    /// <summary>
    /// Class to receive error messages and send to Error view
    /// </summary>
    public class ErrorController : AbstractController
    {
        [AllowAnonymous]
        public ActionResult Index(string message, string details)
        {
            var ex = (string.IsNullOrWhiteSpace(details)) ?
                new Exception(message) :
                new Exception(message, new Exception(details));

            var model = new HandleErrorInfo(ex, "Error", "Index");

            return View("Error", model);
        }
    }
}
