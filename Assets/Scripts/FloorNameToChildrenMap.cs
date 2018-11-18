using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumNamespace;

namespace Assets.Scripts
{
    public static class StaticMappings
    {
        public static Dictionary<string, List<Tuple<Floor, int>>> FloorNameToChildrenMap = new Dictionary<string, List<Tuple<Floor, int>>>
        {
            {"BasicFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
                new Tuple<Floor, int>(Floor.BigCrossHoleFloor, 1),
                new Tuple<Floor, int>(Floor.CrossHoleFloor, 1),
                new Tuple<Floor, int>(Floor.DoubleBasicFloor, 0),
                new Tuple<Floor, int>(Floor.DoubleHorizontalSlidingFloor, 0),
                new Tuple<Floor, int>(Floor.SatelliteHoleFloor, 1),
                new Tuple<Floor, int>(Floor.SingleHorizontalSlidingFloor, 0),
            } },
            {"CrossHoleFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
            } },
            {"BigCrossHoleFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
            } },
            {"SatelliteHoleFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
            } },
            {"SingleHorizontalSlidingFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
            } },
            {"DoubleHorizontalSlidingFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
                new Tuple<Floor, int>(Floor.DoubleHorizontalSlidingFloor, 0)
            } },
            {"DoubleBasicFloor", new List<Tuple<Floor,int>>
            {
                new Tuple<Floor, int>(Floor.BasicFloor, 1),
                new Tuple<Floor, int>(Floor.BigCrossHoleFloor, 1),
                new Tuple<Floor, int>(Floor.DoubleBasicFloor, 0),
                new Tuple<Floor, int>(Floor.DoubleHorizontalSlidingFloor, 0),
                new Tuple<Floor, int>(Floor.SatelliteHoleFloor, 1),
                new Tuple<Floor, int>(Floor.SingleHorizontalSlidingFloor, 0),
            } },
        };
    }
}


