using System;
using System.Collections.Generic;

namespace LezzetAtolyesi.Models;

public partial class Sayfalar
{
    public int SayfaId { get; set; }

    public string? Baslik { get; set; }

    public string? Icerik { get; set; }

    public bool? Aktif { get; set; }

    public bool? Silindi { get; set; }
}
