extern alias PricingRules;
using Xunit;
using PricingRules::UstaPlatform.Domain.Helpers;

namespace UstaPlatform.Tests;

/// <summary>
/// Guard validation helper için basit unit testler
/// </summary>
public class GuardTests
{
    [Fact]
    public void NullString_HataFirlatir()
    {
        // Arrange
        string? nullString = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => 
            Guard.AgainstNullOrEmpty(nullString!, "testParam"));
        
        Assert.Contains("testParam", exception.Message);
    }

    [Fact]
    public void NegatifDeger_HataFirlatir()
    {
        // Arrange
        decimal negatifFiyat = -100m;

        // Act & Assert
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() => 
            Guard.AgainstNegative(negatifFiyat, "fiyat"));
        
        Assert.Contains("negatif olamaz", exception.Message);
    }

    [Fact]
    public void GecerliDeger_HataFirlatmaz()
    {
        // Arrange
        decimal pozitifFiyat = 100m;

        // Act & Assert
        var exception = Record.Exception(() => 
            Guard.AgainstNegative(pozitifFiyat, "fiyat"));
        
        Assert.Null(exception); // Hata olmamalı
    }
}
