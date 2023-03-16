using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Acetesa.TomaPedidos.Transversal
{
    public static class Mail
    {

        public static void SendMail(
            string CredentialUsername, string CredentialPassword, string CredentialKey, string emailAddress, string labelAddress, string subject, StringBuilder body, string toEmail, string conCopia, string attachmentFilename, string attachmentFilename2 = null, string attachmentFilename3 = null, bool esHtml = false, string copiaOculta = null)
        {
            var decryptedPassword = AesOperation.DecryptString(CredentialKey, CredentialPassword);

            var mail = new MailMessage();
            var smtpServer = new SmtpClient();
            smtpServer.Host = "smtp.gmail.com";
            smtpServer.Port = 587;
            smtpServer.EnableSsl = true;
            smtpServer.Credentials = new System.Net.NetworkCredential(CredentialUsername, decryptedPassword/*CredentialPassword*/);
            try
            {
                mail.From = new MailAddress(emailAddress, labelAddress);
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = esHtml;
                mail.Subject = subject;
                mail.Body = body.ToString();

                mail.To.Add(toEmail);
                if (!string.IsNullOrEmpty(conCopia)) mail.CC.Add(new MailAddress(conCopia, ""));
                if (!string.IsNullOrEmpty(copiaOculta)) mail.Bcc.Add(new MailAddress(copiaOculta, ""));
                //Attachment 1
                if (!string.IsNullOrEmpty(attachmentFilename))
                {
                    var attachment = new Attachment(attachmentFilename, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename);
                    disposition.FileName = Path.GetFileName(attachmentFilename);
                    disposition.Size = new FileInfo(attachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                //Attachment 2
                if (!string.IsNullOrEmpty(attachmentFilename2))
                {
                    var attachment = new Attachment(attachmentFilename2, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename2);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename2);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename2);
                    disposition.FileName = Path.GetFileName(attachmentFilename2);
                    disposition.Size = new FileInfo(attachmentFilename2).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                //Attachment 3
                if (!string.IsNullOrEmpty(attachmentFilename3))
                {
                    var attachment = new Attachment(attachmentFilename3, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(attachmentFilename3);
                    disposition.ModificationDate = File.GetLastWriteTime(attachmentFilename3);
                    disposition.ReadDate = File.GetLastAccessTime(attachmentFilename3);
                    disposition.FileName = Path.GetFileName(attachmentFilename3);
                    disposition.Size = new FileInfo(attachmentFilename3).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                smtpServer.Send(mail);
            }
            finally
            {
                smtpServer.Dispose();
                mail.Dispose();
            }
        }

        public static void SendMail(string smptClient, string userNetworkCredential, string passNetworkCredential,
            string emailAddress, string labelAddress, string subject, StringBuilder body, string toEmail, bool esHtml = false)
        {
            var mail = new MailMessage();
            var smtpServer = new SmtpClient(smptClient)
            {
                Credentials = new NetworkCredential(userNetworkCredential, passNetworkCredential)
            };

            try
            {
                mail.From = new MailAddress(emailAddress, labelAddress);
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = esHtml;
                mail.Subject = subject;
                mail.Body = body.ToString();
                mail.To.Add(toEmail);
                smtpServer.Send(mail);
            }
            finally
            {
                smtpServer.Dispose();
                mail.Dispose();
            }
        }
        public static void SendMail(string smptClient, string userNetworkCredential, string passNetworkCredential,
            string emailAddress, string labelAddress, string subject, string body, string toEmail, bool esHtml = false)
        {
            var mail = new MailMessage();
            var smtpServer = new SmtpClient(smptClient)
            {
                Credentials = new NetworkCredential(userNetworkCredential, passNetworkCredential)
            };

            try
            {
                mail.From = new MailAddress(emailAddress, labelAddress);
                mail.BodyEncoding = Encoding.UTF8;
                mail.IsBodyHtml = esHtml;
                mail.Subject = subject;
                mail.Body = body;

                mail.To.Add(toEmail);
                smtpServer.Send(mail);
            }
            finally
            {
                smtpServer.Dispose();
                mail.Dispose();
            }
        }


        public abstract class MailParams
        {
            public string SmtpClient { get; set; }
            public string UserNetworkCredential { get; set; }
            public string PassNetworkCredential { get; set; }
            public string Remitente { get; set; }

            public string Destinatario { get; set; }

            public string Asunto { get; set; }

            public string Cuerpo { get; set; }

            public string Copia { get; set; }

            public string CopiaOculta { get; set; }

            public string RemitenteLabel { get; set; }

            public string AttachmentFilename { get; set; }
            public bool EsHtml { get; set; }
        }

        public static void SendMail(MailParams mailParams)
        {
            SmtpClient client;
            if (!string.IsNullOrEmpty(mailParams.SmtpClient) && 
                !string.IsNullOrEmpty(mailParams.UserNetworkCredential) && 
                !string.IsNullOrEmpty(mailParams.PassNetworkCredential))
            {
                client = new SmtpClient(mailParams.SmtpClient)
                {
                    Credentials = new NetworkCredential(mailParams.UserNetworkCredential, mailParams.PassNetworkCredential)
                };
            }
            else
            {
                client = new SmtpClient();
            }
            var mail = new MailMessage();
            try
            {
                mail.From = new MailAddress(mailParams.Remitente, mailParams.RemitenteLabel);
                mail.To.Add(new MailAddress(mailParams.Destinatario, ""));
                if (!string.IsNullOrEmpty(mailParams.Copia)) mail.CC.Add(new MailAddress(mailParams.Copia, ""));
                if (!string.IsNullOrEmpty(mailParams.CopiaOculta)) mail.Bcc.Add(new MailAddress(mailParams.CopiaOculta, ""));
                mail.Subject = mailParams.Asunto;
                mail.Body = mailParams.Cuerpo;
                mail.IsBodyHtml = mailParams.EsHtml;
                mail.Priority = MailPriority.Normal;
                if (!string.IsNullOrEmpty(mailParams.AttachmentFilename))
                {
                    var attachment = new Attachment(mailParams.AttachmentFilename, MediaTypeNames.Application.Octet);
                    var disposition = attachment.ContentDisposition;
                    disposition.CreationDate = File.GetCreationTime(mailParams.AttachmentFilename);
                    disposition.ModificationDate = File.GetLastWriteTime(mailParams.AttachmentFilename);
                    disposition.ReadDate = File.GetLastAccessTime(mailParams.AttachmentFilename);
                    disposition.FileName = Path.GetFileName(mailParams.AttachmentFilename);
                    disposition.Size = new FileInfo(mailParams.AttachmentFilename).Length;
                    disposition.DispositionType = DispositionTypeNames.Attachment;
                    mail.Attachments.Add(attachment);
                }
                client.Send(mail);
            }
            finally
            {
                client.Dispose();
                mail.Dispose();
            }
        }
    }
}
