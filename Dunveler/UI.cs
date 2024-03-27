using Raylib_CsLo;
using static Raylib_CsLo.RayGui;
using static Raylib_CsLo.Raylib;
using static Dunveler.Game;
using static Dunveler.Resources.Resources;
using System.Diagnostics;

namespace Dunveler
{
    internal unsafe class UI
    {
        public static Font font = LoadFont("Resources\\Fonts\\dunveler_base_font.ttf");

        public static int
            scale = GameSettings.Default.UIScale;

        public static float
            btnSize125 = 125 * scale,
            btnSize50 = 50 * scale,
            btnSize25 = 25 * scale,
            spacebetween = 10 * scale,
            btnX = screenPercent("Width", 15),
            btnY = screenPercent("Height", 50);

        public static unsafe void Start()
        {
            LoadGuiStyleAndSize();
            GuiSetFont(font);
        }

        public static unsafe void LoadGuiStyleAndSize()
        {
            switch (scale)
            {
                case 1:
                    GuiLoadStyle("Resources\\UI\\guiStyle_FScale-1.rgs");
                    break;
                case 2:
                    GuiLoadStyle("Resources\\UI\\guiStyle_FScale-2.rgs");
                    break;
                case 3:
                    GuiLoadStyle("Resources\\UI\\guiStyle_FScale-3.rgs");
                    break;
            }

            GuiSetFont(font);

            btnSize125 = 125 * scale;
            btnSize50 = 50 * scale;
            btnSize25 = 25 * scale;
            spacebetween = 10 * scale;
        }

        public static float screenPercent(string direction, float percent)
        {
            switch (direction)
            {
                case "Width":
                    return GetScreenWidth() * (percent / 100f);
                case "Height":
                    return GetScreenHeight() * (percent / 100f);
            }

            return 0;
        }
    }
}
