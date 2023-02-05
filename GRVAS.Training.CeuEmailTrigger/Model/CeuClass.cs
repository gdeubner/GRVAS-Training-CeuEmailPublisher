namespace GRVAS.Training.CeuEmailTrigger.Model;

internal class CeuClass
{
    public double? Ceus { get; set; }
    public bool? IsInPerson { get; set; }
    public string? Title { get; set; }
    public string? Note { get; set; }
    public int? Enrolled { get; set; }
    public int? MaxEnrolled { get; set; }
    public double? Cost { get; set; }
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
        return $"Class Title: {Title}\n" +
            $"Time: {Date}, {Time}\n" +
            $"Location: {LocationName}\n" +
            $"Cost: ${(Cost!=null?((double)Cost).ToString("F"):"Unknown")}, Open Spots:" +
            $"{(MaxEnrolled == null || Enrolled == null || MaxEnrolled == 0 ?"Unknown" : MaxEnrolled - Enrolled)}\n" +
            $"{(!string.IsNullOrEmpty(Description) ? $"Description: {Description}\n" : "")}" +
            $"{(!string.IsNullOrEmpty(Note) ? $"Note: {Note}\n" : "")}";
    }
}
