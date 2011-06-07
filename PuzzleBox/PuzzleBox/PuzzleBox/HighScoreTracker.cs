﻿using System;
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
        public bool displayHelp = true;
        public bool musicEnabled = true;
        public bool soundEffectsEnabled = true;
        public bool fullScreen = false;
        public bool wideScreen = false;
        public bool keyboardControls = false;
        public LevelData[] timeAttackLevels;
        public LevelData[] moveChallengeLevels;
        public LevelData[] puzzleLevels;

        public HighScoreData()
        {
            timeAttackLevels = new LevelData[10];
            moveChallengeLevels = new LevelData[10];
            puzzleLevels = new LevelData[20];
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
            rank = 0;
        }
    }

    public class HighScoreTracker
    {
        public static StorageDevice device = null;
        public static string highScorePath = "Data\\highscores.txt";

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
            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            if(false == container.FileExists("highscores.sav"))
            {

                HighScoreData defaultData = new HighScoreData();
                for(int i = 0; i < 3; i++)
                {
                    defaultData.moveChallengeLevels[i] = new LevelData();
                    defaultData.moveChallengeLevels[i].rank = 0;
                    defaultData.moveChallengeLevels[i].played = false;
                    defaultData.moveChallengeLevels[i].unlocked = false;
                    defaultData.moveChallengeLevels[i].playerNames[0] = "JLY";
                    defaultData.moveChallengeLevels[i].highScores[0] = 500*(i+1);
                    defaultData.moveChallengeLevels[i].playerNames[1] = "KEV";
                    defaultData.moveChallengeLevels[i].highScores[1] = 400 * (i + 1);
                    defaultData.moveChallengeLevels[i].playerNames[2] = "SRB";
                    defaultData.moveChallengeLevels[i].highScores[2] = 300 * (i + 1);
                    defaultData.moveChallengeLevels[i].playerNames[3] = "TBL";
                    defaultData.moveChallengeLevels[i].highScores[3] = 200 * (i + 1);
                    defaultData.moveChallengeLevels[i].playerNames[4] = "FRK";
                    defaultData.moveChallengeLevels[i].highScores[4] = 100 * (i + 1);

                }
                defaultData.moveChallengeLevels[0].unlocked = true;
                for(int i = 0; i < 20; i++)
                {
                    defaultData.puzzleLevels[i] = new LevelData();
                    defaultData.puzzleLevels[i].rank = 0;
                    defaultData.puzzleLevels[i].played = false;
                    defaultData.puzzleLevels[i].unlocked = false;
                    
                    defaultData.puzzleLevels[i].playerNames[0] = "JLY";
                    defaultData.puzzleLevels[i].highScores[0] = 60000;
                    defaultData.puzzleLevels[i].playerNames[1] = "KEV";
                    defaultData.puzzleLevels[i].highScores[1] = 120000;
                    defaultData.puzzleLevels[i].playerNames[2] = "SRB";
                    defaultData.puzzleLevels[i].highScores[2] = 180000;
                    defaultData.puzzleLevels[i].playerNames[3] = "TBL";
                    defaultData.puzzleLevels[i].highScores[3] = 240000;
                    defaultData.puzzleLevels[i].playerNames[4] = "FRK";
                    defaultData.puzzleLevels[i].highScores[4] = 300000;
                    
                    if(i<6)
                        defaultData.puzzleLevels[i].unlocked = true;
                }
                defaultData.puzzleLevels[0].unlocked = true;
                for(int i = 0; i < 3; i++)
                {
                    defaultData.timeAttackLevels[i] = new LevelData();
                    defaultData.timeAttackLevels[i].rank = 0;
                    defaultData.timeAttackLevels[i].played = false;
                    defaultData.timeAttackLevels[i].unlocked = false;
                    defaultData.timeAttackLevels[i].playerNames[0] = "JLY";
                    defaultData.timeAttackLevels[i].highScores[0] = 500 * (i + 1);
                    defaultData.timeAttackLevels[i].playerNames[1] = "KEV";
                    defaultData.timeAttackLevels[i].highScores[1] = 400 * (i + 1);
                    defaultData.timeAttackLevels[i].playerNames[2] = "SRB";
                    defaultData.timeAttackLevels[i].highScores[2] = 300 * (i + 1);
                    defaultData.timeAttackLevels[i].playerNames[3] = "TBL";
                    defaultData.timeAttackLevels[i].highScores[3] = 200 * (i + 1);
                    defaultData.timeAttackLevels[i].playerNames[4] = "FRK";
                    defaultData.timeAttackLevels[i].highScores[4] = 100 * (i + 1);

                }
                defaultData.timeAttackLevels[0].unlocked = true;
                SaveHighScores(defaultData);
            }
        }

        public static void SaveHighScores(HighScoreData data)
        {
            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            // Open the file, creating it if necessary
            //FileStream stream = File.Open(highScorePath, FileMode.Create);
            Stream stream = container.OpenFile("highscores.sav", FileMode.Create);
            
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

            // Open a storage container.
            IAsyncResult result =
                device.BeginOpenContainer("StorageDemo", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();

            // Open the file
            Stream stream = container.OpenFile("highscores.sav",FileMode.Open);
            //FileStream stream = File.Open(highScorePath, FileMode.OpenOrCreate,
            //FileAccess.Read);
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
