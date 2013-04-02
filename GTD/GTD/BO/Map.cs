using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTD.BO
{
    public class Map
    {
        public int width;
        public int height;

        public Cell[,] map;

        public Texture2D squareTexture;

        public Map()
        {
        }

        public Map(int w, int h)
        {
            width = w;
            height = h - 1;
            h = h - 1;
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
                    spriteBatch.Draw(squareTexture, new Vector2(c.x * 48, 48 + c.y * 48), Color.Black);
                }
                else
                {
                    if (c.walkable)
                    {
                        //draw green
                        spriteBatch.Draw(squareTexture, new Vector2(c.x * 48,  48 + c.y * 48), Color.Green);
                    }
                    else
                    {
                        //draw red
                        spriteBatch.Draw(squareTexture, new Vector2(c.x * 48, 48 + c.y * 48), Color.Red);
                    }
                }
            }
        }
    }
}
