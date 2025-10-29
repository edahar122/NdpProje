using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Infrastructure.Repositories;

/// <summary>
/// In-Memory Talep Repository implementasyonu
/// </summary>
public class InMemoryTalepRepository : ITalepRepository
{
    private readonly List<Talep> _talepler = new();
    private readonly object _lock = new();

    public void Ekle(Talep talep)
    {
        lock (_lock)
        {
            _talepler.Add(talep);
        }
    }

    public Talep? GetById(Guid id)
    {
        lock (_lock)
        {
            return _talepler.FirstOrDefault(t => t.Id == id);
        }
    }

    public IEnumerable<Talep> GetAll()
    {
        lock (_lock)
        {
            return _talepler.ToList();
        }
    }

    public IEnumerable<Talep> GetByDurum(TalepDurumu durum)
    {
        lock (_lock)
        {
            return _talepler.Where(t => t.Durum == durum).ToList();
        }
    }

    public IEnumerable<Talep> GetByVatandas(Guid vatandasId)
    {
        lock (_lock)
        {
            return _talepler.Where(t => t.VatandasId == vatandasId).ToList();
        }
    }

    public void Guncelle(Talep talep)
    {
        lock (_lock)
        {
            var mevcut = _talepler.FirstOrDefault(t => t.Id == talep.Id);
            if (mevcut != null)
            {
                _talepler.Remove(mevcut);
                _talepler.Add(talep);
            }
        }
    }

    public void Sil(Guid id)
    {
        lock (_lock)
        {
            var talep = _talepler.FirstOrDefault(t => t.Id == id);
            if (talep != null)
            {
                _talepler.Remove(talep);
            }
        }
    }
}
