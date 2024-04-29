using static Dunveler.UI;
using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using System.Diagnostics;

namespace Dunveler
{
    internal unsafe class Settings
    {
        static int scaleTemp = scale;

        public static void Draw()
        {
            if (GuiButton(new Rectangle(10, GetScreenHeight() - 10 - btnSize25, btnX - 20, btnSize25), settingsButtonBackAndApply) == 1)
            {
                drawSettings = false;
                scale = scaleTemp;
                GameSettings.Default.UIScale = scale;
                GameSettings.Default.Save();
                LoadGuiStyleAndSize();
            }

            GuiLabel(new Rectangle(10, 0, btnSize125*2, btnSize25), interfaceScaleSettingsNameText);

            GuiSpinner(new Rectangle(10, btnSize25, btnX - 20, btnSize25), null, ref scaleTemp, 1, 3, false);
        }
    }
}
