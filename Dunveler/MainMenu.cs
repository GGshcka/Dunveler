using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.UI;
using static Dunveler.Game;
using System.Globalization;
using System.Diagnostics;
using System.Configuration;
using Microsoft.VisualBasic;
using System.Numerics;
using System.Threading;
using System.Runtime.CompilerServices;

namespace Dunveler
{
    internal class MainMenu
    {
        public static Raylib_cs.Image imAtlasMap, previewImage;
        static Raylib_cs.Texture2D previewTexture;

        public static void StyleTaker() 
        {
            Raylib_cs.Raylib.UnloadTexture(previewTexture);

            Random randEnv = new();
            Byte[][] mapStyle =
            {
                dungeon_atlas,
                mossy_atlas,
                interworld_atlas,
                midasplace_atlas,
                backrooms_atlas,
                vlad_atlas,
            };
            Byte[][] previewStyle =
            {
                dungeon_preview_tile,
                mossy_preview_tile,
                interworld_preview_tile,
                midasplace_preview_tile,
                backrooms_preview_tile,
                vlad_preview_tile,
            };
            int randValue = randEnv.Next(mapStyle.Length);
            imAtlasMap = Raylib_cs.Raylib.LoadImageFromMemory(".png", mapStyle[randValue]);
            previewImage = Raylib_cs.Raylib.LoadImageFromMemory(".png", previewStyle[randValue]);
            previewTexture = Raylib_cs.Raylib.LoadTextureFromImage(previewImage);

            Raylib_cs.Raylib.UnloadImage(previewImage);
        }

        static int subscale;

        public static int usersCount = 0;
        static bool editMode = false;

        public static unsafe void Draw()
        {
            float w = btnX, h = 0;

            switch (scale)
            {
                case 1:
                    subscale = 3;
                    break;
                case 2:
                    subscale = 2;
                    break;
                case 3:
                    subscale = 1;
                    break;
            }

            while (true) 
            {
                Raylib_cs.Raylib.DrawTextureEx(previewTexture, new Vector2(w, h), 0, 2 * subscale, Raylib_cs.Color.White);
                w += previewTexture.Width * 2 * subscale;

                if (w >= GetScreenWidth()) { w = btnX; h += previewTexture.Height * 2 * subscale; }

                if (h >= GetScreenHeight()) { break; }
            }

            DrawRectangle(0, 0, 150 * scale, (int)screenPercent("Height", 100), ColorFromHSV(10.71f, 0.549f, 0.1f));

            if (drawSettings == false)
            {
                DrawRectanglePro(new Rectangle(btnX - btnSize125 / 2 - icon.Width * (0.1f * scale), btnY - btnY / 2 - icon.Height * (0.1f * scale), screenPercent("Width", 100), iconTextureHeight), new Vector2(0, 0), 0, ColorFromHSV(10.71f, 0.549f, 0.1f));
                FullLogoDraw();
                if (GuiButton(new Rectangle(10, GetScreenHeight() - 10 - btnSize25, btnSize25, btnSize25), "S") == 1) StyleTaker();
            }

            if (drawSettings == false && drawDifficulty == false)
            {
                if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY, btnSize125, btnSize50), mainMenuButtonPlay) == 1) { drawDifficulty = true; }

                if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 + spacebetween, btnSize125, btnSize50), mainMenuButtonSettings) == 1) drawSettings = true;

                if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 * 2 + spacebetween * 2, btnSize125, btnSize50), mainMenuButtonExit) == 1) exitWindow = true;

                if (GuiDropdownBox(new Rectangle(GetScreenWidth() - btnSize125 - spacebetween, btnY - btnY / 2 - icon.Height * (0.1f * scale) - (btnSize50 / 2), btnSize125, btnSize50), Leaderboard.usernamesDropdown, ref usersCount, editMode) == 1)
                {
                    editMode = !editMode;
                    if (editMode == false)
                    {
                        Player.playername = Leaderboard.usernames[usersCount];
                        Debug.WriteLine(Player.playername);
                    }
                }
            }
        }
    }
}
