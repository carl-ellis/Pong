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
        public int ticks = 0;
        private float drop = 1.2f;
        private Puck p;

        /// <summary>
        /// Constructor for trail object - this follows the puck as a particle
        /// </summary>
        /// <param name="content">Content manager for loading texture</param>
        /// <param name="p">Puck to follow</param>
        public Trail(ContentManager content, Puck p)
        {
            this.Texture = content.Load<Texture2D>("sprites\\puck");
            this.p = p;
            this.drop = (float)(new Random().NextDouble()) * 0.4f + 0.8f; 
            this.Position = p.Position;
        }

        /// <summary>
        /// Updates the trails position so it chases the puck.
        /// </summary>
        public void update()
        {
            this.Position = this.p.Position - (this.p.Velocity * drop);

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
