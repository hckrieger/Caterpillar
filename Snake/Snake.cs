using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake
{
    class Snake : GameObject
    {
        List<SnakeSection> snakeSections;
        
        public Snake()
        {
            snakeSections = new List<SnakeSection>();

            foreach (SnakeSection obj in snakeSections)
            {
                obj.debugText.Parent = obj;
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            for (int i = 0; i < snakeSections.Count - 1; i++)
            {
                snakeSections[i].debugText.LocalPosition = snakeSections[i].CurrentPosition + new Vector2(16, 8);
                snakeSections[i].debugText.Text = $"{i}";
            }
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);

            foreach (SnakeSection obj in snakeSections)
            {

                obj.Draw(gameTime, spriteBatch);
                obj.debugText.Draw(gameTime, spriteBatch);
            }
                
        }

    

        public List<SnakeSection> SnakeSections
        {
            get { return snakeSections; }
            set { snakeSections = value; }
        }

    }

    class SnakeSection : SpriteGameObject
    {

        Vector2 previousPosition;
        public TextGameObject debugText = new TextGameObject("debug", 1f, Color.Black, TextGameObject.Alignment.Center);

        public SnakeSection() : base("Blocks@3x1", .9f, 2)
        {
        }

  

        public Vector2 CurrentPosition
        {
            get { return LocalPosition; }
            set { LocalPosition = value; }
        }

        public Vector2 PreviousPosition
        {
            get { return previousPosition; }
            set { previousPosition = value; }
        }
    }


}
