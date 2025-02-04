using System;

namespace Render
{
    class Program
    {
        private const int ScreenWidth = 100;
        private const int ScreenHeight = 40;


        private const int MapWidth = 32;
        private const int MapHeight = 32;

        private const double Fov = Math.PI / 3;
        private const double Depth = 20;

        private static double _playerX = 5;
        private static double _playerY = 5;
        private static double _playerA = 0;

        private static string map = "";
        private static readonly char[] Screen = new char[ScreenWidth * ScreenHeight];
        
        static void Main(string[] args)
        {
            Program program = new Program();

            Console.SetWindowSize(ScreenWidth, ScreenHeight);
            Console.SetBufferSize(ScreenWidth, ScreenHeight);
            Console.CursorVisible = false;

            GenerateMap();


            var dateTimeFrom = DateTime.Now;


            while (true)
            {
                var dateTimeTo = DateTime.Now;
                double elapsedTime = (dateTimeTo - dateTimeFrom).TotalMilliseconds;
                dateTimeFrom = DateTime.Now;

                if (Console.KeyAvailable)
                {
                    ConsoleKey consoleKey = Console.ReadKey(true).Key;

                    switch (consoleKey)
                    {
                        case ConsoleKey.A:
                            _playerA -= 0.3 * elapsedTime;
                            break;
                        case ConsoleKey.D:
                            _playerA += 0.3 * elapsedTime;
                            break;
                        case ConsoleKey.W:
                        {
                            _playerX += Math.Sin(_playerA) * elapsedTime;
                            _playerY += Math.Cos(_playerA) * elapsedTime;
                            if(map[(int) _playerY * MapWidth + (int)_playerX] == '#')
                            {
                                _playerX -= Math.Sin(_playerA) * elapsedTime;
                                _playerY -= Math.Cos(_playerA) * elapsedTime;
                            }
                            break;
                        }
                        case ConsoleKey.S:
                        {
                            _playerX -= Math.Sin(_playerA) * elapsedTime;
                            _playerY -= Math.Cos(_playerA) * elapsedTime;
                            if (map[(int)_playerY * MapWidth + (int)_playerX] == '#')
                            {
                                _playerX += Math.Sin(_playerA) * elapsedTime;
                                _playerY += Math.Cos(_playerA) * elapsedTime;
                            }
                            break;
                        }

                    }
                }

                for (int x = 0; x < ScreenWidth; x++)   
                {
                    double raytAngle = _playerA + Fov * (x / (double)ScreenWidth - 0.5);

                    double rayX = Math.Sin(raytAngle);
                    double rayY = Math.Cos(raytAngle);

                    double distanceToWall = 0;
                    bool hitWall = false;

                    while (!hitWall)
                    {
                        distanceToWall += 0.1f;
                        int testX = (int)(_playerX + rayX * distanceToWall);
                        int testY = (int)(_playerY + rayY * distanceToWall);

                        if(testX < 0 || testX >= Depth + _playerX || testY < 0 || testY >= Depth + _playerY)
                        {
                            hitWall = true;
                            distanceToWall = Depth;
                        }
                        else
                        {
                            char testCell = map[testY * MapWidth + testX];
                            if(testCell == '#')
                            {
                                hitWall = true;
                            }
                        }
                    }

                    int ceiling = (int)(ScreenHeight / 2d - ScreenHeight * Fov / distanceToWall);
                    int floor = ScreenHeight - ceiling;

                    char wallShade;

                    if (distanceToWall <= Depth / 4d)
                    {
                        wallShade = '\u2588';
                    }else if(distanceToWall < Depth / 3d)
                    {
                        wallShade = '\u2593';
                    }
                    else if (distanceToWall < Depth / 2d)
                    {
                        wallShade = '\u2592';
                    }
                    else if(distanceToWall < Depth)
                    {
                        wallShade = '\u2591';
                    }
                    else
                    {
                        wallShade = ' ';
                    }
                     
                    for(int y = 0; y < ScreenHeight; y++)
                    {
                        if (y <=  ceiling){
                            Screen[y * ScreenWidth + x] = ' ';
                        }
                        else if(y > ceiling && y <= floor)
                        {
                            Screen[y * ScreenWidth + x] = wallShade;
                        }
                        else
                        {
                            char floorShade;

                            double b = 1 - (y - ScreenHeight / 2d) / (ScreenHeight / 2d);

                            if (b < 0.25)
                            {
                                Screen[y * ScreenWidth + x] = '#';
                            }
                            else if (b < 0.5)
                            {
                               Screen[y * ScreenWidth + x] = 'x';
                            } 
                            else if (b < 0.75)
                            {
                                Screen[y * ScreenWidth + x] = '-';
                            }
                            else if(b < 0.9)
                            {
                                Screen[y * ScreenWidth + x] = '.';
                            }
                        }
                    }
                }

                char[] stats = $"X: {_playerX}, Y : {_playerY}, A: {_playerA}, FPS: {(int)1/ elapsedTime}".ToCharArray();
                stats.CopyTo( Screen, 0 );

                Console.SetCursorPosition(0, 0);
                Console.Write(Screen);
            }
        }

        private static void GenerateMap()
        {
            map += "################################";  //1
            map += "#***********************#******#";  //2
            map += "#***********************#******#";  //3
            map += "#***********************#******#";  //4
            map += "#***********************#******#";  //5
            map += "#***********************#******#";  //6
            map += "#***********************#******#";  //7
            map += "#***********************#******#";  //8
            map += "#****#######************#******#";  //9
            map += "#***********************#******#";  //10
            map += "#***********************#******#";  //11
            map += "#***********************#******#";  //12
            map += "#******************************#";  //13
            map += "#******************************#";  //14
            map += "#*******#**********************#";  //15
            map += "#******************************#";  //16
            map += "#******************************#";  //17
            map += "#******************************#";  //18
            map += "#*********************##########";  //19
            map += "#******************************#";  //20
            map += "#******************************#";  //21
            map += "#******************************#";  //22
            map += "#******************************#";  //23
            map += "############*******************#";  //24
            map += "#**********#*******************#";  //25
            map += "#**********#*******************#";  //26
            map += "#**********#************##*****#";  //27
            map += "#***********************##*****#";  //28
            map += "#***********************##*****#";  //29
            map += "#******************************#";  //30
            map += "#******************************#";  //31
            map += "################################";  //32
        }
    }
}
