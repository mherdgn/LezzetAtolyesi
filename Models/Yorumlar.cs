using System;
using System.Collections.Generic;

namespace LezzetAtolyesi.Models;

public partial class Yorumlar
{
    public int YorumId { get; set; }

    public string? Yorum { get; set; }
    public DateTime? Eklemetarihi { get; set; }

    public int? TarifId { get; set; }

    public int? UyeId { get; set; }

    public bool? Aktif { get; set; }

    public bool? Silindi { get; set; }

    public virtual YemekTarifleri TarifNavigation { get; set; }

    public virtual Kullanicilar Uye { get; set; }
}
