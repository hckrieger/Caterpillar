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
        const int ROWS = 10;
        const int COLS = 10;

        BlockSpace[,] grid;

        GoalPiece goalPiece;

        int xPos;
        int yPos;

        enum State
        {
            Pregame,
            Playing,
            GameOver,
            Win
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right,
            None,
        }

        float moveCount = .35f;
        float startCount;

        Direction currentDirection, impendingDirection;
        State currentState;

        Snake player;
        SnakeSection headOfSnake, tailOfSnake;

        List<Vector2> availableBlocks;

        GameObjectList playingField = new GameObjectList();

        public Direction CurrentDirection
        {
            get { return currentDirection; }
        }

        public GameObjectList PlayingField { get { return playingField; } }

        TextGameObject score = new TextGameObject("debug", 1f, Color.White);
        

        public MainScene()
        {
            score.LocalPosition = new Vector2(20, 20);
            gameObjects.AddChild(score);

            playingField.LocalPosition = new Vector2(0, 100);

            grid = new BlockSpace[ROWS, COLS];

            availableBlocks = new List<Vector2>();

            

            goalPiece = new GoalPiece();
            playingField.AddChild(goalPiece);

            player = new Snake();
            player.SnakeSections.Add(new SnakeSection());
           
            headOfSnake = player.SnakeSections[0];
            headOfSnake.Parent = player;
            headOfSnake.SheetIndex = 1;

            Reset();

            headOfSnake.LocalPosition = grid[xPos, yPos].LocalPosition;


            playingField.AddChild(player);

           

            startCount = moveCount;

            impendingDirection = Direction.None;
            currentDirection = Direction.None;

            goalPiece.SheetIndex = ExtendedGame.Random.Next(4, 12);
            goalPiece.LocalPosition = availableBlocks[ExtendedGame.Random.Next(availableBlocks.Count)];

            gameObjects.AddChild(playingField);

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            score.Text = ((ROWS * COLS) - availableBlocks.Count).ToString();

            if (availableBlocks.Count == 1)
            {
                score.Text = (ROWS * COLS).ToString();
                currentState = State.Win;
            }

            headOfSnake.LocalPosition = grid[xPos, yPos].LocalPosition;
            tailOfSnake = player.SnakeSections[player.SnakeSections.Count - 1];

            for (int i = 0; i < player.SnakeSections.Count; i++)
            {
                if (i > 0)
                {
                    player.SnakeSections[i].LocalPosition = player.SnakeSections[i - 1].PreviousPosition;

                    if (headOfSnake.LocalPosition == player.SnakeSections[i].LocalPosition)
                        currentState = State.GameOver;
                }

            }


            if (currentState == State.Win)
            {
                ExtendedGame.BackgroundColor = Color.Green;
            }


            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (currentState == State.Playing)
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
                        addSnake.SheetIndex = goalPiece.SheetIndex;
                        addSnake.LocalPosition = tailOfSnake.PreviousPosition;
                        playingField.AddChild(addSnake);
                        player.SnakeSections.Add(addSnake);
                        availableBlocks.Remove(tailOfSnake.CurrentPosition);
                        goalPiece.SheetIndex = ExtendedGame.Random.Next(4, 12);
                        goalPiece.LocalPosition = availableBlocks[ExtendedGame.Random.Next(availableBlocks.Count - 1)];

                    }

                }

                moveCount = startCount;
            }



        }

        public override void HandleInput(InputHelper inputHelper)
        {
            base.HandleInput(inputHelper);

            KeySelect(Keys.Down, Direction.Up, Direction.Down);
            KeySelect(Keys.Up, Direction.Down, Direction.Up);
            KeySelect(Keys.Right, Direction.Left, Direction.Right);
            KeySelect(Keys.Left, Direction.Right, Direction.Left);
            //KeySelect(Keys.Space, Direction.None, Direction.None);

            void KeySelect(Keys keyDown, Direction wrongDirection, Direction impendingDirection)
            {
                if (inputHelper.KeyPressed(keyDown) && currentDirection != wrongDirection)
                    this.impendingDirection = impendingDirection;
            }

            if (currentState == State.GameOver)
            {
                if (inputHelper.KeyPressed(Keys.Space))
                    Reset();
            }

            
            
        }

        public override void Reset()
        {
            base.Reset();

            availableBlocks.Clear();

            score.Text = 0.ToString();

            

            foreach (SnakeSection obj in player.SnakeSections)
            {
                playingField.RemoveChild(obj);
            }
            


            player.SnakeSections.RemoveAll(m => m != player.SnakeSections[0]);

            xPos = 2;
            yPos = 4;

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
                    playingField.AddChild(grid[x, y]);

                    availableBlocks.Add(grid[x, y].LocalPosition);
                }

            }

            currentState = State.Playing;
        }
    }
}
