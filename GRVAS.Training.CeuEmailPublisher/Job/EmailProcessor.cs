using GRVAS.Training.CeuEmailCreator.Email;

namespace GRVAS.Training.CeuEmailCreator.Job;

internal class EmailProcessor : IEmailProcessor
{
    private readonly IEmailContentGenerator _emailContentGenerator;
    private readonly IEmailGenerator _emailGenerator;
    private readonly IEmailSender _emailSender;

    public EmailProcessor(
        IEmailContentGenerator emailContentGenerator,
        IEmailGenerator emailGenerator,
        IEmailSender emailSender)
    {
        _emailContentGenerator = emailContentGenerator;
        _emailGenerator = emailGenerator;
        _emailSender = emailSender;
    }

    public async Task<bool> ProcessAsync()
    {
        try
        {
            Console.WriteLine($"Starting Email Processor");

            //call email generator
            var month = DateTime.Now.ToString("MMMM");

            var emailContent = _emailContentGenerator.Generate(month);
            Console.WriteLine($"Email generated: {emailContent}");

            var mimeEmail = _emailGenerator.Generate(emailContent, month);

            var success = await _emailSender.SendAsync(mimeEmail, month);

            Console.WriteLine(success ? $"Successfully sent email" : "Failed to send email");


            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Processing failed. Exc: {e}");
            return false;
        }

    }
}
