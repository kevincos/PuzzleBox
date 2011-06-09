using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PuzzleBox
{
    public class GameSettings
    {        
        public bool musicEnabled = true;
        public bool soundEffectsEnabled = true;
        public bool displayControls = true;
        public bool fullScreen = true;
        public bool wideScreen = true;
        public bool keyboardControls = false;

        public int puzzleViewLevel = 0;
        public int timeAttackViewLevel = 0;
        public int moveChallengeViewLevel = 0;

        public float controlStickTrigger = .25f;
        public float controlStickTriggerView = .85f;
    }
}
