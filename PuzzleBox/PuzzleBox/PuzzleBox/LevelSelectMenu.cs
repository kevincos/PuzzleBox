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
    public  class LevelSelectMenu
    {
        public enum SelectMenuState
        {
            READY,
            SWAPLEFT,
            SWAPRIGHT,
            DOCTORIN,
            DOCTOROUT,            
            TEXT,
            SELECTING,
            LOAD
        }

        public static Texture2D emptyStar;
        public static Texture2D star;

        public List<Settings> levelList;

        int animateTime;

        public int currentLevel = 0;

        public int cooldown = 0;

        string[] textPieces;
        int currentTextPiece;

        int selectedJellyX;
        int selectedJellyY;
        int nameX ;
        int nameY;
        int highScoreX;
        int highScoreY;
        int highScoreLine;
        int ratingX ;
        int ratingY;
        int doctorX ;
        int doctorY;
        int nurseX ;
        int nurseY;
        int docShift;
        int speechX;
        int speechY;
        int previewOffset;
        int previewX;
        int previewY;

        int numSwaps = 0;
        int swapTime = 500;
        bool left = false;

        HighScoreData highScoreData = null;

        public SelectMenuState state = SelectMenuState.READY;

        public LevelSelectMenu()
        {
            ApplyResolutionChanges();
        }

        public void ApplyResolutionChanges()
        {
            if (Game.gameSettings.wideScreen)
            {
                selectedJellyX = Game.screenCenterX+4;
                selectedJellyY = Game.screenCenterY -86;
                nameX = 510;
                nameY = 90;
                highScoreX = 560;
                highScoreY = 410;
                highScoreLine = 22;
                ratingX = 530;
                ratingY = 550;
                doctorX = 350;
                doctorY = 300;
                nurseX = 950;
                nurseY = 300;
                docShift = 450;
                speechX = 600;
                speechY = 600;
                previewOffset = 100;
                previewX = 620;
                previewY = 410;
            }
            else
            {
                selectedJellyX = Game.screenCenterX + 4;
                selectedJellyY = Game.screenCenterY - 86;
                nameX = 390;
                nameY = 120;
                highScoreX = 440;
                highScoreY = 430;
                highScoreLine = 22;
                ratingX = 410;
                ratingY = 570;
                doctorX = 250;
                doctorY = 300;
                nurseX = 800;
                nurseY = 300;
                docShift = 350;
                speechX = 500;
                speechY = 600;
                previewOffset = 100;
                previewX = 500;
                previewY = 430;
            }

        }

        public Settings GetCurrentSettings()
        {
            return levelList[currentLevel];
        }

        public LevelData GetLevelData(int i)
        {            
            if (levelList[i].mode == GameMode.TimeAttack)
            {
                return highScoreData.timeAttackLevels[i];
            }
            else if (levelList[i].mode == GameMode.MoveChallenge)
            {
                return highScoreData.moveChallengeLevels[i];
            }
            else
            {
                return highScoreData.puzzleLevels[i];
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            if (state == SelectMenuState.SWAPLEFT || state == SelectMenuState.SWAPRIGHT)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;

                if (animateTime > swapTime)
                {
                    // Update currentLevel
                    if (levelList[currentLevel].mode == GameMode.TimeAttack)
                        Game.gameSettings.timeAttackViewLevel = currentLevel;
                    if (levelList[currentLevel].mode == GameMode.Puzzle)
                        Game.gameSettings.puzzleViewLevel = currentLevel;
                    if (levelList[currentLevel].mode == GameMode.MoveChallenge)
                        Game.gameSettings.moveChallengeViewLevel = currentLevel;
                    state = SelectMenuState.READY;
                    animateTime = swapTime;
                }
            }

            if (state == SelectMenuState.SELECTING)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > 1000)
                {
                    animateTime = 1000;
                    state = SelectMenuState.READY;
                    return MenuResult.StartTimeAttack;
                }
            }
            if (state == SelectMenuState.LOAD)
            {
                state = SelectMenuState.READY;
                MusicControl.PlayMenuMusic();
                highScoreData = HighScoreTracker.LoadHighScores();
            }
            if (state == SelectMenuState.DOCTORIN)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > 500)
                {
                    animateTime = 500;
                    state = SelectMenuState.TEXT;
                    currentTextPiece = 0;
                }                    
            }
            if (state == SelectMenuState.DOCTOROUT)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > 500)
                {
                    animateTime = 0;
                    LevelData levelData = GetLevelData(currentLevel);
                    if (levelData.unlocked && !(Guide.IsTrialMode && levelList[currentLevel].fullVersionOnly))
                        state = SelectMenuState.SELECTING;
                    else
                        state = SelectMenuState.READY;
                    currentTextPiece = 0;
                }
            }
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown <= 0)
            {
                cooldown = 0;
                GamePadState gamePadState = GamePad.GetState(Game.playerIndex);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                    
                if (state == SelectMenuState.TEXT)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                    {
                        SoundEffects.PlayClick();
                        LevelData levelData = GetLevelData(currentLevel);
                        if (levelData.unlocked == false || (Guide.IsTrialMode && levelList[currentLevel].fullVersionOnly))
                            state = SelectMenuState.DOCTOROUT;
                        currentTextPiece++;
                        animateTime = 0;
                        if (currentTextPiece == textPieces.Count())
                        {
                            state = SelectMenuState.DOCTOROUT;                            
                        }
                        else
                            cooldown = 250;
                    }
                }
                if (state == SelectMenuState.READY)
                {
                    if (gamePadState.IsButtonDown(Buttons.DPadLeft) || gamePadState.IsButtonDown(Buttons.DPadLeft) || Keyboard.GetState().IsKeyDown(Keys.Left) || leftStick.X < -Game.gameSettings.controlStickTrigger || rightStick.X < -Game.gameSettings.controlStickTrigger)
                    {                        
                        if (currentLevel > 0)
                        {
                            SoundEffects.PlayMove();
                            if (!left)
                                numSwaps = 0;
                            numSwaps++;
                            if (numSwaps > 1)
                                swapTime = 400;
                            if (numSwaps > 2)
                                swapTime = 200;

                            left = true;
                            state = SelectMenuState.SWAPLEFT;
                            currentLevel--;
                            animateTime = 0;
                        }
                    }
                    if (gamePadState.IsButtonDown(Buttons.DPadRight) || gamePadState.IsButtonDown(Buttons.DPadRight) || Keyboard.GetState().IsKeyDown(Keys.Right) || leftStick.X > Game.gameSettings.controlStickTrigger || rightStick.X > Game.gameSettings.controlStickTrigger)
                    {
                        if (currentLevel < levelList.Count - 1)
                        {
                            SoundEffects.PlayMove();
                            if (left)
                                numSwaps = 0;
                            numSwaps++;
                            if (numSwaps > 1)
                                swapTime = 400;
                            if (numSwaps > 2)
                                swapTime = 200;

                            left = false;
                            state = SelectMenuState.SWAPRIGHT;
                            currentLevel++;
                            animateTime = 0;                            
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.B) || gamePadState.IsButtonDown(Buttons.B))
                    {
                        SoundEffects.PlayMove();
                        return MenuResult.GoToMainMenu;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                    {
                        SoundEffects.PlayScore();
                        textPieces = levelList[currentLevel].instructions.Split('-');
                        if (textPieces.Count() <= 1)
                        {
                            LevelData levelData = GetLevelData(currentLevel);
                            if (levelData.unlocked && !(Guide.IsTrialMode && levelList[currentLevel].fullVersionOnly))
                                state = SelectMenuState.SELECTING;
                            else
                                state = SelectMenuState.DOCTORIN;
                        }
                        else
                        {
                            state = SelectMenuState.DOCTORIN;
                        }
                        animateTime = 0;
                    }
                    if (state == SelectMenuState.READY)
                    {
                        numSwaps = 0;
                        swapTime = 500;
                    }
                }
            }
            return MenuResult.None;
        }

        public void Draw()
        {
            if (highScoreData == null)
                highScoreData = HighScoreTracker.LoadHighScores();
            for (int i = 0; i < levelList.Count; i++)
            {
                int t = (i-currentLevel) * 100;
                if (state == SelectMenuState.SWAPRIGHT)
                {
                    t += 100;
                    t -= animateTime * 100 / swapTime;
                }
                if (state == SelectMenuState.SWAPLEFT)
                {
                    t -= 100;
                    t += animateTime * 100 / swapTime;
                }
                int x = t*3;
                int y = -t*t/100;
                float scale = Math.Max(0, .50f - (t * t) / 50000f);
                if (state == SelectMenuState.SELECTING)
                {
                    if (i != currentLevel)
                    {
                        scale = Math.Max(0, .50f - (t * t) / 50000f) * (1f - animateTime / 1000f);
                    }
                    else
                    {
                        scale = .5f + .5f * (animateTime / 1000f);
                        y = (int)(100f * (animateTime / 1000f));
                        
                    }
                }

                LevelData levelData = GetLevelData(i);
                if (levelData.unlocked && !(Guide.IsTrialMode && levelList[i].fullVersionOnly))
                    JellyfishRenderer.DrawJellyfish(x + selectedJellyX, selectedJellyY + y, 100, levelList[i].texture, scale);
                else
                    JellyfishRenderer.DrawJellyfish(x + selectedJellyX, selectedJellyY + y, 100, JellyfishRenderer.mysteryJelly, scale);
            }
            if (state == SelectMenuState.DOCTORIN)
            {
                JellyfishRenderer.DrawJellyfish(doctorX - docShift + (int)(docShift * animateTime / 500f), doctorY, 100, JellyfishRenderer.doctorJellyfish, 1f);
                JellyfishRenderer.DrawJellyfish(nurseX + docShift - (int)(docShift * animateTime / 500f), nurseY, 100, JellyfishRenderer.nurseJellyfish, 1f);
            }
            if (state == SelectMenuState.TEXT)
            {                
                JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, 1f);
                JellyfishRenderer.DrawJellyfish(nurseX, nurseY, 100, JellyfishRenderer.nurseJellyfish, 1f);
            }
            if (state == SelectMenuState.DOCTOROUT)
            {
                JellyfishRenderer.DrawJellyfish(doctorX - (int)(docShift * animateTime / 500f), doctorY, 100, JellyfishRenderer.doctorJellyfish, 1f);
                JellyfishRenderer.DrawJellyfish(nurseX + (int)(docShift * animateTime / 500f), nurseY, 100, JellyfishRenderer.nurseJellyfish, 1f);
            }
            if (state == SelectMenuState.READY || state == SelectMenuState.SWAPLEFT || state == SelectMenuState.SWAPRIGHT)
            {
                Game.spriteBatch.DrawString(Game.spriteFont, levelList[currentLevel].name, new Vector2(nameX+50, nameY), Color.LightGreen);
                if(currentLevel > 0)
                    Game.spriteBatch.DrawString(Game.spriteFont, "<", new Vector2(nameX, nameY), Color.LightGreen);
                if (currentLevel < levelList.Count-1)
                    Game.spriteBatch.DrawString(Game.spriteFont, ">", new Vector2(nameX+250, nameY), Color.LightGreen);

                // Draw stars
                LevelData levelData = GetLevelData(currentLevel);
                int ratingOffset = 0;
                if(levelList[currentLevel].mode==GameMode.Puzzle)
                    ratingOffset = previewOffset;
                for (int i = 0; i < 3; i++)
                {
                    Game.spriteBatch.Draw(emptyStar,
                        new Rectangle(ratingX + 75 * i - ratingOffset, ratingY + 40, 64, 64), Color.White);
                }
                if (levelData.played == true)
                {
                    Game.spriteBatch.Draw(star,
                            new Rectangle(ratingX - ratingOffset, ratingY + 40, 64, 64), Color.White);
                    if (levelData.rank >= 2)
                        Game.spriteBatch.Draw(star,
                            new Rectangle(ratingX + 75 - ratingOffset, ratingY + 40, 64, 64), Color.White);
                    if (levelData.rank >= 3)
                        Game.spriteBatch.Draw(star,
                            new Rectangle(ratingX + 150 - ratingOffset, ratingY + 40, 64, 64), Color.White);
                }
                if (levelList[currentLevel].mode == GameMode.TimeAttack)
                {
                    String difficultyString = "BUG";
                    if (levelList[currentLevel].difficulty == Difficulty.EASY)
                        difficultyString = "Beginner";
                    if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        difficultyString = "Standard";
                    if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        difficultyString = "Advanced";
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: " + difficultyString, new Vector2(highScoreX - 20, highScoreY), Color.LightGreen); 
                    
                    Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX, highScoreY+30), Color.LightGreen);

                    levelData = highScoreData.timeAttackLevels[currentLevel];
                    for (int i = 1; i < 6; i++)
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, i + ": "+levelData.playerNames[i-1] + " - " + levelData.highScores[i-1], new Vector2(highScoreX, highScoreY + highScoreLine * i + 35), Color.LightGreen);
                    }
                    
                }
                else if (levelList[currentLevel].mode == GameMode.MoveChallenge)
                {
                    String difficultyString = "BUG";
                    if (levelList[currentLevel].difficulty == Difficulty.EASY)
                        difficultyString = "Beginner";
                    if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        difficultyString = "Standard";
                    if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        difficultyString = "Advanced";
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: " + difficultyString, new Vector2(highScoreX - 20, highScoreY), Color.LightGreen); 
                    
                    Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX, highScoreY+30), Color.LightGreen);
                    if (highScoreData == null)
                        highScoreData = HighScoreTracker.LoadHighScores();
                    for (int i = 1; i < 6; i++)
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, i + ": " + levelData.playerNames[i - 1] + " - " + levelData.highScores[i - 1], new Vector2(highScoreX, highScoreY + highScoreLine * i + 35), Color.LightGreen);
                    }
                }
                else
                {
                    String difficultyString = "BUG";
                    if (levelList[currentLevel].difficulty == Difficulty.EASY)
                        difficultyString = "Beginner";
                    if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        difficultyString = "Standard";
                    if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        difficultyString = "Advanced";
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: "+difficultyString, new Vector2(highScoreX-20 - previewOffset, highScoreY), Color.LightGreen); 
                    Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX -previewOffset, highScoreY+30), Color.LightGreen);
                    if (highScoreData == null)
                        highScoreData = HighScoreTracker.LoadHighScores();                    
                    for (int i = 1; i < 6; i++)
                    {
                        TimeSpan t = new TimeSpan(0, 0, 0, 0, levelData.highScores[i - 1]);
                        Game.spriteBatch.DrawString(Game.spriteFont, string.Format("{0}:{1} - {2}:{3:D2}", i,levelData.playerNames[i - 1], t.Minutes, t.Seconds), new Vector2(highScoreX-previewOffset, highScoreY + highScoreLine * i + 35), Color.LightGreen);                        
                    }
                    Game.spriteBatch.Draw(levelList[currentLevel].preview, new Rectangle(previewX, previewY, 625/2, 469/2), Color.White);
                }
            }
            if (state == SelectMenuState.TEXT)
            {
                LevelData levelData = GetLevelData(currentLevel);

                if (levelData.unlocked && !(Guide.IsTrialMode && levelList[currentLevel].fullVersionOnly))
                {
                    if (textPieces[currentTextPiece].Split(':')[0] == "D")
                        JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    else
                        JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.FlipHorizontally);
                    String text = textPieces[currentTextPiece].Split(':')[1];
                    Game.spriteBatch.DrawString(Game.spriteFont, text, new Vector2(speechX - 250, speechY), Color.Black);
                }
                else if (Guide.IsTrialMode && levelList[currentLevel].fullVersionOnly)
                {
                    JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    Game.spriteBatch.DrawString(Game.spriteFont, "This patient is only available on the full version\nof Jellyfish, MD. Go to the main menu to unlock it!", new Vector2(speechX - 250, speechY), Color.Black);
                }
                else
                {
                    JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    if (levelList[currentLevel].mode == GameMode.Puzzle)
                    {
                        if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, "Earn two stars on any Beginner puzzle \nto unlock this patient!", new Vector2(speechX - 250, speechY), Color.Black);
                        }
                        else if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, "Earn two stars on any Standard puzzle \nto unlock this patient!", new Vector2(speechX - 250, speechY), Color.Black);
                        }
                    }
                    else
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, "Earn two stars on the previous jellyfish \nto unlock this patient!", new Vector2(speechX - 250, speechY), Color.Black);
                    }
                }

            }
            
        }
    }
}
