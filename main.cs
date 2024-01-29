//using Spectre.Console;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace TerminalSnakeGame
{
    internal class Program
    {
        internal static Timer _timer = null;
        internal static int GameSpeed = 85;
        internal static int MapSizeHeight = Console.WindowHeight;
        internal static int MapSizeWidth = MapSizeHeight*2;

        internal static int SnakeX = 2;
        internal static int SnakeY = 3;

        internal static int AppleX;
        internal static int AppleY;

        internal static int Score = 0;


        public static List<Tuple<int,int>> SnakeCordenate = new List<Tuple<int,int>>();

       
        static void Main(string[] args)
        {
            Game();
        }

        static void Game()
        {
            
            Console.Clear();
            SnakeX = 2;
            SnakeY = 3;
            Score = 0;
            Snake.SnakeSize = 2;
            SnakeCordenate.Clear();
            BuildMap();
            NewPositionAppleOnMap();
            _timer = new Timer(TimerCallBack, null, 0, GameSpeed);
            Snake.SetRight();

            // CONTROLS
            while (true)
            {
                switch(Console.ReadKey(true).Key) {
                    case ConsoleKey.UpArrow: Snake.SetUp(); break;
                    case ConsoleKey.DownArrow: Snake.SetDown(); break;
                    case ConsoleKey.RightArrow: Snake.SetRight(); break;
                    case ConsoleKey.LeftArrow: Snake.SetLeft(); break;
                }
            }
        }

        public static void NewPositionAppleOnMap(){
            Random random = new Random();
            AppleX = random.Next(3, MapSizeWidth-2);
            AppleY = random.Next(3, MapSizeHeight-2);
            Console.SetCursorPosition(AppleX,AppleY);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("O");
            Console.ResetColor();

        }

        public static void TimerCallBack(Object o)
        {
            
            Console.SetCursorPosition(MapSizeWidth+3, 3);
            Console.Write($"Score: {Score}");

            //CheckHit(SnakeX, SnakeY);
            Console.SetCursorPosition(SnakeX, SnakeY);
            SnakeCordenate.Add(Tuple.Create(SnakeX, SnakeY));

            switch (Snake.direction)
            {
                case SnakeDirection.Up: SnakeY--; break;
                case SnakeDirection.Down: SnakeY++; break;
                case SnakeDirection.Right: SnakeX++; break;
                case SnakeDirection.Left: SnakeX--; break;
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("@");
            Console.ResetColor();

            if (SnakeCordenate.Count > Snake.SnakeSize)
            {
                Console.SetCursorPosition(SnakeCordenate[0].Item1, SnakeCordenate[0].Item2);
                Console.Write(" ");
                SnakeCordenate.RemoveAt(0);
            }
            CheckHit(SnakeX, SnakeY);
        }

        static void CheckHit(int x, int y){

            // Apple hit
            if(x == AppleX && SnakeY == AppleY){
                Snake.SnakeSize += 1;
                Score += 1;
                NewPositionAppleOnMap();
            }

            //Self hit
            if(SnakeCordenate.Any(w => w.Item1 == x && w.Item2 == y)){
                GameOver();
            }

            // Board hit
            if(x < 2 || x > MapSizeWidth-2)
                GameOver();
            if(y < 2 || y > MapSizeHeight-2)
                GameOver();

        }

        static void GameOver()
        {
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            Console.SetCursorPosition(MapSizeWidth + 3, 15);
            Console.Write("Você morreu! Tentar novamente? [y][n]:");
            
            var tryAgain = Console.ReadKey();
            if (char.ToUpper(tryAgain.KeyChar) == 'Y')
            {
                Game();
            }else{
                Console.Clear();
                Environment.Exit(0);
            }
        }

        
        public enum SnakeDirection
        {
            Up,
            Down,
            Right,
            Left
        }

        struct Snake
        {
            public static SnakeDirection direction;
            public static int SnakeSize = 2; // 2 -> 1 é a cabeça e o outro o corpo

            public static void SetUp() => direction = direction == SnakeDirection.Down ? SnakeDirection.Down : SnakeDirection.Up;
            public static void SetDown() => direction = direction == SnakeDirection.Up ? SnakeDirection.Up : SnakeDirection.Down;
            public static void SetRight() => direction = direction == SnakeDirection.Left ? SnakeDirection.Left : SnakeDirection.Right;
            public static void SetLeft() => direction = direction == SnakeDirection.Right ? SnakeDirection.Right : SnakeDirection.Left;

        }

        static void BuildMap()
        {
            Console.ForegroundColor = ConsoleColor.White;

            for (int i = 1; i < MapSizeHeight; i++)
            {
                for (int j = 1; j < MapSizeWidth; j++)
                {
                    Console.SetCursorPosition(j, i);
                    if (i == 1 || i == MapSizeHeight - 1)
                        Console.Write("#");
                    else
                    {
                        if (j == 1 || j == MapSizeWidth - 1)
                            Console.Write("#");
                    }
                }
            }
        }
    }
}
