using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Raylib_cs;
using static Raylib_cs.Raylib;


namespace Dunveler
{
    internal unsafe class LabyrinthGenerator
    {
        public const int WIDTH = 33; // Width of the maze (must be odd).
        public const int HEIGHT = 33; // Height of the maze (must be odd).

        private static Dictionary<(int, int), bool> maze; //? true (Wall) : false (Space)
        private static List<(int, int)> hasVisited;

        public static void Generate()
        {
            InitializeMaze();
            GenerateMaze(1, 1);
        }

        private static void InitializeMaze()
        {
            maze = new Dictionary<(int, int), bool>();
            for (int x = 0; x < WIDTH; x++)
            {
                for (int y = 0; y < HEIGHT; y++)
                {
                    maze[(x, y)] = true; // Every space is a wall at first.
                }
            }
            hasVisited = new List<(int, int)>();
        }

        public static Image DrawMaze()
        {
            Image imgMap = GenImageColor(WIDTH, HEIGHT, Color.Black);
            Image* imMap = &imgMap;

            for (int y = 0; y < HEIGHT; y++)
            {
                for (int x = 0; x < WIDTH; x++)
                {
                    if (maze[(x, y)] == true)
                    {
                        ImageDrawPixel(imMap, x, y, Color.White);
                    }
                }
            }

            return imgMap;
        }

        private static void GenerateMaze(int x, int y)
        {
            for (int x1 = x; x1 < 4; x1++)
            {
                for (int y1 = y; y1 < 4; y1++)
                {
                    maze[(x1, y1)] = false;
                }
            }

            for (int x1 = WIDTH - 2; x1 > WIDTH - 5; x1--)
            {
                for (int y1 = HEIGHT - 2; y1 > HEIGHT - 5; y1--)
                {
                    maze[(x1, y1)] = false;
                }
            }

            maze[(x, y)] = false; // "Carve out" the space at x, y.

            while (true)
            {
                // Check which neighboring spaces adjacent to
                // the mark have not been visited already:
                List<char> unvisitedNeighbors = new List<char>();
                if (y > 1 && !hasVisited.Contains((x, y - 2)))
                {
                    unvisitedNeighbors.Add('n');
                }

                if (y < HEIGHT - 2 && !hasVisited.Contains((x, y + 2)))
                {
                    unvisitedNeighbors.Add('s');
                }

                if (x > 1 && !hasVisited.Contains((x - 2, y)))
                {
                    unvisitedNeighbors.Add('w');
                }

                if (x < WIDTH - 2 && !hasVisited.Contains((x + 2, y)))
                {
                    unvisitedNeighbors.Add('e');
                }

                if (unvisitedNeighbors.Count == 0)
                {
                    // BASE CASE
                    // All neighboring spaces have been visited, so this is a
                    // dead end. Backtrack to an earlier space:
                    return;
                }
                else
                {
                    // RECURSIVE CASE
                    // Randomly pick an unvisited neighbor to visit:
                    char nextIntersection = unvisitedNeighbors[new Random().Next(unvisitedNeighbors.Count)];

                    // Move the mark to an unvisited neighboring space:
                    int nextX = x;
                    int nextY = y;
                    switch (nextIntersection)
                    {
                        case 'n':
                            nextY = y - 2;
                            maze[(x, y - 1)] = false; // Connecting hallway.
                            break;
                        case 's':
                            nextY = y + 2;
                            maze[(x, y + 1)] = false; // Connecting hallway.
                            break;
                        case 'w':
                            nextX = x - 2;
                            maze[(x - 1, y)] = false; // Connecting hallway.
                            break;
                        case 'e':
                            nextX = x + 2;
                            maze[(x + 1, y)] = false; // Connecting hallway.
                            break;
                    }

                    hasVisited.Add((nextX, nextY)); // Mark as visited.
                    GenerateMaze(nextX, nextY); // Recursively visit this space.
                }
            }
        }
    }
}
