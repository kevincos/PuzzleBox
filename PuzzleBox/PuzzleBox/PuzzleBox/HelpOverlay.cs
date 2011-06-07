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
    public class HelpOverlay
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
            {
                Game.spriteBatch.Draw(help_show, new Vector2(50, Game.screenSizeY - 70), Color.White);
                Game.spriteBatch.DrawString(Game.spriteFont, "SHOW", new Vector2(100, Game.screenSizeY - 65), Color.LightGray);
            }
            else
            {
                Game.spriteBatch.Draw(help_hide, new Vector2(50, Game.screenSizeY - 70), Color.LightGray);
                Game.spriteBatch.DrawString(Game.spriteFont, "HIDE", new Vector2(100, Game.screenSizeY - 65), Color.LightGray);
                if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions == ControlRestrictions.StickOnly)
                    Game.spriteBatch.Draw(help_leftstick, new Vector2(50, Game.screenSizeY - 180), Color.GreenYellow);
                else if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions != ControlRestrictions.None)
                    Game.spriteBatch.Draw(help_leftstick, new Vector2(50, Game.screenSizeY - 180), Color.Red);
                else
                    Game.spriteBatch.Draw(help_leftstick, new Vector2(50, Game.screenSizeY - 180), Color.LightGray);

                if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions == ControlRestrictions.TriggersOnly)
                {
                    Game.spriteBatch.Draw(help_righttrigger, new Vector2(Game.screenSizeX - 210, 50), Color.GreenYellow);
                    Game.spriteBatch.Draw(help_lefttrigger, new Vector2(50, 50), Color.GreenYellow);
                    Game.spriteBatch.DrawString(Game.spriteFont, "PUSH", new Vector2(Game.screenSizeX - 190, 60), Color.GreenYellow);
                    Game.spriteBatch.DrawString(Game.spriteFont, "PULL", new Vector2(125, 60), Color.GreenYellow);                                    
                }
                else if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions != ControlRestrictions.None)
                {
                    Game.spriteBatch.Draw(help_righttrigger, new Vector2(Game.screenSizeX - 210, 50), Color.Red);
                    Game.spriteBatch.Draw(help_lefttrigger, new Vector2(50, 50), Color.Red);
                    Game.spriteBatch.DrawString(Game.spriteFont, "PUSH", new Vector2(Game.screenSizeX - 190, 60), Color.Red);
                    Game.spriteBatch.DrawString(Game.spriteFont, "PULL", new Vector2(125, 60), Color.Red);                                    
                }
                else
                {
                    Game.spriteBatch.DrawString(Game.spriteFont, "PUSH", new Vector2(Game.screenSizeX - 190, 60), Color.LightGray);
                    Game.spriteBatch.DrawString(Game.spriteFont, "PULL", new Vector2(125, 60), Color.LightGray);
                    Game.spriteBatch.Draw(help_righttrigger, new Vector2(Game.screenSizeX-210, 50), Color.LightGray);
                    Game.spriteBatch.Draw(help_lefttrigger, new Vector2(50, 50), Color.LightGray);
                }

                if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions == ControlRestrictions.ShouldersOnly)
                {
                    Game.spriteBatch.Draw(help_rightbutton, new Vector2(Game.screenSizeX - 210, 120), Color.GreenYellow);
                    Game.spriteBatch.Draw(help_leftbutton, new Vector2(50, 120), Color.GreenYellow);
                }
                else if (Game.currentSettings.mode == GameMode.Tutorial && TutorialStage.restrictions != ControlRestrictions.None)
                {
                    Game.spriteBatch.Draw(help_rightbutton, new Vector2(Game.screenSizeX - 210, 120), Color.Red);
                    Game.spriteBatch.Draw(help_leftbutton, new Vector2(50, 120), Color.Red);
                }
                else
                {
                    Game.spriteBatch.Draw(help_rightbutton, new Vector2(Game.screenSizeX - 210, 120), Color.LightGray);
                    Game.spriteBatch.Draw(help_leftbutton, new Vector2(50, 120), Color.LightGray);
                }


                Game.spriteBatch.Draw(help_sound, new Vector2(Game.screenSizeX - 170, 200), Color.LightGray);
                Game.spriteBatch.Draw(help_music, new Vector2(Game.screenSizeX - 170, 250), Color.LightGray);
                if (Game.gameSettings.soundEffectsEnabled == false)
                    Game.spriteBatch.Draw(help_x, new Vector2(Game.screenSizeX - 120, 200), Color.LightGray);
                if (Game.gameSettings.musicEnabled == false)
                    Game.spriteBatch.Draw(help_x, new Vector2(Game.screenSizeX - 120, 250), Color.LightGray);

                if (showBack && Game.currentSettings.mode == GameMode.Puzzle)
                {
                    Game.spriteBatch.Draw(help_undo, new Vector2(Game.screenSizeX - 210, Game.screenSizeY - 155), Color.LightGray);
                    Game.spriteBatch.DrawString(Game.spriteFont, "UNDO", new Vector2(Game.screenSizeX - 150, Game.screenSizeY-150), Color.LightGray);
                }
            }
        }
    }
}
