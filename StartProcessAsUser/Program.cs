namespace StartProcessAsUser
{
    using System;
    using System.ComponentModel;
    using System.Security.AccessControl;
    using System.Text;
    using Asprosys.Security.AccessControl;
    using StartProcessLib;
    using StartProcessLib.PInvoke;

    // http://www.installsetupconfig.com/win32programming/windowstationsdesktops13_4.html
    // http://bytes.com/topic/net/answers/577257-impersonation-vs-job-api
    static class Program
    {
        const string workingDir = @"C:\tmp";
        // create with 'net user test_user password /add'
        const string userName = "test_user";
        const string password = "Pass@word1";

        private static readonly DesktopPermissionManager permissionManager = new DesktopPermissionManager(userName);

        static void Main(string[] args)
        {
            try
            {
                permissionManager.AddDesktopPermission();

                using (var jobObject = new JobObject("StartProcessAsUserJob"))
                {
                    jobObject.KillProcessesOnJobClose = true;

                RunProcs:
                    for (ushort i = 0; i < 1; ++i)
                    {
                        try
                        {
                            var p = DoCreateProcessAsUser();
                            // var p = DoCreateProcessWithLogon();
                            // jobObject.AddProcess(p.hProcess);
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
                    Console.WriteLine("Type 'again' to create again, hit enter to exit...");
                    string cmd = Console.ReadLine().Trim().ToLowerInvariant();
                    if (cmd == "again")
                    {
                        goto RunProcs;
                    }
                }
            }
            finally
            {
                permissionManager.RemoveDesktopPermission();
            }
        }

        private static NativeMethods.ProcessInformation DoCreateProcessWithLogon()
        {
            var cmdLine = new StringBuilder(1024);
            // cmdLine.Append(@"powershell.exe -NoExit -Command ""dir env:"""); // Look at the environment
            cmdLine.Append(@"cmd.exe /k set"); // Look at the environment

            var createProcessFlags = NativeMethods.CreateProcessFlags.CREATE_NEW_CONSOLE |
                NativeMethods.CreateProcessFlags.CREATE_UNICODE_ENVIRONMENT;
            /*
                NativeMethods.CreateProcessFlags.CREATE_NO_WINDOW |
                NativeMethods.CreateProcessFlags.CREATE_BREAKAWAY_FROM_JOB |
                NativeMethods.CreateProcessFlags.CREATE_NEW_CONSOLE;
             */

            var startupInfo = new NativeMethods.StartupInfo();

            NativeMethods.ProcessInformation pi;

            if (NativeMethods.CreateProcessWithLogon(userName, ".", password,
                NativeMethods.LogonFlags.LOGON_WITH_PROFILE,
                null,
                cmdLine,
                createProcessFlags,
                IntPtr.Zero,
                workingDir,
                startupInfo,
                out pi))
            {
                Console.WriteLine("create-process-with-logon cmd: '{0}' pid: '{1}'", cmdLine.ToString(), pi.dwProcessId);
                return pi;
            }
            else
            {
                throw new Win32Exception();
            }
        }

        private static NativeMethods.ProcessInformation DoCreateProcessAsUser()
        {
            var startupInfo = new NativeMethods.StartupInfo();

            string lpApplicationName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            // string lpApplicationName = @"C:\Windows\System32\cmd.exe";

            var cmdLine = new StringBuilder(1024);
            // NB: /k will keep window open. Try running powershell from the window and you won't see any modules available. Weird.
            // This works, however: runas /user:test_user /noprofile /savecred "powershell -NoExit"
            // cmdLine.Append(@" /k set");

            // Other commands to try:
            // cmdLine.Append(@" -NoExit -Command ""dir env:"""); // Look at the environment
            // cmdLine.Append(@"powershell.exe -InputFormat None -NoLogo -NoProfile -NonInteractive -Command ""echo 'START'; Start-Sleep -s 15; echo 'STOP'""");
            // cmdLine.Append(@"cmd /c ping 127.0.0.1 -n 15 -w 1000"); // Useful for "sleep"
            cmdLine.Append(@" -InputFormat None -NoLogo -NoProfile -NonInteractive -Command ""Add-Content -Path C:\tmp\test.txt -Value FOO""");

            // Create structs
            var saProcessAttributes = new NativeMethods.SecurityAttributes();
            var saThreadAttributes = new NativeMethods.SecurityAttributes();

            // Now create the process as the user
            NativeMethods.ProcessInformation pi;

            var createProcessFlags =
                NativeMethods.CreateProcessFlags.CREATE_NO_WINDOW |
                NativeMethods.CreateProcessFlags.CREATE_UNICODE_ENVIRONMENT;
                // NativeMethods.CreateProcessFlags.CREATE_NEW_CONSOLE; // Remove this to have a hidden window. Having this here allows you to see output

            IntPtr primaryToken = Utils.LogonAndGetPrimaryToken(userName, password);

            /*
            uint sessionId = 1;
            if (NativeMethods.SetTokenInformation(primaryToken,
                NativeMethods.TokenInformationClass.TokenSessionId,
                ref sessionId, (uint)Marshal.SizeOf(sessionId)))
            {
             */
            if (NativeMethods.CreateProcessWithToken(primaryToken, NativeMethods.LogonFlags.LOGON_WITH_PROFILE,
                lpApplicationName, cmdLine.ToString(), createProcessFlags, IntPtr.Zero, workingDir,
                startupInfo, out pi))
            {
                Console.WriteLine("create-process-with-token cmd: '{0}' pid: '{1}'", cmdLine.ToString(), pi.dwProcessId);
                return pi;
            }
            else
            {
                throw new Win32Exception();
            }
            /*
            }
            else
            {
                throw new Win32Exception();
            }
             */

            /*
            var profileInfo = new NativeMethods.ProfileInfo();
            profileInfo.lpUserName = userName;

            if (NativeMethods.LoadUserProfile(primaryToken, profileInfo))
            {
                IntPtr envBlock = IntPtr.Zero;
                if (NativeMethods.CreateEnvironmentBlock(out envBlock, primaryToken, false))
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
                        envBlock,
                        workingDir,
                        startupInfo,
                        out pi))
                    {
                        Console.WriteLine("create-process-as-user cmd: '{0}' pid: '{1}'", cmdLine.ToString(), pi.dwProcessId);
                        return pi;
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
             */
        }
    }
}
