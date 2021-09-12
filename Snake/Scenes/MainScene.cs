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
        SnakeSection headOfSnake;

        public MainScene()
        {
            

            goalPiece = new GoalPiece();
            gameObjects.AddChild(goalPiece);

            player = new Snake();
            gameObjects.AddChild(player);

            

            grid = new BlockSpace[ROWS, COLS];

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
                }
            }

            xPos = 10;
            yPos = 10;


            headOfSnake = new SnakeSection();
            headOfSnake.LocalPosition = grid[xPos, yPos].LocalPosition;
            player.SnakeSections.Add(headOfSnake);

            SnakeSection addSnake = new SnakeSection();
            addSnake.LocalPosition = new Vector2(-100, -100);
            player.SnakeSections.Add(addSnake);

            startCount = moveCount;

            impendingDirection = Direction.None;

            

            RandomPlacement();
            
        }

        void RandomPlacement()
        {
            goalPiece.LocalPosition = grid[ExtendedGame.Random.Next(17), ExtendedGame.Random.Next(15)].LocalPosition;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            for (int i = 0; i < player.SnakeSections.Count - 1; i++)
            {
                if (currentState == State.Playing)
                {
                    if (i == 0)
                        player.SnakeSections[i].LocalPosition = grid[xPos, yPos].LocalPosition;
                    else
                        player.SnakeSections[i].LocalPosition = player.SnakeSections[i - 1].PreviousPosition;
                }

            }

            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

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
                        } else 
                            currentState = State.GameOver;
                        break;
                    case Direction.Right:
                        if (xPos != ROWS - 1)
                        {
                            xPos++;
                            currentDirection = Direction.Right;
                        } else 
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
                        } else
                            currentState = State.GameOver;
                        break;
                    default:
                        xPos += 0; yPos += 0;
                        break;
                }
                

                foreach (SnakeSection obj in player.SnakeSections)
                    obj.CurrentPosition = obj.LocalPosition;


                if (headOfSnake.CurrentPosition == goalPiece.LocalPosition)
                {
                    SnakeSection addSnake = new SnakeSection();

                    addSnake.LocalPosition = player.SnakeSections[player.SnakeSections.Count - 1].PreviousPosition;
                    player.SnakeSections.Add(addSnake);

                }

                if (currentState == State.GameOver)
                {
                    ExtendedGame.BackgroundColor = Color.Red;
                }


                moveCount = startCount;
            }

        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            KeySelect(Keys.Up, Direction.Down, Direction.Up);
            KeySelect(Keys.Right, Direction.Left, Direction.Right);
            KeySelect(Keys.Down, Direction.Up, Direction.Down);
            KeySelect(Keys.Left, Direction.Right, Direction.Left);
            KeySelect(Keys.Space, Direction.None, Direction.None);

            void KeySelect(Keys keyDown, Direction wrongDirection, Direction desiredDirection)
            {
                if (inputHelper.KeyPressed(keyDown) && currentDirection != wrongDirection)
                    impendingDirection = desiredDirection;
            }
            
        }
    }
}
