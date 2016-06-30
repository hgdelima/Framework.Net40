
namespace Alcoa.Framework.Common.Enumerator
{
    /// <summary>
    /// Alcoa common constants without specific context
    /// </summary>
    public class CommonConsts
    {
        public const string CompanyName = "";
        public const string CommonPassword = "";
    }

    /// <summary>
    /// Alcoa common SSO claim types
    /// </summary>
    public class SsoClaimTypes
    {
        public const string SsoId = "http://schemas.alcoa.com/claims/id";
        public const string SsoDomain = "http://schemas.alcoa.com/claims/domain";
        public const string SsoApp = "http://schemas.alcoa.com/claims/app";
        public const string SsoAppMnemonic = "http://schemas.alcoa.com/claims/app/mnemonic";
        public const string SsoAppHomeUrl = "http://schemas.alcoa.com/claims/app/homeurl";
        public const string SsoProfile = "http://schemas.alcoa.com/claims/app/profile";
        public const string SsoGroup = "http://schemas.alcoa.com/claims/app/profile/group";
        public const string SsoGroupCurrentLevel = "http://schemas.alcoa.com/claims/app/profile/group/currentlevel";
        public const string SsoGroupParentLevel = "http://schemas.alcoa.com/claims/app/profile/group/parentlevel";
        public const string SsoGroupSubLevels = "http://schemas.alcoa.com/claims/app/profile/group/sublevels";
        public const string SsoGroupOrder = "http://schemas.alcoa.com/claims/app/profile/group/order";
        public const string SsoService = "http://schemas.alcoa.com/claims/app/profile/group/service";
        public const string SsoServiceOrder = "http://schemas.alcoa.com/claims/app/profile/group/service/order";
        public const string SsoServiceUrl = "http://schemas.alcoa.com/claims/app/profile/group/service/url";
        public const string SsoActiveDirectoryGroup = "http://schemas.alcoa.com/claims/app/profile/adgroup";
    }

    /// <summary>
    /// Alcoa common windows process names
    /// </summary>
    public class CommonProcessNames
    {
        public const string Iis = "w3wp";
        public const string IisExpress = "iisexpress";
        public const string VSWebServer = "webdev.webserver40";
    }
}