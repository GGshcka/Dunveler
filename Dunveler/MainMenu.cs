using Raylib_CsLo;
using static Raylib_CsLo.RayGui;
using static Raylib_CsLo.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.UI;
using static Dunveler.Game;
using System.Globalization;
using System.Diagnostics;
using System.Configuration;
using Microsoft.VisualBasic;

namespace Dunveler
{
    internal class MainMenu
    {
        public static unsafe void Draw()
        {
            if (drawSettings == false)
            {
                if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY, btnSize125, btnSize50), mainMenuButtonPlay))
                {
                    currentScreen = GameScreen.Gameplay;
                }

                if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 + spacebetween, btnSize125, btnSize50), mainMenuButtonSettings))
                {
                    drawSettings = true;
                }

                if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 * 2 + spacebetween * 2, btnSize125, btnSize50), mainMenuButtonExit))
                {
                    exitWindow = true;
                }
            }
        }
    }
}
