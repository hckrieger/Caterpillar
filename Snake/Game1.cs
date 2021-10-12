using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Scenes;

namespace Snake
{
    public class Game1 : ExtendedGame
    {
        public const string MAIN = "MAIN_SCENE";
        public const string START = "START_SCREEN";

        public Game1()
        {
            IsMouseVisible = true;
            windowSize = new Point(320, 420);
            worldSize = new Point(320, 420);
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            GameStateManager.AddGameState(MAIN, new MainScene());
            //GameStateManager.SwitchTo(MAIN);

            GameStateManager.AddGameState(START, new StartScreen());
            GameStateManager.SwitchTo(START);
        }

        

    }
}
