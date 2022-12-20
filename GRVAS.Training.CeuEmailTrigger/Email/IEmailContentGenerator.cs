namespace GRVAS.Training.CeuEmailTrigger.Email;

internal interface IEmailContentGenerator
{
    string Generate(string month);
}