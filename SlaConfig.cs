public class SlaConfig
{
    public List<SlaRule> Rules { get; set; } = [];
}

public class SlaRule
{
    public required string Category { get; set; }
    public required string Impact { get; set; }

    private int _maxDays;
    public int MaxDays
    {
        get => _maxDays;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(MaxDays), "MaxDays harus lebih dari 0.");
            _maxDays = value;
        }
    }
}