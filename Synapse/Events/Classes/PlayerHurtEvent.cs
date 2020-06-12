using Synapse.Api;
using System.Diagnostics.CodeAnalysis;

namespace Synapse.Events.Classes
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class PlayerHurtEvent
    {
        // Variables
        private DamageTypes.DamageType _damageType;
        private PlayerStats.HitInfo _info;

        // Properties
        public Player Player { get; internal set; }
        public Player Attacker { get; internal set; }

        public PlayerStats.HitInfo Info
        {
            get => _info;
            set
            {
                _damageType = DamageTypes.None;
                _info = value;
            }
        }

        public DamageTypes.DamageType DamageType
        {
            get
            {
                if (_damageType == DamageTypes.None)
                    _damageType = DamageTypes.FromIndex(_info.Tool);

                return _damageType;
            }
        }

        public int Tool => _info.Tool;
        public string ToolName => DamageType.name;

        public float Amount
        {
            get => _info.Amount;
            set => _info.Amount = value;
        }
    }
}