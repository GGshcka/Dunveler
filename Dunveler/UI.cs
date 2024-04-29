using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
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
            btnX = 150 * scale,
            btnY = screenPercent("Height", 50),
            iconTextureHeight;

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
            btnX = 150 * scale;
            iconTextureHeight = iconTexture.Height * 0.3f * scale;
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

        public static void FullLogoDraw()
        {
            Raylib_cs.Raylib.DrawTextureEx(iconTexture, new System.Numerics.Vector2(btnX - btnSize125 / 2 - icon.Width * (0.1f * scale), btnY - btnY / 2 - icon.Height * (0.1f * scale)), 0, scale * 0.3f, Raylib_cs.Color.White);
            Raylib_cs.Raylib.DrawTextureEx(logoTextShadowTexture, new System.Numerics.Vector2(btnX - btnSize125 / 2 + 3, btnY - btnY / 2 + 3), 0, scale * 2.5f, Raylib_cs.Color.White);
            Raylib_cs.Raylib.DrawTextureEx(logoTextTexture, new System.Numerics.Vector2(btnX - btnSize125 / 2, btnY - btnY / 2), 0, scale * 2.5f, Raylib_cs.Color.White);
        }
    }
}
