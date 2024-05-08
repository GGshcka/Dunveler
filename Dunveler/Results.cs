using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using static Dunveler.UI;
using System.Numerics;
using System.Diagnostics;

namespace Dunveler
{
    class Results
    {
        public static float btnX = ScreenPercent("Width", 50);

        public static void Draw()
        {
            Raylib_cs.Raylib.DrawTextureEx(Info.imgClockTexture, new Vector2(GetScreenWidth() / 2 - (Info.imgClock.Width / 2 * (scale * 2.5f)), -Info.imgClock.Height * scale), 0, scale * 2.5f, Raylib_cs.Color.Gray);
            DrawTextEx(font, Labyrinth.TimerText, new Vector2(GetScreenWidth() / 2 - ((16 * scale * 2)), 8), (10 * scale * 2), 1, PURPLE);

            if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnY/2, btnSize125, btnSize50), GuiIconText(130, mainMenuButtonMainMenu)) == 1)
            {
                currentScreen = GameScreen.MainMenu;
                Raylib_cs.Raylib.UnloadTexture(Info.imgClockTexture);
            }
        }
    }
}
