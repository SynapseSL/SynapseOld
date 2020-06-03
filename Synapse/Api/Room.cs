using UnityEngine;

namespace Synapse.Api
{
    public class Room
    {
        private ZoneType _zone = ZoneType.Unspecified;
        public string Name { get; set; }

        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public Transform Transform { get; set; }
        public Vector3 Position { get; set; }

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