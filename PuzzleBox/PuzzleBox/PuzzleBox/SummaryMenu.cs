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
    public enum SummaryMenuState
    {
        NURSEIN,
        NURSEOUT,
        READY,
        WAIT
    }

    public class SummaryMenu
    {
        int animateTime = 0;
        public SummaryMenuState state = SummaryMenuState.NURSEIN;
        MenuResult result = MenuResult.None;
        int selectedOption = 0;
        List<MenuOption> optionList;
        int cooldown = 100;

        int nurseX ;
        int nurseY ;
        int speechX ;
        int speechY ;

        public String text = "Way to go, Doctor!";
        public bool win = true;

        public SummaryMenu(bool win)
        {
            this.win = win;
            optionList = new List<MenuOption>();
            bool endOfSection = TutorialStage.IsEndOfSection();
            if (TutorialStage.phase!=TutorialPhase.None && endOfSection==false)
            {
                if(!(TutorialStage.phase == TutorialPhase.Intro && TutorialStage.introIndex == TutorialStage.controlLessonIndex))
                    optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
            }
            else if (TutorialStage.phase==TutorialPhase.Fail)
            {
                optionList.Add(new MenuOption(MenuResult.Replay, "Try Again"));
                optionList.Add(new MenuOption(MenuResult.GoToMainMenu, "Main Menu"));
            }
            else if (TutorialStage.phase != TutorialPhase.None && endOfSection == true)
            {
                optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
                optionList.Add(new MenuOption(MenuResult.Replay, "Practice"));
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
            ApplyResolutionChanges();
        }

        public void ApplyResolutionChanges()
        {
            if (Game.gameSettings.wideScreen)
            {
                nurseX = 200;
                nurseY = 360;
                speechX = 652;
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
            if (state == SummaryMenuState.NURSEIN)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 + 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == SummaryMenuState.NURSEOUT)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == SummaryMenuState.READY || state == SummaryMenuState.WAIT)
            {
                JellyfishRenderer.DrawJellyfish(nurseX, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                if (Game.metaState != MetaState.GamePlay)
                {
                    JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    Game.spriteBatch.DrawString(Game.spriteFont, text, new Vector2(speechX - 260, speechY - 5), Color.Black);
                    int offSet = 20;
                    for (int i = 0; i < optionList.Count(); i++)
                    {
                        if (i == selectedOption)
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, optionList[i].optionString, new Vector2(speechX - (optionList.Count - 2) * 130 + offSet, speechY + 55), Game.jellyBlue);
                        }
                        else
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, optionList[i].optionString, new Vector2(speechX - (optionList.Count - 2) * 130 + offSet, speechY + 55), Color.Black);
                        }
                        offSet += optionList[i].optionString.Length * 10 + 40;
                    }
                    if (optionList.Count() == 1)
                    {
                        Game.spriteBatch.Draw(HelpOverlay.summary_confirm, new Rectangle(speechX+255,speechY+60,25,25),Color.Green);
                    }
                }
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (state == SummaryMenuState.WAIT && animateTime > 1000)
            {
                SoundEffects.PlayClick();
                result = MenuResult.GoToResults;
                String nextText = null;
                if (TutorialStage.controlLessonIndex == TutorialStage.introIndex)
                {
                    optionList = new List<MenuOption>();
                }
                else
                {
                    optionList = new List<MenuOption>();
                    optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
                }
                state = SummaryMenuState.READY;
                nextText = TutorialStage.IntroText();
                text = nextText;
                cooldown = 250;
            }
            if (state == SummaryMenuState.NURSEIN || state == SummaryMenuState.NURSEOUT || state == SummaryMenuState.WAIT)
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
                GamePadState gamePadState = GamePad.GetState(Game.playerIndex);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                if (!(TutorialStage.phase != TutorialPhase.None && TutorialStage.introIndex - 1 == TutorialStage.controlLessonIndex))
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                    {
                        SoundEffects.PlayClick();
                        result = optionList[selectedOption].result;
                        String nextText = null;
                        if (TutorialStage.phase == TutorialPhase.Intro)
                        {
                            if (TutorialStage.controlLessonIndex == TutorialStage.introIndex)
                            {
                                optionList = new List<MenuOption>();
                            }
                            else
                            {
                                optionList = new List<MenuOption>();
                                optionList.Add(new MenuOption(MenuResult.GoToResults, "Continue"));
                            }
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
                            if ((TutorialStage.phase == TutorialPhase.Pass && result != MenuResult.Replay) || TutorialStage.phase == TutorialPhase.Fail)
                            {
                                return result;
                            }
                            animateTime = 0;
                            state = SummaryMenuState.NURSEOUT;

                        }
                    }

                    if (gamePadState.IsButtonDown(Buttons.DPadRight) || gamePadState.IsButtonDown(Buttons.DPadRight) || Keyboard.GetState().IsKeyDown(Keys.Right) || leftStick.X > Game.gameSettings.controlStickTrigger || rightStick.X > Game.gameSettings.controlStickTrigger)
                    {
                        SoundEffects.PlayClick();
                        selectedOption++;
                        if (selectedOption >= optionList.Count())
                            selectedOption = optionList.Count() - 1;
                        cooldown = 250;
                    }
                    if (gamePadState.IsButtonDown(Buttons.DPadLeft) || gamePadState.IsButtonDown(Buttons.DPadLeft) || Keyboard.GetState().IsKeyDown(Keys.Left) || leftStick.X < -Game.gameSettings.controlStickTrigger || rightStick.X < -Game.gameSettings.controlStickTrigger)
                    {
                        SoundEffects.PlayClick();
                        selectedOption--;
                        if (selectedOption < 0)
                            selectedOption = 0;
                        cooldown = 250;
                    }
                }
                else
                {
                    if (TutorialStage.restrictions==ControlRestrictions.StickOnly && (gamePadState.IsButtonDown(Buttons.DPadUp) || Keyboard.GetState().IsKeyDown(Keys.Up) || gamePadState.ThumbSticks.Left.Y > Game.gameSettings.controlStickTrigger ||
                        gamePadState.IsButtonDown(Buttons.DPadDown) || Keyboard.GetState().IsKeyDown(Keys.Down) || gamePadState.ThumbSticks.Left.Y < -Game.gameSettings.controlStickTrigger ||
                        gamePadState.IsButtonDown(Buttons.DPadLeft) || Keyboard.GetState().IsKeyDown(Keys.Left) || gamePadState.ThumbSticks.Left.X > Game.gameSettings.controlStickTrigger ||
                        gamePadState.IsButtonDown(Buttons.DPadRight) || Keyboard.GetState().IsKeyDown(Keys.Left) || gamePadState.ThumbSticks.Left.X < -Game.gameSettings.controlStickTrigger))
                    {
                        state = SummaryMenuState.WAIT;
                        animateTime = 0;
                    }
                    if (TutorialStage.restrictions == ControlRestrictions.ShouldersOnly && (gamePadState.IsButtonDown(Buttons.LeftShoulder) || gamePadState.IsButtonDown(Buttons.RightShoulder) || Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.S)))
                    {
                        state = SummaryMenuState.WAIT;
                        animateTime = 0;
                    }
                    if (TutorialStage.restrictions == ControlRestrictions.TriggersOnly && (gamePadState.IsButtonDown(Buttons.LeftTrigger) || gamePadState.IsButtonDown(Buttons.RightTrigger) || Keyboard.GetState().IsKeyDown(Keys.Q) || Keyboard.GetState().IsKeyDown(Keys.W)))
                    {
                        state = SummaryMenuState.WAIT;
                        animateTime = 0;
                    }
                }
            }
            return MenuResult.None;
        }
    }
}
