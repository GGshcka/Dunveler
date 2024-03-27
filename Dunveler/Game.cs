using System.Globalization;
using Raylib_cs;
using static Raylib_cs.Raylib;
using static Dunveler.Resources.Resources;
using System.Numerics;
using System.ComponentModel;

namespace Dunveler;

internal static class Game
{
    public static int screenWidth = GetScreenWidth();
    public static int screenHeight = GetScreenHeight();

    public enum GameScreen { Logo = 0, MainMenu, Gameplay, Results }
    public static GameScreen currentScreen = GameScreen.Logo;

    public static bool exitWindow = false, drawSettings = false;

    public static Image icon = LoadImageFromMemory(".png", dunveler_icon);

    public static unsafe void Main()
    {
        Culture = CultureInfo.GetCultureInfo("en");

        InitWindow(screenWidth, screenHeight, $"Dunveler - {splashText}");
        InitAudioDevice();
        SetExitKey(KeyboardKey.Null);
        SetTargetFPS(60);
        DisableCursor();
        SetWindowIcon(icon);
        SetConfigFlags(ConfigFlags.VSyncHint);
        ToggleFullscreen();

        Texture2D iconTexture = LoadTextureFromImage(icon);

        UI.Start();

        GameScreen previousGameScreen = GameScreen.Logo;
        int framesCounter = 0;

        while (!exitWindow)
        {
            if (WindowShouldClose()) exitWindow = true;

            switch (currentScreen)
            {
                case GameScreen.Logo:
                    framesCounter++;

                    if (framesCounter > 240)
                    {
                        currentScreen = GameScreen.MainMenu;
                    }
                    break;

                case GameScreen.MainMenu:
                    if (previousGameScreen != currentScreen)
                    {
                        if (IsCursorHidden() == true) EnableCursor();
                        previousGameScreen = GameScreen.MainMenu;
                    }
                    break;

                case GameScreen.Gameplay:
                    if (previousGameScreen != currentScreen)
                    {
                        DisableCursor();
                        Labyrinth.TimerClear();
                        LabyrinthGenerator.Generate();
                        Labyrinth.Start(LabyrinthGenerator.DrawMaze());
                        Player.CameraStart();
                        previousGameScreen = GameScreen.Gameplay;
                    }
                    break;

                case GameScreen.Results:
                    if (previousGameScreen != currentScreen)
                    {
                        if (IsCursorHidden() == true) EnableCursor();
                        previousGameScreen = GameScreen.Results;
                    }
                    break;
            }

            BeginDrawing();

            ClearBackground(ColorFromHSV(10.71f, 0.549f, 0.1f));

            switch (currentScreen)
            {
                case GameScreen.Logo:
                    int scale = 1;
                    DrawTextureEx(iconTexture, new Vector2(GetScreenWidth()/2 - icon.Width/2*scale, GetScreenHeight()/2 - icon.Height/2*scale), 0, scale, ColorAlpha(Color.White, (float)framesCounter/240));
                    break;

                case GameScreen.MainMenu:
                    MainMenu.Draw();
                    break;

                case GameScreen.Gameplay:
                    Labyrinth.Draw();
                    Labyrinth.Timer();
                    Player.CameraUpdater();
                    Player.Controls();
                    Info.Draw();
                    break;

                case GameScreen.Results:
                    ClearBackground(ColorFromHSV(10.71f, 0.549f, 0.1f));
                    Results.Draw();
                    break;
            }

            if (drawSettings == true) 
            {
                Settings.Draw();
            }

            EndDrawing();
        }

        Labyrinth.Unloading();
        CloseAudioDevice();
        CloseWindow();
    }
}