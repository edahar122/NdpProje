namespace UstaPlatform.Domain.Entities;

public class Usta
{
    public Guid Id { get; init; }
    public required string UzmanlikAlani { get; init; }
    public required string Ad { get; init; }
    public required string Soyad { get; init; }
    public List<string> Yetenekler { get; init; } = new();
    public double Puan { get; set; }
    public int Yogunluk { get; set; }
    public DateTime KayitZamani { get; init; }
    public decimal SaatBasiUcret { get; init; }
    public bool Aktif { get; set; } = true;
    
    public string TamIsim => $"{Ad} {Soyad}";
    
    public Usta()
    {
        Id = Guid.NewGuid();
        KayitZamani = DateTime.Now;
    }
}
