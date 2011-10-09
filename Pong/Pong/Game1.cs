using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Puck p;
        Pad leftPad;
        Pad rightPad;
        Texture2D background;

        SpriteFont font;
        Vector2 leftScorePos;
        Vector2 rightScorePos;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            background = Content.Load<Texture2D>("sprites//starfield");
            p = new Puck(Content, new Rectangle(0,0,this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));
            leftPad = new Pad(Content, new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height), true);
            rightPad = new Pad(Content, new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height), false);

            leftScorePos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width / 10, this.graphics.GraphicsDevice.Viewport.Height * 0.02f);
            rightScorePos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width - this.graphics.GraphicsDevice.Viewport.Width / 10, this.graphics.GraphicsDevice.Viewport.Height * 0.02f);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Lindsey");
            

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            rightPad.checkCollisionAndRebound(p);
            leftPad.checkCollisionAndRebound(p);

            rightPad.update(Mouse.GetState());
            leftPad.update(Keyboard.GetState());

            // Check to see if puck has left the arena
            int check = p.outOfBounds();
            if (check > 0 )
            {
                // Add score to the puck which won (1 for left edge, right puck scores)
                if (check == 1)
                    rightPad.score += 1;

                if (check == 2)
                    leftPad.score += 1;
                        
                p.reset();
            }
            // Move things
            p.update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0,0, background.Width, background.Height), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            // Draw objects
            leftPad.draw(spriteBatch);
            rightPad.draw(spriteBatch);
            p.draw(spriteBatch);

            // Draw UI
            spriteBatch.DrawString(font, leftPad.score.ToString(), leftScorePos, Color.WhiteSmoke); 
            spriteBatch.DrawString(font, rightPad.score.ToString(), rightScorePos, Color.WhiteSmoke); 

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
