using EmailSenderService;
using EmailSenderService.Database;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;

public class EmailService : IEmailSender
{
    private readonly PdfGenerator _pdfGenerator;

    public EmailService(PdfGenerator pdfGenerator)
    {
        _pdfGenerator = pdfGenerator;
    }

    public async Task SendEmailAsync(EmailData emailData)
    {
        
        byte[] pdfBytes = _pdfGenerator.GeneratePdf(emailData.FileHtml);

        var email = new MimeMessage();

       
        email.From.Add(new MailboxAddress("Narayan Seva Sansthan", "donation@narayanseva.org"));
        email.To.Add(MailboxAddress.Parse("soft.dv1@narayanseva.org"));
       // email.To.Add(MailboxAddress.Parse(emailData.MailTo));
        email.Subject = emailData.Subject;

        var bodyBuilder = new BodyBuilder
        {
            HtmlBody = emailData.MailMessage
        };
        
        if (pdfBytes != null && pdfBytes.Length > 0)
        {
            bodyBuilder.Attachments.Add($"{emailData.Subject}.pdf", pdfBytes, ContentType.Parse("application/pdf"));
        }

        email.Body = bodyBuilder.ToMessageBody();

        using var smtp = new SmtpClient();
        smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        smtp.Authenticate("donation@narayanseva.org", "Don@!ion@NSS");
        await smtp.SendAsync(email);
        smtp.Disconnect(true);
    }
}