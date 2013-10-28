using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoulTrader.Input_System;

namespace SoulTrader.Souls
{
    public class GroundPoundSoul : Soul
    {
        public GroundPoundSoul(Player player) : base(player) { }

        protected override void UsePower()
        {
            if (InputSystem.Action2.isPressed())
            {
                player.GroundPound();
            }
        }
    }
}
