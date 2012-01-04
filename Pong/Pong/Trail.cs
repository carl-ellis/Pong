using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    class Trail
    {
        private Texture2D Texture;
        public Vector2 Position;
        public Vector2 Velocity;
        public Vector2 Acc;
        public int ticks = 0;
        private float SPEED = 4.0f;
        private Puck p;

        /// <summary>
        /// Constructor for trail object - this follows the puck as a particle
        /// </summary>
        /// <param name="content">Content manager for loading texture</param>
        /// <param name="p">Puck to follow</param>
        public Trail(ContentManager content, Puck p)
        {
            this.Texture = content.Load<Texture2D>("sprites\\trail");
            this.p = p;
            this.Position = p.Position - p.Velocity;
            this.Position.X += (float)new Random().NextDouble() * p.Size.Width;
            this.Position.Y += (float)new Random().NextDouble() * p.Size.Height;
            this.Velocity = p.Velocity;
            this.Velocity.X += (float)new Random().NextDouble();
            this.Velocity.Y += (float)new Random().NextDouble();
            //this.Velocity *= this.p.Velocity;
        }

        /// <summary>
        /// Updates the trails position so it chases the puck.
        /// </summary>
        public void update()
        {
            //float vY = this.Position.Y - this.p.Position.Y - (float)new Random().NextDouble() * 2 - 1;
            //float vX = this.Position.X - this.p.Position.X - (float)new Random().NextDouble() * 2 - 1; 
            //this.Velocity = new Vector2(vX, vY) * this.SPEED;

            this.Acc = (this.p.Position - this.Position);
            this.Acc.Normalize();
            this.Acc *= 0.5f;
            this.Velocity += this.Acc;
            this.Velocity.Normalize();
            this.Velocity *= SPEED;

            this.Position += this.Velocity;

            this.ticks++;
        }

        /// <summary>
        /// Draws the trail
        /// </summary>
        /// <param name="sb">Sprite batch</param>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, new Rectangle(0,0, Texture.Width, Texture.Height), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        }
    }
}
