using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SoulTrader
{
    public class Player : Actor
    {
        private Camera camera;

        public int Money { get { return this.money; } }
        private int money = 1000;

        private int deathPenalty = 400;
        private float runSpeed = 75.0f;
        private float jumpHeight = 10.0f;

        private bool jumped = false;

        public Player(Camera camera, string spriteName, Vector2 initialPosition, Vector2 size) 
            : base(spriteName, initialPosition, size)
        {
            this.camera = camera;
        }

        public override void Update(TimeSpan timeSinceLastFrame)
        {
            camera.Update(CenterPosition);

            KeyboardState keyState = Keyboard.GetState();
            float horizontalMovement = (Convert.ToSingle(keyState.IsKeyDown(Keys.D)) - Convert.ToSingle(keyState.IsKeyDown(Keys.A))) * (float)timeSinceLastFrame.TotalSeconds * runSpeed;
            velocity += new Vector2(horizontalMovement, 0.0f);

            if (onGround && keyState.IsKeyDown(Keys.Space))
            {
                velocity += new Vector2(0.0f, jumpHeight);
                onGround = false;
            }

            base.Update(timeSinceLastFrame);
        }

        public override void HandleCollisions()
        {
            base.HandleCollisions();
        }

        protected override void Respawn()
        {
            money -= deathPenalty ;
            base.Respawn();
        }
    }
}
