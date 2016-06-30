
using System.Collections.Generic;

namespace Alcoa.Framework.Common.Presentation.Enumerator
{
    /// <summary>
    /// Common MVC route parameters
    /// </summary>
    public static class CommonRouteConsts
    {
        public const string Controller = "controller";
        public const string Action = "action";
        public const string Id = "id";
        public const string Message = "message";
        public const string Details = "details";
    }

    public static class CommonEnvironmentConsts
    {
        public static List<string> DevEnvironmentUrls;
        public static List<string> QaEnvironmentUrls;

        static CommonEnvironmentConsts()
        {
            DevEnvironmentUrls = new List<string> { "localhost", "dev" };
            QaEnvironmentUrls = new List<string> { "hom", "qa" };
        }
    }

    public static class CommonWsFederationConsts
    {
        public const string ApplicationCode = "wappcode";
    }
}