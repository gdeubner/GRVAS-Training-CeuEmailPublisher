namespace GRVAS.Training.CeuEmailTrigger.Email;

internal interface IEmailSender
{
    Task<bool> SendAsync(MimeMessage mimeMessage, string month);
}