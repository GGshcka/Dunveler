using Raylib_CsLo;
using static Raylib_CsLo.RayGui;
using static Raylib_CsLo.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using static Dunveler.UI;
using System.Numerics;

namespace Dunveler
{
    class Results
    {
        public static float btnX = screenPercent("Width", 50);

        public static void Draw()
        {
            Raylib_cs.Raylib.DrawTextureEx(Info.imgClockTexture, new Vector2(GetScreenWidth() / 2 - (Info.imgClock.Width / 2 * (scale * 2.5f)), -Info.imgClock.Height * scale), 0, scale * 2.5f, Raylib_cs.Color.Gray);
            DrawTextEx(font, Labyrinth.TimerText, new Vector2(GetScreenWidth() / 2 - ((16 * scale * 2)), 8), (10 * scale * 2), 1, PURPLE);

            if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnY/2, btnSize125, btnSize50), mainMenuButtonReturnToMenu))
            {
                currentScreen = GameScreen.MainMenu;
            }
        }
    }
}
