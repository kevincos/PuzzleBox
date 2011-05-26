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
    enum MainMenuState
    {
        DOCTORIN,
        DOCTOROUT,
        READY,
        ANIMATEUP,
        ANIMATEDOWN
    }

    class MainMenu
    {
        public Texture2D background;
        public Texture2D header;

        int animateTime = 0;
        MainMenuState state = MainMenuState.DOCTORIN;
        MenuResult result = MenuResult.None;
        int selectedIndex = 0;
        List<MenuOption> optionList;
        int cooldown = 0;
        int headerX = 100;
        int headerY = 20;
        int headerWidth = 824;
        int headerHeight = 150;
        int doctorX = 600;
        int doctorY = 375;
        int optionListX = 100;
        int optionListY = 220;
        int optionGap = 60;
        int optionWidth = 250;
        int optionHeight = 60;
        int speechX = 712;
        int speechY = 600;

        public MainMenu()
        {
            optionList = new List<MenuOption>();
        }

        public void AddMenuItem(MenuResult result, Texture2D image, String helpText)
        {
            optionList.Add(new MenuOption(result, image, helpText));
        }

        public void Draw()
        {            
            Game.spriteBatch.Draw(background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);
            Game.spriteBatch.Draw(header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
            for (int i = 0; i < optionList.Count; i++)
            {
                if (i == selectedIndex)
                {
                    PuzzleNode p = new PuzzleNode(Color.Blue);
                    p.screenX = optionListX-25;
                    p.screenY = optionListY +optionHeight/2+ i * optionGap;
                    if (state == MainMenuState.ANIMATEDOWN)
                        p.screenY -= (int)(optionGap * (1f - (float)animateTime / (float)250));
                    if (state == MainMenuState.ANIMATEUP)
                        p.screenY += (int)(optionGap * (1f - (float)animateTime / (float)250));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                }
                Game.spriteBatch.Draw(optionList[i].optionText, new Rectangle(optionListX, optionListY + i * optionGap, optionWidth, optionHeight), Color.White);
            }
            if (state == MainMenuState.DOCTORIN)
            {
                JellyfishRenderer.DrawJellyfish(doctorX + 800 - 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX + 600 - 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            else if (state == MainMenuState.DOCTOROUT)
            {
                JellyfishRenderer.DrawJellyfish(doctorX + 200 + 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX + 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            else
            {
                JellyfishRenderer.DrawJellyfish(doctorX+200, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == MainMenuState.READY)
            {
                JellyfishRenderer.DrawSpeechBubble2(speechX, speechY, 100, SpriteEffects.FlipHorizontally);
                Game.spriteBatch.DrawString(Game.spriteFont, optionList[selectedIndex].optionString, new Vector2(speechX - 250, speechY - 15), Color.Black);
                
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (state == MainMenuState.DOCTORIN || state == MainMenuState.DOCTOROUT || state == MainMenuState.ANIMATEDOWN || state == MainMenuState.ANIMATEUP)
            {
                animateTime+=gameTime.ElapsedGameTime.Milliseconds;                
            }
            if (state == MainMenuState.DOCTORIN && animateTime > 250)
            {
                MusicControl.PlayMenuMusic();
                state = MainMenuState.READY;
            }
            if (state == MainMenuState.DOCTOROUT && animateTime > 250)
            {
                animateTime = 0;
                state = MainMenuState.DOCTORIN;
                return result;
            }
            if (state == MainMenuState.ANIMATEDOWN && animateTime > 250)
            {
                animateTime = 0;
                state = MainMenuState.READY;
            }
            if (state == MainMenuState.ANIMATEUP && animateTime > 250)
            {
                animateTime = 0;
                state = MainMenuState.READY;
            }
            if (state == MainMenuState.READY && cooldown == 0)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;

                if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                {
                    SoundEffects.PlayScore();
                    result = optionList[selectedIndex].result;
                    animateTime = 0;
                    state = MainMenuState.DOCTOROUT;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -.95 || rightStick.Y < -.95)
                {
                    if (selectedIndex < optionList.Count() - 1)
                    {
                        SoundEffects.PlayMove();
                        state = MainMenuState.ANIMATEDOWN;
                        animateTime = 0;
                        selectedIndex++;
                        cooldown = 250;
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
                    if (selectedIndex > 0)
                    {
                        SoundEffects.PlayMove();
                        state = MainMenuState.ANIMATEUP;
                        animateTime = 0;

                        selectedIndex--;
                        cooldown = 250;
                    }
                }
            }
            return MenuResult.None;            
        }
    }
}
