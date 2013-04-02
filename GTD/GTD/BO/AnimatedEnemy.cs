using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GTD.BO
{
    public class AnimatedEnemy: Enemy
    {
        public enum Direction
        {
            Left,
            Right,
            Up,
            Down
        }

        public int LeftRow;
        public int RightRow;
        public int UpRow;
        public int DownRow;

        private int NumberOfFramesPerRow;

        public float timeBetweenFrames;
        private float currentTime;

        private int FrameHeight;
        private int FrameWidth;

        private int currentFrame = 0;

        public Direction CurrentDirection;
        public Direction PreviousDirection;
        

        public AnimatedEnemy(string contentName, int leftRow, int rightRow, int upRow, int downRow, int frameWidth, int frameHeight, float timeBetweenFrames) : base(contentName)
        {
            LeftRow = leftRow;
            RightRow = rightRow;
            UpRow = upRow;
            DownRow = downRow;

            CurrentDirection = Direction.Right;
            PreviousDirection = CurrentDirection;

            FrameHeight = frameHeight;
            FrameWidth = frameWidth;

            NumberOfFramesPerRow = Texture.Width / frameWidth;

            this.timeBetweenFrames = timeBetweenFrames;
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentTime > timeBetweenFrames)
            {
                /* Switch frames */
                currentFrame++;

                if (currentFrame > NumberOfFramesPerRow - 1)
                {
                    currentFrame = 0;
                }

                currentTime = 0.0f;
            }

            base.Update(gameTime);
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            Rectangle drawRect = Rectangle.Empty;
            if (CurrentDirection == Direction.Right)
                drawRect = new Rectangle(currentFrame * FrameWidth, RightRow * FrameHeight, FrameWidth, FrameHeight);

            spriteBatch.Draw(Texture, Position, drawRect, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 1.0f);
        }

        public override float OffsetX()
        {
            float difference = Constants.MAP_CELL_WIDTH - FrameWidth;

            return difference / 2;
        }

        public override float OffsetY()
        {
            float difference = Constants.MAP_CELL_HEIGHT - FrameHeight;

            return difference / 2;
        }
    }
}
