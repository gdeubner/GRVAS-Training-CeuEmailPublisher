namespace GRVAS.Training.CeuEmailTrigger.Email;

internal interface IEmailGenerator
{
    MimeMessage Generate(string emailBodyContent, string month, bool classesValid);
}