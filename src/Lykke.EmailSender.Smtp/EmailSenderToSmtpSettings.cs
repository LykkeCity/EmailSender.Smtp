using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.EmailSender.Smtp
{
    public class EmailSenderToSmtpSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }

        public string LocalDomain { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public string DisplayName { get; set; }
        public string From { get; set; }

    }
}
