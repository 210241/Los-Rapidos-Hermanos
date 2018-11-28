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
        Checkpoint,
        Ground,
        Perk,
        Destroyable,
        Cactus,
        Wall,
        HalfPipe,
        Ramp,
        SuperGriavity,
        LastHalfPipe,
        Teleport
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
