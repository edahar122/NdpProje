using UstaPlatform.Domain.Entities;
using UstaPlatform.Domain.Helpers;
using UstaPlatform.Domain.Interfaces;
using UstaPlatform.Infrastructure.Repositories;
using UstaPlatform.Infrastructure.Services;
using UstaPlatform.Pricing;

namespace UstaPlatform.App;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("================================================================");
        Console.WriteLine("       USTA PLATFORM - Arcadia Sehir Platformu");
        Console.WriteLine("      Uzman Eslestirme, Rota Planlama ve Fiyatlandirma");
        Console.WriteLine("================================================================\n");
        
        IUstaRepository ustaRepo = new InMemoryUstaRepository();
        ITalepRepository talepRepo = new InMemoryTalepRepository();
        IWorkOrderRepository workOrderRepo = new InMemoryWorkOrderRepository();
        
        var pricingEngine = new PricingEngine();
        
        Console.WriteLine("* Fiyatlandirma Sistemi Baslatiliyor...");
        Console.WriteLine("================================================================");
        
        pricingEngine.VarsayilanKurallariYukle();
        
        string pluginKlasor = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Plugins");
        Console.WriteLine($"\n* Plugin Klasoru Kontrol Ediliyor: {pluginKlasor}");
        
        if (Directory.Exists(pluginKlasor))
        {
            var dllFiles = Directory.GetFiles(pluginKlasor, "*.dll");
            Console.WriteLine($"* Plugin klasorunde {dllFiles.Length} DLL dosyasi bulundu.");
            
            if (dllFiles.Length > 0)
            {
                Console.WriteLine("* Ek plugin'ler yukleniyor...");
                pricingEngine.KurallariYukle(pluginKlasor);
            }
        }
        else
        {
            Console.WriteLine("* Plugin klasoru bulunamadi. Sadece varsayilan kurallar kullanilacak.");
        }
        
        Console.WriteLine($"* Toplam {pricingEngine.YuklenenKuralSayisi} kural aktif.\n");
        
        var eslestirmeServisi = new UstaEslestirmeServisi(ustaRepo, workOrderRepo, pricingEngine);
        
        Console.WriteLine("* Test Verisi Olusturuluyor...");
        Console.WriteLine("================================================================\n");
        
        OrnekVerileriOlustur(ustaRepo, talepRepo);
        
        Console.WriteLine("\n* DEMO SENARYOLARI");
        Console.WriteLine("================================================================\n");
        
        Console.WriteLine("SENARYO 1: Normal Tesisatci Talebi (Hafta Ici, Gunduz)");
        Console.WriteLine("----------------------------------------------------------------");
        var talep1 = new Talep
        {
            VatandasId = Guid.NewGuid(),
            Aciklama = "Mutfak lavabosunda sizinti var",
            UzmanlikAlani = "Tesisatci",
            TahminiSure = 2m,
            AdresX = 100,
            AdresY = 200,
            AdresDetay = "Merkez Mahallesi, Ataturk Cad. No:15",
            Acil = false,
            IstenenTarih = new DateTime(2024, 1, 15, 10, 0, 0)
        };
        
        var isEmri1 = eslestirmeServisi.TalepIcinIsEmriOlustur(talep1);
        
        Console.WriteLine("\nSENARYO 2: Acil Elektrik Talebi (Hafta Sonu, Gunduz)");
        Console.WriteLine("----------------------------------------------------------------");
        var talep2 = new Talep
        {
            VatandasId = Guid.NewGuid(),
            Aciklama = "Elektrik kesintisi, sigorta atmis",
            UzmanlikAlani = "Elektrikci",
            TahminiSure = 1.5m,
            AdresX = 250,
            AdresY = 300,
            AdresDetay = "Yeni Mahalle, Inonu Sok. No:8",
            Acil = true,
            IstenenTarih = new DateTime(2024, 1, 20, 14, 0, 0)
        };
        
        var isEmri2 = eslestirmeServisi.TalepIcinIsEmriOlustur(talep2);
        
        Console.WriteLine("\nSENARYO 3: Gece Saati Marangoz Talebi");
        Console.WriteLine("----------------------------------------------------------------");
        var talep3 = new Talep
        {
            VatandasId = Guid.NewGuid(),
            Aciklama = "Kapi kilidi kirildi, acil tamir gerekli",
            UzmanlikAlani = "Marangoz",
            TahminiSure = 1m,
            AdresX = 150,
            AdresY = 180,
            AdresDetay = "Eski Mahalle, Cumhuriyet Cad. No:42",
            Acil = false,
            IstenenTarih = new DateTime(2024, 1, 16, 20, 0, 0)
        };
        
        var isEmri3 = eslestirmeServisi.TalepIcinIsEmriOlustur(talep3);
        
        if (isEmri1 != null)
        {
            Console.WriteLine("\n\n* ROTA VE CIZELGE DEMONSTRASYONU");
            Console.WriteLine("================================================================");
            
            var tarih = DateOnly.FromDateTime(isEmri1.PlanlananBaslangic);
            var rota = eslestirmeServisi.RotaOlustur(isEmri1.UstaId, tarih);
            
            Console.WriteLine($"\n* Rota Bilgisi (Usta: {isEmri1.UstaId}, Tarih: {tarih:dd.MM.yyyy})");
            Console.WriteLine($"   Toplam Durak: {rota.ToplamDurak}");
            Console.WriteLine($"   Toplam Mesafe: {rota.ToplamMesafe()} birim");
            Console.WriteLine("   Duraklar:");
            int sira = 1;
            foreach (var (x, y) in rota)
            {
                Console.WriteLine($"      {sira++}. {KonumYardimcisi.KoordinatFormat(x, y)}");
            }
            
            var cizelge = eslestirmeServisi.CizelgeOlustur(isEmri1.UstaId);
            Console.WriteLine($"\n* Cizelge Bilgisi (Indexer Kullanimi)");
            Console.WriteLine($"   Toplam Is Emri: {cizelge.ToplamIsEmri}");
            
            var gunlukIsler = cizelge[tarih];
            Console.WriteLine($"   {tarih:dd.MM.yyyy} tarihindeki isler: {gunlukIsler.Count} adet");
            
            foreach (var is_ in gunlukIsler)
            {
                Console.WriteLine($"      - {is_.PlanlananBaslangic:HH:mm}-{is_.PlanlananBitis:HH:mm} | {ParaFormatlayici.FormatTL(is_.Fiyat)}");
            }
        }
        
        Console.WriteLine("\n\n* PLUGIN DEMONSTRASYONU");
        Console.WriteLine("================================================================");
        Console.WriteLine("* Simdi yeni bir kural ekleniyor: Sadakat Indirimi");
        Console.WriteLine("   (Normalde bu DLL olarak Plugins klasorune eklenirdi)\n");
        
        pricingEngine.KuralEkle(new UstaPlatform.Pricing.Rules.LoyaltyDiscountRule());
        Console.WriteLine($"* Kural eklendi: Sadakat Indirimi (Toplam: {pricingEngine.YuklenenKuralSayisi} kural)\n");
        
        Console.WriteLine("SENARYO 4: Sadik Musteri Talebi");
        Console.WriteLine("----------------------------------------------------------------");
        
        var context = new PricingContext
        {
            TalepId = Guid.NewGuid(),
            UstaId = Guid.NewGuid(),
            UzmanlikAlani = "Tesisatci",
            BaslangicTarihi = new DateTime(2024, 1, 17, 10, 0, 0),
            TahminiSure = 2m,
            SaatBasiUcret = 500m,
            Acil = false,
            AdresX = 100,
            AdresY = 200,
            EkBilgiler = new Dictionary<string, object>
            {
                { "SadikMusteri", true }
            }
        };
        
        decimal sadikMusteriFiyat = pricingEngine.FiyatHesapla(0, context);
        
        Console.WriteLine("\n\n* OZET");
        Console.WriteLine("================================================================");
        Console.WriteLine($"* Toplam {ustaRepo.GetAll().Count()} usta sisteme kayitli");
        Console.WriteLine($"* Toplam {workOrderRepo.GetAll().Count()} is emri olusturuldu");
        Console.WriteLine($"* Toplam {pricingEngine.YuklenenKuralSayisi} fiyatlandirma kurali aktif");
        Console.WriteLine("\n* Tum SOLID prensipleri ve C# ozellikleri basariyla uygulandi!");
        Console.WriteLine("   - OCP: Plugin mimarisi ile yeni kurallar eklenebildi");
        Console.WriteLine("   - SRP: Her sinif tek sorumluluk tasiyor");
        Console.WriteLine("   - DIP: Interface'ler uzerinden bagimlilik yonetimi");
        Console.WriteLine("   - init-only: ID ve KayitZamani alanlari immutable");
        Console.WriteLine("   - Indexer: Schedule[DateOnly] kullanimi");
        Console.WriteLine("   - IEnumerable: Route koleksiyonu ozel implementasyon");
        Console.WriteLine("   - Static Helpers: Guard, ParaFormatlayici, KonumYardimcisi");
        
        Console.WriteLine("\n* UstaPlatform basariyla calisti!\n");
    }
    
    static void OrnekVerileriOlustur(IUstaRepository ustaRepo, ITalepRepository talepRepo)
    {
        var ustalar = new List<Usta>
        {
            new Usta
            {
                Ad = "Mehmet",
                Soyad = "Yilmaz",
                UzmanlikAlani = "Tesisatci",
                SaatBasiUcret = 500m,
                Puan = 4.8,
                Yogunluk = 20,
                Yetenekler = new List<string> { "Sizinti Tamiri", "Tesisat Montaji", "Klozet Degisimi" }
            },
            new Usta
            {
                Ad = "Ayse",
                Soyad = "Demir",
                UzmanlikAlani = "Elektrikci",
                SaatBasiUcret = 600m,
                Puan = 4.9,
                Yogunluk = 15,
                Yetenekler = new List<string> { "Elektrik Tesisati", "Pano Montaji", "Aydinlatma" }
            },
            new Usta
            {
                Ad = "Ali",
                Soyad = "Kara",
                UzmanlikAlani = "Marangoz",
                SaatBasiUcret = 450m,
                Puan = 4.7,
                Yogunluk = 30,
                Yetenekler = new List<string> { "Kapi/Pencere Tamiri", "Mobilya Yapimi", "Ahsap Isleri" }
            },
            new Usta
            {
                Ad = "Fatma",
                Soyad = "Oz",
                UzmanlikAlani = "Tesisatci",
                SaatBasiUcret = 550m,
                Puan = 4.6,
                Yogunluk = 10,
                Yetenekler = new List<string> { "Kombisyon", "Dogalgaz Tesisati", "Sihhi Tesisat" }
            }
        };
        
        foreach (var usta in ustalar)
        {
            ustaRepo.Ekle(usta);
            Console.WriteLine($"   * Usta eklendi: {usta.TamIsim} ({usta.UzmanlikAlani})");
        }
    }
}