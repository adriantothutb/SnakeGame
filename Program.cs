using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Snake
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WindowHeight = 16;
            Console.WindowWidth = 32;
            int screenWidth = Console.WindowWidth; //zmena názvu premennej na čitateľnejšiu
            int screenHeight = Console.WindowHeight; //zmena názvu premennej na čitateľnejšiu
            Random random = new Random(); // premenná pomenovaná zrozumiteľnejšie
            int score = 5;
            bool gameOver = false;   // zmenené int na bool (čitateľnejšie, žiadne 0/1)
            Pixel head = new Pixel(); // hoofd je holandsky, head je zrozumiteľný anglický názov
            head.XPos = screenWidth/2; //zmena na výstižnejší anglický názov
            head.YPos = screenHeight/2; //zmena na výstižnejší anglický názov
            head.ConsoleColor = ConsoleColor.Red; //zmena na anglický názov
            string direction = "RIGHT"; //zmena na výstižnejší názov
            List<int> bodyX = new List<int>(); //zmena názvu premennej na čitateľnejšiu
            List<int> bodyY = new List<int>(); //zmena názvu premennej na čitateľnejšiu
            int berryX = random.Next(0, screenWidth); //zmena názvu premennej na čitateľnejšiu
            int berryY = random.Next(0, screenHeight); //zmena názvu premennej na čitateľnejšiu
            DateTime frameStartTime = DateTime.Now; //zmena na anglický názov
            DateTime currentTime = DateTime.Now; //zmena na anglický názov
            bool buttonPressed = false; // zmenený string na bool
            while (true)
            {
                Console.Clear();
                if (head.XPos == screenWidth-1 || head.XPos == 0 ||head.YPos == screenHeight-1 || head.YPos == 0)
                { 
                    gameOver = true; // namiesto 0/1 bool
                }
                for (int i = 0;i< screenWidth; i++)
                {
                    Console.SetCursorPosition(i, 0);
                    Console.Write("■");
                }
                for (int i = 0; i < screenWidth; i++)
                {
                    Console.SetCursorPosition(i, screenHeight -1);
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
                Console.ForegroundColor = ConsoleColor.Green;
                if (berryX == head.XPos && berryY == head.YPos)
                {
                    score++;
                    berryX = random.Next(1, screenWidth-2);
                    berryY = random.Next(1, screenHeight-2);
                } 
                for (int i = 0; i < bodyX.Count(); i++)
                {
                    Console.SetCursorPosition(bodyX[i], bodyY[i]);
                    Console.Write("■");
                    if (bodyX[i] == head.XPos && bodyY[i] == head.YPos)
                    {
                        gameOver = true;
                    }
                }
                if (gameOver)
                {
                    break;
                }
                Console.SetCursorPosition(head.XPos, head.YPos);
                Console.ForegroundColor = head.ConsoleColor;
                Console.Write("■");
                Console.SetCursorPosition(berryX, berryY);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("■");
                frameStartTime = DateTime.Now;
                buttonPressed = false;
                while (true)
                {
                    currentTime = DateTime.Now;
                    if (currentTime.Subtract(frameStartTime).TotalMilliseconds > 500) { break; }
                    if (Console.KeyAvailable)
                    {
                        ConsoleKeyInfo key = Console.ReadKey(true); // zmena toets z holadštiny na key v angl. jazyku
                        //Console.WriteLine(key.Key.ToString());
                        if (key.Key.Equals(ConsoleKey.UpArrow) && direction != "DOWN" && !buttonPressed) //buttonPressed bool namiesto porovnávania stringov
                        {
                            direction = "UP";
                            buttonPressed = true;
                        }
                        if (key.Key.Equals(ConsoleKey.DownArrow) && direction != "UP" && !buttonPressed)
                        {
                            direction = "DOWN";
                            buttonPressed = true;
                        }
                        if (key.Key.Equals(ConsoleKey.LeftArrow) && direction != "RIGHT" && !buttonPressed)
                        {
                            direction = "LEFT";
                            buttonPressed = true;
                        }
                        if (key.Key.Equals(ConsoleKey.RightArrow) && direction != "LEFT" && !buttonPressed)
                        {
                            direction = "RIGHT";
                            buttonPressed = true;
                        }
                    }
                }
                bodyX.Add(head.XPos);
                bodyY.Add(head.YPos);
                switch (direction)
                {
                    case "UP":
                        head.YPos--;
                        break;
                    case "DOWN":
                        head.YPos++;
                        break;
                    case "LEFT":
                        head.XPos--;
                        break;
                    case "RIGHT":
                        head.XPos++;
                        break;
                }
                if (bodyX.Count() > score)
                {
                    bodyX.RemoveAt(0);
                    bodyY.RemoveAt(0);
                }
            }
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2);
            Console.WriteLine("Game over, Score: "+ score);
            Console.SetCursorPosition(screenWidth / 5, screenHeight / 2 +1);
        }
        class Pixel // názov triedy začína veľkým písmenom, aby bol čitateľnejší a podľa bežného štýlu v C#
        {
            public int XPos { get; set; }
            public int YPos { get; set; }
            public ConsoleColor ConsoleColor { get; set; }
        }
    }
}

