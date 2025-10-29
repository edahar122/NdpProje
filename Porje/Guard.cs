namespace UstaPlatform.Domain.Helpers;

public static class Guard
{
    public static void AgainstNullOrEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException($"{paramName} null veya bos olamaz.", paramName);
        }
    }
    
    public static void AgainstNull<T>(T value, string paramName) where T : class
    {
        if (value == null)
        {
            throw new ArgumentNullException(paramName, $"{paramName} null olamaz.");
        }
    }
    
    public static void AgainstOutOfRange(int value, int min, int max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, 
                $"{paramName} {min} ile {max} arasinda olmalidir. Girilen: {value}");
        }
    }
    
    public static void AgainstOutOfRange(decimal value, decimal min, decimal max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, 
                $"{paramName} {min} ile {max} arasinda olmalidir. Girilen: {value}");
        }
    }
    
    public static void AgainstNegative(decimal value, string paramName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, 
                $"{paramName} negatif olamaz. Girilen: {value}");
        }
    }
    
    public static void AgainstNullOrEmpty<T>(IEnumerable<T> collection, string paramName)
    {
        if (collection == null || !collection.Any())
        {
            throw new ArgumentException($"{paramName} null veya bos olamaz.", paramName);
        }
    }
}
