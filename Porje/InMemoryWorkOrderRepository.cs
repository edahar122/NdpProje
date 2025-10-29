using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Infrastructure.Repositories;

/// <summary>
/// In-Memory İş Emri Repository implementasyonu
/// </summary>
public class InMemoryWorkOrderRepository : IWorkOrderRepository
{
    private readonly List<IsEmri> _isEmirleri = new();
    private readonly object _lock = new();
    
    public void Ekle(IsEmri isEmri)
    {
        lock (_lock)
        {
            _isEmirleri.Add(isEmri);
        }
    }
    
    public IsEmri? GetById(Guid id)
    {
        lock (_lock)
        {
            return _isEmirleri.FirstOrDefault(ie => ie.Id == id);
        }
    }
    
    public IEnumerable<IsEmri> GetAll()
    {
        lock (_lock)
        {
            return _isEmirleri.ToList();
        }
    }
    
    public IEnumerable<IsEmri> GetByUsta(Guid ustaId)
    {
        lock (_lock)
        {
            return _isEmirleri.Where(ie => ie.UstaId == ustaId).ToList();
        }
    }
    
    public IEnumerable<IsEmri> GetByTarih(DateOnly tarih)
    {
        lock (_lock)
        {
            return _isEmirleri
                .Where(ie => DateOnly.FromDateTime(ie.PlanlananBaslangic) == tarih)
                .ToList();
        }
    }
    
    public IEnumerable<IsEmri> GetByDurum(IsEmriDurumu durum)
    {
        lock (_lock)
        {
            return _isEmirleri.Where(ie => ie.Durum == durum).ToList();
        }
    }
    
    public void Guncelle(IsEmri isEmri)
    {
        lock (_lock)
        {
            var mevcut = _isEmirleri.FirstOrDefault(ie => ie.Id == isEmri.Id);
            if (mevcut != null)
            {
                _isEmirleri.Remove(mevcut);
                _isEmirleri.Add(isEmri);
            }
        }
    }
    
    public void Sil(Guid id)
    {
        lock (_lock)
        {
            var isEmri = _isEmirleri.FirstOrDefault(ie => ie.Id == id);
            if (isEmri != null)
            {
                _isEmirleri.Remove(isEmri);
            }
        }
    }
}
