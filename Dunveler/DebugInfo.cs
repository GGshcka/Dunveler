namespace Dunveler;
using Raylib_cs;
using static Raylib_cs.Raylib;


public static class DebugInfo
{
    private static readonly Color InfoColor = Color.Green;

    public static void Draw()
    {
        if (Player.InfoDraw == true)
        {
            DrawRectangle(10, 10, 292, 108, ColorAlpha(Color.Black, 0.65f));

            DrawText($"| POSITION \n" +
                     $"| X:{Math.Round(Player.Camera.Position.X, 1)}\n" +
                     $"| Y:{Math.Round(Player.Camera.Position.Y, 1)} \n" +
                     $"| Z:{Math.Round(Player.Camera.Position.Z, 1)} \n" +
                     $"|-----------------------------\n" +
                     $"| FPS: {GetFPS()}\n" +
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
            DrawText("|\n|\n|\n|\n|\n|\n|", 300, 10, 20, InfoColor);
        }
    }
}
