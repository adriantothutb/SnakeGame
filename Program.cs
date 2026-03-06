using System;

namespace Snake
{
    class Program
    {
        const int WindowHeight = 16;
        const int WindowWidth = WindowHeight * 2;
        const int InitialScore = 5;

        static void Main(string[] args) // Main len spúšťa aplikáciu, neobsahuje logiku jej zastavenia
        {
            RunGame();
        }    

        private static void RunGame()
        {
            Console.WindowHeight = WindowHeight;
            Console.WindowWidth = WindowWidth;

            var state = new GameState(WindowWidth, WindowHeight, new Random())
            {
                Score = InitialScore,
                Head = new Position(WindowWidth / 2, WindowHeight / 2),
                Direction = Direction.Right
            };

            var game = new SnakeGame(state);
            var renderer = new ConsoleRenderer();
            var input = new ConsoleInput();

            game.SpawnBerry();
 
            while (!state.GameOver)
            {
                renderer.Render(state);
                state.Direction = input.Handle(state.Direction);
                game.Update();
            }
            Console.SetCursorPosition(state.ScreenWidth / 5, state.ScreenHeight / 2);
            Console.WriteLine("Game over, Score: " + state.Score);
        }
    }
}