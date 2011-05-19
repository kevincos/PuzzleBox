using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Xna.Framework.Storage;

namespace PuzzleBox
{
    
    public class HighScoreData
    {
        public LevelData[] timeAttackLevels;
        public LevelData[] moveChallengeLevels;
        public LevelData[] puzzleLevels;

        public HighScoreData()
        {
            timeAttackLevels = new LevelData[10];
            moveChallengeLevels = new LevelData[10];
            puzzleLevels = new LevelData[10];
        }
    }

    public class LevelData
    {
        public int[] highScores;
        public string[] playerNames;
        public bool played;
        public bool unlocked;
        public int rank;        

        public LevelData()
        {
            highScores = new int[5];
            playerNames = new string[5];
            played = true;
            rank = 2;
        }
    }

    class HighScoreTracker
    {
        public static string highScorePath = "highscores.txt";

        public static LevelData GetHighScoresForLevel(GameMode mode, int level)
        {
            HighScoreTracker.InitializeHighScores();
            HighScoreData data = LoadHighScores();
            if (mode == GameMode.Puzzle)
            {
                return data.puzzleLevels[level];
            }
            else if (mode == GameMode.TimeAttack)
            {
                return data.timeAttackLevels[level];
            }
            else
            {
                return data.moveChallengeLevels[level];
            }
        }

        public static void InitializeHighScores()
        {
            if (false == File.Exists(highScorePath))
            {
                HighScoreData defaultData = new HighScoreData();
                for(int i = 0; i < 3; i++)
                {
                    defaultData.moveChallengeLevels[i] = new LevelData();
                    defaultData.moveChallengeLevels[i].rank = 1;
                    defaultData.moveChallengeLevels[i].played = false;
                    defaultData.moveChallengeLevels[i].unlocked = false;
                    for (int j = 0; j < 5; j++)
                    {
                        defaultData.moveChallengeLevels[i].playerNames[j] = "KRC";
                        defaultData.moveChallengeLevels[i].highScores[j] = (5-j)*100;
                    }
                }
                defaultData.moveChallengeLevels[0].unlocked = true;
                for(int i = 0; i < 10; i++)
                {
                    defaultData.puzzleLevels[i] = new LevelData();
                    defaultData.puzzleLevels[i].rank = 1;
                    defaultData.puzzleLevels[i].played = false;
                    defaultData.puzzleLevels[i].unlocked = false;
                    for (int j = 0; j < 5; j++)
                    {
                        defaultData.puzzleLevels[i].playerNames[j] = "KRC";
                        defaultData.puzzleLevels[i].highScores[j] = (5 + j) * 60000;
                    }
                }
                defaultData.puzzleLevels[0].unlocked = true;
                for(int i = 0; i < 3; i++)
                {
                    defaultData.timeAttackLevels[i] = new LevelData();
                    defaultData.timeAttackLevels[i].rank = 1;
                    defaultData.timeAttackLevels[i].played = false;
                    defaultData.timeAttackLevels[i].unlocked = false;
                    for (int j = 0; j < 5; j++)
                    {
                        defaultData.timeAttackLevels[i].playerNames[j] = "KRC";
                        defaultData.timeAttackLevels[i].highScores[j] = (5 - j) * 100;
                    }
                }
                defaultData.timeAttackLevels[0].unlocked = true;
                SaveHighScores(defaultData);
            }
        }

        public static void SaveHighScores(HighScoreData data)
        {
            
            // Open the file, creating it if necessary
            FileStream stream = File.Open(highScorePath, FileMode.Create);
            
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Flush();
                stream.Close();
            }
        }

        public static HighScoreData LoadHighScores()
        {
            HighScoreTracker.InitializeHighScores();
        
            HighScoreData data;

            
            // Open the file
            FileStream stream = File.Open(highScorePath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                 data = (HighScoreData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);
        }
    }
}
