namespace UstaPlatform.Domain.Interfaces;

/// <summary>
/// Fiyatlandırma kuralı arayüzü - Plugin mimarisi için
/// Açık/Kapalı Prensibi (OCP) uygulaması
/// </summary>
public interface IPricingRule
{
    /// <summary>
    /// Kuralın adı
    /// </summary>
    string RuleAdi { get; }
    
    /// <summary>
    /// Kuralın açıklaması
    /// </summary>
    string Aciklama { get; }
    
    /// <summary>
    /// Kural önceliği (düşük değer = önce çalışır)
    /// </summary>
    int Oncelik { get; }
    
    /// <summary>
    /// Fiyat hesaplama metodu
    /// </summary>
    /// <param name="temelFiyat">Temel fiyat</param>
    /// <param name="context">Fiyatlandırma bağlamı (tarih, süre, vb.)</param>
    /// <returns>Kural uygulandıktan sonraki fiyat</returns>
    decimal HesaplaFiyat(decimal temelFiyat, PricingContext context);
    
    /// <summary>
    /// Bu kuralın geçerli olup olmadığını kontrol eder
    /// </summary>
    bool KuralGecerliMi(PricingContext context);
}

/// <summary>
/// Fiyatlandırma bağlamı - kuralların karar vermesi için gerekli bilgiler
/// </summary>
public class PricingContext
{
    public required Guid TalepId { get; init; }
    public required Guid UstaId { get; init; }
    public required string UzmanlikAlani { get; init; }
    public required DateTime BaslangicTarihi { get; init; }
    public required decimal TahminiSure { get; init; }
    public required decimal SaatBasiUcret { get; init; }
    public bool Acil { get; init; }
    public int AdresX { get; init; }
    public int AdresY { get; init; }
    public string? Mahalle { get; init; }
    public Dictionary<string, object> EkBilgiler { get; init; } = new();
}
