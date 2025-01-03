using System.Net;

internal class EmailConnectionInfo
{
    public string FromEmail { get; set; }
    public string ToEmail { get; set; }
    public string MailServer { get; set; }
    public NetworkCredential NetworkCredentials { get; set; }
    public int Port { get; set; }
    public bool EnableSsl { get; set; }
    public string Subject { get; set; }
}