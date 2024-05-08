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

    public static bool exitWindow = false, drawSettings = false, drawDifficulty = false, drawAddUser = false;

    public static Image icon = LoadImageFromMemory(".png", dunveler_icon), logoText = LoadImageFromMemory(".png", dunveler_logo_text), logoTextShadow = LoadImageFromMemory(".png", dunveler_logo_textshadow);
    public static Texture2D iconTexture, logoTextTexture, logoTextShadowTexture;

    public static string currentDifficult;

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

        Music mainMenuSongMusic = LoadMusicStream("Resources\\Sounds\\mainMenuSong.mp3");

        iconTexture = LoadTextureFromImage(icon);
        logoTextTexture = LoadTextureFromImage(logoText);
        logoTextShadowTexture = LoadTextureFromImage(logoTextShadow);

        Leaderboard.Start();
        Leaderboard.UserControler();
        MainMenu.StyleTaker();
        UI.Start();

        GameScreen previousGameScreen = GameScreen.Logo;
        int framesCounter = 0;

        while (!exitWindow)
        {
            if (WindowShouldClose()) exitWindow = true;

            UpdateMusicStream(mainMenuSongMusic); 

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
                        PlayMusicStream(mainMenuSongMusic);
                        MainMenu.StyleTaker();
                        Leaderboard.Get();
                        if (IsCursorHidden() == true) EnableCursor();
                        Difficulty.readyToUse = false;
                        previousGameScreen = GameScreen.MainMenu;
                    }
                    break;

                case GameScreen.Gameplay:
                    if (previousGameScreen != currentScreen)
                    {
                        StopMusicStream(mainMenuSongMusic);
                        DisableCursor();
                        Info.Start();
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
                        Leaderboard.Rewrite(currentDifficult);
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
                    ClearBackground(Color.Black);
                    MainMenu.Draw();
                    if (drawDifficulty == true) Difficulty.Draw();
                    if (Leaderboard.drawLeaderboard == true) Leaderboard.Draw();
                    AddUser.Draw();
                    break;

                case GameScreen.Gameplay:
                    Labyrinth.Draw();
                    Labyrinth.Timer();
                    Player.CameraUpdater();
                    Player.Controls();
                    if (Player.InfoDraw == true) Info.Draw();
                    Pause.Draw();
                    break;

                case GameScreen.Results:
                    ClearBackground(ColorFromHSV(10.71f, 0.549f, 0.1f));
                    Results.Draw();
                    break;
            }

            if (drawSettings == true) Settings.Draw();

            EndDrawing();
        }

        Labyrinth.Unloading();
        UnloadMusicStream(mainMenuSongMusic);
        CloseAudioDevice();
        CloseWindow();
    }
}