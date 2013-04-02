using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GTD.BO;

namespace GTD.BOL
{
    /* The EnemyManager is responsible for managing the list of enemies that
     * is contains.
     */
    public class EnemyManager
    {
        public List<Enemy> enemies;

        public EnemyManager()
        {
            enemies = new List<Enemy>();
            PathFinder.Instance.FindPath(new Point(0, 6), new Point(25, 6));
            AddEnemyTest();


        }

        public void AddEnemyTest()
        {
            Enemy e = new AnimatedEnemy("BODY_skeleton", 1, 3, 0, 2, 48, 48, .05f);

            
            
            e.Position = new Vector2(0, 6 * Constants.MAP_CELL_HEIGHT + Map.Y_OFFSET + e.OffsetY());
            e.Speed = 150f;
            e.CurrentWaypoint = PathFinder.Instance.Path[0] + new Vector2(e.OffsetX(), e.OffsetY());
            enemies.Add(e);
        }

        public void Update(GameTime gameTime)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
                
                float WAYPOINT_CLAMP = MathHelper.Lerp(Constants.WAYPOINT_DISTANCE_CLAMP_MIN,
                    Constants.WAYPOINT_DISTANCE_CLAMP_MAX, enemy.Speed / Constants.MAX_ENEMY_MOVESPEED);

                if ((enemy.Position - enemy.CurrentWaypoint).Length() < WAYPOINT_CLAMP)
                {
                    enemy.Position = enemy.CurrentWaypoint;

                    /* check to ensure we're not at the end */
                    if (enemy.CurrentWaypointNumber < PathFinder.Instance.Path.Count - 1)
                    {
                        /* increment the current waypoint # and assign a new current waypoint */
                        enemy.CurrentWaypointNumber++;
                        enemy.CurrentWaypoint = new Vector2(PathFinder.Instance.Path[enemy.CurrentWaypointNumber].X + enemy.OffsetX(),
                            PathFinder.Instance.Path[enemy.CurrentWaypointNumber].Y + enemy.OffsetY());
                    }
                    else
                    {
                        /* done */
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }
    }
}
