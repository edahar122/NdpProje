using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Infrastructure.Repositories;

/// <summary>
/// In-Memory Usta Repository implementasyonu
/// </summary>
public class InMemoryUstaRepository : IUstaRepository
{
    private readonly List<Usta> _ustalar = new();
    private readonly object _lock = new();
    
    public void Ekle(Usta usta)
    {
        lock (_lock)
        {
            _ustalar.Add(usta);
        }
    }
    
    public Usta? GetById(Guid id)
    {
        lock (_lock)
        {
            return _ustalar.FirstOrDefault(u => u.Id == id);
        }
    }
    
    public IEnumerable<Usta> GetAll()
    {
        lock (_lock)
        {
            return _ustalar.ToList();
        }
    }
    
    public IEnumerable<Usta> GetByUzmanlikAlani(string uzmanlikAlani)
    {
        lock (_lock)
        {
            return _ustalar
                .Where(u => u.UzmanlikAlani.Equals(uzmanlikAlani, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
    
    public void Guncelle(Usta usta)
    {
        lock (_lock)
        {
            var mevcut = _ustalar.FirstOrDefault(u => u.Id == usta.Id);
            if (mevcut != null)
            {
                _ustalar.Remove(mevcut);
                _ustalar.Add(usta);
            }
        }
    }
    
    public void Sil(Guid id)
    {
        lock (_lock)
        {
            var usta = _ustalar.FirstOrDefault(u => u.Id == id);
            if (usta != null)
            {
                _ustalar.Remove(usta);
            }
        }
    }
}
