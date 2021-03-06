﻿using System;
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

    public class MainMenu
    {
        public static Texture2D background;
        public static Texture2D header;

        int animateTime = 0;
        MainMenuState state = MainMenuState.READY;
        MenuResult result = MenuResult.None;
        int selectedIndex = 0;
        List<MenuOption> optionList;
        int cooldown = 0;
        int headerX;
        int headerY;
        int headerWidth;
        int headerHeight;
        int doctorX;
        int doctorY;
        int optionListX;
        int optionListY;
        int optionGap;
        int optionHeight;
        int speechX;
        int speechY;
        int creditsX;
        int creditsY;

        public MainMenu()
        {
            optionList = new List<MenuOption>();
            ApplyResolutionChanges();    
        }

        public void ApplyResolutionChanges()
        {
            if (Game.gameSettings.wideScreen)
            {
                headerX = 225;
                headerY = 20;
                headerWidth = 824;
                headerHeight = 150;
                doctorX = 725;
                doctorY = 325;
                optionListX = 150;
                optionListY = 170;
                optionGap = 60;                
                optionHeight = 60;
                speechX = 802;
                speechY = 530;
                creditsX = 425;
                creditsY = 650;
            }
            else
            {
                headerX = 100;
                headerY = 20;
                headerWidth = 824;
                headerHeight = 150;
                doctorX = 625;
                doctorY = 375;
                optionListX = 75;
                optionListY = 220;
                optionGap = 60;
                optionHeight = 60;
                speechX = 712;
                speechY = 590;
                creditsX = 300;
                creditsY = 700;
            }
        }

        public void AddMenuItem(MenuResult result, String optionText, String helpText)
        {
            optionList.Add(new MenuOption(result, optionText, helpText));
        }

        public void RemovePurchase()
        {
            for (int i = 0; i < optionList.Count; i++)
            {
                if (optionList[i].result == MenuResult.BuyFullGame)
                {
                    optionList.RemoveAt(i);
                    selectedIndex = 0;
                    break;
                }
            }            
        }

        public void Draw()
        {            
            Game.spriteBatch.Draw(background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);
            Game.spriteBatch.Draw(header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
            
            for (int i = 0; i < optionList.Count; i++)
            {
                if (i == selectedIndex)
                {
                    PuzzleNode p = new PuzzleNode(Game.jellyBlue);
                    p.screenX = optionListX - 25;
                    p.screenY = optionListY + optionHeight / 2 + i * optionGap;
                    if (state == MainMenuState.ANIMATEDOWN)
                        p.screenY -= (int)(optionGap * (1f - (float)animateTime / (float)250));
                    if (state == MainMenuState.ANIMATEUP)
                        p.screenY += (int)(optionGap * (1f - (float)animateTime / (float)250));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                    Game.spriteBatch.DrawString(Game.menuFont, optionList[i].optionText, new Vector2(optionListX, optionListY + i * optionGap), Color.LightGreen);
                }
                else
                {
                    Game.spriteBatch.DrawString(Game.menuFont, optionList[i].optionText, new Vector2(optionListX, optionListY + i * optionGap), Color.White);
                }
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
                Game.spriteBatch.DrawString(Game.spriteFont, optionList[selectedIndex].optionString, new Vector2(speechX - 255, speechY+5), Color.Black);                
            }
            Game.spriteBatch.DrawString(Game.spriteFont, "Created by Kevin Costello. Artwork by Rachel K Sreebny", new Vector2(creditsX, creditsY), Color.LightGray, 0, Vector2.Zero, .75f, SpriteEffects.None, 0);
            Game.spriteBatch.DrawString(Game.spriteFont, "Music by Kevin MacLeod", new Vector2(creditsX+95, creditsY+22), Color.LightGray, 0, Vector2.Zero, .75f, SpriteEffects.None, 0);
        }

        public MenuResult Update(GameTime gameTime)
        {
            if (Guide.IsTrialMode == false)
            {
                RemovePurchase();
            }
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
                GamePadState gamePadState = GamePad.GetState(Game.playerIndex);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                if (gamePadState.IsButtonDown(Buttons.B))
                    return MenuResult.ReturnToSplashScreen;
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                {
                    SoundEffects.PlayScore();
                    result = optionList[selectedIndex].result;
                    animateTime = 0;
                    if (result == MenuResult.BuyFullGame)
                        return result;
                    state = MainMenuState.DOCTOROUT;
                }
                if (gamePadState.IsButtonDown(Buttons.DPadDown) || gamePadState.IsButtonDown(Buttons.DPadDown) || gamePadState.IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -Game.gameSettings.controlStickTrigger || rightStick.Y < -Game.gameSettings.controlStickTrigger)
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
                if (gamePadState.IsButtonDown(Buttons.DPadUp) || gamePadState.IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > Game.gameSettings.controlStickTrigger || rightStick.Y > Game.gameSettings.controlStickTrigger)
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
