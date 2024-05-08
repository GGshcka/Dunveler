using System.Diagnostics;
using static Dunveler.UI;
using ZeroElectric.Vinculum;
using static ZeroElectric.Vinculum.RayGui;
using static ZeroElectric.Vinculum.Raylib;
using static Dunveler.Resources.Resources;
using static Dunveler.Game;
using System.Xml;
using System.Numerics;

namespace Dunveler
{
    internal class Leaderboard
    {
        public static List<string> usernames = new(), usersInLeaderboard = new(), difficultsNames = new() { "easy", "medium", "hard", "EASY", "MEDIUM", "HARD" };
        public static List<float> timesInLeaderboard = new();

        public static string usernamesDropdown;
        public static int difficultActive = 0;
        public static bool drawLeaderboard = false, editMode = false;

        static XmlDocument leaderboardFile = new XmlDocument();
        static XmlElement? leaderboardFileRoot;

        public static void Start()
        {
            leaderboardFile.Load("Resources\\leaderboard.xml"); 
            leaderboardFileRoot = leaderboardFile.DocumentElement;
        }

        static Vector2 scroll = new Vector2(0, 0);
        
        public static unsafe void Draw()
        {            
            if (GuiButton(new Rectangle(10, GetScreenHeight() - 10 - btnSize25, 150 * 3 * scale - 20, btnSize25), GuiIconText(130, settingsButtonBack)) == 1) drawLeaderboard = false;

            Rectangle bounds = new Rectangle(10, 20 + btnSize25, 150 * 3 * scale - 20, 500);
            Rectangle content = new Rectangle(0, 0, bounds.Width - 15, 1000);
            Rectangle view = new Rectangle(0, 0, bounds.Width, 200);

            GuiScrollPanel(bounds, null, content, ref scroll, ref view);
            
            Vector2 drawPosition = new Vector2(15, 25 + btnSize25) + scroll;
            
            Debug.WriteLine($"Users find counter: {usersInLeaderboard.Count} in {difficultsNames[difficultActive]}");

            for (int i = 0; i < usersInLeaderboard.Count; i++) 
            {
                int shift = (10 * scale + 5) * i; 

                if (drawPosition.Y + shift > bounds.Y) { GuiLabel(new Rectangle(15, 25 + btnSize25 + shift + scroll.Y, btnSize125 * 2, 10 * scale), usersInLeaderboard[i]); }
            }

            if (GuiDropdownBox(new Rectangle(10, 10, 150 * 3 * scale - 20, btnSize25), $"{difficultsNames[3]};{difficultsNames[4]};{difficultsNames[5]}", ref difficultActive, editMode) == 1) { editMode = !editMode; Get(); }
        }

        public static void Get()
        {
            usersInLeaderboard.Clear();
            timesInLeaderboard.Clear();

            if (leaderboardFileRoot != null)
            {
                foreach (XmlElement difficultNode in leaderboardFileRoot)
                {
                    if (difficultNode.Name == difficultsNames[difficultActive])
                    {
                        int i = 0;

                        foreach (XmlNode? userNode in difficultNode.ChildNodes)
                        {
                            usersInLeaderboard.Add($"{userNode?.Name}: {userNode?.Attributes?.GetNamedItem("time")?.Value}");
                            float.TryParse(userNode?.Attributes?.GetNamedItem("time")?.Value?.Replace(":", "."), out float resultTime);
                            Debug.WriteLine(resultTime);
                            timesInLeaderboard.Add(resultTime);
                            i++;
                        }
                    }
                }
            }

            Sorting(timesInLeaderboard, usersInLeaderboard);
        }

        public static bool userFounded = false;

