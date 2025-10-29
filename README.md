# 🏙️ UstaPlatform - Arcadia Şehir Uzmanlık Platformu

**Nesne Yönelimli Programlama (NYP) ve İleri C# Projesi**

Arcadia şehrindeki kayıp uzmanları (Tesisatçı, Elektrikçi, Marangoz, vb.) vatandaş talepleriyle eşleştiren, dinamik fiyatlama ve akıllı rota planlama yapabilen, genişletilebilir ve açık uçlu bir yazılım platformu.

---

## 📋 İçindekiler

- [Proje Özeti](#-proje-özeti)
- [Mimari ve Tasarım](#-mimari-ve-tasarım)
- [Teknolojiler ve C# Özellikleri](#-teknolojiler-ve-c-özellikleri)
- [SOLID Prensipleri](#-solid-prensipleri)
- [Kurulum ve Çalıştırma](#-kurulum-ve-çalıştırma)
- [Plugin Mimarisi](#-plugin-mimarisi-ocp)
- [Demo Senaryoları](#-demo-senaryoları)
- [Proje Yapısı](#-proje-yapısı)

---

## 🎯 Proje Özeti

### Problem
Arcadia şehrinde belediyenin "Uzman Kayıt Sistemi" çökmüş durumda. Tesisatçı, elektrikçi, marangoz gibi bağımsız uzmanlara ulaşılamıyor ve iş planlama sistemi mevcut değil.

### Çözüm
Vatandaş bir talep açtığında:
1. ✅ Doğru uzmanı doğru zamanda bulan
2. 🗺️ Rota planlayan
3. 💰 Fiyat tahmini yapan
4. 📅 Takvime yerleştiren

**Genişletilebilir** ve **açık uçlu** bir platform!

---

## 🏗️ Mimari ve Tasarım

### Çok Katmanlı Mimari (Multi-Layer Architecture)

```
┌─────────────────────────────────────────────┐
│     UstaPlatform.App (Console)              │
│     - Program.cs (Ana Uygulama)             │
└─────────────────┬───────────────────────────┘
                  │
    ┌─────────────┼─────────────┐
    ▼             ▼             ▼
┌─────────┐  ┌──────────┐  ┌─────────┐
│ Domain  │  │Infrastructure│ Pricing │
│         │◄─┤            │◄┤         │
└─────────┘  └──────────┘  └─────────┘
                               │
                               ▼
                    ┌───────────────────┐
                    │ Pricing.Rules     │
                    │ (Plugin DLL)      │
                    └───────────────────┘
```

### Katmanlar

#### 1️⃣ **Domain Katmanı** (`UstaPlatform.Domain`)
**Sorumluluk:** Temel iş nesneleri ve kuralları

**Entities (Varlıklar):**
- `Usta` - Uzman bilgileri (init-only properties)
- `Vatandas` - Vatandaş bilgileri
- `Talep` - İş talebi
- `IsEmri` - İş emri
- `Route` - Özel IEnumerable<(int X, int Y)> koleksiyonu
- `Schedule` - Indexer ile DateOnly erişimi

**Interfaces (Arayüzler):**
- `IPricingRule` - Fiyatlandırma kuralları için contract
- `IUstaRepository` - Usta veri erişimi
- `ITalepRepository` - Talep veri erişimi
- `IWorkOrderRepository` - İş emri veri erişimi

**Helpers (Yardımcılar):**
- `Guard` - Validation (static)
- `ParaFormatlayici` - Para formatı (static)
- `KonumYardimcisi` - Koordinat hesaplamaları (static)

#### 2️⃣ **Infrastructure Katmanı** (`UstaPlatform.Infrastructure`)
**Sorumluluk:** Veri erişimi ve business logic

**Repositories:**
- `InMemoryUstaRepository`
- `InMemoryTalepRepository`
- `InMemoryWorkOrderRepository`

**Services:**
- `UstaEslestirmeServisi` - İş eşleştirme ve iş emri oluşturma

#### 3️⃣ **Pricing Katmanı** (`UstaPlatform.Pricing`)
**Sorumluluk:** Dinamik fiyatlandırma motoru

**Engine:**
- `PricingEngine` - Reflection ile plugin DLL yükleme ve fiyat hesaplama

#### 4️⃣ **Pricing.Rules Katmanı** (`UstaPlatform.Pricing.Rules`)
**Sorumluluk:** Fiyatlandırma kuralları (Plugin DLL)

**Rules:**
- `TemelFiyatKurali` - Temel fiyat hesaplama
- `HaftasonuEkUcretiKurali` - Hafta sonu %50 ek ücret
- `AcilCagriUcretiKurali` - Acil işler %75 ek ücret
- `GeceSaatiEkUcretiKurali` - Gece saati %30 ek ücret
- `LoyaltyDiscountRule` - Sadakat indirimi %10

#### 5️⃣ **App Katmanı** (`UstaPlatform.App`)
**Sorumluluk:** Ana uygulama ve demo senaryoları

---

## 🔧 Teknolojiler ve C# Özellikleri

### ✅ İleri C# Özellikleri

| Özellik | Kullanım Yeri | Açıklama |
|---------|---------------|----------|
| **init-only Properties** | `Usta.Id`, `Talep.TalepEdilmeTarihi` | Nesne oluşturulduktan sonra değiştirilemez |
| **required Modifier** | `Usta.Ad`, `Talep.Aciklama` | Zorunlu property'ler |
| **Object Initializers** | `new Usta { Ad = "Mehmet", ... }` | Okunaklı nesne oluşturma |
| **Collection Initializers** | `new List<Usta> { ... }` | Koleksiyon başlatma |
| **Indexer** | `Schedule[DateOnly tarih]` | Tarihe göre iş emirlerine erişim |
| **Custom IEnumerable<T>** | `Route : IEnumerable<(int X, int Y)>` | Özel koleksiyon + foreach desteği |
| **Tuple** | `(int X, int Y)` | Koordinat bilgisi |
| **Static Classes** | `Guard`, `ParaFormatlayici` | Yardımcı metodlar |
| **Pattern Matching** | `is bool sadik && sadik` | Gelişmiş tip kontrolü |
| **Nullable Reference Types** | `string?`, `DateTime?` | Null safety |
| **Lambda Expressions** | `.Where(u => u.Aktif)` | LINQ sorgularda |
| **Extension Methods** | `IEnumerable` genişletmeleri | - |

### 🎯 .NET 8 ve C# 12 Özellikleri
- **Primary Constructors** - Sınıf tanımında constructor
- **Collection Expressions** - `[1, 2, 3]` şeklinde koleksiyon
- **UTF-8 String Literals** - Console çıktılarında emoji desteği

---

## 🏛️ SOLID Prensipleri

### 1. **S**ingle Responsibility Principle (SRP) - Tek Sorumluluk
✅ Her sınıf tek bir sorumluluğa sahip:
- `Usta` → Sadece usta bilgilerini tutar
- `PricingEngine` → Sadece fiyat hesaplama
- `InMemoryUstaRepository` → Sadece usta veri erişimi
- `UstaEslestirmeServisi` → Sadece iş eşleştirme

### 2. **O**pen/Closed Principle (OCP) - Açık/Kapalı ⭐ **EN ÖNEMLİ**
✅ **Plugin Mimarisi** ile yeni fiyatlandırma kuralları eklenebilir:
```csharp
// Yeni kural ekleme - Ana uygulama değişmiyor!
public class YeniMahalleKurali : IPricingRule
{
    public string RuleAdi => "Yeni Mahalle Ücreti";
    public int Oncelik => 25;
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        return temelFiyat * 1.20m; // %20 ek ücret
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        return context.Mahalle == "Yeni Mahalle";
    }
}
```

DLL olarak derle → `Plugins` klasörüne koy → Otomatik yüklenir! 🎉

### 3. **L**iskov Substitution Principle (LSP) - Liskov Yerine Geçme
✅ Tüm `IPricingRule` implementasyonları birbirinin yerine kullanılabilir

### 4. **I**nterface Segregation Principle (ISP) - Arayüz Ayrımı
✅ Küçük ve odaklanmış interface'ler:
- `IUstaRepository` → Sadece usta işlemleri
- `ITalepRepository` → Sadece talep işlemleri
- `IPricingRule` → Sadece fiyat kuralları

### 5. **D**ependency Inversion Principle (DIP) - Bağımlılık Tersine Çevirme
✅ Üst katmanlar somut sınıflara değil, interface'lere bağımlı:
```csharp
// ✅ DOĞRU: Interface'e bağımlılık
public UstaEslestirmeServisi(
    IUstaRepository ustaRepository,      // Interface
    IWorkOrderRepository workOrderRepo,  // Interface
    PricingEngine pricingEngine)

// ❌ YANLIŞ olurdu:
// public UstaEslestirmeServisi(InMemoryUstaRepository ustaRepo)
```

---

## 📦 Kurulum ve Çalıştırma

### Gereksinimler
- **.NET 8 SDK** - [İndir](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Visual Studio 2022** veya **VS Code** (opsiyonel)

### Kurulum Adımları

#### 1. Projeyi Klonlayın veya İndirin
```bash
cd C:\proje\Porje
```

#### 2. Solution'ı Restore Edin
```bash
dotnet restore Porje.sln
```

#### 3. Projeyi Build Edin
```bash
dotnet build Porje.sln --configuration Release
```

#### 4. Uygulamayı Çalıştırın
```bash
cd Porje
dotnet run --project UstaPlatform.App.csproj
```

### Alternatif: Visual Studio ile Çalıştırma
1. `Porje.sln` dosyasını açın
2. Startup Project olarak `UstaPlatform.App` seçin
3. `F5` veya `Ctrl+F5` ile çalıştırın

---

## 🔌 Plugin Mimarisi (OCP)

### Nasıl Çalışır?

#### 1. Plugin Interface Tanımı
```csharp
public interface IPricingRule
{
    string RuleAdi { get; }
    string Aciklama { get; }
    int Oncelik { get; }
    
    decimal HesaplaFiyat(decimal temelFiyat, PricingContext context);
    bool KuralGecerliMi(PricingContext context);
}
```

#### 2. Pricing Engine - Reflection ile DLL Yükleme
```csharp
public void KurallariYukle(string pluginKlasor)
{
    var dllDosyalari = Directory.GetFiles(pluginKlasor, "*.dll");
    
    foreach (var dllPath in dllDosyalari)
    {
        var assembly = Assembly.LoadFrom(dllPath);
        var ruleTypes = assembly.GetTypes()
            .Where(t => typeof(IPricingRule).IsAssignableFrom(t) 
                     && !t.IsInterface && !t.IsAbstract);
        
        foreach (var ruleType in ruleTypes)
        {
            var rule = (IPricingRule)Activator.CreateInstance(ruleType);
            _rules.Add(rule);
        }
    }
    
    // Önceliğe göre sırala
    _rules.Sort((a, b) => a.Oncelik.CompareTo(b.Oncelik));
}
```

#### 3. Yeni Plugin Ekleme (Demo Senaryo)

**Adım 1:** Yeni kural sınıfı oluştur
```csharp
// LoyaltyDiscountRule.cs
public class LoyaltyDiscountRule : IPricingRule
{
    public string RuleAdi => "Sadakat İndirimi";
    public int Oncelik => 100;
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        return temelFiyat * 0.90m; // %10 indirim
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        return context.EkBilgiler.TryGetValue("SadikMusteri", out var deger)
            && deger is bool sadik && sadik;
    }
}
```

**Adım 2:** DLL olarak derle
```bash
dotnet build UstaPlatform.Pricing.Rules.csproj -c Release
```

**Adım 3:** `Plugins` klasörüne kopyala
```bash
copy bin\Release\net8.0\UstaPlatform.Pricing.Rules.dll ..\UstaPlatform.App\bin\Release\net8.0\Plugins\
```

**Adım 4:** Uygulama tekrar çalıştırıldığında otomatik yüklenir! ✨

---

## 🎬 Demo Senaryoları

### Senaryo 1: Normal İş Talebi (Hafta İçi, Gündüz)
```
📌 Mutfak lavabosunda sızıntı var
   Uzmanlık: Tesisatçı
   Tarih: Pazartesi, 10:00
   Süre: 2 saat
   
💰 Fiyat Hesaplama:
   Temel Fiyat: 500 TL/saat × 2 saat = 1,000 TL
   Toplam: 1,000 TL
```

### Senaryo 2: Hafta Sonu + Acil İş
```
📌 Elektrik kesintisi (ACİL!)
   Uzmanlık: Elektrikçi
   Tarih: Cumartesi, 14:00
   Süre: 1.5 saat
   
💰 Fiyat Hesaplama:
   Temel Fiyat: 600 TL × 1.5 = 900 TL
   + Hafta Sonu (%50): 900 × 1.5 = 1,350 TL
   + Acil Çağrı (%75): 1,350 × 1.75 = 2,362.50 TL
   Toplam: 2,362.50 TL
```

### Senaryo 3: Gece Saati
```
📌 Kapı kilidi kırıldı
   Uzmanlık: Marangoz
   Tarih: Salı, 20:00
   Süre: 1 saat
   
💰 Fiyat Hesaplama:
   Temel Fiyat: 450 TL × 1 = 450 TL
   + Gece Saati (%30): 450 × 1.3 = 585 TL
   Toplam: 585 TL
```

### Senaryo 4: Sadık Müşteri (Plugin Demonstrasyonu)
```
🔌 YENİ KURAL EKLENDI: Sadakat İndirimi
   
📌 Normal tesisatçı talebi + Sadık Müşteri
   Temel Fiyat: 1,000 TL
   - Sadakat İndirimi (%10): 1,000 × 0.9 = 900 TL
   Toplam: 900 TL
```

---

## 📁 Proje Yapısı

```
C:\proje\Porje\
├── Porje.sln                          # Solution dosyası
├── README.md                          # Bu dosya
│
├── Porje/                             # Ana klasör (tüm projeler burada)
│   │
│   ├── UstaPlatform.Domain.csproj     # Domain projesi
│   ├── Usta.cs                        # ✅ init-only properties
│   ├── Vatandas.cs
│   ├── Talep.cs
│   ├── IsEmri.cs
│   ├── Route.cs                       # ✅ Custom IEnumerable<(int,int)>
│   ├── Schedule.cs                    # ✅ Indexer [DateOnly]
│   ├── IPricingRule.cs                # Interface
│   ├── IUstaRepository.cs
│   ├── ITalepRepository.cs
│   ├── IWorkOrderRepository.cs
│   ├── Guard.cs                       # ✅ Static helper
│   ├── ParaFormatlayici.cs            # ✅ Static helper
│   ├── KonumYardimcisi.cs             # ✅ Static helper
│   │
│   ├── UstaPlatform.Infrastructure.csproj  # Infrastructure projesi
│   ├── InMemoryUstaRepository.cs      # Repository impl
│   ├── InMemoryTalepRepository.cs
│   ├── InMemoryWorkOrderRepository.cs
│   ├── UstaEslestirmeServisi.cs       # Business logic
│   │
│   ├── UstaPlatform.Pricing.csproj    # Pricing projesi
│   ├── PricingEngine.cs               # ✅ Reflection + Plugin loader
│   │
│   ├── UstaPlatform.Pricing.Rules.csproj   # Pricing Rules (Plugin DLL)
│   ├── TemelFiyatKurali.cs
│   ├── HaftasonuEkUcretiKurali.cs
│   ├── AcilCagriUcretiKurali.cs
│   ├── GeceSaatiEkUcretiKurali.cs
│   ├── LoyaltyDiscountRule.cs         # ✅ Plugin demo
│   │
│   ├── UstaPlatform.App.csproj        # Ana uygulama
│   ├── Program.cs                     # ✅ Demo senaryoları
│   │
│   └── bin/Release/net8.0/
│       ├── UstaPlatform.App.exe
│       └── Plugins/                   # ✅ Plugin DLL klasörü
│           └── UstaPlatform.Pricing.Rules.dll
```

---

## 🎯 Öğrenilen Konular

### ✅ SOLID Prensipleri
- ✔️ Single Responsibility Principle (SRP)
- ✔️ Open/Closed Principle (OCP) - Plugin Mimarisi
- ✔️ Liskov Substitution Principle (LSP)
- ✔️ Interface Segregation Principle (ISP)
- ✔️ Dependency Inversion Principle (DIP)

### ✅ İleri C# Özellikleri
- ✔️ init-only Properties
- ✔️ required Modifier
- ✔️ Object & Collection Initializers
- ✔️ Indexers
- ✔️ Custom IEnumerable<T>
- ✔️ Static Helper Classes
- ✔️ Tuples
- ✔️ Pattern Matching
- ✔️ Nullable Reference Types

### ✅ Tasarım Desenleri
- ✔️ Repository Pattern
- ✔️ Service Layer Pattern
- ✔️ Plugin Architecture
- ✔️ Strategy Pattern (IPricingRule)

### ✅ .NET Teknolojileri
- ✔️ Reflection API
- ✔️ Assembly Loading
- ✔️ LINQ
- ✔️ Multi-Project Solution

---

## 🏆 Proje Başarı Kriterleri

| Kriter | Durum | Açıklama |
|--------|-------|----------|
| Multi-Project Solution | ✅ | 5 ayrı proje |
| SOLID Prensipleri | ✅ | Tüm prensiplerygulandı |
| init-only Properties | ✅ | ID, KayitZamani |
| Indexer | ✅ | Schedule[DateOnly] |
| Custom IEnumerable | ✅ | Route sınıfı |
| Static Helpers | ✅ | Guard, Para, Konum |
| Plugin Mimarisi | ✅ | Reflection ile DLL yükleme |
| Demo Senaryoları | ✅ | 4 farklı senaryo |
| README.md | ✅ | Bu dosya |

---

## 📊 İstatistikler

- **Toplam Satır:** ~2,500+ LOC
- **Toplam Sınıf:** 25+
- **Toplam Interface:** 4
- **Pricing Kuralı:** 5
- **Demo Senaryo:** 4

---

## 👨‍💻 Geliştirici Notları

### Plugin Ekleme Kolaylığı
Yeni bir fiyatlandırma kuralı eklemek için:
1. `IPricingRule` interface'ini implement et
2. DLL olarak derle
3. `Plugins` klasörüne at
4. Uygulama otomatik yükler!

**Ana uygulama kodunu değiştirmeye gerek yok!** 🎉

### Genişletme Noktaları
- 🔄 Gerçek veritabanı entegrasyonu (Entity Framework)
- 🌐 Web API (ASP.NET Core)
- 📱 Mobil uygulama
- 🗺️ Gerçek harita entegrasyonu
- 📧 Bildirim sistemi (Email/SMS)
- 👥 Kullanıcı yetkilendirme

---

## 📝 Lisans

Bu proje eğitim amaçlıdır.

---

## 🎓 Proje Sahibi

**Ders:** Nesne Yönelimli Programlama (NYP) ve İleri C#  
**Proje:** UstaPlatform - Arcadia Şehir Platformu  
**Tarih:** 2024

---

**✨ UstaPlatform - Arcadia'nın uzmanlarını buluşturan platform! ✨**
