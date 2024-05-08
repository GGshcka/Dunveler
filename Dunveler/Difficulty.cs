using static Dunveler.UI;
using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using System.Diagnostics;

namespace Dunveler
{
    internal class Difficulty
    {
        public static bool readyToUse = false; //

        public static int difficultIndex = 0;

        public static void Draw()
        {
            if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY, btnSize125, btnSize50), GuiIconText(149, difficultyEasy)) == 1 && readyToUse == true)
            {
                LabyrinthGenerator.WIDTH = LabyrinthGenerator.HEIGHT = 23;
                difficultIndex = 0;
                currentDifficult = "easy";
                currentScreen = GameScreen.Gameplay;
                drawDifficulty = false;
            }

            if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 + spacebetween, btnSize125, btnSize50), GuiIconText(150, difficultyMedium)) == 1 && readyToUse == true)
            {
                LabyrinthGenerator.WIDTH = LabyrinthGenerator.HEIGHT = 33;
                difficultIndex = 1;
                currentDifficult = "medium";
                currentScreen = GameScreen.Gameplay;
                drawDifficulty = false;
            }

            if (GuiButton(new Rectangle(btnX - btnSize125 / 2, btnY + btnSize50 * 2 + spacebetween * 2, btnSize125, btnSize50), GuiIconText(152, difficultyHard)) == 1 && readyToUse == true)
            {
                LabyrinthGenerator.WIDTH = LabyrinthGenerator.HEIGHT = 43;
                currentScreen = GameScreen.Gameplay;
                difficultIndex = 2;
                currentDifficult = "hard";
                drawDifficulty = false;
            }

            readyToUse = true;

            if (GuiButton(new Rectangle(10 + spacebetween + btnSize25, GetScreenHeight() - 10 - btnSize25, btnSize50, btnSize25), GuiIconText(130, settingsButtonBack)) == 1) { drawDifficulty = false; readyToUse = false; }
        }
    }
}
