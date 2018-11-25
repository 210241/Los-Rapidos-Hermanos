using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using EnumNamespace;

namespace Assets.Scripts
{
    public class FloorData 
    {
        public Transform FloorTransform { get; set; }
        public float Length { get; set; }

        public float Height { get; set; }

        public Floor FloorType { get; set; }

        public Rotation Rotation { get; set; }

        public Vector3 PositionOffset { get; set; }

        public FloorData(Transform floor, float length, Floor floorType, Rotation rotation, float height = 0.1f)
        {
            FloorTransform = floor;
            Length = length;
            FloorType = floorType;
            Height = height;
            Rotation = rotation;
            PositionOffset = new Vector3(0,0,0);
        }

        public FloorData(Transform floor, float length, Floor floorType, Rotation rotation, Vector3 positionOffset, float height = 0.1f)
        {
            FloorTransform = floor;
            Length = length;
            FloorType = floorType;
            Height = height;
            Rotation = rotation;
            PositionOffset = positionOffset;
        }
    }
}
