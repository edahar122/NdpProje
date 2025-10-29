namespace UstaPlatform.Domain.Entities;

/// <summary>
/// Vatandaşın açtığı iş talebi (Request)
/// </summary>
public class Talep
{
    /// <summary>
    /// Benzersiz kimlik - init-only
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Talep eden vatandaş ID
    /// </summary>
    public required Guid VatandasId { get; init; }
    
    /// <summary>
    /// İş açıklaması
    /// </summary>
    public required string Aciklama { get; init; }
    
    /// <summary>
    /// Uzmanlık alanı gereksinimi
    /// </summary>
    public required string UzmanlikAlani { get; init; }
    
    /// <summary>
    /// Talep edilen tarih ve saat
    /// </summary>
    public DateTime TalepEdilmeTarihi { get; init; }
    
    /// <summary>
    /// İstenen başlangıç tarihi
    /// </summary>
    public DateTime? IstenenTarih { get; init; }
    
    /// <summary>
    /// Tahmini süre (saat)
    /// </summary>
    public decimal TahminiSure { get; init; }
    
    /// <summary>
    /// Adres koordinatı X
    /// </summary>
    public int AdresX { get; init; }
    
    /// <summary>
    /// Adres koordinatı Y
    /// </summary>
    public int AdresY { get; init; }
    
    /// <summary>
    /// Adres detayı
    /// </summary>
    public required string AdresDetay { get; init; }
    
    /// <summary>
    /// Acil mi?
    /// </summary>
    public bool Acil { get; init; }
    
    /// <summary>
    /// Talep durumu
    /// </summary>
    public TalepDurumu Durum { get; set; }
    
    public Talep()
    {
        Id = Guid.NewGuid();
        TalepEdilmeTarihi = DateTime.Now;
        Durum = TalepDurumu.Beklemede;
    }
}

/// <summary>
/// Talep durumu enum
/// </summary>
public enum TalepDurumu
{
    Beklemede,
    Onaylandi,
    Reddedildi,
    Tamamlandi,
    Iptal
}
