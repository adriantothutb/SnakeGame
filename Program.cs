using System;
using System.Collections.Generic;
// using System.Linq; - nepoužité, odstránené

namespace Snake
{
    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth;
            int screenHeight = Console.WindowHeight;
            Random random = new Random();
            int score = 5;
            bool gameOver = false;
            Pixel head = new Pixel();
            head.XPos = screenWidth / 2;
            head.YPos = screenHeight / 2;
            head.ConsoleColor = ConsoleColor.Red;
            Direction direction = Direction.Right;
            List<int> bodyX = new List<int>();
            List<int> bodyY = new List<int>();
            int berryX = random.Next(0, screenWidth);
            int berryY = random.Next(0, screenHeight);
 
            while (true)
            {
                Console.Clear();
                if (head.XPos == screenWidth - 1 || head.XPos == 0 ||head.YPos == screenHeight - 1 || head.YPos == 0)
                { 
                    gameOver = true;
                }

                DrawBorder(screenWidth, screenHeight); // vykreslenie rámika je oddelené od logiky hry
                
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryX == head.XPos && berryY == head.YPos)
                {
                    score++;
                    berryX = random.Next(1, screenWidth - 2);
                    berryY = random.Next(1, screenHeight - 2);
                } 
                
                if (DrawSnakeBody(bodyX, bodyY, head)) // kreslenie + kolízia tela v jednej metóde
                {
                    gameOver = true;
                }
                
                if (gameOver)
                {
                    break;
                }
                
                DrawHead(head); // vykreslenie hlavy oddelené pre lepšiu čitateľnosť
                DrawBerry(berryX, berryY); // vykreslenie berry oddelené (príprava na ďalšie oddelenie GUI)
                
                direction = HandleInput(direction); // spracovanie vstupu oddelené od hlavnej logiky hry

                bodyX.Add(head.XPos);
                bodyY.Add(head.YPos);
                switch (direction)
                {
                    case Direction.Up:
                        head.YPos--;
                        break;
                    case Direction.Down:
                        head.YPos++;
                        break;
                    case Direction.Left:
                        head.XPos--;
                        break;
                    case Direction.Right:
                        head.XPos++;
                        break;
                }
                if (bodyX.Count > score)
                {
                    bodyX.RemoveAt(0);
                    bodyY.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: " + score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 + 1);
        }

        private static void DrawBorder(int screenWidth, int screenHeight) // rámik je čisto "GUI" záležitosť, preto je mimo hlavnej logiky hry
        {
            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("■");
            }

            for (int i = 0; i < screenWidth; i++)
            {
                Console.SetCursorPosition(i, screenHeight - 1);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("■");
            }

            for (int i = 0; i < screenHeight; i++)
            {
                Console.SetCursorPosition(screenWidth - 1, i);
                Console.Write("■");
            }
        }

        private static bool DrawSnakeBody(List<int> bodyX, List<int> bodyY, Pixel head) // vykreslenie tela + kontrola kolízie na jednom mieste
        {
            for (int i = 0; i < bodyX.Count; i++)
            {
                Console.SetCursorPosition(bodyX[i], bodyY[i]);
                Console.Write("■");

                if (bodyX[i] == head.XPos && bodyY[i] == head.YPos)
                {
                    return true; // narazil do vlastného tela
                }
            }

            return false;
        }

        private static void DrawHead(Pixel head) // samostatná metóda, aby Main() nebol zaplnený "Console.*" detailami
        {
            Console.SetCursorPosition(head.XPos, head.YPos);
            Console.ForegroundColor = head.ConsoleColor;
            Console.Write("■");
        }

        private static void DrawBerry(int berryX, int berryY) // berry je samostatný "objekt na scéne"
        {
            Console.SetCursorPosition(berryX, berryY);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("■");
        }

        private static Direction HandleInput(Direction direction) // spracovanie vstupu presunuté z Main(), aby bola hlavná slučka hry čitateľnejšia 
        {
            DateTime frameStartTime = DateTime.Now;
            bool buttonPressed = false;

            while (true)
            {
                DateTime currentTime = DateTime.Now;

                if (currentTime.Subtract(frameStartTime).TotalMilliseconds > 500)
                {
                    break;
                }

                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.UpArrow && direction != Direction.Down && !buttonPressed)
                    {
                        direction = Direction.Up;
                        buttonPressed = true;
                    }

                    if (key.Key == ConsoleKey.DownArrow && direction != Direction.Up && !buttonPressed)
                    {
                        direction = Direction.Down;
                        buttonPressed = true;
                    }

                    if (key.Key == ConsoleKey.LeftArrow && direction != Direction.Right && !buttonPressed)
                    {
                        direction = Direction.Left;
                        buttonPressed = true;
                    }

                    if (key.Key == ConsoleKey.RightArrow && direction != Direction.Left && !buttonPressed)
                    {
                        direction = Direction.Right;
                        buttonPressed = true;
                    }
                }
            }

            return direction;
        }
        class Pixel
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ConsoleColor { get; set; }
        }
    }
}