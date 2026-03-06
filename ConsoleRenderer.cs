using System;

namespace Snake
{
    class ConsoleRenderer
    {
        const char Block = '■';

        public void Render(GameState state)
        {
            Console.Clear();
            DrawBorder(state);
            DrawSnakeBody(state);
            DrawBerry(state);
            DrawHead(state);
        }

        private void DrawBorder(GameState state)
        {
            for (int i = 0; i < state.ScreenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Block);
                Console.SetCursorPosition(i, state.ScreenHeight - 1);
                Console.Write(Block);
                Console.ResetColor();
            }

            for (int i = 0; i < state.ScreenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Block);
                Console.SetCursorPosition(state.ScreenWidth - 1, i);
                Console.Write(Block);
                Console.ResetColor();
            }
        }

        private void DrawSnakeBody(GameState state)
        {
            foreach (var part in state.Body)
            {
                Console.SetCursorPosition(part.X, part.Y);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write(Block);
                Console.ResetColor();
            }
        }

        private void DrawHead(GameState state)
        {
            Console.SetCursorPosition(state.Head.X, state.Head.Y);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(Block);
            Console.ResetColor();
        }

        private void DrawBerry(GameState state)
        {
            Console.SetCursorPosition(state.Berry.X, state.Berry.Y);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write(Block);
            Console.ResetColor();
        }
    }
}