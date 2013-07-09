namespace StartProcessService
{
    using System;
    using System.IO;
    using NLog;
    using Topshelf;

    class Program
    {
        const string DisplayName = "StartProcessService";
        const string ServiceName = "StartProcessService";

        static readonly Logger log = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            log.Info("Current directory is: '{0}'", Directory.GetCurrentDirectory());

            HostFactory.Run(x =>
                {
                    x.Service<WinService>();
                    x.SetDescription(DisplayName);
                    x.SetDisplayName(DisplayName);
                    x.SetServiceName(ServiceName);
                    x.StartAutomatically();
                    x.RunAsLocalSystem();
                    x.UseNLog();
                });
        }
    }
}
