using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

using System.Collections.Generic;
using System;

namespace OptionalProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // game state
        GameState gameState = GameState.Menu;

        // Increment 1: opening screen fields
        // declare resolution of screen
        const int windowWidth = 800;
        const int windowHeight = 600;

        // declare for the opening screen image
        Texture2D openingImage;
        Rectangle drawRectangle;

        // Increment 2: board field
        NumberBoard numberboard;

        // Increment 5: random field
        Random rand = new Random();

        // Increment 5: new game sound effect field
        SoundEffect newGameSound;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Increment 1: set window resolution and make mouse visible
            //set the resolution of screen and mouse visible
            graphics.PreferredBackBufferWidth = windowWidth;
            graphics.PreferredBackBufferHeight = windowHeight;
            IsMouseVisible = true;
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

            // Increment 1: load opening screen and set opening screen draw rectangle
            openingImage = Content.Load<Texture2D>(@"graphics\openingscreen");
            drawRectangle = new Rectangle(0, 0, openingImage.Width, openingImage.Height);

            // Increment 2: create the board object (this will be moved before you're done with the project)
            //variation for numberboard position
            //inrement 5: we'll cut the code at below
            //int boardSideLength = windowHeight - 50;
            //int boardSideCenterX = windowWidth / 2;
            //int boardSideCenterY = windowHeight / 2;
            //Vector2 boardToCenter = new Vector2(boardSideCenterX, boardSideCenterY);

            ////create the numberboard center of the screen
            //numberboard = new NumberBoard(Content, boardToCenter, boardSideLength, 8);


            StartGame();
            // Increment 5: load new game sound effect
            newGameSound = Content.Load<SoundEffect>(@"sounds\applause");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //get keyboard
            KeyboardState keyboard = Keyboard.GetState();

            // Increment 2: change game state if game state is GameState.Menu and user presses Enter
            if (keyboard.IsKeyDown(Keys.Enter)  && gameState == GameState.Menu)
            {
               keyboard.IsKeyUp(Keys.Enter);
               gameState = GameState.Play;
               newGameSound.Play();
            }
            // Increment 4: if we're actually playing, update mouse state and update board
            //if guess correct then numberBoard will update
            //for this we need declare boolen variable then we'll put the numberboard in the boolean
            //if it's true numberboard get update
            bool checkCorrectNumber = false;
            if (gameState == GameState.Play)
            {
                MouseState mouse = Mouse.GetState();
                checkCorrectNumber = numberboard.Update(gameTime, mouse);
                if (checkCorrectNumber)
                {
                    //numberboard.Update(gameTime, mouse);
                    newGameSound.Play();
                    StartGame();
                    
                }
                
            }
            // Increment 5: check for correct guess
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // Increments 1 and 2: draw appropriate items here
            spriteBatch.Begin();
            if (gameState == GameState.Menu)
            {
                spriteBatch.Draw(openingImage, drawRectangle, Color.White);
            }
            else
             {
                numberboard.Draw(spriteBatch);
             }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        /// <summary>
        /// Starts a game
        /// </summary>
        void StartGame()
        {
            // Increment 5: randomly generate new number for game
            int correctNumber = rand.Next(1, 10);
            
            // Increment 5: create the board object
             int boardSideLength = windowHeight - 50;
            int boardSideCenterX = windowWidth / 2;
            int boardSideCenterY = windowHeight / 2;
            Vector2 boardToCenter = new Vector2(boardSideCenterX, boardSideCenterY);

            //create the numberboard center of the screen
            numberboard = new NumberBoard(Content, boardToCenter, boardSideLength, correctNumber);
            
        }
    }
}
