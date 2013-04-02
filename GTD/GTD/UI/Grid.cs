using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Grid
{
    Texture2D dummyTexture;
    Color color;

    public Grid()
    {
    }

    public Grid(GraphicsDevice g, Color c)
    {
        color = c;
        dummyTexture = new Texture2D(g, 1, 1);
        dummyTexture.SetData(new Color[] { Color.White });
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i <= 1248; i += 48)
        {
            spriteBatch.Draw(dummyTexture, new Rectangle(i, 48, 1, 720), color);
        }

        for (int i = 48; i <= 720; i += 48)
        {
            spriteBatch.Draw(dummyTexture, new Rectangle(0, i, 1280, 1), color);
        }
    }
}