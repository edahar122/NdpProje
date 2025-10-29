using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing.Rules;

/// <summary>
/// Acil çağrı ek ücret kuralı - Acil işler için %75 ek ücret
/// </summary>
public class AcilCagriUcretiKurali : IPricingRule
{
    public string RuleAdi => "Acil Çağrı Ücreti";
    public string Aciklama => "Acil işler için %75 ek ücret";
    public int Oncelik => 20;
    
    public decimal HesaplaFiyat(decimal temelFiyat, PricingContext context)
    {
        // %75 ek ücret ekle
        return temelFiyat * 1.75m;
    }
    
    public bool KuralGecerliMi(PricingContext context)
    {
        return context.Acil;
    }
}