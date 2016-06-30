using Alcoa.Framework.Common.Enumerator;
using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Security.Principal;

namespace Alcoa.Framework.Common.Helpers
{
    /// <summary>
    /// Log in windows authentication using impersonation
    /// </summary>
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public sealed class ImpersonationHelper : IDisposable
    {
        private readonly SafeTokenHandle _handle;
        private readonly WindowsImpersonationContext _context;

        /// <summary>
        /// Log in windows authentication using impersonation
        /// </summary>
        public static ImpersonationHelper LogonUser(string domain, string username, string password, LogonType logonType = LogonType.Interactive)
        {
            return new ImpersonationHelper(domain, username, password, logonType);
        }

        private ImpersonationHelper(string domain, string username, string password, LogonType logonType)
        {
            IntPtr handle;
            var ok = NativeMethods.LogonUser(username, domain, password, (int)logonType, 0, out handle);
            if (!ok)
            {
                var errorCode = Marshal.GetLastWin32Error();
                throw new ApplicationException(string.Format("Could not impersonate the elevated user.  LogonUser returned error code {0}.", errorCode));
            }

            _handle = new SafeTokenHandle(handle);
            _context = WindowsIdentity.Impersonate(_handle.DangerousGetHandle());
        }

        /// <summary>
        /// Releases all resources used by the impersonation
        /// </summary>
        public void Dispose()
        {
            _context.Dispose();
            _handle.Dispose();
        }
    }

    internal class NativeMethods
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword, int dwLogonType, int dwLogonProvider, out IntPtr phToken);

        [DllImport("kernel32.dll")]
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        [SuppressUnmanagedCodeSecurity]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool CloseHandle(IntPtr handle);
    }

    internal sealed class SafeTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        internal SafeTokenHandle(IntPtr handle)
            : base(true)
        {
            this.handle = handle;
        }

        protected override bool ReleaseHandle()
        {
            return NativeMethods.CloseHandle(handle);
        }
    }
}
