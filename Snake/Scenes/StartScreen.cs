using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Scenes
{
    class StartScreen : GameState
    {
        TextGameObject title = new TextGameObject("title", 1f, Color.Green, TextGameObject.Alignment.Center);
        TextGameObject pressSpace = new TextGameObject("pressSpace", 1f, Color.White, TextGameObject.Alignment.Center);
        TextGameObject programmedBy = new TextGameObject("programmedBy", 1f, Color.White, TextGameObject.Alignment.Center);
        float timer, startTimer;

        public StartScreen()
        {
            
            title.LocalPosition = new Vector2(160, 50);
            title.Text = "Caterpillar";
            gameObjects.AddChild(title);

            pressSpace.LocalPosition = new Vector2(160, 210);
            pressSpace.Text = "Press Space to start";
            gameObjects.AddChild(pressSpace);

            programmedBy.LocalPosition = new Vector2(160, 345);
            programmedBy.Text = "Programmed by Hunter Krieger";
            gameObjects.AddChild(programmedBy);

            startTimer = .52f;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (timer <= 0)
            {
                pressSpace.Visible = !pressSpace.Visible;
                timer = startTimer;
            }

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);
            if (inputHelper.KeyPressed(Keys.Space))
                ExtendedGame.GameStateManager.SwitchTo(Game1.MAIN);
        }
    }
}
