using System.Diagnostics;
using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;

namespace Dunveler;

public static unsafe class Player
{
    public static string playername = Leaderboard.usernames[MainMenu.usersCount];

    public static bool cur = false;
    public static bool noclip = false;

    public static Camera3D Camera = new();

    internal static float Sens = GameSettings.Default.Sensitivity; //Сенса для мыши от 0.01 до 1;
    internal static float Speed; //Скорость перемещения;

    static Random randEnv = new();
    static Sound[] stepSounds = { 
        LoadSound("Resources\\Sounds\\stepdirt_1.wav"),
        LoadSound("Resources\\Sounds\\stepdirt_2.wav"),
        LoadSound("Resources\\Sounds\\stepdirt_3.wav")
    };

    //static Image pointer = LoadImageFromMemory(".png", pointerImg);

    //private static readonly int ThickCur = 3;

    public static void CameraStart()
    {
        Camera.Position = new Vector3(0f, 0f, 0f); // Camera position
        Camera.Target = new Vector3(1f, 0f, 0f); // Camera looking at point
        Camera.Up = new Vector3(.0f, 1.0f, .0f); // Camera up vector (rotation towards target)
        Camera.FovY = 65.0f; // Camera field-of-view Y
        Camera.Projection = CameraProjection.Perspective;

        CameraYaw(ref Camera, -135 * DEG2RAD, true);
        CameraPitch(ref Camera, -45 * DEG2RAD, true, true, false);
    }

    public static void CameraUpdater()
    {
        Vector3 oldCamPos = Camera.Position;

        if (cur == false)
        {
            UpdateCameraPro(
                ref Camera,
                new Vector3(
                    IsKeyDown(KeyboardKey.W)*Speed - IsKeyDown(KeyboardKey.S) *Speed,
                    IsKeyDown(KeyboardKey.D)*Speed - IsKeyDown(KeyboardKey.A) *Speed,
                    0.0f),
                new Vector3(
                    GetMouseDelta().X*Sens,
                    GetMouseDelta().Y*Sens,
                    0.0f),
                0.0f);
        }

        // Check player collision (we simplify to 2D collision detection)
        Vector2 playerPos = new(Camera.Position.X, Camera.Position.Z);
        float playerRadius = 0.1f;  // Collision radius (player is modelled as a cilinder for collision)

        int playerCellX = (int)(playerPos.X - Labyrinth.mapPosition.X + 0.5f);
        int playerCellY = (int)(playerPos.Y - Labyrinth.mapPosition.Z + 0.5f);

        // Out-of-limits security check
        if (playerCellX < 0) playerCellX = 0;
        else if (playerCellX >= Labyrinth.cubicmap.Width) playerCellX = Labyrinth.cubicmap.Width - 1;

        if (playerCellY < 0) playerCellY = 0;
        else if (playerCellY >= Labyrinth.cubicmap.Height) playerCellY = Labyrinth.cubicmap.Height - 1;

        // Check map collisions using image data and player position
        //TODO*IMPR | Just check player surrounding cells for collision
        if (noclip == false)
        {
            for (int y = 0; y < Labyrinth.cubicmap.Height; y++)
            {
                for (int x = 0; x < Labyrinth.cubicmap.Height; x++)
                {
                    if ((Labyrinth.mapPixels[y * Labyrinth.cubicmap.Width + x].R == 255) &&
                        (Labyrinth.mapPixels[y * Labyrinth.cubicmap.Width + x].G == 255) &&
                        (Labyrinth.mapPixels[y * Labyrinth.cubicmap.Width + x].B == 255) &&
                        (CheckCollisionCircleRec(playerPos, playerRadius,
                        new Rectangle(Labyrinth.mapPosition.X - 0.5f + x * 1.0f, Labyrinth.mapPosition.Z - 0.5f + y * 1.0f, 1.0f, 1.0f))))
                    {
                        // Collision detected, reset camera position
                        Camera.Position = oldCamPos;
                    }
                }
            }

            if (CheckCollisionCircleRec(playerPos, playerRadius, new Rectangle(Labyrinth.exitPosition.X, Labyrinth.exitPosition.Z, Labyrinth.exitSize, Labyrinth.exitSize)))
            {
                currentScreen = GameScreen.Results;
            }
        }
    }

    internal static bool DebugInfoDraw = false, InfoDraw = true;
    static int framesCounter = 0;

    public static void Controls()
    {
        if (IsKeyPressed(KeyboardKey.Escape))
        {
            if (cur = !cur == true) { EnableCursor(); Pause.isPaused = true; }
            else { DisableCursor(); Pause.isPaused = drawSettings = false; }
        }

        if (Pause.isPaused == false)
        {
            if (IsKeyDown(KeyboardKey.W) || IsKeyDown(KeyboardKey.S) || IsKeyDown(KeyboardKey.A) || IsKeyDown(KeyboardKey.D))
            {
                framesCounter++;

                if (framesCounter == 1) PlaySound(stepSounds[randEnv.Next(3)]);
                if (framesCounter > (GetFPS() / (Speed * 100))) framesCounter = 0;
            }

            if (IsKeyPressed(KeyboardKey.U))
            {
                Camera.Position.Y += 0.1f;
                Camera.Target.Y += 0.1f;
            }

            if (IsKeyPressed(KeyboardKey.I))
            {
                Camera.Position.Y -= 0.1f;
                Camera.Target.Y -= 0.1f;
            }

            if (IsKeyPressed(KeyboardKey.F1)) InfoDraw = !InfoDraw;

            if (GameSettings.Default.Cheats == true)
            {
                if (IsKeyPressed(KeyboardKey.F3)) DebugInfoDraw = !DebugInfoDraw;
                if (IsKeyPressed(KeyboardKey.N)) noclip = !noclip;
                if (IsKeyPressed(KeyboardKey.End)) { currentScreen = GameScreen.Results; }
            }

            Speed = IsKeyDown(KeyboardKey.LeftShift) ? 0.04f : 0.02f;
        }
    }
}