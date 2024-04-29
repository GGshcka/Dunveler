using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.UI;
using static Dunveler.Game;
using static Dunveler.Resources.Resources;

namespace Dunveler 
{
    internal class Pause
    {
        public static bool isPaused = false;

        public static void Draw() {
            if (isPaused == true) 
            {
                DrawRectangle(0, 0, (int)screenPercent("Width", 100), (int)screenPercent("Height", 100), ColorAlpha(ColorFromHSV(10.71f, 0.549f, 0.1f), 0.65f));
                DrawRectangle(0, 0, 150 * scale, (int)screenPercent("Height", 100), ColorFromHSV(10.71f, 0.549f, 0.1f));

                if (drawSettings == false)
                {
                    if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY, btnSize125, btnSize50), mainMenuButtonResume) == 1)
                    {
                        isPaused = false;
                        Player.cur = false;
                        DisableCursor();
                    }
                    if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 + spacebetween, btnSize125, btnSize50), mainMenuButtonSettings) == 1)
                    {
                        drawSettings = true;
                    }
                    if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 * 2 + spacebetween * 2, btnSize125, btnSize50), mainMenuButtonMainMenu) == 1)
                    {
                        isPaused = false;
                        Player.cur = false;
                        currentScreen = GameScreen.MainMenu;
                    }

                    FullLogoDraw();
                }
            }
        }
    }
}
