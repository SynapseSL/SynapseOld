﻿using System;
using System.Linq;
using UnityEngine;

namespace Synapse.Api
{
    public class MapPoint
    {
        /// <summary>
        /// Tries to Parse a string to a MapPoint
        /// </summary>
        /// <param name="mappointstring">The string you try to parse</param>
        /// <param name="mapPoint">The MapPoint you Parse</param>
        /// <returns>If the Parsing was sucesfully and mapPoint is not null</returns>
        public static bool TryParse(string mappointstring, out MapPoint mapPoint)
        {
            try
            {
                mapPoint = Parse(mappointstring);
                return true;
            }
            catch
            {
                mapPoint = null;
                return false;
            }
        }

        /// <summary>
        /// Parses a string to a MapPoint
        /// </summary>
        /// <param name="mappointstring">The String you want to Parse</param>
        /// <returns>The MapPoint which was parsed</returns>
        public static MapPoint Parse(string mappointstring) => new MapPoint(mappointstring);

        /// <summary>
        /// Creates a MapPoint 
        /// </summary>
        /// <param name="room">The Room the MapPoint is realtive too</param>
        /// <param name="position">The Position you want to get the MapPoint of</param>
        public MapPoint(Room room, Vector3 position)
        {
            if (position == null) throw new ArgumentNullException("position", "The Argument position of the Constructor MapPoint(Room room,Vector3 position) is null");
            if (room == null) throw new ArgumentNullException("room", "The Argument Room of the Constructor MapPoint(Room room,Vector3 position) is null");

            Room = room;
            RelativePosition = Room.Transform.InverseTransformPoint(position);
        }

        /// <summary>
        /// Creates a MapPoint
        /// </summary>
        /// <param name="mappointstring">The String from which you want to create a MapPoint of</param>
        public MapPoint(string mappointstring)
        {
            string[] args = mappointstring.Split(':');
            if (args.Count() < 4) throw new IndexOutOfRangeException("Parsing of string to MapPoint failed because there was missing informations!He need to look like this: \"Roomname:1,434:-2,346456:1,6554\"");
            var room = Map.Rooms.FirstOrDefault(r => r.Name.ToLower() == args[0].ToLower());
            if (room == null) throw new Exception("Parsing of string to MapPoint failed because of the roomname");

            if (!float.TryParse(args[1], out var x)) throw new Exception("Parsing of string to MapPoint failed because of the Relative x Position!");
            if (!float.TryParse(args[2], out var y)) throw new Exception("Parsing of string to MapPoint failed because of the Relative y Position!");
            if (!float.TryParse(args[3], out var z)) throw new Exception("Parsing of string to MapPoint failed because of the Relative z Position!");

            Room = room;
            RelativePosition = new Vector3(x, y, z);
        }


        /// <summary>
        /// The Room of which the MapPoint is relative too
        /// </summary>
        public readonly Room Room;

        /// <summary>
        /// The Relative Position of the MapPoint to the Room
        /// </summary>
        public readonly Vector3 RelativePosition;

        /// <summary>
        /// The Calculated end Position on the Map
        /// </summary>
        public Vector3 Position { get => Room.Transform.TransformPoint(RelativePosition); }

        /// <summary>
        /// The MapPoint as a String
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"{Room.Name}:{RelativePosition.x}:{RelativePosition.y}:{RelativePosition.z}";
    }
}
