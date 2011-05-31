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
    class TutorialLauncher
    {
        int nurseX = 150;
        int nurseY = 400;
        int doctorX = 850;
        int doctorY = 400;
        int speechX = 512;
        int speechY = 600;
        int jellyX = 517;
        int jellyY = 400;
        int jellyBodyY = 437;

        int cooldown = 0;
        int animateTime = 0;
        int maxAnimateTime = 500;
        TutorialLauncherState state = TutorialLauncherState.ZOOM;

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
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
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
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
                {
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
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.A))
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
            }
            if (state == TutorialLauncherState.NURSETEXT)
            {
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                Game.spriteBatch.DrawString(Game.spriteFont, "Welcome! You must be Doctor Jellyfish's \nnew assistant. Look! Here he is now...", new Vector2(speechX - 260, speechY), Color.Black);
            }
            if (state == TutorialLauncherState.FINALTEXT)
            {
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                Game.spriteBatch.DrawString(Game.spriteFont, "We can practice on Mister Jellyfish here! \nLet's take a look inside and get started...", new Vector2(speechX - 260, speechY), Color.Black);
            }

        }        
    }
}
