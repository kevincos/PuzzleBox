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
    class LevelSelectMenu
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

        int selectedJellyX = 517;
        int selectedJellyY = 300;
        int nameX = 420;
        int nameY = 120;
        //int infoX = 120;
        //int infoY = 450;
        //int infoLine = 20;
        int highScoreX = 440;
        int highScoreY = 430;
        int highScoreLine = 20;
        int ratingX = 410;
        int ratingY = 570;

        int doctorX = 250;
        int doctorY = 300;
        int nurseX = 800;
        int nurseY = 300;
        int docShift = 350;
        int speechX = 500;
        int speechY = 600;

        HighScoreData highScoreData = null;

        public SelectMenuState state = SelectMenuState.READY;

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
                if (animateTime > 500)
                {
                    state = SelectMenuState.READY;
                    animateTime = 500;
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
                    if (levelData.unlocked)
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
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                    
                if (state == SelectMenuState.TEXT)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                    {
                        SoundEffects.PlayClick();
                        LevelData levelData = GetLevelData(currentLevel);
                        if (levelData.unlocked == false)
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Left) || leftStick.X < -.95 || rightStick.X < -.95)
                    {                        
                        if (currentLevel > 0)
                        {
                            SoundEffects.soundSwoosh.Play();
                            state = SelectMenuState.SWAPLEFT;
                            currentLevel--;
                            animateTime = 0;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right) || leftStick.X > .95 || rightStick.X > .95)
                    {
                        if (currentLevel < levelList.Count - 1)
                        {
                            SoundEffects.soundSwoosh.Play();                        
                            state = SelectMenuState.SWAPRIGHT;
                            currentLevel++;
                            animateTime = 0;                            
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.B) || gamePadState.IsButtonDown(Buttons.B))
                    {
                        SoundEffects.soundSwoosh.Play();
                        return MenuResult.GoToMainMenu;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                    {
                        SoundEffects.soundBloop.Play();
                        textPieces = levelList[currentLevel].instructions.Split('-');
                        if (textPieces.Count() <= 1)
                        {
                            LevelData levelData = GetLevelData(currentLevel);
                            if (levelData.unlocked)
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
                    t -= animateTime * 100 / 500;
                }
                if (state == SelectMenuState.SWAPLEFT)
                {
                    t -= 100;
                    t += animateTime * 100 / 500;
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
                if(levelData.unlocked)
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
            if (state == SelectMenuState.READY)
            {
                Game.spriteBatch.DrawString(Game.spriteFont, levelList[currentLevel].name, new Vector2(nameX+50, nameY), Color.LightGreen);
                if(currentLevel > 0)
                    Game.spriteBatch.DrawString(Game.spriteFont, "<", new Vector2(nameX, nameY), Color.LightGreen);
                if (currentLevel < levelList.Count-1)
                    Game.spriteBatch.DrawString(Game.spriteFont, ">", new Vector2(nameX+200, nameY), Color.LightGreen);

                // Draw stars
                LevelData levelData = GetLevelData(currentLevel);
                for (int i = 0; i < 3; i++)
                {
                    Game.spriteBatch.Draw(emptyStar,
                        new Rectangle(ratingX + 75 * i, ratingY + 40, 64, 64), Color.White);
                }
                if (levelData.played == true)
                {
                    Game.spriteBatch.Draw(star,
                            new Rectangle(ratingX, ratingY + 40, 64, 64), Color.White);
                    if (levelData.rank >= 2)
                        Game.spriteBatch.Draw(star,
                            new Rectangle(ratingX + 75, ratingY + 40, 64, 64), Color.White);
                    if (levelData.rank >= 3)
                        Game.spriteBatch.Draw(star,
                            new Rectangle(ratingX + 150, ratingY + 40, 64, 64), Color.White);
                }
                if (levelList[currentLevel].mode == GameMode.TimeAttack)
                {
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Goal: Score as many points as possible!", new Vector2(infoX, infoY), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Orb Density: " + levelList[currentLevel].grayOrbStart, new Vector2(infoX, infoY+infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Toggle Orb %: " + levelList[currentLevel].toggleFreq, new Vector2(infoX, infoY + 2 * infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Countdown Orb %: " + levelList[currentLevel].counterFreq, new Vector2(infoX, infoY + 3 * infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Timer Orb %: " + levelList[currentLevel].timerFreq, new Vector2(infoX, infoY + 4 * infoLine), Color.LightGreen);
                    String difficultyString = "BUG";
                    if (levelList[currentLevel].difficulty == Difficulty.EASY)
                        difficultyString = "Beginner";
                    if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        difficultyString = "Standard";
                    if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        difficultyString = "Advanced";
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: " + difficultyString, new Vector2(highScoreX - 40, highScoreY), Color.LightGreen); 
                    
                    Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX, highScoreY+30), Color.LightGreen);

                    levelData = highScoreData.timeAttackLevels[currentLevel];
                    for (int i = 1; i < 6; i++)
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, i + ": "+levelData.playerNames[i-1] + " - " + levelData.highScores[i-1], new Vector2(highScoreX, highScoreY + highScoreLine * i + 35), Color.LightGreen);
                    }
                    
                }
                else if (levelList[currentLevel].mode == GameMode.MoveChallenge)
                {
                    ///Game.spriteBatch.DrawString(Game.spriteFont, "Goal: Score as many points as possible!", new Vector2(infoX, infoY), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Orb Density: " + levelList[currentLevel].grayOrbStart, new Vector2(infoX, infoY + infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Toggle Orb %: " + levelList[currentLevel].toggleFreq, new Vector2(infoX, infoY + 2 * infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Countdown Orb %: " + levelList[currentLevel].counterFreq, new Vector2(infoX, infoY + 3 * infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Timer Orb %: " + levelList[currentLevel].timerFreq, new Vector2(infoX, infoY + 4 * infoLine), Color.LightGreen);
                    String difficultyString = "BUG";
                    if (levelList[currentLevel].difficulty == Difficulty.EASY)
                        difficultyString = "Beginner";
                    if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        difficultyString = "Standard";
                    if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        difficultyString = "Advanced";
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: " + difficultyString, new Vector2(highScoreX - 40, highScoreY), Color.LightGreen); 
                    
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
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Goal: Score as many points as possible!", new Vector2(infoX, infoY), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: Medium", new Vector2(infoX, infoY + infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "Grade: B", new Vector2(infoX, infoY + 2 * infoLine), Color.LightGreen);
                    //Game.spriteBatch.DrawString(Game.spriteFont, "High Score: 5000", new Vector2(infoX, infoY + 3 * infoLine), Color.LightGreen);
                    String difficultyString = "BUG";
                    if (levelList[currentLevel].difficulty == Difficulty.EASY)
                        difficultyString = "Beginner";
                    if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        difficultyString = "Standard";
                    if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        difficultyString = "Advanced";
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: "+difficultyString, new Vector2(highScoreX-40, highScoreY), Color.LightGreen); 
                    Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX, highScoreY+30), Color.LightGreen);
                    if (highScoreData == null)
                        highScoreData = HighScoreTracker.LoadHighScores();                    
                    for (int i = 1; i < 6; i++)
                    {
                        TimeSpan t = new TimeSpan(0, 0, 0, 0, levelData.highScores[i - 1]);
                        Game.spriteBatch.DrawString(Game.spriteFont, string.Format("{0}:{1} - {2}:{3:D2}", i,levelData.playerNames[i - 1], t.Minutes, t.Seconds), new Vector2(highScoreX, highScoreY + highScoreLine * i + 35), Color.LightGreen);                        
                    }                    
                }
            }
            if (state == SelectMenuState.TEXT)
            {
                LevelData levelData = GetLevelData(currentLevel);         
                if (levelData.unlocked)
                {
                    if (textPieces[currentTextPiece].Split(':')[0] == "D")
                        JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    else
                        JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.FlipHorizontally);
                    String text = textPieces[currentTextPiece].Split(':')[1];
                    Game.spriteBatch.DrawString(Game.spriteFont, text, new Vector2(speechX - 250, speechY - 15), Color.Black);
                }
                else
                {
                    JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                    if (levelList[currentLevel].mode == GameMode.Puzzle)
                    {
                        if (levelList[currentLevel].difficulty == Difficulty.MEDIUM)
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, "Earn two stars on any Beginner puzzle \nto unlock this patient!", new Vector2(speechX - 250, speechY - 15), Color.Black);
                        }
                        else if (levelList[currentLevel].difficulty == Difficulty.HARD)
                        {
                            Game.spriteBatch.DrawString(Game.spriteFont, "Earn two stars on any Standard puzzle \nto unlock this patient!", new Vector2(speechX - 250, speechY - 15), Color.Black);
                        }
                    }
                    else
                    {
                        Game.spriteBatch.DrawString(Game.spriteFont, "Earn two stars on the previous jellyfish \nto unlock this patient!", new Vector2(speechX - 250, speechY - 15), Color.Black);
                    }
                }

            }
            
        }
    }
}
