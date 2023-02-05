namespace GRVAS.Training.CeuEmailTrigger.Validation;

internal class ClassValidator : IClassValidator
{

    private readonly ILogger<ClassValidator> _logger;

    public bool Validate(List<List<CeuClass>> classLists)
    {
        try
        {
            var classes = new List<CeuClass>();
            foreach (var classList in classLists )
            {
                classes = classes.Concat(classes).ToList();
            }

            if (classes == null || classes.Count == 0)
            {
                return false;
            }

            foreach (CeuClass c in classes)
            {
                if (c == null)
                {
                    return false;
                }
                if (string.IsNullOrEmpty(c.Title))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(c.Time))
                {
                    return false;
                }
                if (string.IsNullOrEmpty(c.Date))
                {
                    return false;
                }
            }

            return true;
        }
        catch(Exception ex)
        {
            _logger.LogError($"Error validating classes. Exc: {ex}");
            return false;
        }
    }
}
