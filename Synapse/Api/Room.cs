using Synapse.Api.Enums;
using UnityEngine;

namespace Synapse.Api
{
    public class Room
    {
        private string name;
        private ZoneType _zone = ZoneType.Unspecified;
        /// <summary>
        /// The name of the room
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (value == "Root_*&*Outside Cams") name = "Outside";
                else name = value;
            }
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        /// <summary>
        /// The transform of the room
        /// </summary>
        public Transform Transform { get; set; }
        /// <summary>
        /// The position of the room
        /// </summary>
        public Vector3 Position { get; set; }

        // ReSharper disable once UnusedMember.Global
        /// <summary>
        /// The ZoneType in which the room is
        /// </summary>
        public ZoneType Zone
        {
            get
            {
                if (_zone != ZoneType.Unspecified)
                    return _zone;

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if (Position.y == -1997f)
                    _zone = ZoneType.Unspecified;

                else if (Position.y >= 0f && Position.y < 500f)
                    _zone = ZoneType.LightContainment;

                else if (Position.y < -100 && Position.y > -1000f)
                    _zone = ZoneType.HeavyContainment;

                else if (Name.Contains("ENT") || Name.Contains("INTERCOM"))
                    _zone = ZoneType.Entrance;

                else if (Position.y >= 5)
                    _zone = ZoneType.Surface;

                return _zone;
            }
        }
    }
}