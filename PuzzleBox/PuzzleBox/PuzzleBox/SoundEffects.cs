using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace PuzzleBox
{
    class SoundEffects
    {
        public static SoundEffect soundSwoosh;
        public static SoundEffect soundBloop;
        
        public static void Init()
        {
        }

        public static void PlayMove()
        {
            soundSwoosh.Play();
        }

        public static void PlayScore()
        {
            soundBloop.Play();
        }
    }
}
