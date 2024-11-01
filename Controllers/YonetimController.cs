using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LezzetAtolyesi.Models;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace LezzetAtolyesi.Controllers
{
    [Authorize(Roles = "Yonetici")] //Bütün Yönetim Sayfası Şifreli Girişe Açık
    public class YonetimController : Controller
    {

        YemektarifleriDbContext db = new YemektarifleriDbContext();
        public IActionResult Index()
        {
            return View();
        }


        //Bilgilerim
        public IActionResult Bilgilerim()
        {
            int kulid = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var kullanici = db.Kullanicilars.Find(kulid);
            kullanici.Parola = "";
            return View(kullanici);
        }
        public IActionResult BilgilerimiGuncelle(Kullanicilar kul)
        {
            var kullanici = db.Kullanicilars.Where(k => k.Silindi == false && k.KullaniciId == kul.KullaniciId).FirstOrDefault();
            kullanici.Aktif = kul.Aktif;
            kullanici.Adi = kul.Adi;
            kullanici.Soyadi = kul.Soyadi;
            kullanici.Eposta = kul.Eposta;
            kullanici.Telefon = kul.Telefon;
            try
            {
                if(kul.Parola.Trim().Length !=0)
                {
                    kullanici.Parola = MD5Sifrele(kul.Parola.Trim());
                }
            }
            catch { }

            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Bilgilerim");
        }


        //Sayfa İşlemleri
        public IActionResult Sayfalar()
        {
            var sayfalar = db.Sayfalars.Where(s => s.Silindi == false).OrderBy(s => s.Baslik).ToList();

            return View(sayfalar);
        }

        public IActionResult SayfaEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SayfaEkle(Sayfalar s)
        {
            s.Silindi = false;
            db.Sayfalars.Add(s);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }

        public IActionResult SayfaGetir(int id)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();

            return View("SayfaGuncelle", sayfa);
        }

        public IActionResult SayfaGuncelle(Sayfalar syf)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == syf.SayfaId).FirstOrDefault();
            sayfa.Baslik = syf.Baslik;
            sayfa.Icerik = syf.Icerik;
            sayfa.Aktif = syf.Aktif;
            db.Sayfalars.Update(sayfa);
            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }

        public IActionResult SayfaSil(int id)
        {
            var sayfa = db.Sayfalars.Where(s => s.Silindi == false && s.SayfaId == id).FirstOrDefault();
            sayfa.Silindi = true;
            db.Sayfalars.Update(sayfa);

            //var sayfa2 = db.Sayfalars.Find(id);
            //db.Remove(sayfa2);

            db.SaveChanges();
            return RedirectToAction("Sayfalar");
        }

        //Kategori İşlemleri

        public IActionResult Kategoriler()
        {
            var kategoriler = db.Kategorilers.Where(k => k.Silindi == false).OrderBy(k => k.Kategoriadi).ToList();

            return View(kategoriler);
        }

        public IActionResult KategoriEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KategoriEkle(Kategoriler k)
        {
            k.Silindi = false;
            db.Kategorilers.Add(k);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }

        public IActionResult KategoriGetir(int id)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == id).FirstOrDefault();

            return View("KategoriGuncelle", kategori);
        }

        public IActionResult KategoriYemekler(int id)
        {
            var yemekler = db.YemekTarifleris.Include(k => k.Kategori).Where(y => y.Silindi == false && y.KategoriId == id).ToList();

            //return View(yemekler);
            return View("Tarifler", yemekler);

        }

        public IActionResult KategoriGuncelle(Kategoriler ktgr)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == ktgr.KategoriId).FirstOrDefault();
            kategori.Kategoriadi = ktgr.Kategoriadi;
            kategori.Aktif = ktgr.Aktif;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategori");
        }

        public IActionResult KategoriSil(int id)
        {
            var kategori = db.Kategorilers.Where(k => k.Silindi == false && k.KategoriId == id).FirstOrDefault();
            kategori.Silindi = true;
            db.Kategorilers.Update(kategori);
            db.SaveChanges();
            return RedirectToAction("Kategoriler");
        }


        //Tarif İşlemleri
        public IActionResult Tarifler()
        {
            var tarifler = db.YemekTarifleris.Include(k => k.Kategori).Where(t => t.Silindi == false).OrderBy(s => s.Sira)
                /*OrderBy(t => t.Yemekadi)*/.ToList();

            return View(tarifler);
        }

        public IActionResult TarifEkle()
        {
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                select new SelectListItem
                {
                  Text=k.Kategoriadi,
                  Value=k.KategoriId.ToString()
                }
            );
            ViewBag.KategoriId = kategoriler;
            return View();
        }

        [HttpPost]
        public IActionResult TarifEkle(YemekTarifleri t)
        {
            t.Silindi = false;
            t.Eklemetarihi=DateTime.Now;
            db.YemekTarifleris.Add(t);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }

        public IActionResult TarifGetir(int id)
        {
            var tarif = db.YemekTarifleris.Include(k => k.Kategori).Where(t => t.Silindi==false && t.TarifId == id).FirstOrDefault();
            var kategoriler = (from k in db.Kategorilers.Where(k => k.Silindi == false && k.Aktif == true).ToList()
                select new SelectListItem
                {
                   Text=k.Kategoriadi,
                   Value=k.KategoriId.ToString()
                }
           );
            ViewBag.KategoriId = kategoriler;
            return View("TarifGuncelle", tarif);
        }

        public IActionResult TarifYorumlari(int id)
        {
            var yorumlar = db.Yorumlars.Where(y => y.Silindi == false && y.TarifId == id).ToList();

            //return View(yorumlar);
            return View("Yorumlar", yorumlar);

        }

        public IActionResult TarifGuncelle(YemekTarifleri trf)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.TarifId == trf.TarifId).FirstOrDefault();
            tarif.Yemekadi = trf.Yemekadi;
            tarif.Tarif = trf.Tarif;
            tarif.Sira = trf.Sira;
            tarif.KategoriId = trf.KategoriId;
            tarif.Aktif = trf.Aktif;
            db.YemekTarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }

        public IActionResult TarifSil(int id)
        {
            var tarif = db.YemekTarifleris.Where(t => t.Silindi == false && t.TarifId == id).FirstOrDefault();
            tarif.Silindi = true;
            db.YemekTarifleris.Update(tarif);
            db.SaveChanges();
            return RedirectToAction("Tarifler");
        }

        public IActionResult CikisYap()
        {
            return View();
        }

        //Yorumlar

        [HttpGet]

        public IActionResult Yorumlar()
        {
            var yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false).OrderByDescending(y => y.Eklemetarihi).ToList();
            return View(yorumlar);
        }

        [HttpPost]

        public IActionResult Yorumlar(string listelemeturu)
        {
            var yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false).OrderByDescending(y => y.Eklemetarihi).ToList();
            switch (listelemeturu)
            {
                case "Onayli":
                    yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == true).OrderByDescending(y => y.Eklemetarihi).ToList();
                    break;
                case "Onaysiz":
                    yorumlar = db.Yorumlars.Include(t => t.TarifNavigation).Include(u => u.Uye).Where(y => y.Silindi == false && y.Aktif == true).OrderByDescending(y => y.Eklemetarihi).ToList();
                    break;
            }

            return View(yorumlar);
        }
        public IActionResult Onayla(int id)
        {
            var yorum = db.Yorumlars.Where(y => y.Silindi == false && y.YorumId == id).FirstOrDefault();
            yorum.Aktif = Convert.ToBoolean((-1*Convert.ToInt32(yorum.Aktif))+1);
            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }
        public IActionResult YorumuSil(int id)
        {
            var yorum = db.Yorumlars.Where(y => y.Silindi == false && y.YorumId == id).FirstOrDefault();
            yorum.Silindi = true;
            db.Yorumlars.Update(yorum);
            db.SaveChanges();
            return RedirectToAction("Yorumlar");
        }


        //Kullanıcı İşlemleri
        public IActionResult Kullanicilar()
        {
            var kullanicilar = db.Kullanicilars.Where(k => k.Silindi == false)
                .OrderBy(k => k.Yetki).OrderBy(k => k.Adi).OrderBy(k => k.Soyadi).ToList();

            return View(kullanicilar);
        }

        public IActionResult KullaniciEkle()
        {
            return View();
        }

        [HttpPost]
        public IActionResult KullaniciEkle(Kullanicilar k)
        {
            k.Silindi = false;
            k.Parola=MD5Sifrele(k.Parola.Trim());
            db.Kullanicilars.Add(k);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }

        public IActionResult KullaniciGetir(int id)
        {
            var kullanici = db.Kullanicilars.Where(k => k.Silindi == false && k.KullaniciId == id).FirstOrDefault();
            kullanici.Parola = "";

            return View("KullaniciGuncelle", kullanici);
        }

        public IActionResult KullaniciGuncelle(Kullanicilar kul)
        {
            var kullanici = db.Kullanicilars.Where(k => k.Silindi == false && k.KullaniciId == kul.KullaniciId).FirstOrDefault();

            if (kullanici == null)
            {
                // Eğer kullanıcı null dönerse, uygun bir hata mesajı verebilir veya bir hata sayfasına yönlendirebiliriz.
                return NotFound("Güncellenmek istenen kullanıcı bulunamadı.");
            }

            kullanici.Aktif = kul.Aktif;
            kullanici.Adi=kul.Adi;
            kullanici.Soyadi = kul.Soyadi;
            kullanici.Eposta = kul.Eposta;
            kullanici.Telefon = kul.Telefon;



            try
            {
                if (!string.IsNullOrWhiteSpace(kul.Parola))
                {
                    kullanici.Parola = MD5Sifrele(kul.Parola.Trim());
                }
            }
            catch
            {
                // Hata oluştuğunda bir işlem yapabiliriz.
            }


            kullanici.Yetki = kul.Yetki;
            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }

        public IActionResult KullaniciSil(int id)
        {
            var kullanici = db.Kullanicilars.Where(k => k.Silindi == false && k.KullaniciId == id).FirstOrDefault();
            kullanici.Silindi = true;
            db.Kullanicilars.Update(kullanici);
            db.SaveChanges();
            return RedirectToAction("Kullanicilar");
        }


        //Menü İşlemleri
        public IActionResult Menuler()
        {
            var menuler = db.Menulers.Where(m => m.Silindi == false).OrderBy(m => m.Baslik).ToList();

            return View(menuler);
        }

        public IActionResult MenuEkle()
        {
            var menuler = (from k in db.Menulers.Where(k => k.Silindi == false && k.Aktif == true && k.UstId == null).ToList()
                               select new SelectListItem
                               {
                                   Text=k.Baslik,
                                   Value=k. MenuId.ToString()
                               }
           );
            var m2 = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = " Yok",
                Value = "0"
                }
            };
           // menuler.Union(m2);
            ViewBag.ustmenuler = menuler.Union(m2).OrderBy(t => t.Text);
            var sayfalar = (from k in db.Sayfalars.Where(k => k.Silindi == false && k.Aktif == true).OrderBy(s => s.Baslik).ToList()
                            select new SelectListItem
                            {
                                Text=k.Baslik,
                                Value=k.SayfaId.ToString()
                            }
           );
            ViewBag.sayfalar = sayfalar;

            return View();
        }

        [HttpPost]
        public IActionResult MenuEkle(Menuler m)
        {
            if (m.UstId == 0)
            {
                m.UstId = null;
            }

            m.Silindi = false;
            db.Menulers.Add(m);
            db.SaveChanges();
            return RedirectToAction("Menuler");
        }

        public IActionResult MenuGetir(int id)
        {
            var menuler = (from k in db.Menulers.Where(k => k.Silindi == false && k.Aktif == true && k.UstId == null).ToList()
                           select new SelectListItem
                           {
                               Text=k.Baslik,
                               Value=k.MenuId.ToString()
                           }
          );
            var m2 = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = " Yok",
                Value = "0"
                }
            };
            // menuler.Union(m2);
            ViewBag.ustmenuler = menuler.Union(m2).OrderBy(t => t.Text);
            var sayfalar = (from k in db.Sayfalars.Where(k => k.Silindi == false && k.Aktif == true).OrderBy(s => s.Baslik).ToList()
                            select new SelectListItem
                            {
                                Text=k.Baslik,
                                Value=k.SayfaId.ToString()
                            }
           );

            ViewBag.sayfalar = sayfalar;
            var menu = db.Menulers.Where(m => m.Silindi == false && m.MenuId == id).FirstOrDefault();

            return View("MenuGuncelle", menu);
        }

        public IActionResult MenuGuncelle(Menuler men)
        {
            var menu = db.Menulers.Where(m => m.Silindi == false && m.MenuId == men.MenuId).FirstOrDefault();
            menu.Baslik = men.Baslik;
            menu.Url = men.Url;
            menu.Aktif =  men.Aktif;
            menu.Sira = men.Sira;
            if (men.UstId == 0)
            {
                men.UstId = null;
            }
            menu.UstId =  men.UstId;
            db.Menulers.Update(menu);
            db.SaveChanges();
            return RedirectToAction("Menuler");
        }

        public IActionResult MenuSil(int id)
        {
            var menu = db.Menulers.Where(m => m.Silindi == false && m.MenuId == id).FirstOrDefault();
            menu.Silindi = true;
            db.Menulers.Update(menu);
            db.SaveChanges();
            return RedirectToAction("Menuler");
        }



        public static string MD5Sifrele(string sifrelenecekMetin)
        {

            // MD5CryptoServiceProvider sınıfının bir örneğini oluşturduk.
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            //Parametre olarak gelen veriyi byte dizisine dönüştürdük.
            byte[] dizi = Encoding.UTF8.GetBytes(sifrelenecekMetin);
            //dizinin hash'ini hesaplattık.
            dizi = md5.ComputeHash(dizi);
            //Hashlenmiş verileri depolamak için StringBuilder nesnesi oluşturduk.
            StringBuilder sb = new StringBuilder();
            //Her byte'i dizi içerisinden alarak string türüne dönüştürdük.

            foreach (byte ba in dizi)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            //hexadecimal(onaltılık) stringi geri döndürdük.
            return sb.ToString();
        }


    }

}

