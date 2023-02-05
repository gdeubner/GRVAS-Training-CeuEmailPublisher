namespace GRVAS.Training.CeuEmailTrigger.Validation;

internal interface IClassValidator
{
    bool Validate(List<List<CeuClass>> classLists);
}