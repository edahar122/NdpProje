using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing.Rules;

/// <summary>
/// Sadakat indirimi kuralı - Demo için plugin örneği
/// Müşteri sadakati durumunda %10 indirim
/// </summary>
public class LoyaltyDiscountRule : IPricingRule
{
    public string RuleAdi => "Sadakat İndirimi";
    public string Aciklama => "Sadık müşterilere %10 indirim";
    public int Oncelik => 100; // En son uygulanır (yüksek öncelik)
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        // %10 indirim
        return temelFiyat * 0.90m;
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        // EkBilgiler içinde "SadikMusteri" kontrolü
        if (context.EkBilgiler.TryGetValue("SadikMusteri", out var deger))
        {
            return deger is bool sadik && sadik;
        }
        return false;
    }
}
