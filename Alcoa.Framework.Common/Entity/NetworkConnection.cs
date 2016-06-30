using System;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;

namespace Alcoa.Framework.Common.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkConnection : IDisposable
    {
        private string _networkPath;

        [DllImport("mpr.dll")]
        private static extern int WNetAddConnection2(NetResource netResource, string password, string username, int flags);

        [DllImport("mpr.dll")]
        private static extern int WNetCancelConnection2(string name, int flags, bool force);

        /// <summary>
        /// 
        /// </summary>
        public NetworkConnection(string networkPath, NetworkCredential networkCredential)
        {
            _networkPath = networkPath;

            var netResource = new NetResource()
            {
                Scope = ResourceScope.GlobalNetwork,
                ResourceType = ResourceType.Disk,
                DisplayType = ResourceDisplaytype.Share,
                RemoteName = networkPath
            };

            var domainAndUser = string.IsNullOrEmpty(networkCredential.Domain)
                ? networkCredential.UserName
                : string.Format(@"{0}\{1}", networkCredential.Domain, networkCredential.UserName);

            var result = WNetAddConnection2(
                netResource,
                networkCredential.Password,
                domainAndUser,
                0);

            if (result != 0)
                throw new Win32Exception(result, @"Error to connect remote server path. Error code: " + result);
        }

        /// <summary>
        /// Network object destructor
        /// </summary>
        ~NetworkConnection()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the network object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            WNetCancelConnection2(_networkPath, 0, true);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public class NetResource
    {
        public ResourceScope Scope;
        public ResourceType ResourceType;
        public ResourceDisplaytype DisplayType;
        public int Usage;
        public string LocalName;
        public string RemoteName;
        public string Comment;
        public string Provider;
    }

    public enum ResourceScope : int
    {
        Connected = 1,
        GlobalNetwork,
        Remembered,
        Recent,
        Context
    };

    public enum ResourceType : int
    {
        Any = 0,
        Disk = 1,
        Print = 2,
        Reserved = 8,
    }

    public enum ResourceDisplaytype : int
    {
        Generic = 0x0,
        Domain = 0x01,
        Server = 0x02,
        Share = 0x03,
        File = 0x04,
        Group = 0x05,
        Network = 0x06,
        Root = 0x07,
        Shareadmin = 0x08,
        Directory = 0x09,
        Tree = 0x0a,
        Ndscontainer = 0x0b
    }
}