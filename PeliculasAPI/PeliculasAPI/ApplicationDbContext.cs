using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Entidades;
using System.Security.Claims;

namespace PeliculasAPI
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PeliculasActores>()
                .HasKey(x => new { x.ActorId, x.PeliculaId });

            modelBuilder.Entity<PeliculasGeneros>()
                .HasKey(x => new { x.GeneroId, x.PeliculaId });

            modelBuilder.Entity<PeliculasSalasDeCine>()
                .HasKey(x => new { x.PeliculaId, x.SalaDeCineId});

            SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            var rolAdminId = "ee685ce2-86ba-461c-aac9-957450ee0221";
            var usuarioAdminId = "21f59b2b-10b8-4ef3-9225-e3204a4cf550";

            var rolAdmin = new IdentityRole()
            {
                Id = rolAdminId,
                Name = "Admin",
                NormalizedName = "Admin"
            };

            var passwordHasher = new PasswordHasher<IdentityUser>();

            var username = "brian@hotmail.com";

            var usuarioAdmin = new IdentityUser()
            {
                Id = usuarioAdminId,
                UserName = username,
                NormalizedUserName = username,
                Email = username,

                NormalizedEmail = username,
                PasswordHash = passwordHasher.HashPassword(null, "Aa123456!")
            };

            //modelBuilder.Entity<IdentityUser>()
            //    .HasData(usuarioAdmin);
            //modelBuilder.Entity<IdentityRole>()
            //    .HasData(rolAdmin);
            //modelBuilder.Entity<IdentityUserClaim<string>>()
            //    .HasData(new IdentityUserClaim<string>()
            //    {
            //        Id = 1,
            //        ClaimType = ClaimTypes.Role,
            //        UserId = usuarioAdminId,
            //        ClaimValue = "Admin"
            //    });
        }

        public DbSet<Genero> Generos { get; set; }
        public DbSet<Actor> Actores { get; set; }
        public DbSet<Pelicula> Peliculas { get; set; }
        public DbSet<PeliculasActores> PeliculasActores { get; set; }
        public DbSet<PeliculasGeneros> PeliculasGeneros { get; set; }
        public DbSet<SalaDeCine> SalasDeCine { get; set; }
        public DbSet<PeliculasSalasDeCine> PeliculasSalasDeCines { get; set; }
        public DbSet<Review> Reviews { get; set; }
    }
}
