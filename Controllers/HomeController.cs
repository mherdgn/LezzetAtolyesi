using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LezzetAtolyesi.Models;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace LezzetAtolyesi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
            
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        //YemektarifleriDbContext db = new YemektarifleriDbContext();
        public IActionResult Index(int id)
        {
            YemektarifleriDbContext db = new YemektarifleriDbContext();
            var sayfa = db.Sayfalars.Where(a=>a.Silindi==false && a.Aktif==true && a.SayfaId==id).FirstOrDefault();

            return View(sayfa);
        }

        //Tarif
        public IActionResult TumTarifler()
        {
            YemektarifleriDbContext db = new YemektarifleriDbContext();
            var tarifler = db.YemekTarifleris.Include(k=>k.Kategori).Where(y => y.Silindi==false && y.Aktif==true).OrderBy(s=>s.Sira).ToList();
            //ToList hepsini listeler.
            return View(tarifler);
        }   
        public IActionResult Tarif(int id) //int id, parametre olarak id alsýn demek.
        {
            YemektarifleriDbContext db = new YemektarifleriDbContext();

            TarifYorumlar t = new TarifYorumlar();

            var tarifler = db.YemekTarifleris.Include(k => k.Kategori).Where(y => y.Silindi==false && y.Aktif==true && y.TarifId == id).FirstOrDefault();
            //FirstOrDefault, biz id aldýðýmýz için id'si alýnaný gösterir.
            t.tarif = tarifler;

            var yorumlar = db.Yorumlars.Include(u=>u.Uye).Where(y => y.Silindi==false && y.Aktif==true && y.TarifId == id).OrderByDescending(y => y.Eklemetarihi).ToList();
            //Yorumlarýn hepsi gelsin istiyoruz, o yüzden ToList()
            t.yorumlar = yorumlar;  
            return View(t);
        }

        public IActionResult YorumYap(Yorumlar yor)
        {
            YemektarifleriDbContext db = new YemektarifleriDbContext();
            yor.Silindi = false;
            yor.Aktif = false;
            yor.Eklemetarihi = DateTime.Now;
            db.Yorumlars.Add(yor);
            db.SaveChanges();
            TempData["mesaj"] = "Yorumunuz alýndý,onay bekleniyor..";
            return Redirect("/Home/Tarif" +yor.TarifId);
        }

            public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
