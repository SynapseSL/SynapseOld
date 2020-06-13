namespace Synapse.Api
{
    public enum Effect
    {
        /// <summary>
        /// The Player cant open his Inventory and Reload his Weapons
        /// </summary>
        /// <remarks>0 = Disabled,1 = Enabled</remarks>
        Amnesia,
        /// <summary>
        /// Quickly drains stamina then Health If theres none left
        /// </summary>
        /// <remarks>0 = Disabled,1 = Enabled</remarks>
        Asphyxiated,
        /// <summary>
        /// Damage-over time starting high and ramping low.Ticks every 5s.
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Bleeding,
        /// <summary>
        /// applies extrem screen blur
        /// </summary>
        Blinded,
        /// <summary>
        /// slightly inreases all damage taken
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Burned,
        /// <summary>
        /// Blurs the screen as the Player turns
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabel</remarks>
        Concussed,
        /// <summary>
        /// Teleports to the Pocket Dimension and drains Hp until he escapes
        /// </summary>
        /// <remarks>1 = Enabled</remarks>
        Corroding,
        /// <summary>
        /// Heavily muffles all sounds
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enablked</remarks>
        Deafened,
        /// <summary>
        /// Remove 10% of max health each second
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Decontaminating,
        /// <summary>
        /// Slows all Movement speed
        /// </summary>
        /// <remarks>0 = Diabled, 1 = Enabled</remarks>
        Disabled,
        /// <summary>
        /// prevents all movement
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Ensnared,
        /// <summary>
        /// laves stamina capacity and regeneration rate
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Exhausted,
        /// <summary>
        /// Flash the Player
        /// </summary>
        /// <remarks>0 = Disabled, 1-244 = time in ms 255 = forever</remarks>
        Flashed,
        /// <summary>
        /// Sprinting drains 2 Hp/s
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Hemorrhage,
        /// <summary>
        /// Infinte Stamina
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Invigorated,
        /// <summary>
        /// slightly Increases stamina consumption
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Panic,
        /// <summary>
        /// Damage-over time starting low and ramping high.Ticks every 5s.
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        Poisoned,
        /// <summary>
        /// The Player will walk faster
        /// </summary>
        /// <remarks>0 = Disabled, 1 = 1xCola, 2 = 2xCola, 3 = 3xCola, 4 = 4xCola</remarks>
        Scp207,
        /// <summary>
        /// The Player cant be seen by other Humans.He need do activate Scp268 in his Inventory
        /// </summary>
        /// <remarks>0 = Disabled,1 = Enabled</remarks>
        Scp268,
        /// <summary>
        /// Slows down Players but not Scps
        /// </summary>
        /// <remarks>0 = Disabled, 1 = Enabled</remarks>
        SinkHole,
        /// <summary>
        /// The Vision of Scp939
        /// </summary>
        /// <remarks>0 = Disabled, 1 = OnlyMarker, 2 = Only Screen Filtest, 3 = Everything</remarks>
        Visuals939
    }
}
