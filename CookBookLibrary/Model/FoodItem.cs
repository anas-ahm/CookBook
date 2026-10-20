namespace CookBookLibrary.Model;

public class FoodItem
{
    // Instance Field
    private int _id;
    private string _name = string.Empty;
    private double _calories;
    private double _protein;
    private double _fat;
    private double _saturatedFat;
    private double _carbs;
    private double _sugar;
    private double _fiber;
    private double _salt;

    // All nutrition values are per 100 g
    public int Id
    {
        get { return _id; }
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Id), "Id cannot be negative.");
            }
            _id = value;
        }
    }
    
    public string Name
    {
        get { return _name; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("Name cannot be empty.", nameof(Name));
            }
            _name = value;
        }
    }

    public double Calories
    {
        get { return _calories; }
        set
        {
            CheckNotNegative(value, nameof(Calories));
            _calories = value;
        }
    }

    public double Protein
    {
        get { return _protein; }
        set
        {
            CheckNotNegative(value, nameof(Protein));
            _protein = value;
        }
    }

    public double Fat
    {
        get { return _fat; }
        set
        {
            CheckNotNegative(value, nameof(Fat));
            if (value < _saturatedFat)
            {
                throw new ArgumentOutOfRangeException(nameof(Fat), "Fat cannot be lower than saturated fat.");
            }
            _fat = value;
        }
    }

    public double SaturatedFat
    {
        get { return _saturatedFat; }
        set
        {
            CheckNotNegative(value, nameof(SaturatedFat));
            if (value > _fat)
            {
                throw new ArgumentOutOfRangeException(nameof(SaturatedFat), "Saturated fat cannot be higher than fat.");
            }
            _saturatedFat = value;
        }
    }

    public double Carbs
    {
        get { return _carbs; }
        set
        {
            CheckNotNegative(value, nameof(Carbs));
            if (value < _sugar || value < _fiber)
            {
                throw new ArgumentOutOfRangeException(nameof(Carbs), "Carbs cannot be lower than sugar or fiber.");
            }
            _carbs = value;
        }
    }

    public double Sugar
    {
        get { return _sugar; }
        set
        {
            CheckNotNegative(value, nameof(Sugar));
            if (value > _carbs)
            {
                throw new ArgumentOutOfRangeException(nameof(Sugar), "Sugar cannot be higher than carbs.");
            }
            _sugar = value;
        }
    }

    public double Fiber
    {
        get { return _fiber; }
        set
        {
            CheckNotNegative(value, nameof(Fiber));
            if (value > _carbs)
            {
                throw new ArgumentOutOfRangeException(nameof(Fiber), "Fiber cannot be higher than carbs.");
            }
            _fiber = value;
        }
    }

    public double Salt
    {
        get { return _salt; }
        set
        {
            CheckNotNegative(value, nameof(Salt));
            _salt = value;
        }
    }

    private static void CheckNotNegative(double value, string propertyName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(propertyName, "Value cannot be negative.");
        }
    }
}