        public static void Rewrite(string currentDifficult)
        {
            if (leaderboardFileRoot != null)
            {
                // обход всех узлов в корневом элементе
                foreach (XmlElement difficultNode in leaderboardFileRoot)
                {
                    if (difficultNode.Name == currentDifficult)
                    {
                        foreach (XmlNode? userNode in difficultNode.ChildNodes)
                        {
                            if (userNode?.Name == Player.playername)
                            {
                                float.TryParse(userNode?.Attributes?.GetNamedItem("time")?.Value?.Replace(":", "."), out float resultTime);
                                float.TryParse(Labyrinth.TimerText.Replace(":", "."), out float timeAsFloat);

                                if (resultTime >= timeAsFloat)
                                {
                                    userNode.Attributes.GetNamedItem("time").Value = $"{Labyrinth.TimerText}";

                                    Debug.WriteLine($"In {difficultNode.Name} mode changed for {userNode.Name}. New time is {userNode.Attributes.GetNamedItem("time")?.Value}");
                                }
                                else Debug.WriteLine($"In {difficultNode.Name} mode dosen't changed for {userNode?.Name}");

                                userFounded = true;
                            }
                        }

                        if (userFounded == false)
                        {
                            XmlElement newUserNode = leaderboardFile.CreateElement(Player.playername);
                            XmlAttribute newUserTimeAttr = leaderboardFile.CreateAttribute("time");

                            newUserNode.Attributes.SetNamedItem(newUserTimeAttr).Value = $"{Labyrinth.TimerText}";

                            difficultNode.AppendChild(newUserNode);

                            foreach (XmlNode userNode in difficultNode.ChildNodes)
                            {
                                if (userNode.Name == Player.playername)
                                {
                                    Debug.WriteLine($"In {difficultNode.Name} mode created user {userNode.Name} with {userNode?.Attributes?.GetNamedItem("time")?.Value}");
                                }
                            }
                        }
                    }
                }

                leaderboardFile.Save("Resources\\leaderboard.xml");
                userFounded = false;
            }
        }

        public static void UserControler() 
        {
            usernamesDropdown = "";
            usernames.Clear();

            if (leaderboardFileRoot != null)
            {
                foreach (XmlElement difficultNode in leaderboardFileRoot) 
                {
                    if (difficultNode.Name == "players")
                    {
                        int i = 0;

                        Debug.WriteLine("INFO: ALL USERS:");
                        foreach (XmlNode playerNode in difficultNode.ChildNodes) { usernames.Add(playerNode.Name); Debug.WriteLine($"       |-> {playerNode.Name}"); }
                        i++;
                        break;
                    }
                }
            }

            foreach (string f in usernames) usernamesDropdown += $"{f};";
            usernamesDropdown = usernamesDropdown.Remove(usernamesDropdown.Length - 1);

            userFounded = false;
        }

        public static void AddUserInXML(string username)
        {
            if (leaderboardFileRoot != null)
            {
                foreach (XmlElement difficultNode in leaderboardFileRoot)
                {
                    if (difficultNode.Name == "players")
                    {
                        if (username.Length <= 0) break; 

                        foreach (XmlNode playerNode in difficultNode.ChildNodes)
                        {
                            if (playerNode.Name == username)
                            {
                                AddUser.errorString = "Same user exist!";
                                userFounded = true;
                                break;
                            }
                        }

                        if (difficultNode.ChildNodes.Count == 6)
                        {
                            AddUser.errorString = "Too many users! (6)";
                            userFounded = true;
                            break;
                        }

                        if (userFounded == false)
                        {
                            XmlElement newUserNode = leaderboardFile.CreateElement(username);
                            difficultNode.AppendChild(newUserNode);

                            leaderboardFile.Save("Resources\\leaderboard.xml");
                        }
                    }
                }
                userFounded = false;
            }
        }

        public static void UserDelete(string user) {
            if (leaderboardFileRoot != null) {
                foreach (XmlElement difficultNode in leaderboardFileRoot) {
                    foreach (XmlNode playerNode in difficultNode.ChildNodes) {
                        if (playerNode.Name == user && difficultNode.ChildNodes.Count > 1) {
                            difficultNode.RemoveChild(playerNode);
                        }
                    }
                }
                leaderboardFile.Save("Resources\\leaderboard.xml");
            }
        }

        public static void Sorting(List<float> anFloatList, List<string> anStringList)
        {
            //Основной цикл (количество повторений равно количеству элементов массива)
            for (int i = 0; i < anFloatList.Count - 1; i++)
            {
                //Вложенный цикл (количество повторений, равно количеству элементов массива минус 1 и минус количество выполненных повторений основного цикла)
                for (int j = 0; j < anFloatList.Count - 2 - i; j++)
                {
                    //Если элемент массива с индексом j больше следующего за ним элемента
                    if (anFloatList[j] > anFloatList[j + 1])
                    {
                        float tmpFloat = anFloatList[j];
                        anFloatList[j] = anFloatList[j + 1];
                        anFloatList[j + 1] = tmpFloat;

                        string tmpString = anStringList[j];
                        anStringList[j] = anStringList[j + 1];
                        anStringList[j + 1] = tmpString;
                    }
                }
            }
        }
    }
}
