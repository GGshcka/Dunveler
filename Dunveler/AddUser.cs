using static Dunveler.UI;
using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using System.Diagnostics;
using System.Xml.Linq;

namespace Dunveler
{
    internal class AddUser
    {
        const int MAX_INPUT_CHARS = 12;

        static bool mouseOnText = false, editMode, deleteProtection = false;
        static int usersCount = 0;
        static Rectangle textBox;
        public static string textFromTextBox = "", errorString = "", waitForDeleting = Leaderboard.usernames[usersCount];

        public static void Draw()
        {
            if (drawAddUser == true)
            {
                TextBox();

                GuiLabel(new Rectangle(10, 10, btnSize125*2, btnSize25), "User to delete:");
                GuiLabel(new Rectangle(10, 20 + btnSize50, btnSize125*2, btnSize25), "Enter a nickname:");

                if (GuiButton(new Rectangle(10, GetScreenHeight() - 30 - btnSize75, btnSize150 - 20, btnSize25), addUserButtonAdd) == 1) 
                {
                    errorString = "";
                    Leaderboard.AddUserInXML(textFromTextBox);
                    if (errorString == "") 
                    {
                        Leaderboard.UserControler();
                        drawAddUser = false; 
                    } 
                }
                if (GuiButton(new Rectangle(10, GetScreenHeight() - 10 - btnSize25, btnSize150 - 20, btnSize25), settingsButtonBack) == 1) { drawAddUser = false; }

                if (GuiDropdownBox(new Rectangle(10, 10 + btnSize25, btnSize150 - 20, btnSize25), Leaderboard.usernamesDropdown, ref usersCount, editMode) == 1)
                {
                    editMode = !editMode;
                    if (editMode == false)
                    {
                        waitForDeleting = Leaderboard.usernames[usersCount];
                    }
                    else deleteProtection = true;
                }

                if (GuiButton(new Rectangle(10, GetScreenHeight() - 20 - btnSize50, btnSize150 - 20, btnSize25), mainMenuButtonNewUser) == 1) 
                {
                    if (deleteProtection == true) {
                        Leaderboard.UserDelete(waitForDeleting);
                        Leaderboard.UserControler();
                        Leaderboard.Get();
                        usersCount = 0;
                    }
                    deleteProtection = false;
                }
            }
            else textFromTextBox = errorString = "";
        }

        static char[] invalidChars = { '!', '"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '/', ';', '<', '=', '>', '?', '@', '[', '\\', ']', '^', '`', '{', '|', '}', '~', ' ' };

        static void TextBox()
        {
            textBox = new Rectangle(10, 20 + btnSize75, btnX - 20, btnSize25);

            if (CheckCollisionPointRec(GetMousePosition(), textBox)) mouseOnText = true;
            else mouseOnText = false;

            if (mouseOnText)
            {
                // Set the window's cursor to the I-Beam
                SetMouseCursor(MouseCursor.MOUSE_CURSOR_IBEAM);

                // Get char pressed (unicode character) on the queue
                int key = GetCharPressed();
                bool invalid = false;

                // Check if more characters have been pressed on the same frame
                while (key > 0)
                {
                    // NOTE: Only allow keys in range [32..125]
                    if ((key >= 32) && (key <= 125) && (textFromTextBox.Length < MAX_INPUT_CHARS))
                    {
                        foreach (char c in invalidChars)
                        {
                            if ((char)key == c) invalid = true;
                        }
                        
                        if (invalid == false) textFromTextBox += (char)key;
                    }

                    key = GetCharPressed();  // Check next character in the queue
                }

                if (IsKeyPressed(KeyboardKey.KEY_BACKSPACE) && textFromTextBox.Length > 0)
                {
                    textFromTextBox = textFromTextBox.Substring(0, textFromTextBox.Length - 1);
                }
            }
            else SetMouseCursor(MouseCursor.MOUSE_CURSOR_DEFAULT);

            DrawRectangleRec(textBox, ColorFromHSV(15, 0.5581f, 0.3373f));
            if (mouseOnText) DrawRectangleLinesEx(new Rectangle((int)textBox.X, (int)textBox.Y, (int)textBox.width, (int)textBox.height), scale+1, ColorFromHSV(48.17f, 0.8455f, 0.9647f));
            else DrawRectangleLinesEx(new Rectangle((int)textBox.X, (int)textBox.Y, (int)textBox.width, (int)textBox.height), scale+1, ColorFromHSV(30.9f, 0.7791f, 0.6745f));

            DrawTextEx(font, textFromTextBox, new System.Numerics.Vector2((int)textBox.X + 5 + scale, (int)textBox.Y + (6 * scale)), 6 * scale * 2, scale, ColorFromHSV(48.17f, 0.8455f, 0.9647f));

            DrawTextEx(font, errorString, new System.Numerics.Vector2(10, btnSize25 + textBox.Y + 10), 6 * scale, scale, RED);
        }
    }
}
