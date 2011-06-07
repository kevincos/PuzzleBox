using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace PuzzleBox
{
    public class MusicControl
    {
        public static Song music_menu;
        public static Song music_game;

        public static Song currentTrack;

        public static void PlayMenuMusic()
        {
            if (Game.gameSettings.musicEnabled)
            {
                if (currentTrack != music_menu)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(music_menu);
                    currentTrack = music_menu;
                }
            }
        }

        public static void PlayGameMusic()
        {
            if (Game.gameSettings.musicEnabled)
            {
                if (currentTrack != music_game)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(music_game);
                    currentTrack = music_game;
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
                if (Game.metaState == MetaState.GamePlay)
                {
                    if (currentTrack == music_game)
                        MediaPlayer.Resume();
                    else
                    {
                        currentTrack = music_game;
                        MediaPlayer.Play(currentTrack);
                    }
                }
                else
                {
                    if (currentTrack == music_menu)
                        MediaPlayer.Resume();
                    else
                    {
                        currentTrack = music_menu;
                        MediaPlayer.Play(currentTrack);
                    }
                }
            }
        }

    }
}
