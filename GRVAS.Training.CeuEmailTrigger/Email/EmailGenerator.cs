namespace GRVAS.Training.CeuEmailTrigger.Email;

internal class EmailGenerator : IEmailGenerator
{
    private readonly string _sender;
    private readonly string _recipient;

    public EmailGenerator(
        string sender,
        string recipient)
    {
        _sender = sender;
        _recipient = recipient;
    }

    public MimeMessage Generate(string emailBodyContent, string month)
    {
        try
        {
            var message = new MimeMessage();

            message.To.Add(new MailboxAddress(string.Empty, _recipient));

            message.From.Add(new MailboxAddress(string.Empty, _sender));
            message.Subject = $"GRVAS {month} CEU Opportunities";

            var emailBody = new BodyBuilder();
            /*string emailTextBody;
            var path = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Email", "EmailTemplates", "ExportEmail.html");
            emailTextBody = File.ReadAllText(path);
            emailTextBody = emailTextBody.Replace("{RouteReporterUrl}", $"{_routeReporterUrl}");*/

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = emailBodyContent;

            message.Body = bodyBuilder.ToMessageBody();

            return message;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to generate email body. Exc: [{e}]");
            return null;
        }
    }
}
