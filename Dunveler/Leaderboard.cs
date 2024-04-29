using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Dunveler
{
    internal class Leaderboard
    {
        public static List<string> usernames = new List<string>();

        public static string usernamesDropdown;

        static XmlDocument leaderboardFile = new XmlDocument();
        static XmlElement? leaderboardFileRoot;

        public static void Get()
        {
            leaderboardFile.Load("Resources\\leaderboard.xml");
            leaderboardFileRoot = leaderboardFile.DocumentElement;
            if (leaderboardFileRoot != null)
            {
                foreach (XmlElement difficultNode in leaderboardFileRoot)
                {
                    if (difficultNode.Name != "players")
                    {
                        Debug.WriteLine(difficultNode.Name);

                        foreach (XmlNode? userNode in difficultNode.ChildNodes)
                        {
                            Debug.WriteLine($"|{userNode.Name} {userNode.Attributes.GetNamedItem("mins").Value}:{userNode.Attributes.GetNamedItem("sec").Value}");
                        }
                    }
                }
            }
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
                            if (userNode.Name == Player.playername)
                            {
                                int.TryParse(userNode.Attributes.GetNamedItem("mins").Value, out int resultMins);
                                int.TryParse(userNode.Attributes.GetNamedItem("sec").Value, out int resultSec);

                                if (resultMins >= Labyrinth.currentTimeMins)
                                {
                                    if (resultSec > Labyrinth.currentTimeSec)
                                    {
                                        userNode.Attributes.GetNamedItem("mins").Value = $"{Labyrinth.currentTimeMins}";
                                        userNode.Attributes.GetNamedItem("sec").Value = $"{Labyrinth.currentTimeSec}";

                                        Debug.WriteLine($"In {difficultNode.Name} mode changed for {userNode.Name} {userNode.Attributes.GetNamedItem("mins").Value}:{userNode.Attributes.GetNamedItem("sec").Value}");
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine($"In {difficultNode.Name} mode dosen't changed for {userNode.Name}");
                                    Debug.WriteLine($"{resultMins} > {Labyrinth.currentTimeMins} | {userNode.Attributes.GetNamedItem("mins").Value}");
                                    Debug.WriteLine($"{resultSec} > {Labyrinth.currentTimeSec} | {userNode.Attributes.GetNamedItem("sec").Value}");
                                }

                                userFounded = true;
                            }
                        }

                        if (userFounded == false)
                        {
                            XmlElement newUserNode = leaderboardFile.CreateElement(Player.playername);
                            XmlAttribute newUserMinsAttr = leaderboardFile.CreateAttribute("mins");
                            XmlAttribute newUserSecAttr = leaderboardFile.CreateAttribute("sec");

                            newUserNode.Attributes.SetNamedItem(newUserMinsAttr).Value = $"{Labyrinth.currentTimeMins}";
                            newUserNode.Attributes.SetNamedItem(newUserSecAttr).Value = $"{Labyrinth.currentTimeSec}";

                            difficultNode.AppendChild(newUserNode);

                            foreach (XmlNode userNode in difficultNode.ChildNodes)
                            {
                                if (userNode.Name == Player.playername)
                                {
                                    Debug.WriteLine($"In {difficultNode.Name} mode created user {userNode.Name} with {userNode.Attributes.GetNamedItem("mins").Value}:{userNode.Attributes.GetNamedItem("sec").Value}");
                                }
                            }
                        }
                    }
                }

                leaderboardFile.Save("Resources\\leaderboard.xml");
                userFounded = false;
            }
        }

        public static void UserControler( string getset ) 
        {
            if (leaderboardFileRoot != null)
            {
                foreach (XmlElement difficultNode in leaderboardFileRoot) 
                {
                    if (difficultNode.Name == "players")
                    {
                        int i = 0;

                        switch (getset)
                        {
                            case "get":
                                foreach (XmlNode playerNode in difficultNode.ChildNodes) { usernames.Add(playerNode.Name); Debug.WriteLine(playerNode.Name); }
                                i++;
                                break;

                            case "set":
                                foreach (XmlNode playerNode in difficultNode.ChildNodes) if (playerNode.Name == Player.playername) userFounded = true;
                                if (userFounded == false) { XmlElement newUserNode = leaderboardFile.CreateElement(Player.playername); difficultNode.AppendChild(newUserNode); }
                                else Debug.WriteLine("User exist...");
                                break;
                        }
                    }
                }
            }

            foreach (string f in usernames) usernamesDropdown += $"{f};";

            usernamesDropdown = usernamesDropdown.Remove(usernamesDropdown.Length - 1);

            userFounded = false;
        }
    }
}
