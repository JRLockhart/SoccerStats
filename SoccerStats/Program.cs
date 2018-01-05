using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using System.Net;

namespace SoccerStats
{
    class Program
    {
        static void Main(string[] args)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);

            var fileName = Path.Combine(directory.FullName, "SoccerGameResults.csv");
            var fileContents = ReadSoccerResults(fileName);
            fileName = Path.Combine(directory.FullName, "players.json");
            var players = DeserializePlayers(fileName);

            foreach(var player in players)
            {
                
            }

            Console.WriteLine(GetGoogleHomePage());
            Console.ReadLine();



        }

        public static string ReadFile(string fileName)
        {
            using (var reader = new StreamReader(fileName))
            {
                return reader.ReadToEnd();
            }
        }

        public static List<GameResult> ReadSoccerResults(string fileName)
        {
            var soccerResults = new List<GameResult>();
            using (var reader = new StreamReader(fileName))
            {
                string line = "";

                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {

                    var gameResult = new GameResult();
                    string[] values = line.Split(',');
                    gameResult.GameDate = DateTime.Parse(values[0]);
                    DateTime gameDate;

                    if(DateTime.TryParse(values[0], out gameDate))
                    {
                        gameResult.GameDate = gameDate;
                    }
                    gameResult.TeamName = values[1];
                    HomeOrAway homeOrAway;
                    if (Enum.TryParse(values[2], out homeOrAway))
                    {
                        gameResult.HomeOrAway = homeOrAway;
                    }

                    int parseInt;
                    if(int.TryParse(values[3], out parseInt))
                    {
                        gameResult.Goals = parseInt;
                    }
                    if (int.TryParse(values[4], out parseInt))
                    {
                        gameResult.GoalAttempts = parseInt;
                    }
                    if (int.TryParse(values[5], out parseInt))
                    {
                        gameResult.ShotsOnGoal = parseInt;
                    }
                    if (int.TryParse(values[6], out parseInt))
                    {
                        gameResult.ShotsOffGoal = parseInt;
                    }

                    double possessionPercent;
                    if(double.TryParse(values[7], out possessionPercent))
                    {
                        gameResult.PosessionPercent = possessionPercent;
                    }

                   
                    soccerResults.Add(gameResult);
                }  
            }
            return soccerResults;
        }

        public static List<Player> DeserializePlayers(string fileName)
        {
            var players = new List<Player>();
            var serializer = new JsonSerializer();
            using (var reader = new StreamReader(fileName))
                using (var jsonReader = new JsonTextReader(reader))
            {
                serializer.Deserialize<List<Player>>(jsonReader);
            }
           
            return players;
        }

        public static List<Player> GetTopTenPlayers(List<Player> players)
        {
            var topTenPlayers = new List<Player>();
            players.Sort(new PlayerComparer());
            int counter = 0;
            foreach(var player in players)
            {
                topTenPlayers.Add(player);
                counter++;
                if (counter == 10)
                    break;
            }
            return players;
        }

        public static void SerializePlayerToFile(List<Player> players, string fileName)
        {

        }

        public static string GetGoogleHomePage()
        {
            var webClient = new WebClient();
            byte[] googleHome = webClient.DownloadData("https://www.google.com");

            using (var stream = new MemoryStream(googleHome))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        public static string GetNewsForPlayer(string playerName)
        {
            var webClient = new WebClient();
            byte[] searchResults = webClient.DownloadData("https://www.google.com");

            using (var stream = new MemoryStream(searchResults))
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
