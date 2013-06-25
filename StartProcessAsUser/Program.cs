namespace StartProcessAsUser
{
    using System;
    using System.ComponentModel;
    using System.Security.AccessControl;
    using System.Text;
    using Asprosys.Security.AccessControl;
    using PInvoke;

    static class Program
    {
        // create with 'net user test_user password /add'
        const string userName = "test_user";
        const string password = "password";

        static void Main(string[] args)
        {
            try
            {
                AddDesktopPermission();

                using (var jobObject = new JobObject("StartProcessAsUserJob"))
                {
                    jobObject.KillProcessesOnJobClose = true;

                RunProcs:
                    for (ushort i = 0; i < 1; ++i)
                    {
                        try
                        {
                            var p = CreateProcess(i);
                            jobObject.AddProcess(p);
                        }
                        catch (Win32Exception ex)
                        {
                            Console.Error.WriteLine("ERROR: '{0}' Error code: '{1}' Native error code: '{2}'", ex.Message, ex.ErrorCode, ex.NativeErrorCode);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine("ERROR: '{0}'", ex.Message);
                        }
                    }
                    Console.WriteLine("Type 'again' to run again, hit enter to exit...");
                    string cmd = Console.ReadLine().Trim().ToLowerInvariant();
                    if (cmd == "again")
                    {
                        goto RunProcs;
                    }
                }
            }
            finally
            {
                RemoveDesktopPermission();
            }
        }

        private static IntPtr CreateProcess(ushort i)
        {
            IntPtr hToken = IntPtr.Zero;

            if (NativeMethods.RevertToSelf())
            {
                if (NativeMethods.LogonUser(userName, ".", password,
                        NativeMethods.LogonType.LOGON32_LOGON_INTERACTIVE,
                        NativeMethods.LogonProvider.LOGON32_PROVIDER_DEFAULT, out hToken))
                {

                    IntPtr primaryToken = IntPtr.Zero;
                    var sa = new NativeMethods.SecurityAttributes();
                    if (NativeMethods.DuplicateTokenEx(
                        hToken,
                        NativeMethods.Constants.GENERIC_ALL_ACCESS,
                        sa,
                        NativeMethods.SecurityImpersonationLevel.SecurityImpersonation,
                        NativeMethods.TokenType.TokenPrimary,
                        out primaryToken))
                    {
                        try
                        {
                            return DoCreateProcessAsUser(primaryToken);
                        }
                        finally
                        {
                            NativeMethods.CloseHandle(primaryToken);
                        }
                    }
                    else
                    {
                        throw new Win32Exception();
                    }
                }
                else
                {
                    throw new Win32Exception();
                }
            }
            else
            {
                throw new Win32Exception();
            }
        }

        private static IntPtr DoCreateProcessAsUser(IntPtr primaryToken)
        {
            var startupInfo = new NativeMethods.StartupInfo();

            string lpApplicationName = null;

            var cmdLine = new StringBuilder(1024);
            cmdLine.Append(@"cmd /c ""set && pause""");
            // cmdLine.Append(@"powershell.exe -NoExit -Command ""dir env:"""); // Look at the environment
            // cmdLine.Append(@"powershell.exe -InputFormat None -NoLogo -NoProfile -NonInteractive -Command ""echo 'START'; Start-Sleep -s 15; echo 'STOP'""");
            // cmdLine.Append(@"cmd /c ping 127.0.0.1 -n 15 -w 1000");

            // Create structs
            var saProcessAttributes = new NativeMethods.SecurityAttributes();
            var saThreadAttributes = new NativeMethods.SecurityAttributes();

            // Now create the process as the user
            NativeMethods.ProcessInformation pi;

            var createProcessFlags = NativeMethods.CreateProcessFlags.CREATE_NO_WINDOW |
                NativeMethods.CreateProcessFlags.CREATE_UNICODE_ENVIRONMENT |
                NativeMethods.CreateProcessFlags.CREATE_NEW_CONSOLE; // Remove this to have a hidden window. Having this here allows you to see output

            // IntPtr envStrings = NativeMethods.GetEnvironmentStrings();
            IntPtr envStrings = IntPtr.Zero; // inherit the parent environment which I think is what is screwing up powershell

            string workingDir = @"C:\tmp";

            if (primaryToken == IntPtr.Zero)
            {
                if (NativeMethods.CreateProcess(
                    lpApplicationName,
                    cmdLine.ToString(),
                    saProcessAttributes,
                    saThreadAttributes,
                    false, // bInheritHandles
                    createProcessFlags,
                    envStrings,
                    workingDir,
                    startupInfo,
                    out pi))
                {
                    NativeMethods.CloseHandle(pi.hThread);
                    Console.WriteLine("create-process cmd: '{0}' pid: '{1}'", cmdLine.ToString(), pi.dwProcessId);
                    return pi.hProcess;
                }
                else
                {
                    throw new Win32Exception();
                }
            }
            else
            {
                // http://odetocode.com/blogs/scott/archive/2004/10/29/createprocessasuser.aspx
                if (NativeMethods.CreateProcessAsUser(
                    primaryToken,
                    lpApplicationName,
                    cmdLine, // lpCommandLine
                    saProcessAttributes,
                    saThreadAttributes,
                    false, // bInheritHandles
                    createProcessFlags,
                    envStrings,
                    workingDir,
                    startupInfo,
                    out pi))
                {
                    NativeMethods.CloseHandle(pi.hThread);
                    Console.WriteLine("create-process-as-user cmd: '{0}' pid: '{1}'", cmdLine.ToString(), pi.dwProcessId);
                    return pi.hProcess;
                }
                else
                {
                    throw new Win32Exception();
                }
            }
        }

        private static void AddDesktopPermission()
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
                Console.Error.WriteLine(ex.Message);
            }
        }

        private static void RemoveDesktopPermission()
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
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
