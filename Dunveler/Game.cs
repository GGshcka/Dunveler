using System.Globalization;
using System.Numerics;
using Dunveler.Resources;
using Raylib_cs;

namespace Dunveler;

class Game
{
    public static void Main()
    {
        Resources.Resources.Culture = CultureInfo.GetCultureInfo("ru");
        Raylib.InitWindow(800, 480, "Dunveler - " + Resources.Resources.splashText);
        Raylib.SetTargetFPS(60);
        //Raylib.SetWindowIcon(icon);

        Font font = Raylib.LoadFont("Resources/Fonts/pixelcyr_normal.ttf");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.SkyBlue);

            Raylib.DrawTextEx(font, Resources.Resources.helloWorldText, new Vector2(10,10), 40, 10, Color.White);

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}