using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTD.BO
{
    /* The Map class defines the grid on which enemies may move and includes
     * obstacles in the form of setting cell.walkable = false
     * It's simply a reference and provides no logic on its own
     */
    public class Map
    {
        public int width;
        public int height;

        public const int Y_OFFSET = 48;
        public const int X_OFFSET = 0;

        public Cell[,] map;

        public Texture2D squareTexture;

        public Map()
        {
        }

        public Map(int w, int h)
        {
            width = w;
            height = h;
            
            map = new Cell[w, h];

            for (int i = 0; i < w; ++i)
            {
                for (int j = 0; j < h; ++j)
                {
                    map[i, j] = new Cell(i, j);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell c in map)
            {
                if (c.bestPath)
                {
                    //draw black
                    spriteBatch.Draw(squareTexture, new Vector2(c.x * 48 + X_OFFSET, c.y * 48 + Y_OFFSET), Color.Black);
                }
                else
                {
                    if (c.walkable)
                    {
                        //draw green
                        spriteBatch.Draw(squareTexture, new Vector2(c.x * 48 + X_OFFSET,  c.y * 48 + Y_OFFSET), Color.White);
                    }
                    else
                    {
                        //draw red
                        spriteBatch.Draw(squareTexture, new Vector2(c.x * 48 + X_OFFSET, c.y * 48 + Y_OFFSET), Color.Red);
                    }
                }
            }
        }
    }
}
