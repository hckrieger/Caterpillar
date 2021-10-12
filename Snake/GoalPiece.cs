using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class GoalPiece : SpriteGameObject
    {
        float count = .25f;
        float startCount;
        public GoalPiece() : base("SnakeCircles@4x3", .6f, 4)
        {
            startCount = count;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            count -= dt;

            if (count <= 0)
            {
                Visible = !Visible;
                count = startCount;
            }
            
        }
    }
}
