using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Snake.Scenes;

namespace Snake
{
    public class Game1 : ExtendedGame
    {
        const string MAIN = "MAIN_SCENE";
        public Game1()
        {
            IsMouseVisible = true;
        }


        protected override void LoadContent()
        {
            base.LoadContent();
            GameStateManager.AddGameState(MAIN, new MainScene());
            GameStateManager.SwitchTo(MAIN);
        }

    }
}
