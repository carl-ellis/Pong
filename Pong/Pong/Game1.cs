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
        // Main game window 
        const int MAIN_GAME_STATE   = 1;
        // Screen when game has ended
        const int END_GAME_STATE    = 2;
        // Screen for loading a new ball
        const int NEW_BALL_STATE    = 3;
        // New game screen, logo and such
        const int NEW_GAME_STATE    = 4;
        // Pause game, with restart command
        const int PAUSE_GAME_STATE  = 5;

        // TIME FOR NEW BALL TO ENTER PLAY
        const int NEW_BALL_COUNTER = 3000;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Puck p;
        Pad leftPad;
        Pad rightPad;
        Texture2D background;
        Texture2D overlay;
        Texture2D winner;

        int ballsLeft;

        SpriteFont font;
        Vector2 leftScorePos;
        Vector2 rightScorePos;
        Vector2 ballsLeftPos;
        Vector2 winnerPos;
        Vector2 winnerTextPos;
        Vector2 ballTextPos;
        Vector2 ballTextLabelPos;

        // Game states
        int gamestate;

        // new ball calcs
        int counter;



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
            p = new Puck(Content, new Rectangle(0,0,this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height));
            leftPad = new Pad(Content, new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height), true);
            rightPad = new Pad(Content, new Rectangle(0, 0, this.graphics.GraphicsDevice.Viewport.Width, this.graphics.GraphicsDevice.Viewport.Height), false);

            leftScorePos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width / 10, this.graphics.GraphicsDevice.Viewport.Height * 0.02f);
            rightScorePos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width - this.graphics.GraphicsDevice.Viewport.Width / 10, this.graphics.GraphicsDevice.Viewport.Height * 0.02f);
            ballsLeftPos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width/2, this.graphics.GraphicsDevice.Viewport.Height * 0.02f);
            winnerPos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width/2 - 200, this.graphics.GraphicsDevice.Viewport.Height/2 - 100);
            winnerTextPos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width/2, this.graphics.GraphicsDevice.Viewport.Height/2);
            ballTextPos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width / 2, this.graphics.GraphicsDevice.Viewport.Height / 2 + 10);
            ballTextLabelPos = new Vector2(this.graphics.GraphicsDevice.Viewport.Width / 2, this.graphics.GraphicsDevice.Viewport.Height / 2 - 10);
            ballsLeft = 2;

            gamestate = MAIN_GAME_STATE;

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
            background = Content.Load<Texture2D>("sprites//starfield");
            overlay = Content.Load<Texture2D>("sprites//overlay");
            winner = Content.Load<Texture2D>("sprites//winner");
            

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

            switch(gamestate)
            {
                case MAIN_GAME_STATE:
                case NEW_BALL_STATE:
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
                            rightPad.score += Math.Abs((int)p.Velocity.X*10/4);
        
                        if (check == 2)
                            leftPad.score += Math.Abs((int)p.Velocity.X*10/4);
        
                        ballsLeft--;

                        if (ballsLeft <= 0)
                        {
                            gamestate = END_GAME_STATE;
                        }
                        else
                        {
                            // Enter state for new ball coming to the field
                            counter = NEW_BALL_COUNTER;
                            p.reset();
                            gamestate = NEW_BALL_STATE;
                        }
                    }
                    // Move things
                    if (gamestate == MAIN_GAME_STATE)
                    {
                        p.update();
                    }
                    else if (gamestate == NEW_BALL_STATE) 
                    {
                        // start the new ball counter
                        counter -= gameTime.ElapsedGameTime.Milliseconds;
                        if (counter <= 0)
                        {
                            gamestate = MAIN_GAME_STATE;
                        }
                     
                    }
                    break;
            }

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

            switch (gamestate)
            {
                case MAIN_GAME_STATE: 
                case END_GAME_STATE:
                case NEW_BALL_STATE:
                    spriteBatch.Draw(background, Vector2.Zero, new Rectangle(0,0, background.Width, background.Height), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
        
                    // Draw objects
                    leftPad.draw(spriteBatch);
                    rightPad.draw(spriteBatch);
                    if (gamestate == MAIN_GAME_STATE)
                        p.draw(spriteBatch);
        
                    // Draw UI
                    spriteBatch.DrawString(font, leftPad.score.ToString(), leftScorePos, Color.WhiteSmoke); 
                    spriteBatch.DrawString(font, rightPad.score.ToString(), rightScorePos, Color.WhiteSmoke); 
                    spriteBatch.DrawString(font, ballsLeft.ToString(), ballsLeftPos, Color.WhiteSmoke);

                    // if end game state put up the end credits
                    if (gamestate == END_GAME_STATE)
                    {
                        // Winner text
                        String winnerText = " has WON with ";
                        if (leftPad.score > rightPad.score)
                        {
                            winnerText = "Left " + winnerText + leftPad.score.ToString() + " points!";
                        }
                        else if (leftPad.score == rightPad.score)
                        {
                            winnerText = "EVERYONE " + winnerText + rightPad.score.ToString() + " points!";
                        }
                        else
                        {
                            winnerText = "Right " + winnerText + rightPad.score.ToString() + " points!";
                        }

                        //adjust text position based on text length... dodgey I know
                        winnerTextPos.X = this.graphics.GraphicsDevice.Viewport.Width/2 - (int)(winnerText.Length * 4.4);

                        spriteBatch.Draw(overlay, Vector2.Zero, new Rectangle(0,0, overlay.Width, overlay.Height), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.Draw(winner, winnerPos, new Rectangle(0,0, winner.Width, winner.Height), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font,  winnerText, winnerTextPos, Color.WhiteSmoke);
                    }
                    // if new ball, show counter
                    else if (gamestate == NEW_BALL_STATE)
                    {
                        // Ball text
                        String ballText = Math.Floor(((double)counter)/1000).ToString();
                        String ballLabelText = "NEW BALL IN:";

                        //adjust text position based on text length... dodgey I know
                        ballTextLabelPos.X = this.graphics.GraphicsDevice.Viewport.Width / 2 - (int)(ballLabelText.Length * 4.4);

                        spriteBatch.Draw(overlay, Vector2.Zero, new Rectangle(0,0, overlay.Width, overlay.Height), Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
                        spriteBatch.DrawString(font, ballLabelText, ballTextLabelPos, Color.WhiteSmoke);
                        spriteBatch.DrawString(font, ballText, ballTextPos, Color.WhiteSmoke);
                    }

                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
