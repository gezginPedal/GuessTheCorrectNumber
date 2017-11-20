using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OptionalProject
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    class NumberTile
    {
        #region Fields
        // original length of each side of the tile
        int originalSideLength;

        // whether or not this tile is the correct number
        bool isCorrectNumber;

        // drawing support
        Texture2D texture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        // Increment 5: field for blinking tile texture
        Texture2D blinkTile;

        // Increment 5: field for current texture
        Texture2D currentTexture;

        // blinking support
        const int TotalBlinkMilliseconds = 4000;
        int elapsedBlinkMilliseconds = 0;
        const int TotalFrameMilliseconds = 1000;
        int elapsedFrameMilliseconds = 0;

        // Increment 4: fields for shrinking support
        const int TotalShrinkMilliseconds = 5000;
        int elapsedShrinkMilliseconds = 0;

        // Increment 4: fields to keep track of visible, blinking, and shrinking
        bool tileVisible = true;        
        bool tileBlink;
        bool tileShrink = false;

        // Increment 4: fields for click support
        bool clickStarted = false;
        bool buttonReleased = true;
        // Increment 5: sound effect field
        SoundEffect correctOrIncorrect;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
        public NumberTile(ContentManager contentManager, Vector2 center, int sideLength,
            int number, int correctNumber)//, SoundEffect sound)
        {
            // set original side length field
            this.originalSideLength = sideLength;

            // load content for the tile and create draw rectangle
            LoadContent(contentManager, number);
            drawRectangle = new Rectangle((int)center.X - sideLength / 2,
                 (int)center.Y - sideLength / 2, sideLength, sideLength);

            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;

            // Increment 5: load sound effect field to correct or incorrect sound effect
            // based on whether or not this tile is the correct number
           // correctOrIncorrect = sound;
            if(number == correctNumber)
            {
                isCorrectNumber = true;
                correctOrIncorrect = contentManager.Load<SoundEffect>(@"sounds\explosion");
            }
            else
            {
                isCorrectNumber = false;
                correctOrIncorrect = contentManager.Load<SoundEffect>(@"sounds\loser");
            }




        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the tile based on game time and mouse state
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
            //// Increment 4: add code to highlight/unhighlight the tile
            //if(drawRectangle.Contains(mouse.X,mouse.Y) && clickStarted == true)
            //{
            //    sourceRectangle.X = texture.Width / 2;
            //}
            //if(drawRectangle.Contains(mouse.X, mouse.Y) && clickStarted == true)
            //{
            //    sourceRectangle.X = 0;
            //}

            // check for mouse over button
            //if (drawRectangle.Contains(mouse.X, mouse.Y))
            //{
            //    //change the color, while mouse over the number 
            //    sourceRectangle.X = texture.Width / 2;

            //    // mouse click
            //    if (mouse.LeftButton == ButtonState.Pressed &&
            //        buttonReleased)
            //    {

            //        clickStarted = true;
            //        buttonReleased = false;
            //    }
            //    else if (mouse.LeftButton == ButtonState.Released)
            //    {
            //        buttonReleased = true;

            //        //if click finished on button
            //        if (clickStarted == true)
            //        {
            //            // if click correct number
            //            if (isCorrectNumber == true)
            //            {
            //                tileBlink = true;
            //            }

            //            //if guess wrong set the tile shrink true;
            //            else
            //            {
            //                tileShrink = true;
            //            }
            //            clickStarted = false;
            //        }
            //    }
            //}
            //else
            //{
            //    sourceRectangle.X = 0;

            //    // no clicking on this button
            //    clickStarted = false;
            //    buttonReleased = false;
            //}
            
            
            // Increments 4 and 5: add code for shrinking and blinking support
            if (tileBlink)
            {
                elapsedBlinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedBlinkMilliseconds > TotalBlinkMilliseconds)
                {
                    elapsedBlinkMilliseconds = 0;
                    if (!tileVisible)
                    {

                        sourceRectangle.X = 0;
                    }

                }
                else
                {
                    elapsedFrameMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                    if (elapsedFrameMilliseconds > TotalFrameMilliseconds)
                    {
                        elapsedFrameMilliseconds = 0;
                        sourceRectangle.X = currentTexture.Width / 2;
                        //tileVisible was false at step 48
                        //return true won't blink the correct number. It will return to begin
                        return true;
                        
                    }
                }
            }
            else if (tileShrink == true)
            {
                //update the shrink animation 
                elapsedShrinkMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                //Calculate new tile side length ratio
                float newTileSideLength = (float)originalSideLength *
                    elapsedShrinkMilliseconds / TotalShrinkMilliseconds;



                if (drawRectangle.Width > 0)
                {
                    //shrink
                    drawRectangle.Height -= (int)newTileSideLength;
                    drawRectangle.Width -= (int)newTileSideLength;
                    //shrink to center
                    drawRectangle.X += (int)newTileSideLength / 2;
                    drawRectangle.Y += (int)newTileSideLength / 2;
                }

                
                else if(drawRectangle.Width < 0)
                {
                    tileVisible = false;
                    
                }
            }

           
            //highlight while shrink
            else
            {
                // check for mouse over button
                if (drawRectangle.Contains(mouse.X, mouse.Y))
                {
                    //change the color, while mouse over the number 
                    sourceRectangle.X = texture.Width / 2;

                    // mouse click
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        buttonReleased)
                    {
                        
                        clickStarted = true;
                        buttonReleased = false;
                    }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        buttonReleased = true;

                        //if click finished on button
                        if (clickStarted == true)
                        {
                            // if click correct number
                            if (isCorrectNumber == true)
                            {
                                tileBlink = true;
                                //color to blinking version
                                currentTexture = blinkTile;
                                sourceRectangle.X = 0;
                                correctOrIncorrect.Play();
                            }

                            //if guess wrong set the tile shrink true;
                            else
                            {
                                tileShrink = true;
                                correctOrIncorrect.Play();
                            }
                            clickStarted = false;
                        }
                    }
                }
                else
                {
                    sourceRectangle.X = 0;

                    // no clicking on this button
                    clickStarted = false;
                    buttonReleased = false;
                }
            }
            // Increment 5: play sound effect

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Increments 3, 4, and 5: draw the tile
            if (tileVisible == true)
            {
                spriteBatch.Draw(currentTexture, drawRectangle, sourceRectangle, Color.White);
            }
            
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number)
        {
            // convert the number to a string
            string numberString = ConvertIntToString(number);

            // Increment 3: load content for the tile and set source rectangle
            texture = contentManager.Load<Texture2D>(@"graphics\card\" + numberString);

            sourceRectangle = new Rectangle(drawRectangle.X, drawRectangle.Y, texture.Width /2 , texture.Height);

            // Increment 5: load blinking tile texture
            blinkTile = contentManager.Load<Texture2D>(@"graphics\blinking\blinking" + numberString);

            // Increment 5: set current texture
            currentTexture = texture;
        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }

        }

        #endregion
    }
}
