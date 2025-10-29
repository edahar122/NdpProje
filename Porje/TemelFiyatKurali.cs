using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing.Rules;

/// <summary>
/// Temel fiyat kuralı - Saat başı ücret * Tahmini süre
/// </summary>
public class TemelFiyatKurali : IPricingRule
{
    public string RuleAdi => "Temel Fiyat";
    public string Aciklama => "Saat başı ücret × Tahmini süre";
    public int Oncelik => 1;
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        // İlk kural olduğu için temel fiyatı hesaplar
        return context.SaatBasiUcret * context.TahminiSure;
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        return true; // Her zaman geçerli
    }
}
