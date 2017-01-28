using System;

namespace Home.Bills.Notification.Infrastructure
{
    public class SmtpAccount
    {
        public Guid Id { get; set; }

        public string FromAddress { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public int Port { get; set; }

        public bool EnableSSl { get; set; }

        public string Server { get; set; }
    }
}