using System.Globalization;

namespace UstaPlatform.Domain.Helpers;

/// <summary>
/// Para formatı yardımcı static sınıfı (MoneyFormatter)
/// </summary>
public static class ParaFormatlayici
{
    private static readonly CultureInfo TurkishCulture = new CultureInfo("tr-TR");
    
    /// <summary>
    /// Tutarı Türk Lirası formatında gösterir (₺1.234,56)
    /// </summary>
    public static string Format(decimal tutar)
    {
        return tutar.ToString("C2", TurkishCulture);
    }
    
    /// <summary>
    /// Tutarı Türk Lirası formatında gösterir (₺1.234,56)
    /// </summary>
    public static string FormatTL(decimal tutar)
    {
        return $"{tutar:N2} TL";
    }
    
    /// <summary>
    /// Tutarı kısa formatda gösterir (1,234.56)
    /// </summary>
    public static string FormatShort(decimal tutar)
    {
        return tutar.ToString("N2", CultureInfo.InvariantCulture);
    }
    
    /// <summary>
    /// String'den decimal'e parse eder
    /// </summary>
    public static decimal Parse(string tutarStr)
    {
        tutarStr = tutarStr.Replace("₺", "").Replace("TL", "").Trim();
        return decimal.Parse(tutarStr, TurkishCulture);
    }
    
    /// <summary>
    /// String'den decimal'e parse etmeyi dener
    /// </summary>
    public static bool TryParse(string tutarStr, out decimal tutar)
    {
        tutarStr = tutarStr.Replace("₺", "").Replace("TL", "").Trim();
        return decimal.TryParse(tutarStr, NumberStyles.Currency, TurkishCulture, out tutar);
    }
}
