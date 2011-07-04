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
    public enum TutorialLauncherState
    {
        ZOOM,
        NURSEIN,
        NURSETEXT,
        DOCTORIN,
        DOCTORTEXT,
        DOCTOROUT,
        FINALTEXT
    }

    public class TutorialLauncher
    {
        int nurseX;
        int nurseY;
        int doctorX;
        int doctorY;
        int speechX;
        int speechY;
        int jellyX;
        int jellyY;        

        int cooldown = 0;
        int animateTime = 0;
        int maxAnimateTime = 500;
        TutorialLauncherState state = TutorialLauncherState.ZOOM;

        public TutorialLauncher()
        {
            ApplyResolutionChanges();
        }

        public void ApplyResolutionChanges()
        {
            if (Game.gameSettings.wideScreen)
            {
                nurseX = 200;
                nurseY = 360;
                doctorX = 1050;
                doctorY = 360;
                speechX = 652;
                speechY = 600;
                jellyX = Game.screenCenterX + 4;
                jellyY = Game.screenCenterY +14;
            }
            else
            {
                nurseX = 150;
                nurseY = 400;
                doctorX = 850;
                doctorY = 400;
                speechX = 512;
                speechY = 600;
                jellyX = 517;
                jellyY = 400;        
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            animateTime += gameTime.ElapsedGameTime.Milliseconds;
            
            if (state == TutorialLauncherState.ZOOM && animateTime > maxAnimateTime)
            {
                state = TutorialLauncherState.NURSEIN;
                animateTime = 0;
            }
            if (state == TutorialLauncherState.NURSEIN && animateTime > maxAnimateTime)
            {
                state = TutorialLauncherState.NURSETEXT;
            }
            if (state == TutorialLauncherState.NURSETEXT)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(Game.playerIndex).IsButtonDown(Buttons.A))
                {
                    state = TutorialLauncherState.DOCTORIN;
                    animateTime = 0;
                }
            }
            if (state == TutorialLauncherState.DOCTORIN && animateTime > maxAnimateTime)
            {
                state = TutorialLauncherState.DOCTORTEXT;
            }
            if (state == TutorialLauncherState.DOCTORTEXT)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(Game.playerIndex).IsButtonDown(Buttons.A))
                {
                    SoundEffects.PlayClick();
                    state = TutorialLauncherState.DOCTOROUT;
                    animateTime = 0;
                }
            }
            if (state == TutorialLauncherState.DOCTOROUT && animateTime > maxAnimateTime)
            {
                state = TutorialLauncherState.FINALTEXT;
            }
            if (state == TutorialLauncherState.FINALTEXT)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(Game.playerIndex).IsButtonDown(Buttons.A))
                {
                    return MenuResult.StartTutorial;
                }
            }
            return MenuResult.None;
        }

        public void Draw()
        {
            if (state != TutorialLauncherState.ZOOM)
            {
                JellyfishRenderer.DrawJellyfish(jellyX, jellyY, 100, JellyfishRenderer.yellowJelly, 1f, SpriteEffects.None);
            }
            if (state == TutorialLauncherState.ZOOM)
            {
                JellyfishRenderer.DrawJellyfish(jellyX, jellyY, 100, JellyfishRenderer.yellowJelly, (1f * animateTime) / (1f * maxAnimateTime), SpriteEffects.None);
            }
            if (state == TutorialLauncherState.NURSEIN)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 + 300 * animateTime / 500, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);                
            }
            if (state == TutorialLauncherState.DOCTORIN)
            {
                JellyfishRenderer.DrawJellyfish(doctorX + 300 - 300 * animateTime / 500, nurseY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);                
            }
            if (state == TutorialLauncherState.DOCTOROUT)
            {
                JellyfishRenderer.DrawJellyfish(doctorX + 300 * animateTime / 500, nurseY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);                
            }
            if (state != TutorialLauncherState.ZOOM && state != TutorialLauncherState.NURSEIN)
            {
                JellyfishRenderer.DrawJellyfish(nurseX, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);                
            }
            if (state == TutorialLauncherState.DOCTORTEXT)
            {
                JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.FlipHorizontally);
                Game.spriteBatch.DrawString(Game.spriteFont, "Hey there! Jellyfish Surgery is very tricky, but \nNurse Jellyfish will teach you the basics..", new Vector2(speechX - 260, speechY), Color.Black);
                Game.spriteBatch.Draw(HelpOverlay.summary_confirm, new Rectangle(speechX + 255, speechY + 60, 25, 25), Color.Green);
            }
            if (state == TutorialLauncherState.NURSETEXT)
            {
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                Game.spriteBatch.DrawString(Game.spriteFont, "Welcome! You must be Doctor Jellyfish's \nnew assistant. Look! Here he is now...", new Vector2(speechX - 260, speechY), Color.Black);
                Game.spriteBatch.Draw(HelpOverlay.summary_confirm, new Rectangle(speechX + 255, speechY + 60, 25, 25), Color.Green);
            }
            if (state == TutorialLauncherState.FINALTEXT)
            {
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                Game.spriteBatch.DrawString(Game.spriteFont, "We can practice on Mister Jellyfish here! \nLet's take a look inside and get started...", new Vector2(speechX - 260, speechY), Color.Black);
                Game.spriteBatch.Draw(HelpOverlay.summary_confirm, new Rectangle(speechX + 255, speechY + 60, 25, 25), Color.Green);
            }

        }        
    }
}
