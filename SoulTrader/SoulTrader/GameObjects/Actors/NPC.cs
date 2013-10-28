using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using SoulTrader.GameObjects.Actors;
using SoulTrader.GameObjects.Actors.Projectiles;
using SoulTrader.SceneSystem;

namespace SoulTrader
{
    public class NPC : Actor
    {
        public Soul Soul { get { return soul; } }
        private Soul soul = null;

        public int Wallet { get { return wallet; } }
        private int wallet = 0;

        public int Cost { get { return cost; } }
        private int cost = 0;

        public NPC(string spriteName, Soul soul, int cost, Vector2 initialPosition, Vector2 size)
            : base(spriteName, initialPosition, size)
        {
            this.soul = soul;
            this.cost = cost;
        }

        protected override bool OnGameObjectCollision(GameObject collider)
        {
            if (collider is MoneyBall)
            {
                if (soul == null)
                {
                    return true;
                }

                MoneyBall moneyBall = collider as MoneyBall;

                if (Wallet < Cost)
                {
                    wallet += moneyBall.Value;
                    ActiveScene.Scene.RemoveAndUninitialize(collider);
                }
                if (Wallet >= Cost)
                {
                    moneyBall.Parent.AddSoul(this.soul);
                    this.soul = null;

                    ActiveScene.Scene.RemoveAndUninitialize(this);
                    ActiveScene.Scene.AddAndInitialize(new Husk("black", this.worldPosition, this.worldScale));
                }

                return true;
            }

            return base.OnGameObjectCollision(collider);
        }
    }
}
