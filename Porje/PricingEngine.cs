using System.Reflection;
using UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Pricing;

public class PricingEngine
{
    private readonly List<IPricingRule> _rules = new();
    
    public int YuklenenKuralSayisi => _rules.Count;
    
    public IReadOnlyList<IPricingRule> Kurallar => _rules.AsReadOnly();
    
    public void VarsayilanKurallariYukle()
    {
        Console.WriteLine("* Varsayilan kurallar yukleniyor...");
        
        try
        {
            KuralEkle(new UstaPlatform.Pricing.Rules.TemelFiyatKurali());
            KuralEkle(new UstaPlatform.Pricing.Rules.HaftasonuEkUcretiKurali());
            KuralEkle(new UstaPlatform.Pricing.Rules.AcilCagriUcretiKurali());
            KuralEkle(new UstaPlatform.Pricing.Rules.GeceSaatiEkUcretiKurali());
            
            Console.WriteLine($"* {_rules.Count} varsayilan kural yuklendi\n");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"* Varsayilan kurallar yuklenirken hata: {ex.Message}\n");
        }
    }
    
    public void KurallariYukle(string pluginKlasor)
    {
        if (!Directory.Exists(pluginKlasor))
        {
            Console.WriteLine($"* Plugin klasoru bulunamadi: {pluginKlasor}");
            return;
        }
        
        var dllDosyalari = Directory.GetFiles(pluginKlasor, "*.dll");
        
        Console.WriteLine($"* {dllDosyalari.Length} DLL dosyasi taranıyor...");
        
        foreach (var dllPath in dllDosyalari)
        {
            try
            {
                var assembly = Assembly.LoadFrom(dllPath);
                var ruleTypes = assembly.GetTypes()
                    .Where(t => typeof(IPricingRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
                
                foreach (var ruleType in ruleTypes)
                {
                    var rule = (IPricingRule?)Activator.CreateInstance(ruleType);
                    if (rule != null)
                    {
                        _rules.Add(rule);
                        Console.WriteLine($"* Kural yuklendi: {rule.RuleAdi} (Oncelik: {rule.Oncelik})");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"* DLL yuklenirken hata: {Path.GetFileName(dllPath)} - {ex.Message}");
            }
        }
        
        _rules.Sort((a, b) => a.Oncelik.CompareTo(b.Oncelik));
        
        Console.WriteLine($"* Toplam {_rules.Count} fiyatlandirma kurali yuklendi.\n");
    }
    
    public void KuralEkle(IPricingRule rule)
    {
        _rules.Add(rule);
        _rules.Sort((a, b) => a.Oncelik.CompareTo(b.Oncelik));
    }
    
    public decimal FiyatHesapla(decimal temelFiyat, PricingContext context)
    {
        decimal sonucFiyat = temelFiyat;
        
        Console.WriteLine($"\n* Fiyat Hesaplama Basladi:");
        Console.WriteLine($"   Temel Fiyat: {temelFiyat:C2}");
        
        foreach (var rule in _rules)
        {
            if (rule.KuralGecerliMi(context))
            {
                decimal oncekiFiyat = sonucFiyat;
                sonucFiyat = rule.HesaplaFiyat(sonucFiyat, context);
                decimal fark = sonucFiyat - oncekiFiyat;
                string isaret = fark >= 0 ? "+" : "";
                
                Console.WriteLine($"   [{rule.RuleAdi}] {oncekiFiyat:C2} -> {sonucFiyat:C2} ({isaret}{fark:C2})");
            }
        }
        
        Console.WriteLine($"   ================================");
        Console.WriteLine($"   Toplam Fiyat: {sonucFiyat:C2}\n");
        
        return sonucFiyat;
    }
    
    public string FiyatHesaplaDetayli(decimal temelFiyat, PricingContext context)
    {
        var detay = new System.Text.StringBuilder();
        decimal sonucFiyat = temelFiyat;
        
        detay.AppendLine($"Temel Fiyat: {temelFiyat:C2}");
        
        foreach (var rule in _rules)
        {
            if (rule.KuralGecerliMi(context))
            {
                decimal oncekiFiyat = sonucFiyat;
                sonucFiyat = rule.HesaplaFiyat(sonucFiyat, context);
                decimal fark = sonucFiyat - oncekiFiyat;
                
                detay.AppendLine($"  {rule.RuleAdi}: {oncekiFiyat:C2} -> {sonucFiyat:C2} ({fark:+0.00;-0.00})");
            }
        }
        
        detay.AppendLine($"Toplam: {sonucFiyat:C2}");
        
        return detay.ToString();
    }
}
