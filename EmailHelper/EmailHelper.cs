using System.Net.Mail;
using System.Net;

public class EmailHelper
{
    public async Task SendEmailAsync(string recipientEmail, string subject, string body)
    {
        try
        {
            using (var smtpClient = new SmtpClient("smtp.gmail.com", 587)) 
            {
                smtpClient.Credentials = new NetworkCredential("nhiallualcho@gmail.com", "gqjm jupa qrwj bqsq"); 
                smtpClient.EnableSsl = true;
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("nhiallualcho@gmail.com", "padiatric"), 
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(recipientEmail);

                await smtpClient.SendMailAsync(mailMessage);
            }
        }
        catch (Exception ex)
        {
            // Handle or log exception
            Console.WriteLine($"Error sending email: {ex.Message}");
        }
    }
}
