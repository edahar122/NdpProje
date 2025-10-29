namespace UstaPlatform.Domain.Entities;

public class Vatandas
{
    public Guid Id { get; init; }
    public required string Ad { get; init; }
    public required string Soyad { get; init; }
    public required string Telefon { get; init; }
    public string? EPosta { get; init; }
    public DateTime KayitZamani { get; init; }
    
    public string TamIsim => $"{Ad} {Soyad}";
    
    public Vatandas()
    {
        Id = Guid.NewGuid();
        KayitZamani = DateTime.Now;
    }
}
