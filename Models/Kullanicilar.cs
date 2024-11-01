using System;
using System.Collections.Generic;

namespace LezzetAtolyesi.Models;

public partial class Kullanicilar
{
    internal bool yetki;

    public int KullaniciId { get; set; }

    public string? Adi { get; set; }

    public string? Soyadi { get; set; }

    public string? Eposta { get; set; }

    public string? Telefon { get; set; }

    public string? Parola { get; set; }

    public bool? Yetki { get; set; }

    public bool? Aktif { get; set; }

    public bool? Silindi { get; set; }

    public virtual ICollection<Yorumlar> Yorumlars { get; set; } = new List<Yorumlar>();
}
