using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing.Rules;

/// <summary>
/// Gece saati ek ücret kuralı - 18:00-08:00 arası %30 ek ücret
/// </summary>
public class GeceSaatiEkUcretiKurali : IPricingRule
{
    public string RuleAdi => "Gece Saati Ek Ücreti";
    public string Aciklama => "18:00-08:00 arası %30 ek ücret";
    public int Oncelik => 15;
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        // %30 ek ücret ekle
        return temelFiyat * 1.30m;
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        var saat = context.BaslangicTarihi.Hour;
        return saat >= 18 || saat < 8;
    }
}
