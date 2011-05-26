using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PuzzleBox
{
    class HelpOverlay
    {
        public static Texture2D help_lefttrigger;
        public static Texture2D help_righttrigger;
        public static Texture2D help_leftbutton;
        public static Texture2D help_rightbutton;
        public static Texture2D help_leftstick;
        public static Texture2D help_hide;
        public static Texture2D help_show;
        public static Texture2D help_undo;
        public static Texture2D help_sound;
        public static Texture2D help_music;
        public static Texture2D help_x;

        public static void Draw(bool showBack)
        {
            if (Game.gameSettings.displayControls == false)
                Game.spriteBatch.Draw(help_show, new Vector2(50, 700), Color.White);
            else
            {
                Game.spriteBatch.Draw(help_hide, new Vector2(50, 700), Color.White);
                if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions == ControlRestrictions.StickOnly)
                    Game.spriteBatch.Draw(help_leftstick, new Vector2(50, 600), Color.LightGreen);
                else if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions != ControlRestrictions.None)
                    Game.spriteBatch.Draw(help_leftstick, new Vector2(50, 600), Color.Red);
                else
                    Game.spriteBatch.Draw(help_leftstick, new Vector2(50, 600), Color.White);

                if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions == ControlRestrictions.TriggersOnly)
                {
                    Game.spriteBatch.Draw(help_righttrigger, new Vector2(800, 50), Color.LightGreen);
                    Game.spriteBatch.Draw(help_lefttrigger, new Vector2(50, 50), Color.LightGreen);
                }
                else if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions != ControlRestrictions.None)
                {
                    Game.spriteBatch.Draw(help_righttrigger, new Vector2(800, 50), Color.Red);
                    Game.spriteBatch.Draw(help_lefttrigger, new Vector2(50, 50), Color.Red);
                }
                else
                {
                    Game.spriteBatch.Draw(help_righttrigger, new Vector2(800, 50), Color.White);
                    Game.spriteBatch.Draw(help_lefttrigger, new Vector2(50, 50), Color.White);
                }

                if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions == ControlRestrictions.ShouldersOnly)
                {
                    Game.spriteBatch.Draw(help_rightbutton, new Vector2(800, 120), Color.LightGreen);
                    Game.spriteBatch.Draw(help_leftbutton, new Vector2(50, 120), Color.LightGreen);
                }
                else if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions != ControlRestrictions.None)
                {
                    Game.spriteBatch.Draw(help_rightbutton, new Vector2(800, 120), Color.Red);
                    Game.spriteBatch.Draw(help_leftbutton, new Vector2(50, 120), Color.Red);
                }
                else
                {
                    Game.spriteBatch.Draw(help_rightbutton, new Vector2(800, 120), Color.White);
                    Game.spriteBatch.Draw(help_leftbutton, new Vector2(50, 120), Color.White);
                }
                

                Game.spriteBatch.Draw(help_sound, new Vector2(850, 200), Color.White);
                Game.spriteBatch.Draw(help_music, new Vector2(850, 250), Color.White);
                if (Game.gameSettings.soundEffectsEnabled == false)
                    Game.spriteBatch.Draw(help_x, new Vector2(900, 200), Color.White);
                if (Game.gameSettings.musicEnabled == false)
                    Game.spriteBatch.Draw(help_x, new Vector2(900, 250), Color.White);

                if (showBack && Game.currentSettings.mode == GameMode.Puzzle)
                {                    
                    Game.spriteBatch.Draw(help_undo, new Vector2(825, 650), Color.White);
                }
            }
        }
    }
}
