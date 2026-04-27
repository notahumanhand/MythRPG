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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Character>().HasData(
                new Character { CharacterId = 1, CharacterName = "Briar Elderose", CharacterClass = "Witch", CharacterLevel = 4 }
            );
            modelBuilder.Entity<Bonus>().HasData(
                new Bonus { BonusId = 1, Amount = 2, Modifies = "Prowess", Type = "Other" },
                new Bonus { BonusId = 2, Amount = 2, Modifies = "Arcana", Type = "Other" }
            );
            modelBuilder.Entity<Trait>().HasData(
                new Trait { 
                    TraitId = 1,
                    Name = "Increased Prowess",
                    Source = "Class Trait",
                    ActionCost = "None",
                    ResourceCost = "None",
                    Description = "Add 2 points to your Prowess pool total. You may take this trait multiple times."
                },
                new Trait
                {
                    TraitId = 2,
                    Name = "Improved Arcanum",
                    Source = "Class Trait",
                    ActionCost = "None",
                    ResourceCost = "None",
                    Description = "Add 2 points to your Arcana pool total. You may take this trait multiple times."
                }
            );
            modelBuilder.Entity<Spell>().HasData(
                new Spell
                {
                    SpellId = 1,
                    Name = "Entangle",
                    Cost = 1,
                    Colour = "Black",
                    Type = "Debuff",
                    Casting = "Basic Action",
                    Duration = "Round",
                    Range = "Short",
                    Concentration = false,
                    Effect = "Choose a target within range and make a spell attack, contested by their Magical Defence. On a success, the target is Restrained until the start of your next turn."
                },
                new Spell
                {
                    SpellId = 2,
                    Name = "Divine Bolt",
                    Cost = 3,
                    Colour = "White",
                    Type = "Attack",
                    Casting = "Basic Action",
                    Duration = "Instant",
                    Range = "Short",
                    Concentration = false,
                    Effect = "Make a ranged spell attack against a creature, contested by their Magical Defence. On a hit, deal 3d6+C Radiant damage to the creature. This damage increases by 1d6 if they are a Fiend or Undead."
                },
                new Spell
                {
                    SpellId = 3,
                    Name = "Invigorating Touch",
                    Cost = 2,
                    Colour = "Red",
                    Type = "Healing",
                    Casting = "Basic Action",
                    Duration = "Instant",
                    Range = "Melee",
                    Concentration = false,
                    Effect = "Choose a willing creature within range. They regain 1d8+C Health."
                }
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
            modelBuilder.Entity<Character>().Navigation(e => e.Traits).AutoInclude();
            modelBuilder.Entity<Character>().Navigation(e => e.Spells).AutoInclude();
            //modelBuilder.Entity<Trait>().Navigation(e => e.Bonuses).AutoInclude();
        }
        
    }
}
