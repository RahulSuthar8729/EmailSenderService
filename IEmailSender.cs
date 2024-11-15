using EmailSenderService.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderService
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailData emailData);
    }
}
