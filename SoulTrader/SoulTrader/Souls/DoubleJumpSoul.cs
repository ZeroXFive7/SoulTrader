using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoulTrader.Input_System;

namespace SoulTrader.Souls
{
    public class DoubleJumpSoul : Soul
    {
        public DoubleJumpSoul(Player player) : base(player) { }

        protected override void UsePower()
        {
            if (InputSystem.Action1.isPressed())
            {
                player.AirJump();
            }
        }
    }
}
