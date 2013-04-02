using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Serialization;
using GTD.BO;

public class Enemy
{
    public float Speed
    {
        get;
        set;
    }

    public int HP
    {
        get;
        set;
    }

    public int Defense
    {
        get;
        set;
    }

    public string ContentName
    {
        get;
        set;
    }

    public Texture2D Texture
    {
        get;
        set;
    }

    [XmlIgnore]
    public Vector2 CurrentWaypoint
    {
        get;
        set;
    }

    [XmlIgnore]
    public Vector2 Position
    {
        get;
        set;
    }

    public int CurrentWaypointNumber
    {
        get;
        set;
    }

    public bool Snapped
    {
        get;
        set;
    }

    public Enemy(string contentName)
    {
        this.Texture = GTD.Game1.sharedContent.Load<Texture2D>(contentName);
        
    }

    public Enemy Clone()
    {
        /* returns a new copy of this Enemy */
        return this.MemberwiseClone() as Enemy;
    }

    public virtual void Update(GameTime gameTime)
    {
        Vector2 direction = CurrentWaypoint - Position;
        if (direction.Length() != 0f)
            direction.Normalize();
        Position += direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Texture, Position, Color.Blue);
    }

    public virtual float OffsetX()
    {
        int difference = (int)Constants.MAP_CELL_WIDTH - Texture.Width;

        return difference / 2;
    }

    public virtual float OffsetY()
    {
        int difference = (int)Constants.MAP_CELL_HEIGHT - Texture.Height;

        return difference / 2;
    }
}