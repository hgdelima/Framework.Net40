using Alcoa.Entity.Entity;
using Alcoa.Framework.Common.Enumerator;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Alcoa.Framework.Common
{
    /// <summary>
    /// Class that helps manipulate Email operations, and should be configured at .Config file
    /// </summary>
    public static class EmailHelper
    {
        private static bool _sendEmailEnabled;
        private static string _smtpServer;
        private static int _smtpServerPortNumber;
        private static bool _smtpIsSSLServer;
        private static string _smtpUser;
        private static string _smtpPassword;
        private static int _smtpTimeOut;
        private static string _emailDefaultTo;
        private static string _emailDefaultFrom;

        static EmailHelper()
        {
            _sendEmailEnabled = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.EnableMailSend).ToBool();
            _smtpServer = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SmtpServer);
            _smtpServerPortNumber = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SmtpServerPortNumber).ToInt();
            _smtpIsSSLServer = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SmtpServerIsSsl).ToBool();
            _smtpUser = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SmtpUser);
            _smtpPassword = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SmtpPassword);
            _smtpTimeOut = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.SmtpTimeout).ToInt();
            _emailDefaultTo = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.EmailDefaultTo);
            _emailDefaultFrom = ConfigHelper.GetAppSetting(CommonAppSettingKeyName.EmailDefaultFrom);
        }

        /// <summary>
        /// Send e-mail using predefined configurations at initialization
        /// </summary>
        public static string Send(BaseEmail email)
        {
            if (!_sendEmailEnabled)
                return string.Empty;

            try
            {
                if (string.IsNullOrWhiteSpace(email.From))
                    email.From = _emailDefaultFrom;

                if (string.IsNullOrWhiteSpace(email.To))
                    email.To = _emailDefaultTo;

                var message = new MailMessage
                {
                    From = new MailAddress(email.From, email.FromName),
                    IsBodyHtml = email.IsHtml,
                    Subject = email.Subject,
                    Body = email.Body,
                };

                foreach (string addressTo in email.To.Split(';', ',').Where(e => !string.IsNullOrWhiteSpace(e)))
                    message.To.Add(addressTo.Trim());

                foreach (var attachs in email.Attachments.Where(at => at.FileStream.Length > default(long)))
                    message.Attachments.Add(new Attachment(attachs.FileStream, attachs.FileNameWithExtension));

                SmtpClient smtpClient = GetSmtpClient();
                smtpClient.Send(message);

                return string.Empty;
            }
            catch (Exception ex)
            {
                return ex.GetAllExceptionMessages();
            }
        }

        /// <summary>
        /// Get pre-configured SMTP server to send
        /// </summary>
        private static SmtpClient GetSmtpClient()
        {
            var clienteSmtp = new SmtpClient(_smtpServer, _smtpServerPortNumber)
            {
                EnableSsl = _smtpIsSSLServer,
                Timeout = (_smtpTimeOut <= 0 ? 10000 : _smtpTimeOut)
            };

            if (!string.IsNullOrEmpty(_smtpUser))
            {
                var NetworkCredential = new NetworkCredential(_smtpUser, _smtpPassword);
                clienteSmtp.Credentials = NetworkCredential;
            }

            return (clienteSmtp);
        }
    }
}
