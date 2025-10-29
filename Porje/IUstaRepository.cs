using UstaPlatform.Domain.Entities;

namespace UstaPlatform.Domain.Interfaces;

/// <summary>
/// Usta veri erişim arayüzü - DIP (Dependency Inversion Principle) için
/// </summary>
public interface IUstaRepository
{
    void Ekle(Usta usta);
    Usta? GetById(Guid id);
    IEnumerable<Usta> GetAll();
    IEnumerable<Usta> GetByUzmanlikAlani(string uzmanlikAlani);
    void Guncelle(Usta usta);
    void Sil(Guid id);
}
