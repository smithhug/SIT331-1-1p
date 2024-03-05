using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace SIT331_1_1p
{
    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    public class Robot
    {
        public int Y { get; set; }
        public int X { get; set; }
        public Direction D { get; set; }
        public int MapSize { get; }
        public bool IsPlaced { get; private set; }

        public Robot(int mapSize)
        {
            MapSize = mapSize;
            IsPlaced = false;
        }

        public void Place(int x, int y, Direction d)
        {
            X = x;
            Y = y;
            D = d;
            IsPlaced = true;
        }
        
        public void Move() {
            int nextX = X;
            int nextY = Y;

            switch (D)
            {
                case Direction.NORTH:
                    nextY++;
                    break;
                case Direction.EAST:
                    nextX++;
                    break;
                case Direction.SOUTH:
                    nextY--;
                    break;
                case Direction.WEST:
                    nextX--;
                    break;
            }

            if (IsValidPos(nextX, nextY))
            {
                X = nextX;
                Y = nextY;
            }
            else
            {
                Console.WriteLine("Invalid move. Robot cannot fall off the map.");
            }

        }

        public void Turn(string LeftOrRight) {
            if (LeftOrRight == "Left") {
                D = (Direction)(((int)D + 3) % 4);
            } else if (LeftOrRight == "Right") {
                D = (Direction)(((int)D + 1) % 4);
            }
        }

        private bool IsValidPos(int x, int y)
        {
            return x >= 0 && x < MapSize && y >= 0 && y < MapSize;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            //set inital map and robot
            const int mapSize = 5;
            var robot = new Robot(mapSize);
            //size constraints
            if (mapSize > 100 | mapSize < 2)
            {
                Console.Write("Invalid map size (N). Must be N=>2 and N<=100");
                return;
            }
                

            while (true)
            {
                Console.Write("Enter command: ");
                string input = Console.ReadLine().Trim().ToUpper();

                if (input.StartsWith("PLACE"))
                {
                    string p1 = input.Split('(')[1];
                    string p2 = p1.Split(')')[0];
                    string[] parts = p2.Split(',');

                    //parse input
                    if (parts.Length == 3 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y) && Enum.TryParse(parts[2], out Direction direction))
                    {
                        robot.Place(x, y, direction);
                        Console.WriteLine($"Current position: ({robot.X},{robot.Y}), Facing: {robot.D}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid PLACE command. Please use format: PLACE(X,Y,D)");
                    }
                }
                if (robot.IsPlaced)
                {
                    if (input == "MOVE")
                    {
                        robot.Move();
                        Console.WriteLine($"Current position: ({robot.X},{robot.Y}), Facing: {robot.D}");
                    }
                    else if (input == "LEFT")
                    {
                        robot.Turn("Left");
                        Console.WriteLine($"Current position: ({robot.X},{robot.Y}), Facing: {robot.D}");
                    }
                    else if (input == "RIGHT")
                    {
                        robot.Turn("Right");
                        Console.WriteLine($"Current position: ({robot.X},{robot.Y}), Facing: {robot.D}");
                    }
                    else if(input.StartsWith("PLACE") != true)
                    {
                        Console.WriteLine("Invalid command.");
                    }
                }
                else
                {
                    Console.WriteLine("Robot must be placed before it can move.");
                }
            }

        }
    }
}
