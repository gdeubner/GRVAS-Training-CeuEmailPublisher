namespace GRVAS.Training.CeuEmailCreator.Job;

internal interface IEmailProcessor
{
    Task<bool> ProcessAsync();
}