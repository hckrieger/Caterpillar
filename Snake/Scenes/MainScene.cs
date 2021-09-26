using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace Snake.Scenes
{
    class MainScene : GameState
    {
        const int ROWS = 17;
        const int COLS = 15;

        BlockSpace[,] grid;

        GoalPiece goalPiece;

        int xPos = 3;
        int yPos = 3;

        enum State
        {
            Playing,
            GameOver
        }

        enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            None,
        }

        float moveCount = .3f;
        float startCount;

        Direction currentDirection, impendingDirection;
        State currentState;

        Snake player;
        SnakeSection headOfSnake, tailOfSnake;

        List<Vector2> availableBlocks;
        TextGameObject debugCount;

        

        public MainScene()
        {
            goalPiece = new GoalPiece();
            gameObjects.AddChild(goalPiece);

            player = new Snake();
            gameObjects.AddChild(player);

            grid = new BlockSpace[ROWS, COLS];

            availableBlocks = new List<Vector2>();

            for (int x = 0; x < ROWS; x++)
            {
                for (int y = 0; y < COLS; y++)
                {
                    grid[x, y] = new BlockSpace(new Point(x, y));

                    if ((x + y) % 2 == 0)
                        grid[x, y].SheetIndex = 0;
                    else
                        grid[x, y].SheetIndex = 1;

                    grid[x, y].LocalPosition = new Vector2(x * 32, y * 32);
                    gameObjects.AddChild(grid[x, y]);

                    availableBlocks.Add(grid[x, y].LocalPosition);
                }
            }

            xPos = 1;
            yPos = 1;

            
            
            player.SnakeSections.Add(new SnakeSection());
            headOfSnake = player.SnakeSections[0];
            headOfSnake.LocalPosition = grid[xPos, yPos].LocalPosition;

            headOfSnake.PreviousPosition = headOfSnake.CurrentPosition;


            startCount = moveCount;

            impendingDirection = Direction.None;
            currentDirection = Direction.None;


            goalPiece.LocalPosition = grid[4, 4].LocalPosition;

            debugCount = new TextGameObject("debug", 1f, Color.White);
            debugCount.LocalPosition = new Vector2(100, 700);
            gameObjects.AddChild(debugCount);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            debugCount.Text = availableBlocks.Count.ToString();

            headOfSnake.LocalPosition = grid[xPos, yPos].LocalPosition;
            tailOfSnake = player.SnakeSections[player.SnakeSections.Count - 1];



            for (int i = 1; i < player.SnakeSections.Count; i++)
            {
                player.SnakeSections[i].LocalPosition = player.SnakeSections[i - 1].PreviousPosition;
                if (headOfSnake.LocalPosition == player.SnakeSections[i].LocalPosition)
                {
                    currentState = State.GameOver;
                }
            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentState != State.GameOver)
                moveCount -= dt;

            if (moveCount <= 0)
            {

                foreach (SnakeSection obj in player.SnakeSections)
                    obj.PreviousPosition = obj.CurrentPosition;



                switch (impendingDirection)
                {
                    case Direction.None:
                        xPos += 0; yPos += 0;
                        break;
                    case Direction.Up:
                        if (yPos != 0)
                        {
                            yPos--;
                            currentDirection = Direction.Up;
                        }
                        else
                            currentState = State.GameOver;
                        break;
                    case Direction.Right:
                        if (xPos != ROWS - 1)
                        {
                            xPos++;
                            currentDirection = Direction.Right;
                        }
                        else
                            currentState = State.GameOver;
                        break;
                    case Direction.Down:
                        if (yPos != COLS - 1)
                        {
                            yPos++;
                            currentDirection = Direction.Down;
                        }
                        else
                            currentState = State.GameOver;
                        break;
                    case Direction.Left:
                        if (xPos != 0)
                        {
                            xPos--;
                            currentDirection = Direction.Left;
                        }
                        else
                            currentState = State.GameOver;
                        break;
                    default:
                        xPos += 0; yPos += 0;
                        break;
                }



                foreach (SnakeSection obj in player.SnakeSections)
                    obj.CurrentPosition = obj.LocalPosition;


                availableBlocks.Remove(headOfSnake.CurrentPosition);
                availableBlocks.Add(tailOfSnake.PreviousPosition);


                if (currentState != State.GameOver)
                {

                    if (headOfSnake.CurrentPosition == goalPiece.LocalPosition)
                    {

                        SnakeSection addSnake = new SnakeSection();
                        addSnake.LocalPosition = tailOfSnake.PreviousPosition;
                        player.SnakeSections.Add(addSnake);
                        availableBlocks.Remove(tailOfSnake.CurrentPosition);
                        goalPiece.LocalPosition = availableBlocks[ExtendedGame.Random.Next(availableBlocks.Count - 1)];

                    }

                }


                    

                moveCount = startCount;
            }

            if (currentState == State.GameOver)
            {
                ExtendedGame.BackgroundColor = Color.Red;
            }

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            KeySelect(Keys.Down, Direction.Up, Direction.Down);
            KeySelect(Keys.Up, Direction.Down, Direction.Up);
            KeySelect(Keys.Right, Direction.Left, Direction.Right);
            KeySelect(Keys.Left, Direction.Right, Direction.Left);
            KeySelect(Keys.Space, Direction.None, Direction.None);

            void KeySelect(Keys keyDown, Direction wrongDirection, Direction impendingDirection)
            {
                if (inputHelper.KeyPressed(keyDown) && currentDirection != wrongDirection)
                    this.impendingDirection = impendingDirection;
            }
            
        }
    }
}
