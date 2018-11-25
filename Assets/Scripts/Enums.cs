using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnumNamespace
{
   public enum Perks
    {
        BottlePoint,
        TacoIncreaseSpeed,
        BlockShooting,
        ReverseControls,
        GhostPerk
    }

    public enum Players
    {
        PlayerOne,
        PlayerTwo
    }

    public enum Objects
    {
        Taco,
        Cactus,
        Bullet,
        Points
    }

    public enum Tags
    {
        Ground,
        Perk,
        Destroyable,
        Cactus,
        Wall,
        Ramp
    }

    public enum Axis
    {
        LeftRightPadOne,
        ForwardBackwardPadOne,
        LeftRightPadTwo,
        ForwardBackwardPadTwo,
        PrimaryAttackOne,
        PrimaryAttackTwo
    }

    public enum Floor
    {
        BasicFloor,
        CrossHoleFloor,
        BigCrossHoleFloor,
        SatelliteHoleFloor,
        SingleHorizontalSlidingFloor,
        DoubleHorizontalSlidingFloor,
        DoubleBasicFloor,        
    }

}
