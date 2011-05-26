using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PuzzleBox
{
    class MusicControl
    {
        public static Song track1;
        public static Song track2;

        public static Song currentTrack;

        public static void PlayMenuMusic()
        {
            if (Game.gameSettings.musicEnabled)
            {
                if (currentTrack != track1)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(track1);
                    currentTrack = track1;
                }
            }
        }

        public static void PlayGameMusic()
        {
            if (Game.gameSettings.musicEnabled)
            {
                if (currentTrack != track2)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(track2);
                    currentTrack = track2;
                }
            }
        }

        public static void Stop()
        {
            MediaPlayer.Stop();
            currentTrack = null;
        }

        public static void Pause()
        {
            MediaPlayer.Pause();
        }

        public static void Resume()
        {
            if (Game.gameSettings.musicEnabled)
            {
                MediaPlayer.Resume();
            }
        }

    }

}
