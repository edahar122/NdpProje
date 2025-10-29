using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing.Rules;

/// <summary>
/// Hafta sonu ek ücret kuralı - Cumartesi ve Pazar %50 ek ücret
/// </summary>
public class HaftasonuEkUcretiKurali : IPricingRule
{
    public string RuleAdi => "Hafta Sonu Ek Ücreti";
    public string Aciklama => "Cumartesi ve Pazar günleri %50 ek ücret";
    public int Oncelik => 10;
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        // %50 ek ücret ekle
        return temelFiyat * 1.50m;
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        var gun = context.BaslangicTarihi.DayOfWeek;
        return gun == DayOfWeek.Saturday || gun == DayOfWeek.Sunday;
    }
}
