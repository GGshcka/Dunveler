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
        static float sensTemp = GameSettings.Default.Sensitivity * 100;
        static int scaleTemp = scale, sensTempAsInt = (int)sensTemp;

        public static void Draw()
        {
            if (GuiButton(new Rectangle(10, GetScreenHeight() - 10 - btnSize25, btnX - 20, btnSize25), GuiIconText(112, settingsButtonBackAndApply)) == 1)
            {
                drawSettings = false;
                GameSettings.Default.Sensitivity = Player.Sens = sensTempAsInt / 100f;
                GameSettings.Default.UIScale = scale = scaleTemp;
                GameSettings.Default.Save();
                LoadGuiStyleAndSize();
            }

            GuiLabel(new Rectangle(10, 0, btnSize125*2, btnSize25), interfaceScaleSettingsNameText);

            GuiSpinner(new Rectangle(10, btnSize25, btnX - 20, btnSize25), null, ref scaleTemp, 1, 3, false);

            GuiLabel(new Rectangle(10, 10 + btnSize50, btnSize125*2, btnSize25), "Sensitivity:");

            GuiSpinner(new Rectangle(10, 10 + btnSize75, btnX - 20, btnSize25), null, ref sensTempAsInt, 1, 100, false);
        }
    }
}
