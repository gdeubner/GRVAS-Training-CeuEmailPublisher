namespace GRVAS.Training.CeuEmailCreator.Model;

internal class CeuClass
{
    public bool IsInPerson { get; set; }
    public string? Title { get; set; }
    public string? Note { get; set; }
    public int Enrolled { get; set; }
    public int MaxEnrolled { get; set; }
    public string? Cost { get; set; }
    public string? Date { get; set; }
    public string? Time { get; set; }
    public string? Description { get; set; }
    public string? LocationName { get; set; }
    public string? StreetAddress { get; set; }
    public string? Town { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }

    public string FormClassOutput()
    {
        return @$"Class Title: {Title}
            Time: {Date}, {Time}
            Location: {LocationName}
            Cost: ${Cost}, Open Spots: {(MaxEnrolled == 0 ? "Unknown" : MaxEnrolled - Enrolled)}
            Description: {Description}";
    }
}
