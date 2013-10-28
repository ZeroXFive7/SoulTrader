using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoulTrader.GameObjects.Actors;
using SoulTrader.GameObjects.Actors.Projectiles;
using SoulTrader.Input_System;
using SoulTrader.SceneSystem;
using SoulTrader.Souls;

namespace SoulTrader
{
    public class Player : Actor
    {
        private Camera camera;

        public int Money { get { return this.money; } }
        private int money = 1000;

        private int deathPenalty = 400;
        private float runSpeed = 75.0f;
        private float maxRunSpeed = 100.0f;
        private float jumpHeight = 10.0f;

        private Vector2 spawnPoint;

        private bool isPounding = false;
        private bool isAirJumping = false;

        public List<Soul> Portfolio { get { return portfolio; } }
        private List<Soul> portfolio = new List<Soul>();

        public Player(Camera camera, string spriteName, Vector2 initialPosition, Vector2 size) 
            : base(spriteName, initialPosition, size)
        {
            this.camera = camera;

            this.spawnPoint = initialPosition;

            //AddSoul(new GroundPoundSoul(this));
            //AddSoul(new DoubleJumpSoul(this));
        }

        public override void Update(TimeSpan timeSinceLastFrame)
        {
            camera.Update(CenterPosition, timeSinceLastFrame);

            foreach (Soul soul in portfolio)
            {
                soul.Update();
            }

            if (isPounding && onGround)
            {
                isPounding = false;
            }

            if (isAirJumping && onGround)
            {
                isAirJumping = false;
            }

            if (!isPounding)
            {
                float horizontalMovement = (Convert.ToSingle(InputSystem.RunRight.IsDown()) - Convert.ToSingle(InputSystem.RunLeft.IsDown())) * (float)timeSinceLastFrame.TotalSeconds * runSpeed;
                velocity += new Vector2(horizontalMovement, 0.0f);
                velocity.X = Math.Min(velocity.X, maxRunSpeed);
            }

            if (InputSystem.Action1.isPressed())
            {
                Jump();
            }

            if (InputSystem.Action3.isPressed())
            {
                int thrownGold = 50;
                money -= thrownGold;
                float direction = velocity.X > 0.0f ? 1.0f : -1.0f;
                ActiveScene.Scene.AddAndInitialize(new MoneyBall("gold", thrownGold, this, new Vector2(direction * 20.0f, 10.0f), this.CenterPosition, new Vector2(20.0f, 20.0f)));
            }

            base.Update(timeSinceLastFrame);
        }

        public void Jump()
        {
            if (onGround && velocity.Y <= 0.0f)
            {
                velocity += new Vector2(0.0f, jumpHeight);
                onGround = false;
            }
        }

        public void AirJump()
        {
            if (!onGround && !isAirJumping)
            {
                velocity.Y = jumpHeight;
                isAirJumping = true;
            }
        }

        public void GroundPound()
        {
            if (!onGround)
            {
                velocity = Vector2.Zero;
                velocity.Y -= 10;
                isPounding = true;
            }
        }

        protected override bool OnGameObjectCollision(GameObject collider)
        {
            if (collider is KillZone)
            {
                Respawn();
                return false;
            }
            else if (collider is MoneyBall)
            {
                MoneyBall moneyBall = collider as MoneyBall;
                if (moneyBall.HasLeftPlayer)
                {
                    this.money += moneyBall.Value;
                    ActiveScene.Scene.RemoveAndUninitialize(collider);
                }
                return true;
            }

            return base.OnGameObjectCollision(collider);
        }

        protected void Respawn()
        {
            money -= deathPenalty ;
            velocity = Vector2.Zero;
            UpdatePosition(spawnPoint);
        }

        public void AddSoul(Soul soul)
        {
            soul.RegisterPlayer(this);
            portfolio.Add(soul);
        }
    }
}
