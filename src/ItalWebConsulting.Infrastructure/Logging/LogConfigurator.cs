using ItalWebConsulting.Infrastructure.Comunication;
using NLog;
using NLog.Targets;
using System;
using System.IO;
using System.Linq;

namespace ItalWebConsulting.Infrastructure.Logging
{
    public class LogConfigurator
    {
        private static ILogger logger = LogManager.GetCurrentClassLogger();

        public static void Configure()
        {
            LogManager.LoadConfiguration(String.Concat(Directory.GetCurrentDirectory(), "\\nlog.config"));
            var all = LogManager.Configuration.AllTargets;
            var files = all.ToList().Where(t => t.Name.Equals("logFile", StringComparison.OrdinalIgnoreCase)).ToList();

            foreach (var ft in files)
            {
                var file = ft as FileTarget;
                ConfiguraFileLog(file);
            }
            
            /*
            var outputLog = nLogConfiguration.FileName.Replace("{$CurrDir}", Directory.GetCurrentDirectory());
            if (!string.IsNullOrWhiteSpace(nLogConfiguration.LblAmbiente))
                outputLog = outputLog.Replace("{$A}", nLogConfiguration.LblAmbiente);

            var concurrentFlatFilePath = outputLog;
            outputLog = Path.GetDirectoryName(outputLog);

            var all = LogManager.Configuration.AllTargets;
            var mail = all.ToList().FirstOrDefault(t => t.Name.Equals("logMail", StringComparison.OrdinalIgnoreCase)) as NLog.MailKit.MailTarget;
            var file = all.ToList().FirstOrDefault(t => t.Name.Equals("logFile", StringComparison.OrdinalIgnoreCase)) as FileTarget;

            if (mail != null)
                ConfiguraEmail(nLogConfiguration, smtpSetting, mail);
            if (file != null)
                ConfiguraFileLog(concurrentFlatFilePath, file);
            logger.Info("Log Configurato con successo");
            return concurrentFlatFilePath;
            */
        }

        public static void ChangeFileLogPath(string newFolderLog)
        {
            var all = LogManager.Configuration.AllTargets;
            var files = all.ToList();
            foreach (var ft in files)
            {
                var file = ft as FileTarget;
                if (file == null)
                    continue;

                //var concurrentFlatFilePath = Path.Combine(outputLog, "concurrentFlat.log");
                var fPathLog = file.FileName.ToString().Replace("'", "");
                var fName = DateTime.Today.ToString("yyyyMMdd") +"_" + Path.GetFileName(fPathLog);
                var folder = Path.GetDirectoryName(fPathLog);
                var newPath = Path.Combine(folder, newFolderLog, fName);
                file.FileName = newPath;
            }

            LogManager.Flush();
            LogManager.ReconfigExistingLoggers();
        }

        private static void ConfiguraFileLog(FileTarget fileTarget)
        {
            //fileTarget.FileName = outputLog;
            //var fName = fileTarget.FileName ;

            fileTarget.MaxArchiveFiles = 15;
            fileTarget.ArchiveNumbering = ArchiveNumberingMode.DateAndSequence;
            fileTarget.ArchiveAboveSize = 5000000; //5Mb max size


            LogManager.ReconfigExistingLoggers();
        }

        //private static void ConfiguraEmail(NLogConfiguration nLogConfiguration, SmtpSettings smtpSetting, NLog.MailKit.MailTarget mailTarget)
        //{
        //    if (smtpSetting == null)
        //        return;
        //    mailTarget.From = smtpSetting.DefaultSender.EmailAdress;
        //    mailTarget.SmtpServer = smtpSetting.Server;
        //    mailTarget.SmtpPassword = smtpSetting.Password;
        //    mailTarget.SmtpUserName = smtpSetting.User;
        //    mailTarget.SmtpPort = smtpSetting.Port;
        //    mailTarget.Subject = nLogConfiguration.LblAmbiente + "Errore imprevisto da: ${processname} v:${assembly-version} on ${machinename}";
        //    mailTarget.Layout = "${date:format=yyyy-MM-dd HH\\:mm\\:ss}${newline}${windows-identity} running ${processname} v:${assembly-version} on ${machinename}${newline}At: ${callsite}${newline}Message: ${message}${newline}Exception:${newline}${exception:format=toString,Data:maxInnerExceptionLevel=10}${newline}";
        //    mailTarget.To = nLogConfiguration.MailAddressesOnError;

        //}
    }
}
