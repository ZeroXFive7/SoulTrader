using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoulTrader
{
    public abstract class Soul
    {
        protected Player player = null;

        public Soul()
        {
        }

        public Soul(Player player)
        {
            this.player = player;
        }

        public void RegisterPlayer(Player player)
        {
            this.player = player;
        }

        public void Update()
        {
            if (player == null)
            {
                return;
            }

            UsePower();
        }

        protected abstract void UsePower();
    }
}
