using Blazorise;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MythRPG.Core;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MythRPG.Data
{
    public class MythRPGContext : IdentityDbContext<User>
    {
        public MythRPGContext(DbContextOptions<MythRPGContext> options): base(options)
        {
            
        }
        public DbSet<Character> Characters { get; set; }
        public DbSet<Trait> Traits { get; set; }
        public DbSet<Bonus> Bonuses { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<CharacterClass> CharacterClasses { get; set; }
        public DbSet<SpellColour> SpellColours { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SpellColour>().HasData(
                new SpellColour { SpellColourId = 1, Name = "Black" },
                new SpellColour { SpellColourId = 2, Name = "White" },
                new SpellColour { SpellColourId = 3, Name = "Red" },
                new SpellColour { SpellColourId = 4, Name = "Orange" },
                new SpellColour { SpellColourId = 5, Name = "Gold" },
                new SpellColour { SpellColourId = 6, Name = "Green" },
                new SpellColour { SpellColourId = 7, Name = "Blue" },
                new SpellColour { SpellColourId = 8, Name = "Purple" },
                new SpellColour { SpellColourId = 9, Name = "Brown" }
            );

            modelBuilder.Entity<Character>()
                .HasMany(e => e.Traits)
                .WithMany();
            modelBuilder.Entity<Character>()
                .HasMany(e => e.Spells)
                .WithMany();
            modelBuilder.Entity<Trait>()
                .HasMany(e => e.Bonuses)
                .WithMany();
            modelBuilder.Entity<Trait>()
                .HasMany(t => t.EligibleClasses)
                .WithMany()
                .UsingEntity("TraitEligibleClasses");
            modelBuilder.Entity<SpellColour>()
                .HasIndex(c => c.Name)
                .IsUnique();
            modelBuilder.Entity<CharacterClass>()
                .HasMany(c => c.GrantedTraits)
                .WithMany()
                .UsingEntity("CharacterClassGrantedTraits");
            modelBuilder.Entity<CharacterClass>()
                .HasMany(c => c.SpellColours)
                .WithMany();

            modelBuilder.Entity<Trait>()
                .HasIndex(t => t.Name)
                .IsUnique();
            modelBuilder.Entity<SpellColour>()
                .HasIndex(c => c.Name)
                .IsUnique();
            modelBuilder.Entity<Trait>()
                .HasIndex(t => t.Name)
                .IsUnique();

            modelBuilder.Entity<Character>().Navigation(e => e.Traits).AutoInclude();
            modelBuilder.Entity<Character>().Navigation(e => e.Spells).AutoInclude();
            modelBuilder.Entity<CharacterClass>().Navigation(c => c.GrantedTraits).AutoInclude();
            modelBuilder.Entity<CharacterClass>().Navigation(c => c.SpellColours).AutoInclude();
            //modelBuilder.Entity<Trait>().Navigation(e => e.Bonuses).AutoInclude();
        }
        
    }
}
