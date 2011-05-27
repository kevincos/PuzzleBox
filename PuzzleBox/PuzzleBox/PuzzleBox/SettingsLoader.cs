using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzleBox
{
    class SettingsLoader
    {
        public static Settings Tutorial()
        {
            Settings s = new Settings();
            s.allowResets = false;
            s.displayScore = true;
            s.displayTimer = false;
            s.texture = JellyfishRenderer.orangeJelly;
            s.countdownTimer = false;
            s.difficulty = Difficulty.EASY;
            s.randomOrbs = false;
            s.mode = GameMode.Tutorial;
            s.refillQueues = false;
            s.instructions = "";
            s.two_star = 120000;
            s.three_star = 30000;
            return s;
        }

        public static List<Settings> LoadPuzzleLevels()
        {
            List<Settings> levels = new List<Settings>();
            for(int i =0; i < 20; i++)
            {                
                Settings s = new Settings();
                s.allowResets = false;
                s.displayScore = false;
                s.displayTimer = true;                
                s.countdownTimer = false;
                s.difficulty = Difficulty.EASY;
                s.randomOrbs = false;
                s.mode = GameMode.Puzzle;
                s.refillQueues = false;
                s.instructions = "";
                s.two_star = 120000;
                s.three_star = 30000;
                levels.Add(s);
            }
            levels[0].levelfilename = "allgraytemplate.txt";
            levels[0].name = "Template Level";
            levels[0].texture = JellyfishRenderer.baseballJelly2;
            levels[1].levelfilename = "puzzle1.txt";
            levels[1].name = "Level1";
            levels[1].instructions = "N:Doctor! This patient has severe allergies to \nRED toxins!-D:Got it! We'll have to be extra careful \nnot to burst any of the RED globs.-N:Be careful!";
            levels[1].winType = WinType.CLEAR;
            levels[1].loseType = LoseType.BADCOLOR;
            levels[1].dangerColor = Color.Red;
            levels[1].dangerColorDisplay = "RED";
            levels[1].texture = JellyfishRenderer.profJelly;
            levels[2].levelfilename = "puzzle2.txt";
            levels[2].name = "Level2";
            levels[2].winType = WinType.CLEAR;
            levels[2].loseType = LoseType.NOMOVES;
            levels[2].texture = JellyfishRenderer.officerJelly;
            levels[3].levelfilename = "puzzle3.txt";
            levels[3].name = "Level3";
            levels[3].texture = JellyfishRenderer.firemanJelly;
            levels[4].levelfilename = "puzzle4.txt";
            levels[4].name = "Level4";
            levels[4].difficulty = Difficulty.MEDIUM;
            levels[4].texture = JellyfishRenderer.libraryJelly;
            levels[5].levelfilename = "puzzle5.txt";
            levels[5].name = "Level5";
            levels[5].difficulty = Difficulty.MEDIUM;
            levels[5].texture = JellyfishRenderer.mogulJelly;
            levels[6].texture = JellyfishRenderer.clownJelly;
            levels[6].levelfilename = "puzzle6.txt";
            levels[6].name = "Level6";
            levels[6].winType = WinType.CLEAR;
            levels[6].loseType = LoseType.BADCOLOR;
            levels[6].dangerColor = Color.Yellow;
            levels[6].dangerColorDisplay = "YELLOW";
            levels[6].difficulty = Difficulty.MEDIUM;
            
            levels[7].levelfilename = "puzzle7.txt";
            levels[7].name = "Level7";
            levels[7].difficulty = Difficulty.HARD;
            
            levels[8].levelfilename = "puzzle8.txt";
            levels[8].name = "level8";
            levels[8].difficulty = Difficulty.HARD;

            levels[9].levelfilename = "puzzle9.txt";
            levels[9].name = "Level9";
            levels[9].difficulty = Difficulty.HARD;

            levels[10].levelfilename = "puzzle10.txt";
            levels[10].name = "Level10";
            levels[10].difficulty = Difficulty.EASY;

            levels[11].levelfilename = "puzzle11.txt";
            levels[11].name = "Level11";
            levels[11].difficulty = Difficulty.EASY;

            levels[12].levelfilename = "puzzle12.txt";
            levels[12].name = "Level12";
            levels[12].difficulty = Difficulty.EASY;

            levels[13].levelfilename = "puzzle13.txt";
            levels[13].name = "Level13";
            levels[13].difficulty = Difficulty.EASY;

            levels[14].levelfilename = "puzzle14.txt";
            levels[14].name = "Level14";
            levels[14].difficulty = Difficulty.EASY;

            levels[15].levelfilename = "puzzle15.txt";
            levels[15].name = "Level15";
            levels[15].difficulty = Difficulty.EASY;
            levels[15].loseType = LoseType.BADCOLOR;
            levels[15].dangerColor = Color.Green;
            levels[15].dangerColorDisplay = "GREEN";

            levels[16].levelfilename = "puzzle16.txt";
            levels[16].name = "Level16";
            levels[16].difficulty = Difficulty.EASY;

            levels[17].levelfilename = "puzzle17.txt";
            levels[17].name = "Level17";
            levels[17].difficulty = Difficulty.EASY;

            levels[18].levelfilename = "puzzle18.txt";
            levels[18].name = "Level18";
            levels[18].difficulty = Difficulty.EASY;

            levels[19].levelfilename = "puzzle19.txt";
            levels[19].name = "Level19";
            levels[19].difficulty = Difficulty.EASY;


            return levels;
        }

        public static List<Settings> LoadTimeAttackLevels()
        {
            List<Settings> levels = new List<Settings>();
            for (int i = 0; i < 3; i++)
            {
                Settings s = new Settings();
                s.allowResets = true;
                s.difficulty = Difficulty.EASY;
                s.displayScore = true;
                s.displayTimer = true;
                s.countdownTimer = true;
                s.displayMoveCount = false;
                s.randomOrbs = true;
                s.initialTime = 120000;                
                s.countdownTimer = true;
                s.grayOrbStart = 18;
                s.toggleFreq = 0;
                s.counterFreq = 0;
                s.timerFreq = 0;
                s.timerLimit = 0;
                s.mode = GameMode.TimeAttack;
                s.instructions = "N:Score as many points as you can before time \nruns out!-D:Let's start!";
                s.two_star = 2000;
                s.three_star = 3000;
                levels.Add(s);
            }
            levels[0].name = "Beginner";
            levels[0].grayOrbStart = 24;
            levels[0].texture = JellyfishRenderer.baseballJelly;
            levels[0].two_star = 300;
            levels[0].three_star = 1000;
            levels[1].name = "Standard";
            levels[1].difficulty = Difficulty.MEDIUM;
            levels[2].name = "Advanced";
            levels[2].grayOrbStart = 0;
            levels[2].texture = JellyfishRenderer.mustacheJelly;
            levels[2].two_star = 6000;
            levels[2].three_star = 10000;
            levels[2].difficulty = Difficulty.HARD;
            return levels;
        }

        public static List<Settings> LoadMoveCountLevels()
        {
            List<Settings> levels = new List<Settings>();
            for (int i = 0; i < 3; i++)
            {
                Settings s = new Settings();
                s.allowResets = true;
                s.displayScore = true;
                s.displayTimer = false;
                s.displayMoveCount = true;
                s.availableMoves = 100;
                s.difficulty = Difficulty.EASY;
                s.randomOrbs = true;                
                s.grayOrbStart = 18;
                s.toggleFreq = 0;
                s.counterFreq = 0;
                s.timerFreq = 0;
                s.timerLimit = 0;
                s.mode = GameMode.MoveChallenge;
                s.instructions = "N:Score as many points as you can before you \nrun out of moves! Take your time, Doctor!-D:Let's start!";
                s.two_star = 2000;
                s.three_star = 3000;
                levels.Add(s);
            }
            levels[0].name = "Beginner";
            levels[0].grayOrbStart = 24;
            levels[0].texture = JellyfishRenderer.baseballJelly;
            levels[0].two_star = 300;
            levels[0].three_star = 1000;
            levels[1].name = "Standard";
            levels[1].difficulty = Difficulty.MEDIUM;
            levels[2].name = "Advanced";
            levels[2].grayOrbStart = 0;
            levels[2].texture = JellyfishRenderer.mustacheJelly;
            levels[2].two_star = 6000;
            levels[2].three_star = 10000;
            levels[2].difficulty = Difficulty.HARD;
            return levels;
        }
    }
}
