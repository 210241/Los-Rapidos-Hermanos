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
        public int Length { get; set; }

        public Floor FloorType { get; set; }

        public FloorData(Transform floor, int length, Floor floorType)
        {
            FloorTransform = floor;
            Length = length;
            FloorType = floorType;
        }
    }
}
