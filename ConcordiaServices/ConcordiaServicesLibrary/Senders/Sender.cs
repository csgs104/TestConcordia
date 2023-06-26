namespace ConcordiaServicesLibrary.Senders;

using System.Net;
using System.Net.Mail;
using System.Xml.Linq;

public class Sender
{
    private readonly string _fromEmail;
    private readonly string _fromPassword;
    private readonly string _toEmail;
    private readonly string _host;
    private readonly int _port;

    public Sender(string fromEmail, string fromPassword, 
	              string toEmail, string host, int port)
    {
        _fromEmail = fromEmail;
        _fromPassword = fromPassword;
        _toEmail = toEmail;
        _host = host;
        _port = port;
    }

    public Sender(string email, string password, string host, int port) 
	 : this(email, email, password, host, port)
    { }

    public virtual void SendMail(string subject, string body, string file)
    {
        try
        {
            // configuring SmtpClient
            using var smtpClient = new SmtpClient(_host, _port);
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(_fromEmail, _fromPassword);
            smtpClient.EnableSsl = true;
            // configuring Attachment
            using var attachment = new Attachment(file);
            // configuring Mail
            using var mailMessage = new MailMessage(_fromEmail, _toEmail, subject, body);
            mailMessage.Attachments.Add(attachment);
            // sending Mail
            smtpClient.Send(mailMessage);
            // Console.WriteLine("Email has been sent.");
        }
        catch /* (Exception ex) */
        {
            // Console.WriteLine("Email has not been sent.");
            // Console.WriteLine($"Error: {ex.Message}.");
        }
    }
}