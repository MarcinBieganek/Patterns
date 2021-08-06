using System;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Zadanie_1
{
    public class SmtpClientFacade
    {
        public void Send(string From, string To, string Subject, 
                         string Body, Stream Attachment, string AttachmentMimeType)
        {
            SmtpClient client   = new SmtpClient();
            MailMessage message = new MailMessage(From, To, Subject, Body);

            ContentType ct      = new ContentType(AttachmentMimeType);
            Attachment data     = new Attachment(Attachment, ct);
            message.Attachments.Add(data);

            client.Send(message);
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            SmtpClientFacade smtpclientf = new SmtpClientFacade();

            Console.ReadLine();
        }
    }
}
