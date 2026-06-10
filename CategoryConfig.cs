public class CategoryConfig
{
    public List<CategoryItem> Categories { get; set; } = [];
}

public class CategoryItem
{
    private int _id;
    public int Id
    {
        get => _id;
        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(Id), "Id harus lebih dari 0.");
            _id = value;
        }
    }

    private string _name = string.Empty;
    public string Name
    {
        get => _name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(
                    "Name tidak boleh kosong atau whitespace.", nameof(Name));

            _name = value.Trim();
        }
    }
}