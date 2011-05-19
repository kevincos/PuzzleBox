using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace PuzzleBox
{
    class Logger
    {
        public static StreamWriter w;

        public static int totalScore;
        public static int numDoubles;
        public static int pointsFromDoubles;
        public static int numTriples;
        public static int pointsFromTriples;
        public static int numQuads;
        public static int pointsFromQuads;
        public static int numFullLines;
        public static int pointsFromFullLines;
        public static int numGrayOrbs;
        public static int numToggleOrbs;
        public static int numCounterOrbs;
        public static int numTimerOrbs;
        public static int numBonusOrbs;
        public static bool loggingEnabled = false;

        public static void Init()
        {
            if (loggingEnabled)
            {
                String path = "logFile" + DateTime.Now.Day + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".txt";
                try
                {
                    w = new StreamWriter(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static void ClearLogger()
        {
            totalScore = 0;
            numDoubles = 0;
            pointsFromDoubles = 0;
            numTriples = 0;
            pointsFromTriples = 0;
            numQuads = 0;
            pointsFromQuads = 0;
            numFullLines = 0;
            pointsFromFullLines = 0;
            numGrayOrbs = 0;
            numBonusOrbs = 0;
            numCounterOrbs = 0;
            numTimerOrbs = 0;
            numToggleOrbs = 0;
        }

        public static void LogGame()
        {
            if (loggingEnabled)
            {
                try
                {
                    w.WriteLine("GAME:\t" + DateTime.Now + "\t" + totalScore
                        + "\t" + numDoubles
                        + "\t" + pointsFromDoubles
                        + "\t" + numTriples
                        + "\t" + pointsFromTriples
                        + "\t" + numQuads
                        + "\t" + pointsFromQuads
                        + "\t" + numFullLines
                        + "\t" + pointsFromFullLines
                        + "\t" + numGrayOrbs
                        + "\t" + numToggleOrbs
                        + "\t" + numCounterOrbs
                        + "\t" + numTimerOrbs
                        + "\t" + numBonusOrbs);
                }
                catch { }
            }
        }

        public static void CloseLogger()
        {
            if (loggingEnabled)
            {
                w.Flush();
                w.Close();
            }
        }
    }
}
