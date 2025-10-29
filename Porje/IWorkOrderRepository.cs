using UstaPlatform.Domain.Entities;

namespace UstaPlatform.Domain.Interfaces;

/// <summary>
/// İş Emri veri erişim arayüzü - DIP için
/// </summary>
public interface IWorkOrderRepository
{
    void Ekle(IsEmri isEmri);
    IsEmri? GetById(Guid id);
    IEnumerable<IsEmri> GetAll();
    IEnumerable<IsEmri> GetByUsta(Guid ustaId);
    IEnumerable<IsEmri> GetByTarih(DateOnly tarih);
    IEnumerable<IsEmri> GetByDurum(IsEmriDurumu durum);
    void Guncelle(IsEmri isEmri);
    void Sil(Guid id);
}
