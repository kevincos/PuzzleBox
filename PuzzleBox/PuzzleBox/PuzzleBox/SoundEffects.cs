using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PuzzleBox
{
    public class SoundEffects
    {
        public static SoundEffect soundSwoosh;
        public static SoundEffect soundBloop;
        public static SoundEffect soundBeep;
        public static SoundEffect soundClick;
        
        public static void Init()
        {
        }

        public static void PlayMove()
        {
            if(Game.gameSettings.soundEffectsEnabled)
                soundSwoosh.Play();
        }

        public static void PlayScore()
        {
            if (Game.gameSettings.soundEffectsEnabled)
                soundBloop.Play();
        }

        public static void PlayClick()
        {
            if (Game.gameSettings.soundEffectsEnabled)
                soundClick.Play();
        }

        public static void PlayAlert()
        {
            if (Game.gameSettings.soundEffectsEnabled)
                soundBeep.Play();
        }
    }
}
