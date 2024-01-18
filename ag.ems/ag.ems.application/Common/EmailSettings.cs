namespace ag.ems.application.Common
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = null!;
        public int SmtpPort { get; set; }
        public string FromAddress { get; set; } = null!;
    }
}