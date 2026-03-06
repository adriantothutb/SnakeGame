using System;
using System.Collections.Generic;

namespace Snake
{
    class GameState
    {
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }

        public Random Random { get; }

        public int Score { get; set; }
        public bool GameOver { get; set; }

        public Position Head { get; set; }
        public List<Position> Body { get; } = new List<Position>();

        public Direction Direction { get; set; }


        public Position Berry { get; set; }

        public GameState(int width, int height, Random random)
        {
            ScreenWidth = width;
            ScreenHeight = height;
            Random = random;
        }
    }
}