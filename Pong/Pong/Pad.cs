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
    class Pad
    {
        private Texture2D Texture;
        private Rectangle Size;
        private float Scale = 1.0f;
        private Vector2 Position;
        private Vector2 Velocity;
        private Rectangle Arena;
        private bool isLeft;
        private float OFFSET = 0.05f;
        private float SPEED = 10.0f;
        public BoundingBox bbox;
        private bool rebounding = false;
        public int score = 0;
        // For working out angle change on collisions


        public Pad(ContentManager content, Rectangle arena, Boolean isLeft)
        {
            Texture = content.Load<Texture2D>("sprites\\pad");
            Size = new Rectangle(0, 0, (int)(Texture.Width * Scale), (int)(Texture.Height * Scale));
            

            this.Arena = arena;
            this.isLeft = isLeft;
            
            this.reset(isLeft);
            
        }

        /// <summary>
        /// Draws the sprite, assumes sprite batch has begun
        /// </summary>
        /// <param name="sb">sprite batch</param>
        public void draw(SpriteBatch sb)
        {
            sb.Draw(Texture, Position, new Rectangle(0, 0, Texture.Width, Texture.Height), Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Resets pads position at centre of screen
        /// </summary>
        /// <param name="isLeft"></param>
        private void reset(bool isLeft)
        {
            // place pads at a given offset from the edge
            float offset = this.Arena.Width * OFFSET;
            if (isLeft)
            {
                this.Position = new Vector2(offset, this.Arena.Height / 2 - this.Size.Height / 2);
            }
            else
            {
                this.Position = new Vector2(this.Arena.Width - offset, this.Arena.Height / 2 - this.Size.Height / 2);
            }
            this.updateBBox();
        }

        /// <summary>
        /// Control when using the mouse
        /// </summary>
        /// <param name="mouseState">Mouse state</param>
        internal void update(MouseState mouseState)
        {
            if (mouseState.Y >= 0 && mouseState.Y <= Arena.Height - Size.Height)
                this.Position.Y = mouseState.Y;
            this.updateBBox();
        }

        /// <summary>
        /// Control when using the keyboard
        /// </summary>
        /// <param name="keyboardState"></param>
        internal void update(KeyboardState keyboardState)
        {
            // Get movement offset based on keypress
            float moveBy = 0.0f;
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                moveBy = SPEED;
            }
            else if (keyboardState.IsKeyDown(Keys.Up))
            {
                moveBy = -SPEED;
            }
                        
            // Calculate new position
            Vector2 newPosition = Position;
            newPosition.Y += moveBy;

            // If within the arena, move there
            if (newPosition.Y >= 0 && newPosition.Y <= Arena.Height - Size.Height)
                Position = newPosition;
            this.updateBBox();
        }

        //Updates the bounding box
        private void updateBBox()
        {
            if (isLeft)
            {
                this.bbox = new BoundingBox(new Vector3(Position + new Vector2(Size.Width-1, 0), 0), new Vector3(Position + new Vector2(Size.Width+1, Size.Height), 0));
            }
            else
            {
                this.bbox = new BoundingBox(new Vector3(Position - new Vector2(1.0f,0), 0), new Vector3(Position + new Vector2(Size.Width+1, Size.Height), 0));
            }
            
        }

        /// <summary>
        /// Check to see if the puck has collided with the pad, if so, reverse the pucks direction
        /// </summary>
        /// <param name="p">Puck object</param>
        public void checkCollisionAndRebound(Puck p)
        {
            if (this.bbox.Intersects(p.bbox))
            {
                if (!rebounding)
                {
                    float diff = Math.Abs(p.Position.Y - this.Position.Y);
                    float fulldiff = this.Size.Height;
                    float speed = (((diff / fulldiff)) * p.SPEED) - p.SPEED / 2;
                    p.flipX();
                    p.setAngle(speed);
                    rebounding = true;
                }
            }
            else
            {
                rebounding = false;
            }
        }
    }
}
