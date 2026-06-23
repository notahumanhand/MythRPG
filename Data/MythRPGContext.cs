using Blazorise;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using MythRPG.Core;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
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
        public DbSet<Prerequisite> Prerequisites { get; set; }
        public DbSet<Spell> Spells { get; set; }
        public DbSet<CharacterClass> CharacterClasses { get; set; }
        public DbSet<SpellColour> SpellColours { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SpellColour>().HasData(
                new SpellColour { SpellColourId = 1, Name = "Black", HexCode = "#121214" },
                new SpellColour { SpellColourId = 2, Name = "White", HexCode = "#FFFFFF" },
                new SpellColour { SpellColourId = 3, Name = "Red", HexCode = "#7D1616" },
                new SpellColour { SpellColourId = 4, Name = "Orange", HexCode = "#DF4200" },
                new SpellColour { SpellColourId = 5, Name = "Gold", HexCode = "#E08500" },
                new SpellColour { SpellColourId = 6, Name = "Green", HexCode = "#2F4931" },
                new SpellColour { SpellColourId = 7, Name = "Blue", HexCode = "#3169D9" },
                new SpellColour { SpellColourId = 8, Name = "Purple", HexCode = "#971EA4" },
                new SpellColour { SpellColourId = 9, Name = "Brown", HexCode = "#4E3526" }
            );

            modelBuilder.Entity<Character>()
                .HasMany(e => e.Traits)
                .WithMany();
            modelBuilder.Entity<Character>()
                .HasMany(e => e.Spells)
                .WithMany();
            modelBuilder.Entity<Character>()
                .HasOne(c => c.CharacterClass)
                .WithMany();
            modelBuilder.Entity<Trait>()
                .HasMany(e => e.Bonuses)
                .WithMany();
            modelBuilder.Entity<Trait>()
                .HasMany(t => t.Prerequisites)
                .WithOne()
                .HasForeignKey(p => p.TraitId)
                .OnDelete(DeleteBehavior.Cascade);
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

            modelBuilder.Entity<Prerequisite>()
                .Property(p => p.Type)
                .HasConversion<string>();

            modelBuilder.Entity<Character>().Navigation(c => c.CharacterClass).AutoInclude();
            modelBuilder.Entity<Character>().Navigation(e => e.Traits).AutoInclude();
            modelBuilder.Entity<Character>().Navigation(e => e.Spells).AutoInclude();
            modelBuilder.Entity<CharacterClass>().Navigation(c => c.GrantedTraits).AutoInclude();
            modelBuilder.Entity<CharacterClass>().Navigation(c => c.SpellColours).AutoInclude();
            modelBuilder.Entity<Trait>().Navigation(e => e.Bonuses).AutoInclude();
            modelBuilder.Entity<Trait>().Navigation(t => t.Prerequisites).AutoInclude();
        }
        
    }
}
