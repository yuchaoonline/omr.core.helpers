namespace OMR.Tests.Misc.Email
{
    using System;
    using System.Net.Mail;
    using System.Text;

    public class MailMessageGenerator
    {
        public MailAddress From { get; set; }

        public MailAddressCollection ReplyTo { get; set; }

        public MailAddressCollection To { get; set; }

        public MailAddressCollection Bcc { get; set; }

        public MailAddressCollection Cc { get; set; }

        public AlternateViewCollection AlternateViews { get; set; }

        public LinkedResourceCollection LinkedSources { get; set; }

        public AttachmentCollection Attachments { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        public MailPriority Priority { get; set; }

        public MailMessageGenerator()
        {
            To = new MailAddressCollection();
            Bcc = new MailAddressCollection();
            Cc = new MailAddressCollection();
            IsBodyHtml = true;
            Subject = string.Empty;
            Body = string.Empty;
        }

        public MailMessage Create()
        {
            if (From == null)
                throw new ArgumentNullException("FromMailAddress");
            if (To == null)
                throw new ArgumentNullException("ToMailAddress");

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = From;

            foreach (var item in To)
                mailMessage.To.Add(item);

            foreach (var item in ReplyTo)
                mailMessage.ReplyToList.Add(item);

            foreach (var item in Bcc)
                mailMessage.Bcc.Add(item);

            foreach (var item in Cc)
                mailMessage.CC.Add(item);

            
            if (Attachments != null)
            {
                foreach (var item in Attachments)
                    mailMessage.Attachments.Add(item);
            }

            if (AlternateViews != null)
            {
                foreach (var item in AlternateViews)
                {
                    foreach (var linkedSource in item.LinkedResources)
                        item.LinkedResources.Add(linkedSource);

                    mailMessage.AlternateViews.Add(item);
                }
            }

            mailMessage.Subject = Subject;
            mailMessage.Body = Body;
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.SubjectEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = IsBodyHtml;
            mailMessage.Priority = Priority;

            return mailMessage;
        }
    }
}
