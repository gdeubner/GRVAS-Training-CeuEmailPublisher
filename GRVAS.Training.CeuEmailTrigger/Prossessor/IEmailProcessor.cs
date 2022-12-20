namespace GRVAS.Training.CeuEmailTrigger.Prossessor;

internal interface IEmailProcessor
{
    Task<bool> ProcessAsync();
}