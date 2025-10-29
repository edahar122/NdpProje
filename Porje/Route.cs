using System.Collections;

namespace UstaPlatform.Domain.Entities;

/// <summary>
/// Bir uzmanın günlük ziyaret edeceği adreslerin sırası
/// IEnumerable<(int X, int Y)> implementasyonu ile koleksiyon başlatıcılarını destekler
/// </summary>
public class Route : IEnumerable<(int X, int Y)>
{
    private readonly List<(int X, int Y)> _duraklar = new();
    
    /// <summary>
    /// Rota sahibi usta ID
    /// </summary>
    public Guid UstaId { get; init; }
    
    /// <summary>
    /// Rota tarihi
    /// </summary>
    public DateOnly Tarih { get; init; }
    
    /// <summary>
    /// Toplam durak sayısı
    /// </summary>
    public int ToplamDurak => _duraklar.Count;
    
    /// <summary>
    /// Koleksiyon başlatıcıları için Add metodu
    /// </summary>
    public void Add(int x, int y)
    {
        _duraklar.Add((x, y));
    }
    
    /// <summary>
    /// Koordinat tuple ile ekleme
    /// </summary>
    public void Add((int X, int Y) konum)
    {
        _duraklar.Add(konum);
    }
    
    /// <summary>
    /// Tüm durakları temizle
    /// </summary>
    public void Clear()
    {
        _duraklar.Clear();
    }
    
    /// <summary>
    /// Belirtilen konumu içeriyor mu?
    /// </summary>
    public bool Contains((int X, int Y) konum)
    {
        return _duraklar.Contains(konum);
    }
    
    /// <summary>
    /// İndex ile durağa erişim
    /// </summary>
    public (int X, int Y) this[int index] => _duraklar[index];
    
    /// <summary>
    /// Toplam rota mesafesi hesaplama (Manhattan distance)
    /// </summary>
    public int ToplamMesafe()
    {
        if (_duraklar.Count < 2)
            return 0;
        
        int toplam = 0;
        for (int i = 0; i < _duraklar.Count - 1; i++)
        {
            var (x1, y1) = _duraklar[i];
            var (x2, y2) = _duraklar[i + 1];
            toplam += Math.Abs(x2 - x1) + Math.Abs(y2 - y1);
        }
        return toplam;
    }
    
    // IEnumerable<(int X, int Y)> implementasyonu
    public IEnumerator<(int X, int Y)> GetEnumerator()
    {
        return _duraklar.GetEnumerator();
    }
    
    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
