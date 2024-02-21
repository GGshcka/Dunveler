using System.Globalization;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace Dunveler;

internal static class Game
{
    public static int screenWidth = GetScreenWidth();
    public static int screenHeight = GetScreenHeight();

    public static void Main()
    {
        Resources.Resources.Culture = CultureInfo.GetCultureInfo("en");
        Image img = LoadImageFromMemory(".png", Resources.Resources.dunveler_icon);

        InitWindow(screenWidth, screenHeight, $"Dunveler - {Resources.Resources.splashText}");
        SetTargetFPS(60);
        DisableCursor();
        SetWindowIcon(img);
        SetExitKey(KeyboardKey.Escape);
        SetConfigFlags(ConfigFlags.VSyncHint);
        ToggleFullscreen();

        Player.PlayerCameraStart();
        Labyrinth.StartLabyrinth();

        while (!WindowShouldClose())
        {
            BeginDrawing();
            Labyrinth.DrawLabyrinth();

            Player.PlayerCamera();
            Player.Controls();

            DrawFPS(10, 10);
            EndDrawing();
        }

        Labyrinth.UnloadLabrinth();
        CloseWindow();
    }
}