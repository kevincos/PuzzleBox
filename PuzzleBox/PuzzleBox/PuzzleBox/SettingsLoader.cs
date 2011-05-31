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
            s.texture = JellyfishRenderer.yellowJelly;
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
            levels[0].name = "Professor Jellyfish";
            levels[0].difficulty = Difficulty.EASY;
            levels[0].winType = WinType.CLEAR;
            levels[0].loseType = LoseType.NOMOVES;
            levels[0].texture = JellyfishRenderer.profJelly;
            levels[0].instructions = "N:Doctor, it looks like we'll need to match \nsets of 3 or more bubbles to heal this patient! -D:Remember, if you make a mistake, press the \nB Button to undo your last move. -N: Good luck!";

            levels[1].levelfilename = "puzzle5.txt";
            levels[1].name = "Librarian Jellyfish";
            levels[1].difficulty = Difficulty.EASY;
            levels[1].texture = JellyfishRenderer.libraryJelly;
            levels[1].preview = Preview.level2;
            levels[1].instructions = "N:Fascinating! I've never seen bubbles like\nthis before... -N:Each time you take an action, they swap between \nclear and colored states.-D:They'll only match if they're in their colored\n state. This might be tricky. -N:Take your time and experiment until you\nunderstand how these bubbles behave.";

            levels[2].levelfilename = "puzzle9.txt";
            levels[2].name = "Officer Jellyfish";
            levels[2].difficulty = Difficulty.EASY;
            levels[2].preview = Preview.level3;
            levels[2].texture = JellyfishRenderer.officerJelly;
            levels[2].instructions = "N:Remember how new bubbles slide in from the \ntentacles to replace the ones you've popped? -D:You'll need to pay attention to what colors\nare coming up.-N:Plan ahead so that you don't get stuck!";
        
            levels[3].levelfilename = "puzzle13.txt";
            levels[3].name = "Artist Jellyfish";
            levels[3].difficulty = Difficulty.EASY;
            levels[3].texture = JellyfishRenderer.artistJelly;
            levels[3].preview = Preview.level4;
            levels[3].instructions = "N:Hmm... there a lot of orange bubbles at the\ntips of the tentacles. -D:You'll need to match all of them with just\na single orange bubble. -N:There must be a way to do that!";

            levels[4].levelfilename = "puzzle14.txt";
            levels[4].name = "Explorer Jellyfish";
            levels[4].difficulty = Difficulty.EASY;
            levels[4].texture = JellyfishRenderer.explorerJelly;
            levels[4].preview = Preview.level5;
            levels[4].instructions = "N:Gosh! That's a lot of bubbles to deal with.\nAny advice, Doctor?-D:I think we should try and match bubbles from \ndifferent tentacles as evenly as possible.-N:Good idea. That way we won't get stuck.";

            levels[5].levelfilename = "puzzle15.txt";
            levels[5].name = "Psychic Jellyfish";
            levels[5].difficulty = Difficulty.EASY;
            levels[5].loseType = LoseType.BADCOLOR;
            levels[5].dangerColor = Color.Green;
            levels[5].dangerColorDisplay = "GREEN";
            levels[5].texture = JellyfishRenderer.fortuneJelly;
            levels[5].preview = Preview.level6;
            levels[5].instructions = "N:Oh no! This Jellyfish is allergic to Green toxins.\nIf we pop a Green bubble, the toxin will poison him. -D:We'll do the best we can. Try to pop everything\nEXCEPT the Green bubbles. -N:I hope thats enough to cure him!";

            // Standard Levels
            levels[6].levelfilename = "puzzle3.txt";
            levels[6].name = "Fireman Jellyfish";
            levels[6].difficulty = Difficulty.MEDIUM;
            levels[6].texture = JellyfishRenderer.firemanJelly;
            levels[6].preview = Preview.level7;
            levels[6].instructions = "N:I've got a funny feeling about this one. Matching\nsets of 3 bubbles usually seems like a good idea... -D:However, our goal is to pop ALL the bubbles. Pay\nattention to how many of each color there are. -N:Let's be careful and try to plan ahead.";

            levels[7].levelfilename = "puzzle4.txt";
            levels[7].name = "Chef Jellyfish";
            levels[7].difficulty = Difficulty.MEDIUM;
            levels[7].texture = JellyfishRenderer.chefJelly;
            levels[7].preview = Preview.level8;
            levels[7].instructions = "N: Take a look at the bubbles in the corners.-D:Good eye! I think we'll need to form lots \nof bonus bubbles to clear all this out.-N:Agreed! Doctor, try to create bonus bubbles \nwhenever posssible!";

            levels[8].texture = JellyfishRenderer.clownJelly;
            levels[8].levelfilename = "puzzle6.txt";
            levels[8].name = "Clown Jellyfish";
            levels[8].winType = WinType.CLEAR;
            levels[8].loseType = LoseType.BADCOLOR;
            levels[8].dangerColor = Color.Yellow;
            levels[8].dangerColorDisplay = "YELLOW";
            levels[8].difficulty = Difficulty.MEDIUM;
            levels[8].preview = Preview.level9;
            levels[8].instructions = "N: Oh my! This Jellyfish is allergic to Yellow \nbubbles. And there's more of those crazy bubbles!-D:Okay! Let's match up everything but the\nYellow bubbles.";

            levels[9].levelfilename = "puzzle7.txt";
            levels[9].name = "Madame Jellyfish";
            levels[9].difficulty = Difficulty.MEDIUM;
            levels[9].texture = JellyfishRenderer.hookerJelly;
            levels[9].preview = Preview.level10;
            levels[9].instructions = "N:Hmm... I think we'll need to match these\nbubbles in sets.-D:Your intuition is spot on, Nurse Jellyfish.\nLet's do it!";

            levels[10].levelfilename = "puzzle11.txt";
            levels[10].difficulty = Difficulty.MEDIUM;
            levels[10].preview = Preview.level11;
            levels[10].instructions = "N:I'm not so sure about this one. Any advice, \nDoctor?-D:Keep in mind that no matter what rotations you \ndo, it's impossible to match a bubble in the corner\nof the body with a middle tentacle. -D:Does that make any sense? -N:I'm not sure. Maybe it'll be clearer\nonce we're inside!";
            levels[10].texture = JellyfishRenderer.greenJelly;
            levels[10].name = "Baby Jellyfish";            

            levels[11].levelfilename = "puzzle12.txt";            
            levels[11].difficulty = Difficulty.MEDIUM;
            levels[11].preview = Preview.level12;
            levels[11].instructions = "N:What do you think, Doctor? -D:I could give you some advice, but I\nthink you're ready to tackle this one on your own. -N:How exciting!";
            levels[11].texture = JellyfishRenderer.yellowJelly;
            levels[11].name = "Mister Jellyfish";
            
            levels[12].levelfilename = "puzzle17.txt";           
            levels[12].difficulty = Difficulty.MEDIUM;
            levels[12].preview = Preview.level13;
            levels[12].instructions = "N:Ack! More of this weird shifty bubbles.-D:Don't worry. It'll take some work, but\nI'm sure we can do it!";
            levels[12].texture = JellyfishRenderer.redJelly;
            levels[12].name = "Miss Jellyfish";

            levels[13].levelfilename = "puzzle18.txt";
            levels[13].name = "King Jellyfish";
            levels[13].difficulty = Difficulty.MEDIUM;
            levels[13].texture = JellyfishRenderer.kingJelly;
            levels[13].preview = Preview.level14;
            levels[13].instructions = "N:Look at all those Yellow and Orange bubbles\nat the top.-D:I'll bet there's a way we can pop almost all of \nthem at the same time!";
            
            levels[14].levelfilename = "puzzle19.txt";            
            levels[14].difficulty = Difficulty.MEDIUM;
            levels[14].preview = Preview.level15;
            levels[14].instructions = "N:This is getting tough. I think we'll need\nto combine a lot of our techniques to get this.-D:We'll need to figure out when to get bonus orbs\nand when to match larger sets.";
            levels[14].texture = JellyfishRenderer.ballerinaJelly;
            levels[14].name = "Ballerina Jellyfish";

            //Hard Levels
            levels[15].levelfilename = "puzzle1.txt";
            levels[15].instructions = "N:Doctor! This patient has severe allergies to \nRED toxins!-D:Got it! We'll have to be extra careful \nnot to burst any of the RED globs.-N:Be careful!";
            levels[15].winType = WinType.CLEAR;
            levels[15].loseType = LoseType.BADCOLOR;
            levels[15].dangerColor = Color.Red;
            levels[15].dangerColorDisplay = "RED";
            levels[15].difficulty = Difficulty.HARD;
            levels[15].preview = Preview.level16;
            levels[15].name = "Newsie Jellyfish";
            levels[15].texture = JellyfishRenderer.newsieJelly;
                                    
            levels[16].levelfilename = "puzzle16.txt";
            levels[16].name = "Biker Jellyfish";
            levels[16].difficulty = Difficulty.HARD;
            levels[16].preview = Preview.level19;
            levels[16].instructions = "N:This doesn't look good! It's more of those toggle \nbubbles. -D:Plus, they're right next to regular bubbles. We \nneed to be extra careful to match our sets.";
            levels[16].texture = JellyfishRenderer.bikerJelly;

            levels[17].levelfilename = "puzzle10.txt";
            levels[17].name = "Cowboy Jellyfish";
            levels[17].difficulty = Difficulty.HARD;
            levels[17].preview = Preview.level18;
            levels[17].instructions = "N:I have no idea how to solve this one! -D:Honestly, I'm not so sure either, but\nI've got a hunch that the key is the pink bubbles.";
            levels[17].texture = JellyfishRenderer.cowboyJelly;

            levels[18].levelfilename = "puzzle8.txt";
            levels[18].name = "Ninja Jellyfish";
            levels[18].difficulty = Difficulty.HARD;
            levels[18].preview = Preview.level17;
            levels[18].instructions = "N:Doctor, any advice? -D:Um... I have no idea! This looks really tough...-N:Eep! Well... I'm sure we'll figure something out... \nRight?";
            levels[18].texture = JellyfishRenderer.ninjaJelly;

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
            levels[0].name = "Baseball Jellyfish";
            levels[0].grayOrbStart = 24;
            levels[0].texture = JellyfishRenderer.baseballJelly2;
            levels[0].two_star = 300;
            levels[0].three_star = 1000;
            levels[1].name = "Mogul Jellyfish";
            levels[1].difficulty = Difficulty.MEDIUM;
            levels[1].texture = JellyfishRenderer.mogulJelly;
            levels[2].name = "Karate Jellyfish";
            levels[2].grayOrbStart = 0;
            levels[2].texture = JellyfishRenderer.karateJelly;
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
                s.two_star = 1500;
                s.three_star = 3000;
                levels.Add(s);
            }
            levels[0].name = "Birthday Jellyfish";
            levels[0].grayOrbStart = 24;
            levels[0].texture = JellyfishRenderer.birthdayJelly;
            levels[0].two_star = 300;
            levels[0].three_star = 1000;
            levels[1].name = "Captain Jellyfish";
            levels[1].difficulty = Difficulty.MEDIUM;
            levels[1].texture = JellyfishRenderer.capnJelly;
            levels[2].name = "Queen Jellyfish";
            levels[2].grayOrbStart = 0;
            levels[2].texture = JellyfishRenderer.queenJelly;
            levels[2].two_star = 6000;
            levels[2].three_star = 10000;
            levels[2].difficulty = Difficulty.HARD;
            return levels;
        }
    }
}
