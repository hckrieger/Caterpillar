using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Snake.Scenes;
using System;
using System.Collections.Generic;
using System.Text;
using static Snake.Scenes.MainScene;

namespace Snake
{
    class Snake : GameObject
    {
        List<SnakeSection> snakeSections;





        public Snake()
        {
            snakeSections = new List<SnakeSection>();
        }



        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            
            foreach (SnakeSection obj in snakeSections)
            {

                obj.Draw(gameTime, spriteBatch);

                for (int i = 0; i < snakeSections.Count; i++)
                {
                    if (MainScene.CurrentDirection == MainScene.Direction.Up)
                    {
                        snakeSections[0].SheetIndex = 0;
                    }

                    if (MainScene.CurrentDirection == MainScene.Direction.Right)
                    {
                        snakeSections[0].SheetIndex = 1;
                    }

                    if (MainScene.CurrentDirection == MainScene.Direction.Down)
                    {
                        snakeSections[0].SheetIndex = 2;
                    }

                    if (MainScene.CurrentDirection == MainScene.Direction.Left)
                    {
                        snakeSections[0].SheetIndex = 3;
                    }

                }

            }
        }


        public MainScene MainScene {
            get
            {
                return (MainScene)ExtendedGame.GameStateManager.GetGameState(Game1.MAIN);
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

        public SnakeSection() : base("SnakeCircles@4x3", .9f, 0)
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