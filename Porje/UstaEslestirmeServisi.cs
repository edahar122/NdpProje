using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Interfaces;
using UstaPlatform.Pricing;

namespace UstaPlatform.Infrastructure.Services;

public class UstaEslestirmeServisi
{
    private readonly IUstaRepository _ustaRepository;
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly PricingEngine _pricingEngine;
    
    public UstaEslestirmeServisi(
        IUstaRepository ustaRepository, 
        IWorkOrderRepository workOrderRepository,
        PricingEngine pricingEngine)
    {
        _ustaRepository = ustaRepository;
        _workOrderRepository = workOrderRepository;
        _pricingEngine = pricingEngine;
    }
    
    public IsEmri? TalepIcinIsEmriOlustur(Talep talep)
    {
        Console.WriteLine($"\n* Talep icin usta aranıyor: {talep.Aciklama}");
        Console.WriteLine($"   Uzmanlik alani: {talep.UzmanlikAlani}");
        
        var uygunUstalar = _ustaRepository
            .GetByUzmanlikAlani(talep.UzmanlikAlani)
            .Where(u => u.Aktif)
            .ToList();
        
        if (!uygunUstalar.Any())
        {
            Console.WriteLine("   * Uygun usta bulunamadi!");
            return null;
        }
        
        Console.WriteLine($"   * {uygunUstalar.Count} uygun usta bulundu");
        
        var secilenUsta = uygunUstalar
            .OrderBy(u => u.Yogunluk)
            .ThenByDescending(u => u.Puan)
            .First();
        
        Console.WriteLine($"   * Secilen Usta: {secilenUsta.TamIsim} (Yogunluk: %{secilenUsta.Yogunluk}, Puan: {secilenUsta.Puan:F1})");
        
        var context = new PricingContext
        {
            TalepId = talep.Id,
            UstaId = secilenUsta.Id,
            UzmanlikAlani = talep.UzmanlikAlani,
            BaslangicTarihi = talep.IstenenTarih ?? DateTime.Now.AddHours(2),
            TahminiSure = talep.TahminiSure,
            SaatBasiUcret = secilenUsta.SaatBasiUcret,
            Acil = talep.Acil,
            AdresX = talep.AdresX,
            AdresY = talep.AdresY
        };
        
        decimal fiyat = _pricingEngine.FiyatHesapla(0, context);
        
        var baslangic = context.BaslangicTarihi;
        var bitis = baslangic.AddHours((double)talep.TahminiSure);
        
        var isEmri = new IsEmri
        {
            TalepId = talep.Id,
            UstaId = secilenUsta.Id,
            PlanlananBaslangic = baslangic,
            PlanlananBitis = bitis,
            Fiyat = fiyat,
            AdresX = talep.AdresX,
            AdresY = talep.AdresY,
            AdresDetay = talep.AdresDetay
        };
        
        _workOrderRepository.Ekle(isEmri);
        
        secilenUsta.Yogunluk += 10;
        _ustaRepository.Guncelle(secilenUsta);
        
        Console.WriteLine($"   * Is emri olusturuldu! ID: {isEmri.Id}");
        Console.WriteLine($"   * Tarih: {baslangic:dd.MM.yyyy HH:mm} - {bitis:HH:mm}");
        Console.WriteLine($"   * Toplam Fiyat: {fiyat:C2}");
        
        return isEmri;
    }
    
    public Route RotaOlustur(Guid ustaId, DateOnly tarih)
    {
        var isEmirleri = _workOrderRepository
            .GetByUsta(ustaId)
            .Where(ie => DateOnly.FromDateTime(ie.PlanlananBaslangic) == tarih)
            .OrderBy(ie => ie.PlanlananBaslangic)
            .ToList();
        
        var rota = new Route
        {
            UstaId = ustaId,
            Tarih = tarih
        };
        
        foreach (var isEmri in isEmirleri)
        {
            rota.Add(isEmri.AdresX, isEmri.AdresY);
        }
        
        return rota;
    }
    
    public Schedule CizelgeOlustur(Guid ustaId)
    {
        var isEmirleri = _workOrderRepository.GetByUsta(ustaId);
        
        var cizelge = new Schedule
        {
            UstaId = ustaId
        };
        
        foreach (var isEmri in isEmirleri)
        {
            cizelge.IsEmriEkle(isEmri);
        }
        
        return cizelge;
    }
}