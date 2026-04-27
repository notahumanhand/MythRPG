using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace MythRPG.Core
{
    public class Character
    {
        public int CharacterId { get; set; }

        public string? UserId { get; set; }
        public string CharacterName { get; set; } = string.Empty;

        [Range(1, 20, ErrorMessage = "Class must be between 1 and 20.")]
        public int CharacterLevel { get; set; }
        public string? CharacterClass { get; set; }
        public string? MythicPath { get; set; }

        [Range(0, 5, ErrorMessage = "Attributes must be between 0 and 5")]
        public int Might {  get; set; }

        [Range(0, 5, ErrorMessage = "Attributes must be between 0 and 5")]
        public int Agility { get; set; }

        [Range(0, 5, ErrorMessage = "Attributes must be between 0 and 5")]
        public int Knowledge { get; set; }

        [Range(0, 5, ErrorMessage = "Attributes must be between 0 and 5")]
        public int Charisma { get; set; }

        public List<Trait> Traits { get; set; } = new List<Trait>();

        public List<Spell> Spells { get; set; } = new List<Spell>();

        public string Species { get; set; } = string.Empty;
        public string? PrimaryTrait { get; set; }
        public string? SecondaryTrait { get; set; }
        public string? TertiaryTrait { get; set; }
        public string? Size { get; set; }
        public string? Armour { get; set; }
        public bool? Shield { get; set; }
        public string? PortraitUrl { get; set; }

        private int HalfLevel()
        {
            decimal d = this.CharacterLevel / 2;
            return (int)Math.Ceiling(d);
        }
        public int CalculateHealth()
        {
            int health = (this.Might + 1) * this.CharacterLevel + 5;
            return health;
        }
        public int CalculateProwess()
        {
            int prowess = HalfLevel() * this.Agility;
            int bonuses = 0;
            foreach (var trait in this.Traits)
            {
                foreach (var bonus in trait.Bonuses)
                {
                    if (bonus.Modifies is not null && bonus.Modifies.Equals("Prowess")) bonuses += bonus.Amount;
                }
            }
            prowess += bonuses;
            return prowess;
        }
        public int CalculateArcana()
        {
            int arcana = HalfLevel() * this.Charisma;
            if (this.CharacterClass is not null && this.CharacterClass.Equals("Mage")) arcana += this.Knowledge;
            int bonuses = 0;
            foreach (var trait in  this.Traits)
            {
                foreach (var bonus in trait.Bonuses)
                {
                    if (bonus.Modifies is not null && bonus.Modifies.Equals("Arcana")) bonuses += bonus.Amount;
                }
            }
            arcana += bonuses;
            return arcana;
        }
    }
}


