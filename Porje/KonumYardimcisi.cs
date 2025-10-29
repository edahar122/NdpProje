namespace UstaPlatform.Domain.Helpers;

/// <summary>
/// Konum/Koordinat yardımcı static sınıfı (GeoHelper)
/// </summary>
public static class KonumYardimcisi
{
    /// <summary>
    /// İki nokta arasındaki Manhattan mesafesini hesaplar
    /// </summary>
    public static int ManhattanMesafe((int X, int Y) nokta1, (int X, int Y) nokta2)
    {
        return Math.Abs(nokta2.X - nokta1.X) + Math.Abs(nokta2.Y - nokta1.Y);
    }
    
    /// <summary>
    /// İki nokta arasındaki Manhattan mesafesini hesaplar
    /// </summary>
    public static int ManhattanMesafe(int x1, int y1, int x2, int y2)
    {
        return Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
    }
    
    /// <summary>
    /// İki nokta arasındaki Euclidean (kuş uçuşu) mesafesini hesaplar
    /// </summary>
    public static double EuclideanMesafe((int X, int Y) nokta1, (int X, int Y) nokta2)
    {
        int dx = nokta2.X - nokta1.X;
        int dy = nokta2.Y - nokta1.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    /// <summary>
    /// İki nokta arasındaki Euclidean (kuş uçuşu) mesafesini hesaplar
    /// </summary>
    public static double EuclideanMesafe(int x1, int y1, int x2, int y2)
    {
        int dx = x2 - x1;
        int dy = y2 - y1;
        return Math.Sqrt(dx * dx + dy * dy);
    }
    
    /// <summary>
    /// Koordinatların geçerli olup olmadığını kontrol eder
    /// </summary>
    public static bool KoordinatGecerliMi(int x, int y, int minX = 0, int maxX = 1000, int minY = 0, int maxY = 1000)
    {
        return x >= minX && x <= maxX && y >= minY && y <= maxY;
    }
    
    /// <summary>
    /// Bir noktanın belirli bir merkeze olan uzaklığını hesaplar
    /// </summary>
    public static int MerkezUzaklik((int X, int Y) nokta, (int X, int Y) merkez)
    {
        return ManhattanMesafe(nokta, merkez);
    }
    
    /// <summary>
    /// Koordinat bilgisini string formatında döndürür
    /// </summary>
    public static string KoordinatFormat(int x, int y)
    {
        return $"({x}, {y})";
    }
    
    /// <summary>
    /// Koordinat bilgisini string formatında döndürür
    /// </summary>
    public static string KoordinatFormat((int X, int Y) konum)
    {
        return $"({konum.X}, {konum.Y})";
    }
}
