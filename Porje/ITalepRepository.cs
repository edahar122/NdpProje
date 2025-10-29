using UstaPlatform.Domain.Entities;

namespace UstaPlatform.Domain.Interfaces;

/// <summary>
/// Talep veri erişim arayüzü - DIP için
/// </summary>
public interface ITalepRepository
{
    void Ekle(Talep talep);
    Talep? GetById(Guid id);
    IEnumerable<Talep> GetAll();
    IEnumerable<Talep> GetByDurum(TalepDurumu durum);
    IEnumerable<Talep> GetByVatandas(Guid vatandasId);
    void Guncelle(Talep talep);
    void Sil(Guid id);
}
