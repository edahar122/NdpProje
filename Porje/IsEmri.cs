namespace UstaPlatform.Domain.Entities;

/// <summary>
/// Onaylanmış, ustaya atanmış ve planlanmış iş (WorkOrder)
/// </summary>
public class IsEmri
{
    /// <summary>
    /// Benzersiz kimlik - init-only
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// İlişkili talep ID
    /// </summary>
    public required Guid TalepId { get; init; }
    
    /// <summary>
    /// Atanan usta ID
    /// </summary>
    public required Guid UstaId { get; init; }
    
    /// <summary>
    /// Planlanan başlangıç tarihi ve saati
    /// </summary>
    public DateTime PlanlananBaslangic { get; set; }
    
    /// <summary>
    /// Planlanan bitiş tarihi ve saati
    /// </summary>
    public DateTime PlanlananBitis { get; set; }
    
    /// <summary>
    /// Hesaplanan fiyat (TL)
    /// </summary>
    public decimal Fiyat { get; set; }
    
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
    /// Rota sıra numarası (günlük rotada kaçıncı durak)
    /// </summary>
    public int? RotaSirasi { get; set; }
    
    /// <summary>
    /// İş emri durumu
    /// </summary>
    public IsEmriDurumu Durum { get; set; }
    
    /// <summary>
    /// Oluşturulma zamanı - init-only
    /// </summary>
    public DateTime OlusturulmaZamani { get; init; }
    
    /// <summary>
    /// Gerçek başlangıç zamanı
    /// </summary>
    public DateTime? GercekBaslangic { get; set; }
    
    /// <summary>
    /// Gerçek bitiş zamanı
    /// </summary>
    public DateTime? GercekBitis { get; set; }
    
    /// <summary>
    /// Notlar
    /// </summary>
    public string? Notlar { get; set; }
    
    public IsEmri()
    {
        Id = Guid.NewGuid();
        OlusturulmaZamani = DateTime.Now;
        Durum = IsEmriDurumu.Planlanmis;
    }
}

/// <summary>
/// İş emri durumu enum
/// </summary>
public enum IsEmriDurumu
{
    Planlanmis,
    Devam,
    Tamamlandi,
    Iptal
}
