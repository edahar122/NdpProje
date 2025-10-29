extern alias PricingRules;
using Xunit;
using PricingRules::UstaPlatform.Pricing.Rules;
using PricingRules::UstaPlatform.Domain.Interfaces;

namespace UstaPlatform.Tests;

/// <summary>
/// AcilCagriUcretiKurali için basit unit testler
/// </summary>
public class AcilCagriUcretiKuraliTests
{
    [Fact]
    public void AcilTalep_YuzdeYetmisBesEkUcret_Hesaplanir()
    {
        // Arrange
        var kural = new AcilCagriUcretiKurali();
        var context = new PricingContext
        {
            TalepId = Guid.NewGuid(),
            UstaId = Guid.NewGuid(),
            UzmanlikAlani = "Tesisatçı",
            BaslangicTarihi = DateTime.Now,
            TahminiSure = 2m,
            SaatBasiUcret = 500m,
            Acil = true,
            AdresX = 100,
            AdresY = 200
        };

        // Act
        decimal sonuc = kural.HesaplaFiyat(1000m, context);

        // Assert
        Assert.Equal(1750m, sonuc); // 1000 * 1.75 = 1750
    }

    [Fact]
    public void AcilDegilse_KuralGecerliDegil()
    {
        // Arrange
        var kural = new AcilCagriUcretiKurali();
        var context = new PricingContext
        {
            TalepId = Guid.NewGuid(),
            UstaId = Guid.NewGuid(),
            UzmanlikAlani = "Elektrikçi",
            BaslangicTarihi = DateTime.Now,
            TahminiSure = 1.5m,
            SaatBasiUcret = 600m,
            Acil = false, // ACİL DEĞİL
            AdresX = 150,
            AdresY = 250
        };

        // Act
        bool gecerliMi = kural.KuralGecerliMi(context);

        // Assert
        Assert.False(gecerliMi);
    }
}
