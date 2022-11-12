using MimeKit;

namespace GRVAS.Training.CeuEmailCreator.Email;

internal interface IEmailGenerator
{
    MimeMessage Generate(string emailBodyContent, string month);
}