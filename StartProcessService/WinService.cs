namespace StartProcessService
{
    using System;
    using System.Linq;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using NLog;
    using StartProcessLib;
    using StartProcessLib.PInvoke;
    using Topshelf;

    public class WinService : ServiceControl
    {
        const string testPowershellScript = @"
$now = Get-Date
$sleepSeconds = Get-Random -Minimum 30 -Maximum 300
Add-Content -Path C:\tmp\test.txt -Value ""Hello from $pid the time is $now - sleeping for $sleepSeconds seconds...""
Start-Sleep -s $sleepSeconds
$now = Get-Date
Add-Content -Path C:\tmp\test.txt -Value ""Hello from $pid the time is $now""
";
        const string workingDir = @"C:\tmp";
        // create with 'net user test_user password /add'
        const string userName = "test_user";
        const string password = "Pass@word1";

        private readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly CancellationTokenSource cts = new CancellationTokenSource();
        private readonly CancellationToken ct;

        private Task serviceTask;

        public WinService()
        {
            this.ct = cts.Token;
        }

        public bool Start(HostControl hostControl)
        {
            serviceTask = Task.Run((Action)ServiceMain, ct);
            return true;
        }

        public bool Stop(HostControl hostControl)
        {
            try
            {
                cts.Cancel();
                Task.WaitAll(new[] { serviceTask }, TimeSpan.FromSeconds(25));
            }
            catch (Exception ex)
            {
                log.InfoException("Exception(s) during Stop.", ex);
            }
            return true;
        }

        private void ServiceMain()
        {
            const string testFile = @"C:\tmp\test-it.ps1";
            File.Delete(testFile);
            File.WriteAllText(testFile, testPowershellScript);

            var permissionManager = new DesktopPermissionManager(userName);

            var startupInfo = new NativeMethods.StartupInfo();
            string lpApplicationName = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
            var cmdLine = new StringBuilder(1024);
            cmdLine.AppendFormat(@" -InputFormat None -NoLogo -NoProfile -NonInteractive -File {0}", testFile);

            permissionManager.AddDesktopPermission();

            using (var jobObject = new JobObject("StartProcessServiceJobObject"))
            {
                jobObject.KillProcessesOnJobClose = true;

                while (!ct.IsCancellationRequested)
                {
                    try
                    {
                        log.Debug("Executing command: '{0}'", cmdLine);

                        // Now create the process as the user
                        NativeMethods.ProcessInformation pi;

                        var saProcessAttributes = new NativeMethods.SecurityAttributes();
                        var saThreadAttributes = new NativeMethods.SecurityAttributes();

                        var createProcessFlags =
                            NativeMethods.CreateProcessFlags.CREATE_NO_WINDOW |
                            NativeMethods.CreateProcessFlags.CREATE_UNICODE_ENVIRONMENT;

                        IntPtr primaryToken = Utils.LogonAndGetPrimaryToken(userName, password);

                        if (NativeMethods.CreateProcessAsUser(primaryToken,
                            lpApplicationName,
                            cmdLine,
                            saProcessAttributes,
                            saThreadAttributes,
                            false,
                            createProcessFlags,
                            IntPtr.Zero,
                            workingDir,
                            startupInfo,
                            out pi))
                        {
                            log.Debug("created process: '{0}' pid: '{1}'", cmdLine.ToString(), pi.dwProcessId);
                            jobObject.AddProcess(pi.hProcess);
                            log.Debug("job object has '{0}' processes in it.", jobObject.GetJobProcesses().Count());
                            NativeMethods.CloseHandle(pi.hProcess);
                            NativeMethods.CloseHandle(pi.hThread);
                        }
                        else
                        {
                            int err = Marshal.GetLastWin32Error();
                            log.Error("Error '{0}' creating process.", err);
                        }
                    }
                    catch (Exception ex)
                    {
                        log.ErrorException("Exception creating process.", ex);
                    }
                    finally
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }
            }

            permissionManager.RemoveDesktopPermission();
        }
    }
}
