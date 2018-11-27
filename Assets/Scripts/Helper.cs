using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnumNamespace;

namespace Assets.Scripts
{
    public class Helper
    {
        public static Tags StringToTags(string tag)
        {
            switch (tag)
            {
                case "Ground":
                    return Tags.Ground;
                case "Perk":
                    return Tags.Perk;
                case "Destroyable":
                    return Tags.Destroyable;
                case "Cactus":
                    return Tags.Cactus;
                case "Wall":
                    return Tags.Wall;
                default:
                    return Tags.Ground;
            }
        }
    }
}
