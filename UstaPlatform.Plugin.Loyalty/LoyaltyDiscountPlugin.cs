using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Plugin.Loyalty;

/// <summary>
/// Sadakat indirimi kuralı - Plugin demonstrasyonu
/// Bu kural ayrı bir DLL olarak Plugins klasörüne konulabilir
/// </summary>
public class LoyaltyDiscountPlugin : IPricingRule
{
    public string RuleAdi => "Sadakat İndirimi (Plugin)";
    public string Aciklama => "Sadık müşterilere %10 indirim (DLL Plugin)";
    public int Oncelik => 100; // En son uygulanır
    
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
