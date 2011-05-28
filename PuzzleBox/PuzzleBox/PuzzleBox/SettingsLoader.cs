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
            for(int i =0; i < 19; i++)
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
            //levels[0].levelfilename = "allgraytemplate.txt";
            //levels[0].name = "Template Level";
            //levels[0].texture = JellyfishRenderer.baseballJelly2;

            // Easy Levels

            levels[0].levelfilename = "puzzle2.txt";
            levels[0].name = "Level2";
            levels[0].difficulty = Difficulty.EASY;
            levels[0].winType = WinType.CLEAR;
            levels[0].loseType = LoseType.NOMOVES;
            levels[0].texture = JellyfishRenderer.officerJelly;

            levels[1].levelfilename = "puzzle5.txt";
            levels[1].name = "Level5";
            levels[1].difficulty = Difficulty.EASY;
            levels[1].texture = JellyfishRenderer.mogulJelly;
            levels[1].preview = Preview.level2;

            levels[2].levelfilename = "puzzle9.txt";
            levels[2].name = "Level9";
            levels[2].difficulty = Difficulty.EASY;
            levels[2].preview = Preview.level3;
            levels[2].texture = JellyfishRenderer.artistJelly;
        
            levels[3].levelfilename = "puzzle13.txt";
            levels[3].name = "Level13";
            levels[3].difficulty = Difficulty.EASY;
            levels[3].texture = JellyfishRenderer.bikerJelly;
            levels[3].preview = Preview.level4;

            levels[4].levelfilename = "puzzle14.txt";
            levels[4].name = "Level14";
            levels[4].difficulty = Difficulty.EASY;
            levels[4].texture = JellyfishRenderer.birthdayJelly;
            levels[4].preview = Preview.level5;

            levels[5].levelfilename = "puzzle15.txt";
            levels[5].name = "Level15";
            levels[5].difficulty = Difficulty.EASY;
            levels[5].loseType = LoseType.BADCOLOR;
            levels[5].dangerColor = Color.Green;
            levels[5].dangerColorDisplay = "GREEN";
            levels[5].texture = JellyfishRenderer.capnJelly;
            levels[5].preview = Preview.level6;

            // Standard Levels
            levels[6].levelfilename = "puzzle3.txt";
            levels[6].name = "Level3";
            levels[6].difficulty = Difficulty.MEDIUM;
            levels[6].texture = JellyfishRenderer.firemanJelly;
            levels[6].preview = Preview.level7;

            levels[7].levelfilename = "puzzle4.txt";
            levels[7].name = "Level4";
            levels[7].difficulty = Difficulty.MEDIUM;
            levels[7].texture = JellyfishRenderer.libraryJelly;
            levels[7].preview = Preview.level8;

            levels[8].texture = JellyfishRenderer.clownJelly;
            levels[8].levelfilename = "puzzle6.txt";
            levels[8].name = "Level6";
            levels[8].winType = WinType.CLEAR;
            levels[8].loseType = LoseType.BADCOLOR;
            levels[8].dangerColor = Color.Yellow;
            levels[8].dangerColorDisplay = "YELLOW";
            levels[8].difficulty = Difficulty.MEDIUM;
            levels[8].preview = Preview.level9;

            levels[9].levelfilename = "puzzle7.txt";
            levels[9].name = "Level7";
            levels[9].difficulty = Difficulty.MEDIUM;
            levels[9].texture = JellyfishRenderer.explorerJelly;
            levels[9].preview = Preview.level10;

            levels[10].levelfilename = "puzzle11.txt";
            levels[10].name = "Level11";
            levels[10].difficulty = Difficulty.MEDIUM;
            levels[10].texture = JellyfishRenderer.chefJelly;
            levels[10].preview = Preview.level11;

            levels[11].levelfilename = "puzzle12.txt";
            levels[11].name = "Level12";
            levels[11].difficulty = Difficulty.MEDIUM;
            levels[11].texture = JellyfishRenderer.fortuneJelly;
            levels[11].preview = Preview.level12;

            levels[12].levelfilename = "puzzle17.txt";
            levels[12].name = "Level17";
            levels[12].difficulty = Difficulty.MEDIUM;
            levels[12].texture = JellyfishRenderer.karateJelly;
            levels[12].preview = Preview.level13;

            levels[13].levelfilename = "puzzle18.txt";
            levels[13].name = "Level18";
            levels[13].difficulty = Difficulty.MEDIUM;
            levels[13].texture = JellyfishRenderer.kingJelly;
            levels[13].preview = Preview.level14;

            levels[14].levelfilename = "puzzle19.txt";
            levels[14].name = "Level19";
            levels[14].difficulty = Difficulty.MEDIUM;
            levels[14].preview = Preview.level15;
            
            //Hard Levels
            levels[15].levelfilename = "puzzle1.txt";
            levels[15].name = "Level1";
            levels[15].instructions = "N:Doctor! This patient has severe allergies to \nRED toxins!-D:Got it! We'll have to be extra careful \nnot to burst any of the RED globs.-N:Be careful!";
            levels[15].winType = WinType.CLEAR;
            levels[15].loseType = LoseType.BADCOLOR;
            levels[15].dangerColor = Color.Red;
            levels[15].dangerColorDisplay = "RED";
            levels[15].difficulty = Difficulty.HARD;
            levels[15].texture = JellyfishRenderer.profJelly;
            levels[15].preview = Preview.level16;
                        
            levels[16].levelfilename = "puzzle8.txt";
            levels[16].name = "level8";
            levels[16].difficulty = Difficulty.HARD;
            levels[16].preview = Preview.level17;
            
            levels[17].levelfilename = "puzzle10.txt";
            levels[17].name = "Level10";
            levels[17].difficulty = Difficulty.HARD;
            levels[17].preview = Preview.level18;
            
            levels[18].levelfilename = "puzzle16.txt";
            levels[18].name = "Level16";
            levels[18].difficulty = Difficulty.HARD;
            levels[18].preview = Preview.level19;

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
