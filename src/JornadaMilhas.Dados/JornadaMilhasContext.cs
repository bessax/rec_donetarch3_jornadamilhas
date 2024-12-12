using JornadaMilhas.Dominio.Entidades;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace JornadaMilhas.Dados;
public class JornadaMilhasContext : IdentityDbContext
{
    private readonly IConfiguration _configuration;
    public DbSet<OfertaViagem> OfertasViagem { get; set; }
    public DbSet<Rota> Rota { get; set; }

    public JornadaMilhasContext()
    {

    }
    public JornadaMilhasContext(DbContextOptions<JornadaMilhasContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (optionsBuilder.IsConfigured)
        {
            return;
        }
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer(_configuration.GetConnectionString("ConnectionStrings:DefaultConnection"));

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Rota
        modelBuilder.Entity<Rota>().HasKey(e => e.Id);
        modelBuilder.Entity<Rota>()
                        .Property(a => a.Origem);
        modelBuilder.Entity<Rota>()
                        .Property(a => a.Destino); 
        modelBuilder.Entity<Rota>().Ignore(a => a.Erros);
        modelBuilder.Entity<Rota>().Ignore(a => a.EhValido);

        //OfertaViagem
        modelBuilder.Entity<OfertaViagem>().HasKey(e => e.Id);
        modelBuilder.Entity<OfertaViagem>()
                        .OwnsOne(o => o.Periodo,periodo =>
                        {
                            periodo.Property(e => e.DataInicial).HasColumnName("DataInicial");
                            periodo.Property(e => e.DataFinal).HasColumnName("DataFinal");
                            periodo.Ignore(e=>e.Erros);
                            periodo.Ignore(e=>e.EhValido);
                        });
        modelBuilder.Entity<OfertaViagem>()
            .Property(o => o.Preco);
        modelBuilder.Entity<OfertaViagem>().Ignore(a => a.Erros);
         modelBuilder.Entity<OfertaViagem>().Ignore(a => a.EhValido);

        base.OnModelCreating(modelBuilder);
    }

}
