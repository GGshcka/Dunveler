using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.UI;
using static Dunveler.Game;

namespace Dunveler;

public static class Info
{
    private static readonly Color InfoColor = Color.Green;
    public static Image imgClock = LoadImageFromMemory(".png", clock);
    public static Texture2D imgClockTexture = LoadTextureFromImage(imgClock);

    public static void Draw()
    {
        if (Player.DebugInfoDraw == true)
        {
            DrawRectangle(10, 10, 292, 123, ColorAlpha(Color.Black, 0.65f));

            DrawText($"| POSITION \n" +
                     $"| X:{Math.Round(Player.Camera.Position.X, 1)}\n" +
                     $"| Y:{Math.Round(Player.Camera.Position.Y, 1)} \n" +
                     $"| Z:{Math.Round(Player.Camera.Position.Z, 1)} \n" +
                     $"|-----------------------------\n" +
                     $"| FPS: {GetFPS()}\n" +
                     $"| NOCLIP (N): {Player.noclip}\n" +
                     $"| Speed: {Math.Round(Player.Speed * 100, 1)} {(IsKeyDown(KeyboardKey.LeftShift) ? "(RUN)" : "(WALK)")}\n"
                     , 10, 10, 20, InfoColor);
            DrawText($"| TARGET \n" +
                     $"| X:{Math.Round(Player.Camera.Target.X, 1)} \n" +
                     $"| Y:{Math.Round(Player.Camera.Target.Y, 1)} \n" +
                     $"| Z:{Math.Round(Player.Camera.Target.Z, 1)}", 130, 10, 20, InfoColor);
            DrawText($"| UP \n" +
                     $"| X:{Math.Round(Player.Camera.Up.X, 1)} \n" +
                     $"| Y:{Math.Round(Player.Camera.Up.Y, 1)} \n" +
                     $"| Z:{Math.Round(Player.Camera.Up.Z, 1)}", 235, 10, 20, InfoColor);
            DrawText("|\n|\n|\n|\n|\n|\n|\n|", 300, 10, 20, InfoColor);
        }

        DrawTextureEx(imgClockTexture, new Vector2(GetScreenWidth() / 2 - (imgClock.Width/2 * (scale*2.5f)), -imgClock.Height * scale), 0, scale*2.5f, Color.LightGray);
        Raylib_CsLo.Raylib.DrawTextEx(font, Labyrinth.TimerText, new Vector2(GetScreenWidth() / 2 - ((16 * (int)scale * 2)), 8), (10 * (int)scale * 2), 1, Raylib_CsLo.Raylib.WHITE);
    }
}
