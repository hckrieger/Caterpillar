﻿using Engine;
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
            GetReady,
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

        float moveCount = .375f;
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

        TextGameObject score = new TextGameObject("score", 1f, Color.Green);
        TextGameObject message = new TextGameObject("message", 1f, Color.Green);
        

        public MainScene()
        {
            score.LocalPosition = new Vector2(200, 37);
            gameObjects.AddChild(score);

            
            gameObjects.AddChild(message);

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

            gameObjects.AddChild(playingField);
        }

        public override void Reset() //Values that are reset everytime the player starts a new game
        {
            base.Reset();

            

            if (currentState == State.GameOver || currentState == State.Win)
            {
                availableBlocks.Clear();

                foreach (SnakeSection obj in player.SnakeSections)
                    playingField.RemoveChild(obj);

                player.SnakeSections.RemoveAll(m => m != player.SnakeSections[0]);

                Score(0);
            }

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

            goalPiece.SheetIndex = ExtendedGame.Random.Next(4, 12);
            goalPiece.LocalPosition = availableBlocks[ExtendedGame.Random.Next(availableBlocks.Count)];

            currentDirection = Direction.Right;
            currentState = State.GetReady;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            
            Score((ROWS * COLS) - availableBlocks.Count);

            if (availableBlocks.Count == 1)
            {
                Score(ROWS * COLS);
                currentState = State.Win;
            }

            Message();

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
                {
                    this.impendingDirection = impendingDirection;

                    if (currentState == State.GetReady && keyDown != Keys.Left)
                        currentState = State.Playing;
                }

            }

            if (currentState == State.GameOver || currentState == State.Win)
            {
                
                if (inputHelper.KeyPressed(Keys.Space))
                    Reset();
            }
        }


        public string Score(int currentScore)
        {
            score.Text = $"Score: {currentScore}";
            return score.Text;
        }

        public void Message()
        {
            switch(currentState)
            {
                case State.GetReady:
                    message.Text = "Instructions:\n" +
                        "-Move with arrow keys\n" +
                        "-Eat the blinking ball\n" +
                        "-Don't run into the wall\n" +
                        "-Don't run into yourself";

                    message.LocalPosition = new Vector2(5, 0);
                    break;
                case State.Playing:
                    message.Text = "Go!";
                    message.LocalPosition = new Vector2(65, 40);
                    break;
                case State.GameOver:
                    message.Text = "Game Over!\nPlay again? \nPress Space";
                    message.LocalPosition = new Vector2(30, 20);
                    break;
                case State.Win:
                    message.Text = "Wow! You won!\nGreat Job!\nPlay again?\nPress Space";
                    message.LocalPosition = new Vector2(30, 10);
                    break;
                default:
                    message.Text = "Welcome to Caterpillar";
                    break;
            }
        }
    }
}
