using Engine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class GoalPiece : SpriteGameObject
    {
        float count = .35f;
        float startCount;
        public GoalPiece() : base("Blocks@3x1", 1f, 2)
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
