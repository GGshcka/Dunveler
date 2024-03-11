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
        Image img = LoadImageFromMemory(".png", Resources.Resources.dunvelerIcon);

        InitWindow(screenWidth, screenHeight, $"Dunveler - {Resources.Resources.splashText}");
        SetTargetFPS(60);
        DisableCursor();
        SetWindowIcon(img);
        SetExitKey(KeyboardKey.Escape);
        SetConfigFlags(ConfigFlags.VSyncHint);
        ToggleFullscreen();

        Player.CameraStart();
        Labyrinth.Start();

        while (!WindowShouldClose())
        {
            BeginDrawing();
            Labyrinth.Draw();

            Player.CameraUpdater();
            Player.Controls();

            DebugInfo.Draw();

            EndDrawing();
        }

        Labyrinth.Unloading();
        CloseWindow();
    }
}