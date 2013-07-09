namespace StartProcessLib
{
    using System;
    using System.Security.AccessControl;
    using Asprosys.Security.AccessControl;
    using NLog;
    using PInvoke;

    public class DesktopPermissionManager
    {
        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly string userName;

        public DesktopPermissionManager(string userName)
        {
            if (String.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentNullException("userName");
            }
            this.userName = userName;
        }

        public void AddDesktopPermission()
        {
            try
            {
                IntPtr hWinSta = NativeMethods.GetProcessWindowStation();
                var ws = new WindowStationSecurity(hWinSta, AccessControlSections.Access);
                ws.AddAccessRule(new WindowStationAccessRule(userName, WindowStationRights.AllAccess, AccessControlType.Allow));
                ws.AcceptChanges();

                IntPtr hDesk = NativeMethods.GetThreadDesktop(NativeMethods.GetCurrentThreadId());
                var ds = new DesktopSecurity(hDesk, AccessControlSections.Access);
                ds.AddAccessRule(new DesktopAccessRule(userName, DesktopRights.AllAccess, AccessControlType.Allow));
                ds.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.ErrorException("Exception adding desktop permissions!", ex);
            }
        }

        public void RemoveDesktopPermission()
        {
            try
            {
                IntPtr hWinSta = NativeMethods.GetProcessWindowStation();
                var ws = new WindowStationSecurity(hWinSta, AccessControlSections.Access);
                ws.RemoveAccessRule(new WindowStationAccessRule(userName, WindowStationRights.AllAccess, AccessControlType.Allow));
                ws.AcceptChanges();

                IntPtr hDesk = NativeMethods.GetThreadDesktop(NativeMethods.GetCurrentThreadId());
                var ds = new DesktopSecurity(hDesk, AccessControlSections.Access);
                ds.RemoveAccessRule(new DesktopAccessRule(userName, DesktopRights.AllAccess, AccessControlType.Allow));
                ds.AcceptChanges();
            }
            catch (Exception ex)
            {
                log.ErrorException("Exception removing desktop permissions!", ex);
            }
        }
    }
}
