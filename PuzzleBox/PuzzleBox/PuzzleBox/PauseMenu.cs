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
    enum PauseMenuState
    {
        NURSEIN,
        NURSEOUT,
        READY
    }

    public class PauseMenu
    {
        int animateTime = 0;
        PauseMenuState state = PauseMenuState.NURSEIN;
        MenuResult result = MenuResult.None;
        int selectedOption = 0;
        List<MenuOption> optionList;
        int cooldown = 0;

        int nurseX ;
        int nurseY ;
        int speechX ;
        int speechY ;

        public PauseMenu()
        {
            optionList = new List<MenuOption>();
            if (TutorialStage.phase == TutorialPhase.None)
            {
                optionList.Add(new MenuOption(MenuResult.ResumeGame, "Resume"));
                optionList.Add(new MenuOption(MenuResult.Replay, "Restart"));
                optionList.Add(new MenuOption(MenuResult.GoToLevelSelect, "Level Select"));
                optionList.Add(new MenuOption(MenuResult.GoToMainMenu, "Main Menu"));
            }
            else
            {
                optionList.Add(new MenuOption(MenuResult.ResumeGame, "Resume"));
                optionList.Add(new MenuOption(MenuResult.GoToMainMenu, "Main Menu"));
            }
            ApplyResolutionChanges();
        }

        public void ApplyResolutionChanges()
        {
            if (Game.gameSettings.wideScreen)
            {
                nurseX = 250;
                nurseY = 360;
                speechX = 632;
                speechY = 600;
            }
            else
            {
                nurseX = 150;
                nurseY = 400;
                speechX = 512;
                speechY = 600;
            }
        }

        public void Draw()
        {            
            if (state == PauseMenuState.NURSEIN)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 + 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == PauseMenuState.NURSEOUT)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally); 
            }
            if (state == PauseMenuState.READY)
            {
                JellyfishRenderer.DrawJellyfish(nurseX, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                Game.spriteBatch.DrawString(Game.spriteFont, "Doctor?", new Vector2(speechX-250, speechY), Color.Black);
                int offSet = 20;                
                for (int i = 0; i < optionList.Count(); i++)
                {
                    if (i == selectedOption)
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, optionList[i].optionString, new Vector2(speechX - (optionList.Count - 2) * 125 + offSet, speechY + 40), Game.jellyBlue);
                    }
                    else
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, optionList[i].optionString, new Vector2(speechX - (optionList.Count - 2) * 125 + offSet, speechY + 40), Color.Black);
                    }
                    offSet += optionList[i].optionString.Length*10+40;
                }                
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (state == PauseMenuState.NURSEIN || state == PauseMenuState.NURSEOUT)
            {
                animateTime+=gameTime.ElapsedGameTime.Milliseconds;                
            }
            if (state == PauseMenuState.NURSEIN && animateTime > 250)
            {
                state = PauseMenuState.READY;
            }
            if (state == PauseMenuState.NURSEOUT && animateTime > 250)
            {
                animateTime = 0;
                state = PauseMenuState.NURSEIN;
                return result;
            }
            if (state == PauseMenuState.READY && cooldown == 0)
            {
                GamePadState gamePadState = GamePad.GetState(Game.playerIndex);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                {
                    result = optionList[selectedOption].result;
                    animateTime = 0;
                    state = PauseMenuState.NURSEOUT;
                    SoundEffects.PlayMove();
                }
                if (gamePadState.IsButtonDown(Buttons.DPadRight) || gamePadState.IsButtonDown(Buttons.DPadRight) || Keyboard.GetState().IsKeyDown(Keys.Right) || leftStick.X > Game.gameSettings.controlStickTrigger || rightStick.X > Game.gameSettings.controlStickTrigger)
                {
                    selectedOption++;
                    SoundEffects.PlayClick();
                    if (selectedOption >= optionList.Count())
                        selectedOption = optionList.Count() - 1;
                    cooldown = 250;
                }
                if (gamePadState.IsButtonDown(Buttons.DPadLeft) || gamePadState.IsButtonDown(Buttons.DPadLeft) || Keyboard.GetState().IsKeyDown(Keys.Left) || leftStick.X < -Game.gameSettings.controlStickTrigger || rightStick.X < -Game.gameSettings.controlStickTrigger)
                {
                    selectedOption--;
                    SoundEffects.PlayClick();
                    if (selectedOption < 0)
                        selectedOption = 0;
                    cooldown = 250;
                }
                if (gamePadState.IsButtonDown(Buttons.B))
                {
                    result = MenuResult.ResumeGame;
                    animateTime = 0;
                    state = PauseMenuState.NURSEOUT;
                    SoundEffects.PlayMove();
                }
            }
            return MenuResult.None;            
        }
    }
}
