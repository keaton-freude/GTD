using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTD.BO
{
    public class Tower
    {
        public Vector2 Position;
        public Texture2D Texture;

        public Tower()
        {

        }

        public Tower(string contentName, Vector2 pos)
        {
            Texture = GTD.Game1.sharedContent.Load<Texture2D>(contentName);
            Position = pos;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
