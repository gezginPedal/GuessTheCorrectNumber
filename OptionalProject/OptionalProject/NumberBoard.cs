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
    /// The board of number tiles to guess
    /// </remarks>
    class NumberBoard
    {
        #region Fields

        const int BorderSize = 8;
        const int NumColumns = 3;
        const int NumRows = NumColumns;

        // drawing support
        Texture2D boardTexture;
        Rectangle drawRectangle;

        // side length for each tile
        int tileSideLength;

        // tiles
        NumberTile[,] tiles = new NumberTile[NumRows, NumColumns];
        
        // sound effects

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the board</param>
        /// <param name="sideLength">the side length for the board</param>
        /// <param name="correctNumber">the correct number</param>
        public NumberBoard(ContentManager contentManager, Vector2 center, int sideLength, 
            int correctNumber)//, SoundEffect sound)
        {
            // Increment 2: load content for the board and create draw rectangle
            LoadContent(contentManager);
            drawRectangle = new Rectangle((int)center.X - sideLength / 2, 
                (int)center.Y - sideLength / 2,
                sideLength, sideLength);

            // Increment 2: calculate side length for number tiles
            // (3 * tilesLength) + ( 4 * borderSize) = sideLength
            // 3 = NumColumn
            // therefore (numColumn * tilesLength) + ((numColumn + 1) * borderSize) = sideLength
            //if we leave alone the tileSideLength then we'll found formula at below
            tileSideLength = ((sideLength) - ((NumColumns + 1) * BorderSize)) / NumColumns;

            // Increments 3 and 5: initialize array of number tiles
            
            for(int row = 0; row < NumRows; row++)
            {
                for(int column = 0; column < NumColumns; column++)
                {
                    int number = NumColumns * row + column + 1;
                    tiles[row, column] = new NumberTile(contentManager, CalculateTileCenter(row, column), tileSideLength, number, correctNumber);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the board based on the current mouse state. The only required action is to identify
        /// that the left mouse button has been clicked and update the state of the appropriate number
        /// tile.
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
            //result for check step 52
            //we created for boolean correct result
            //if result boolean turn to true, we have found the number correctly
            //for this. we create boolean and we added "tiles" to boolean variation
            //if boolean return to true, we found the number. if it be false, return to false

            bool result = false;

            // Increment 4: update all the number tiles
            for (int row = 0; row < NumRows; row++)
            {
                
                for (int col = 0; col < NumColumns; col++)
                {
                    result = tiles[row, col].Update(gameTime, mouse);
                    if (result)
                    {
                        return true;
                    }
                }
            }

            // Increment 5: return appropriate value for correct tile guessed
            

            // return false because the correct number wasn't guessed
            return false;
        }
       

        /// <summary>
        /// Draws the board
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Increment 2: draw the board
           spriteBatch.Draw(boardTexture, drawRectangle, Color.AliceBlue);
            
            // Increment 3: draw all the number tiles
            foreach(var tile in tiles)
            {
                tile.Draw(spriteBatch);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the board
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        private void LoadContent(ContentManager contentManager)
        {
            // Increment 2: load the background for the board
            boardTexture = contentManager.Load<Texture2D>(@"graphics\board");
            

        }

        /// <summary>
        /// Calculates the center of the tile at the given row and column
        /// </summary>
        /// <param name="row">the row in the array</param>
        /// <param name="column">the column in the array</param>
        /// <returns>the center of the tile in the given row and column</returns>
        private Vector2 CalculateTileCenter(int row, int column)
        {
            int upperLeftX = drawRectangle.X + (BorderSize * (column + 1)) + 
                tileSideLength * column;
            int upperLeftY = drawRectangle.Y + (BorderSize * (row + 1)) + 
                tileSideLength * row;
            return new Vector2(upperLeftX + tileSideLength / 2,
                upperLeftY + tileSideLength / 2);
        }

        #endregion
    }
}
