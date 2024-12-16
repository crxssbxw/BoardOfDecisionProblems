using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace BoardOfDecisionProblems.Encrypt
{
    /// <summary>
    /// Класс для шифрования
    /// </summary>
    public static class DataEncryption
    {
        private static string[] nouns =
            {
                "Adventure",
                "Advice",
                "Affection",
                "Aim",
                "Ambition",
                "Anger",
                "Anxiety",
                "Argument",
                "Art",
                "Attraction",
                "Beauty",
                "Belief",
                "Benefit",
                "Betrayal",
                "Bliss",
                "Blood",
                "Bond",
                "Book",
                "Brain",
                "Breath",
                "Bridge",
                "Brotherhood",
                "Cake",
                "Calm",
                "Career",
                "Care",
                "Chance",
                "Change",
                "Choice",
                "Circumstance",
                "City",
                "Class",
                "Cloud",
                "Coffee",
                "Coincidence",
                "Comfort",
                "Communication",
                "Community",
                "Company",
                "Competition",
                "Conflict",
                "Connection",
                "Consciousness",
                "Conversation",
                "Countryside",
                "Courage",
                "Creation",
                "Crime",
                "Crisis",
                "Culture",
                "Dance",
                "Danger",
                "Decision",
                "Delight",
                "Desire",
                "Destiny",
                "Development",
                "Discussion",
                "Dream",
                "Education",
                "Emotion",
                "Energy",
                "Enthusiasm",
                "Event",
                "Experience",
                "Experiment",
                "Failure",
                "Fame",
                "Family",
                "Fashion",
                "Fear",
                "Feeling",
                "Film",
                "Fire",
                "Flower",
                "Food",
                "Friendship",
                "Fun",
                "Future",
                "Game",
                "Garden",
                "Gift",
                "Goal",
                "Government",
                "Grace",
                "Growth",
                "Happiness",
                "Hatred",
                "Health",
                "History",
                "Home",
                "Hope",
                "Hour",
                "Humanity",
                "Idea",
                "Imagination",
                "Importance",
                "Improvement",
                "Industry",
                "Influence"
            };

        private static string[] adjectives =
            {
                "Amazing",
                "Bright",
                "Brilliant",
                "Charming",
                "Creative",
                "Curious",
                "Daring",
                "Delicate",
                "Dynamic",
                "Elegant",
                "Excellent",
                "Fabulous",
                "Fantastic",
                "Fascinating",
                "Fearless",
                "Flawless",
                "Flexible",
                "Gentle",
                "Gorgeous",
                "Graceful",
                "Happy",
                "Harmonious",
                "Hilarious",
                "Impressive",
                "Incredible",
                "Ingenious",
                "Joyful",
                "Kind",
                "Lovely",
                "Lucky",
                "Magnificent",
                "Marvelous",
                "Modern",
                "Motivated",
                "Noble",
                "Optimistic",
                "Passionate",
                "Perfect",
                "Playful",
                "Powerful",
                "Quick",
                "Quirky",
                "Radiant",
                "Remarkable",
                "Romantic",
                "Sincere",
                "Smart",
                "Successful",
                "Talented",
                "Tender",
                "Timely",
                "Unique",
                "Unstoppable",
                "Vibrant",
                "Vivacious",
                "Wonderful",
                "Witty",
                "Worthy",
                "Zany",
                "Zealous",
                "Active",
                "Adventurous",
                "Amiable",
                "Attractive",
                "Brave",
                "Calm",
                "Careful",
                "Cheerful",
                "Clever",
                "Confident",
                "Courageous",
                "Decisive",
                "Determined",
                "Diligent",
                "Disciplined",
                "Dreamy",
                "Easygoing",
                "Efficient",
                "Enthusiastic",
                "Fair",
                "Faithful",
                "Friendly",
                "Generous",
                "Hardworking",
                "Honest",
                "Hopeful",
                "Humble",
                "Idealistic",
                "Independent",
                "Industrious",
                "Innovative",
                "Intelligent",
                "Keen",
                "Lively",
                "Mature",
                "Mysterious",
                "Noble",
                "Passionate",
                "Playful"
            };
        /// <summary>
        /// Шифрует строку алгоритмом SHA256
        /// </summary>
        /// <param name="str">Шифруемая строка</param>
        /// <returns>Зашифровання строка</returns>
        public static string EncrtyptString(string str)
        {
            SHA256 sha256 = SHA256.Create();
            byte[] bytesstr = Encoding.UTF8.GetBytes(str);
            byte[] hash = sha256.ComputeHash(bytesstr);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        } 

        /// <summary>
        /// Генерирует случайный пароль в виде строки
        /// </summary>
        /// <returns>Пароль</returns>
        public static string RandomPasswordString()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string symbols = "@$#%^&!*?";

            string password = string.Empty;
            string wordpart = string.Empty;
            string symbolpart = string.Empty;
            int numpart;

            for (int i = 0; i < 7; i++)
            {
                wordpart += chars[new Random().Next(chars.Length)];
            }
            symbolpart = symbols[new Random().Next(symbols.Length)].ToString();
            numpart = new Random().Next(0,10);
            password = wordpart + symbolpart + numpart.ToString();
            return password;
        }

        public static string RandomLoginString()
        {
            string n = string.Empty;
            string a = string.Empty;

            n = nouns[new Random().Next(nouns.Length)];
            a = adjectives[new Random().Next(adjectives.Length)];

            return $"{n}_{a}";
        }
    }
}
