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
    class Puck
    {
        private Texture2D Texture;
        public Rectangle Size;
        private float Scale = 1.0f;
        public Vector2 Position;
        private Vector2 Velocity;
        private Rectangle Arena;
        private bool bouncing = false;
        public float SPEED = 7.0f;
        public BoundingBox bbox;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="content">Content manager to load texture from</param>
        /// <param name="arena">Size of arena</param>
        public Puck(ContentManager content, Rectangle arena)
        {
            Texture = content.Load<Texture2D>("sprites\\puck");
            Size = new Rectangle(0, 0, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));

            this.Arena = arena;
            
            this.reset();
            this.changeVelocity();

            
        }

        /// <summary>
        /// Draws the sprite, assumes sprite batch has begun
        /// </summary>
        /// <param name="sb">sprite batch</param>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, new Rectangle(0,0, Texture.Width, Texture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Updates ouck position
        /// </summary>
        public void update()
        {
            Position += Velocity;
            this.updateBBox();
        }

        /// <summary>
        /// Checks if the puck has left the arena at the left or right
        /// </summary>
        /// <returns>0 for not out of bounds, 1 for left, 2 for right</returns>
        public int outOfBounds()
        {
            // Check the puck hasn't hit the top or bottom of the arena
            if (this.Position.Y <= this.Arena.Top || this.Position.Y + this.Size.Height >= this.Arena.Bottom)
            {
                if (!bouncing)
                {
                    // Bounce, may as well do this here
                    this.flipY();
                    bouncing = true;
                }
            }
            else
            {
                bouncing = false;
            }

            // Left arena at either end, requires game logic
            if (this.Position.X <= this.Arena.Left) 
                return 1;

            if ((this.Position.X + this.Size.Width) >= this.Arena.Right)
                return 2;

            return 0;
        }

        /// <summary>
        /// Gives a random Y velocity
        /// </summary>
        public void changeVelocity()
        {
            this.Velocity.Y = (float)(SPEED * ((new Random().NextDouble() * 2) - 1));
        }

        /// <summary>
        /// Flips the Y velocity
        /// </summary>
        public void flipY()
        {
            this.Velocity.Y *= -1;
        }
         
        /// <summary>
        /// Flips the X velcocity
        /// </summary>
        public void flipX()
        {
            this.Velocity.X *= -1.1f;
        }

        /// <summary>
        /// Changes the angle randomly
        /// </summary>
        public void AdjustAngle()
        {
            this.Velocity.Y += (((float)(new Random().NextDouble())*10) - 5);
        }

        /// <summary>
        /// Changes the angle a given ammount
        /// </summary>
        /// <param name="angle">angle diff</param>
        public void AdjustAngle(float angle)
        {
            this.Velocity.Y += angle;
        }

        /// <summary>
        /// Puts the puck centre and nrandomises speed and direction
        /// </summary>
        public void reset()
        {
            Position = new Vector2(Arena.Width / 2 - Size.Width / 2, Arena.Height / 2 - Size.Height / 2);
            this.Velocity = new Vector2(SPEED * (((new Random().NextDouble() * 2) - 1 < 0) ? -1 : 1));
            this.changeVelocity();
            this.AdjustAngle();
        }

        private void updateBBox()
        {
            this.bbox = new BoundingBox(new Vector3(Position, 0), new Vector3(Position + new Vector2(Size.Width, Size.Height), 0));
        }


        internal void setAngle(float speed)
        {
            this.Velocity.Y = speed;
        }
    }
}
