namespace Synapse.Events.Classes
{
    public class PlayerHurtClass
    {
        //Variablen
        private DamageTypes.DamageType damageType;
        private PlayerStats.HitInfo info;

        //Eigenschaften
        public ReferenceHub Player { get; internal set; }
        public ReferenceHub Attacker { get; internal set; }
        public PlayerStats.HitInfo Info
        {
            get => info;
            set
            {
                damageType = DamageTypes.None;
                info = value;
            }
        }

        public DamageTypes.DamageType DamageType
        {
            get
            {
                if (damageType == DamageTypes.None)
                    damageType = DamageTypes.FromIndex(info.Tool);

                return damageType;
            }
        }
        public int Tool => info.Tool;
        public string ToolName => DamageType.name;
        public float Amount
        {
            get => info.Amount;
            set => info.Amount = value;
        }
    }
}