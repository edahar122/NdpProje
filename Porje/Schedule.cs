namespace UstaPlatform.Domain.Entities;

/// <summary>
/// Ustaların iş emri takvimi - Indexer ile DateOnly erişimi
/// </summary>
public class Schedule
{
    private readonly Dictionary<DateOnly, List<IsEmri>> _takvim = new();
    
    /// <summary>
    /// Çizelge sahibi usta ID
    /// </summary>
    public Guid UstaId { get; init; }
    
    /// <summary>
    /// İndexer - Tarihe göre iş emirlerine erişim
    /// Schedule[DateOnly gün] yapısı
    /// </summary>
    public List<IsEmri> this[DateOnly tarih]
    {
        get
        {
            if (!_takvim.ContainsKey(tarih))
            {
                _takvim[tarih] = new List<IsEmri>();
            }
            return _takvim[tarih];
        }
    }
    
    /// <summary>
    /// İş emri ekleme
    /// </summary>
    public void IsEmriEkle(IsEmri isEmri)
    {
        var tarih = DateOnly.FromDateTime(isEmri.PlanlananBaslangic);
        this[tarih].Add(isEmri);
    }
    
    /// <summary>
    /// İş emri kaldırma
    /// </summary>
    public bool IsEmriKaldir(Guid isEmriId)
    {
        foreach (var tarih in _takvim.Keys)
        {
            var isEmri = _takvim[tarih].FirstOrDefault(ie => ie.Id == isEmriId);
            if (isEmri != null)
            {
                _takvim[tarih].Remove(isEmri);
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Belirtilen tarihte iş var mı?
    /// </summary>
    public bool IsVarMi(DateOnly tarih)
    {
        return _takvim.ContainsKey(tarih) && _takvim[tarih].Count > 0;
    }
    
    /// <summary>
    /// Belirtilen tarih aralığındaki tüm iş emirlerini getir
    /// </summary>
    public IEnumerable<IsEmri> GetIsEmirleri(DateOnly baslangic, DateOnly bitis)
    {
        return _takvim
            .Where(kvp => kvp.Key >= baslangic && kvp.Key <= bitis)
            .SelectMany(kvp => kvp.Value)
            .OrderBy(ie => ie.PlanlananBaslangic);
    }
    
    /// <summary>
    /// Tüm tarihleri getir
    /// </summary>
    public IEnumerable<DateOnly> Tarihler => _takvim.Keys.OrderBy(t => t);
    
    /// <summary>
    /// Toplam iş emri sayısı
    /// </summary>
    public int ToplamIsEmri => _takvim.Values.Sum(list => list.Count);
    
    /// <summary>
    /// Belirtilen tarihteki toplam çalışma saati
    /// </summary>
    public decimal GunlukCalismaSaati(DateOnly tarih)
    {
        if (!_takvim.ContainsKey(tarih))
            return 0;
        
        return _takvim[tarih]
            .Sum(ie => (decimal)(ie.PlanlananBitis - ie.PlanlananBaslangic).TotalHours);
    }
}
