using static Dunveler.UI;
using Raylib_CsLo;
using static Raylib_CsLo.RayGui;
using static Raylib_CsLo.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using System.Diagnostics;

namespace Dunveler
{
    internal unsafe class Settings
    {
        public static void Draw()
        {
            if (GuiButton(new Rectangle(20, 20, btnSize50, btnSize25), settingsButtonBack))
            {
                drawSettings = false;
            }

            if (GuiButton(new Rectangle(20, 20 + btnSize25 + spacebetween, btnSize25, btnSize25), "<"))
            {
                if (scale > 1)
                {
                    scale--;
                    GameSettings.Default.UIScale = scale;
                    GameSettings.Default.Save();
                    LoadGuiStyleAndSize();
                }
            }

            GuiLabel(new Rectangle(20 + btnSize25 + spacebetween + (spacebetween/2), 20 + btnSize25 + spacebetween, btnSize25, btnSize25), $"{scale}");

            if (GuiButton(new Rectangle(20 + (btnSize25 * 2) + spacebetween, 20 + btnSize25 + spacebetween, btnSize25, btnSize25), ">"))
            {
                if (scale < 3)
                {
                    scale++;
                    GameSettings.Default.UIScale = scale;
                    GameSettings.Default.Save();
                    LoadGuiStyleAndSize();
                }
            }
        }
    }
}
