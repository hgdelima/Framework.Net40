using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Web;
using System.Web.Routing;

namespace Alcoa.Framework.Common.Presentation.Web.Mvc
{
    /// <summary>
    /// Class that helps manipulate URL operations
    /// </summary>
    [Browsable(false), DebuggerStepThrough]
    public static class UrlHelper
    {
        /// <summary>
        /// Converts an URI object to a MVC Routetable object
        /// </summary>
        [Browsable(false), DebuggerHidden]
        public static RouteData ConvertUrlToRoute(Uri uri)
        {
            return RouteTable.Routes.GetRouteData(new InternalHttpContext(uri, HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath));
        }

        [Browsable(false)]
        private class InternalHttpContext : HttpContextBase
        {
            private readonly HttpRequestBase _request;

            public InternalHttpContext(Uri uri, string appRelativePath)
            {
                _request = new InternalRequestContext(uri, appRelativePath);
            }

            public override HttpRequestBase Request { get { return _request; } }
        }

        [Browsable(false)]
        private class InternalRequestContext : HttpRequestBase
        {
            private readonly string _pathInfo;
            public override string PathInfo { get { return _pathInfo; } }

            private readonly string _appRelativePath;
            public override string AppRelativeCurrentExecutionFilePath { get { return _appRelativePath; } }

            public InternalRequestContext(Uri uri, string appRelativePath)
            {
                _pathInfo = uri.Query;
                _appRelativePath = appRelativePath;
            }
        }
    }
}