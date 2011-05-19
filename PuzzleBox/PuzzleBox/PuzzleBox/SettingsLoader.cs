using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzleBox
{
    class SettingsLoader
    {
        public static List<Settings> LoadPuzzleLevels()
        {
            List<Settings> levels = new List<Settings>();
            for(int i =0; i < 10; i++)
            {                
                Settings s = new Settings();
                s.allowResets = false;
                s.displayScore = false;
                s.displayTimer = true;
                s.countdownTimer = false;
                s.randomOrbs = false;
                s.mode = GameMode.Puzzle;
                s.refillQueues = false;
                s.instructions = "";
                s.two_star = 4000;
                s.three_star = 2000;
                levels.Add(s);
            }
            levels[0].levelfilename = "allgraytemplate.txt";
            levels[0].name = "Template Level";
            levels[1].levelfilename = "puzzle1.txt";
            levels[1].name = "Level1";
            levels[1].instructions = "N:Doctor! This patient has severe allergies to \nRED toxins!-D:Got it! We'll have to be extra careful \nnot to burst any of the RED globs.-N:Be careful!";
            levels[1].winType = WinType.CLEAR;
            levels[1].loseType = LoseType.BADCOLOR;
            levels[1].dangerColor = Color.Red;
            levels[1].dangerColorDisplay = "RED";
            levels[2].levelfilename = "puzzle2.txt";
            levels[2].name = "Level2";
            levels[2].winType = WinType.CLEAR;
            levels[2].loseType = LoseType.NOMOVES;
            levels[3].levelfilename = "puzzle3.txt";
            levels[3].name = "Level3"; 
            levels[4].levelfilename = "puzzle4.txt";
            levels[4].name = "Level4"; 
            levels[5].levelfilename = "puzzle5.txt";
            levels[5].name = "Level5";
            levels[6].levelfilename = "puzzle6.txt";
            levels[6].name = "Level6"; 
            levels[7].levelfilename = "puzzle7.txt";
            levels[7].name = "Level7"; 
            levels[8].levelfilename = "puzzle8.txt";
            levels[8].name = "8"; 
            levels[9].levelfilename = "puzzle9.txt";
            levels[9].name = "Level9"; 
            return levels;
        }

        public static List<Settings> LoadTimeAttackLevels()
        {
            List<Settings> levels = new List<Settings>();
            for (int i = 0; i < 3; i++)
            {
                Settings s = new Settings();
                s.allowResets = true;
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
            levels[2].name = "Advanced";
            levels[2].grayOrbStart = 0;
            levels[2].texture = JellyfishRenderer.mustacheJelly;
            levels[2].two_star = 6000;
            levels[2].three_star = 10000;
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
            levels[2].name = "Advanced";
            levels[2].grayOrbStart = 0;
            levels[2].texture = JellyfishRenderer.mustacheJelly;
            levels[2].two_star = 6000;
            levels[2].three_star = 10000;  
            return levels;
        }
    }
}
