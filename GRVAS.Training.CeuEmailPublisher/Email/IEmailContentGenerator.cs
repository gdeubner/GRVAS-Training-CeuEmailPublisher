namespace GRVAS.Training.CeuEmailCreator.Email;

internal interface IEmailContentGenerator
{
    string Generate(string month);
}