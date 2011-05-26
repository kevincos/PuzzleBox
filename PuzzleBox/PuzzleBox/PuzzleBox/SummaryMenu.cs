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
    enum SummaryMenuState
    {
        NURSEIN,
        NURSEOUT,
        READY
    }

    class SummaryMenu
    {
        int animateTime = 0;
        public SummaryMenuState state = SummaryMenuState.NURSEIN;
        MenuResult result = MenuResult.None;
        int selectedOption = 0;
        List<MenuOption> optionList;
        int cooldown = 100;

        int nurseX = 150;
        int nurseY = 400;
        int speechX = 512;
        int speechY = 600;

        public String text = "Way to go, Doctor!";
        public bool win = true;

        public SummaryMenu(bool win)
        {
            this.win = win;
            optionList = new List<MenuOption>();
            bool endOfSection = TutorialStage.IsEndOfSection();
            if (TutorialStage.phase!=TutorialPhase.None && endOfSection==false)
            {
                optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
            }
            else if (TutorialStage.phase != TutorialPhase.None && endOfSection == true)
            {
                optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
                optionList.Add(new MenuOption(MenuResult.Replay, "Practice"));                            
            }
            else if (TutorialStage.phase==TutorialPhase.Fail)
            {
                optionList.Add(new MenuOption(MenuResult.Replay, "Try Again"));
                optionList.Add(new MenuOption(MenuResult.GoToMainMenu, "Main Menu"));
            }
            else if (win)
                optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
            else
            {
                optionList.Add(new MenuOption(MenuResult.Undo, "Undo"));
                optionList.Add(new MenuOption(MenuResult.Replay, "Try Again"));
                optionList.Add(new MenuOption(MenuResult.GoToLevelSelect, "Level Select"));
                optionList.Add(new MenuOption(MenuResult.GoToMainMenu, "Main Menu"));
            }
        }

        public void Draw()
        {
            if (state == SummaryMenuState.NURSEIN)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 + 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == SummaryMenuState.NURSEOUT)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == SummaryMenuState.READY)
            {
                JellyfishRenderer.DrawJellyfish(nurseX, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                if (Game.metaState != MetaState.GamePlay)
                {
                    JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    Game.spriteBatch.DrawString(Game.spriteFont, text, new Vector2(speechX - 260, speechY - 22), Color.Black);
                    int offSet = 50;
                    for (int i = 0; i < optionList.Count(); i++)
                    {
                        if (i == selectedOption)
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, optionList[i].optionString, new Vector2(speechX - (optionList.Count - 2) * 130 + offSet, speechY + 28), Color.Blue);
                        }
                        else
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, optionList[i].optionString, new Vector2(speechX - (optionList.Count - 2) * 130 + offSet, speechY + 28), Color.Black);
                        }
                        offSet += optionList[i].optionString.Length * 10 + 40;
                    }
                }
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (state == SummaryMenuState.NURSEIN || state == SummaryMenuState.NURSEOUT)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (state == SummaryMenuState.NURSEIN && animateTime > 250)
            {
                state = SummaryMenuState.READY;
            }
            if (state == SummaryMenuState.NURSEOUT && animateTime > 250)
            {
                animateTime = 0;
                state = SummaryMenuState.NURSEIN;
                return result;
            }
            if (state == SummaryMenuState.READY && cooldown == 0)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                {
                    SoundEffects.PlayClick();
                    result = optionList[selectedOption].result;
                    String nextText = null;
                    if (TutorialStage.phase==TutorialPhase.Intro)
                    {
                        nextText = TutorialStage.IntroText();                        
                    }
                    else if (TutorialStage.phase == TutorialPhase.Pass)
                    {
                        nextText = TutorialStage.SuccessText();
                        if (TutorialStage.IsEndOfSection())
                        {                            
                            optionList = new List<MenuOption>();
                            optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
                            optionList.Add(new MenuOption(MenuResult.Replay, "Practice"));                            
                        }
                    }
                    else if (TutorialStage.phase == TutorialPhase.Fail)
                    {
                        nextText = TutorialStage.FailureText();
                        if (TutorialStage.IsEndOfSection())
                        {
                            optionList = new List<MenuOption>();
                            optionList.Add(new MenuOption(MenuResult.Replay, "Try Again"));
                            optionList.Add(new MenuOption(MenuResult.GoToMainMenu, "Main Menu"));                            
                        }
                    }
                    if (nextText != null)
                    {
                        text = nextText;
                        cooldown = 250;
                    }
                    else
                    {
                        if (TutorialStage.phase == TutorialPhase.Pass || TutorialStage.phase == TutorialPhase.Fail)
                        {
                            return result;
                        }
                        animateTime = 0;
                        state = SummaryMenuState.NURSEOUT;
                        
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) || leftStick.X > .95 || rightStick.X > .95)
                {
                    SoundEffects.PlayClick();
                    selectedOption++;
                    if (selectedOption >= optionList.Count())
                        selectedOption = optionList.Count() - 1;
                    cooldown = 250;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || leftStick.X < -.95 || rightStick.X < -.95)
                {
                    SoundEffects.PlayClick();
                    selectedOption--;
                    if (selectedOption < 0)
                        selectedOption = 0;
                    cooldown = 250;
                }
            }
            return MenuResult.None;
        }
    }
}
