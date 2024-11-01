using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace LezzetAtolyesi.Models;

public partial class YemektarifleriDbContext : DbContext
{
    public YemektarifleriDbContext()
    {
    }

    public YemektarifleriDbContext(DbContextOptions<YemektarifleriDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Kategoriler> Kategorilers { get; set; }

    public virtual DbSet<Kullanicilar> Kullanicilars { get; set; }

    public virtual DbSet<Menuler> Menulers { get; set; }

    public virtual DbSet<Sayfalar> Sayfalars { get; set; }

    public virtual DbSet<YemekTarifleri> YemekTarifleris { get; set; }

    public virtual DbSet<Yorumlar> Yorumlars { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=MINEHATUN\\MSSQLSERVER5;Database=yemektarifleriDB;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Kategoriler>(entity =>
        {
            entity.HasKey(e => e.KategoriId).HasName("PK__Kategori__1D91819D74480720");

            entity.ToTable("Kategoriler");

            entity.Property(e => e.KategoriId).HasColumnName("kategoriId");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.Kategoriadi)
                .HasMaxLength(100)
                .HasColumnName("kategoriadi");
            entity.Property(e => e.Silindi).HasColumnName("silindi");
        });

        modelBuilder.Entity<Kullanicilar>(entity =>
        {
            entity.HasKey(e => e.KullaniciId).HasName("PK__Kullanic__848DC56BB181F652");

            entity.ToTable("Kullanicilar");

            entity.Property(e => e.KullaniciId).HasColumnName("kullaniciId");
            entity.Property(e => e.Adi)
                .HasMaxLength(100)
                .HasColumnName("adi");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.Eposta)
                .HasMaxLength(100)
                .HasColumnName("eposta");
            entity.Property(e => e.Parola)
                .HasMaxLength(35)
                .HasColumnName("parola");
            entity.Property(e => e.Silindi).HasColumnName("silindi");
            entity.Property(e => e.Soyadi)
                .HasMaxLength(100)
                .HasColumnName("soyadi");
            entity.Property(e => e.Telefon)
                .HasMaxLength(15)
                .HasColumnName("telefon");
            entity.Property(e => e.Yetki).HasColumnName("yetki");
        });

        modelBuilder.Entity<Menuler>(entity =>
        {
            entity.HasKey(e => e.MenuId).HasName("PK__Menuler__3B40717444E23FA3");

            entity.ToTable("Menuler");

            entity.Property(e => e.MenuId).HasColumnName("menuId");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.Baslik)
                .HasMaxLength(250)
                .HasColumnName("baslik");
            entity.Property(e => e.Silindi).HasColumnName("silindi");
            entity.Property(e => e.Sira).HasColumnName("sira");
            entity.Property(e => e.Url)
                .HasMaxLength(255)
                .HasColumnName("url");
            entity.Property(e => e.UstId).HasColumnName("ustId");
        });

        modelBuilder.Entity<Sayfalar>(entity =>
        {
            entity.HasKey(e => e.SayfaId).HasName("PK__Sayfalar__F81FC2D2505D1007");

            entity.ToTable("Sayfalar");

            entity.Property(e => e.SayfaId).HasColumnName("sayfaId");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.Baslik)
                .HasMaxLength(250)
                .HasColumnName("baslik");
            entity.Property(e => e.Icerik)
                .HasColumnType("ntext")
                .HasColumnName("icerik");
            entity.Property(e => e.Silindi).HasColumnName("silindi");
        });

        modelBuilder.Entity<YemekTarifleri>(entity =>
        {
            entity.HasKey(e => e.TarifId).HasName("PK__YemekTar__87FD2108243D2DE7");

            entity.ToTable("YemekTarifleri");

            entity.Property(e => e.TarifId).HasColumnName("tarifId");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.Eklemetarihi)
                .HasColumnType("datetime")
                .HasColumnName("eklemetarihi");
            entity.Property(e => e.KategoriId).HasColumnName("kategoriID");
            entity.Property(e => e.Silindi).HasColumnName("silindi");
            entity.Property(e => e.Sira).HasColumnName("sira");
            entity.Property(e => e.Tarif)
                .HasColumnType("ntext")
                .HasColumnName("tarif");
            entity.Property(e => e.Yemekadi)
                .HasMaxLength(250)
                .HasColumnName("yemekadi");

            entity.HasOne(d => d.Kategori).WithMany(p => p.YemekTarifleris)
                .HasForeignKey(d => d.KategoriId)
                .HasConstraintName("FK__YemekTari__kateg__3D5E1FD2");
        });

        modelBuilder.Entity<Yorumlar>(entity =>
        {
            entity.HasKey(e => e.YorumId).HasName("PK__Yorumlar__2221919F2F11D98E");

            entity.ToTable("Yorumlar");

            entity.Property(e => e.YorumId).HasColumnName("yorumId");
            entity.Property(e => e.Aktif).HasColumnName("aktif");
            entity.Property(e => e.Eklemetarihi)
                .HasColumnType("datetime")
                .HasColumnName("eklemetarihi");
            entity.Property(e => e.Silindi).HasColumnName("silindi");
            //entity.Property(e => e.Tarif)
            //    .HasColumnType("ntext")
            //    .HasColumnName("tarif");
            entity.Property(e => e.TarifId).HasColumnName("tarifID");
            entity.Property(e => e.UyeId).HasColumnName("uyeID");
            entity.Property(e => e.Yorum)
                .HasMaxLength(500)
                .HasColumnName("yorum");

            entity.HasOne(d => d.TarifNavigation).WithMany(p => p.Yorumlars)
                .HasForeignKey(d => d.TarifId)
                .HasConstraintName("FK__Yorumlar__tarifI__4222D4EF");

            entity.HasOne(d => d.Uye).WithMany(p => p.Yorumlars)
                .HasForeignKey(d => d.UyeId)
                .HasConstraintName("FK__Yorumlar__uyeID__4316F928");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
