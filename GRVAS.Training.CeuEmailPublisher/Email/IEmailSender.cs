using MimeKit;

namespace GRVAS.Training.CeuEmailCreator.Email;

internal interface IEmailSender
{
    Task<bool> SendAsync(MimeMessage mimeMessage, string month);
}