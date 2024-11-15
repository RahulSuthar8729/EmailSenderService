using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSenderService.Database
{
    public class EmailData    {
        public long Code { get; set; }
        public string MailTo { get; set; }
        public string MailFrom { get; set; }
        public string Subject { get; set; }
        public string MailCcTo { get; set; }
        public string MailBccTo { get; set; }
        public string MailHeader { get; set; }
        public string MailFooter { get; set; }
        public string MailMessage { get; set; }
        public string DataFlag { get; set; }
        public int? FY_ID { get; set; }
        public DateTime? DOE { get; set; }
        public string MessageId { get; set; }
        public string DelStatus { get; set; }
        public DateTime? DoneAt { get; set; }
        public DateTime? SentAt { get; set; }
        public string StatusReason { get; set; }
        public string? SingleBulk { get; set; }
        public string SmsSignature { get; set; }
        public string Msg { get; set; }
        public bool? IsMailSent { get; set; }
        public bool? IsSmsSent { get; set; }
        public string MobileNo { get; set; }
        public string ReceiveIdCode { get; set; }
        public string FileHtml { get; set; }
        
    }
}
