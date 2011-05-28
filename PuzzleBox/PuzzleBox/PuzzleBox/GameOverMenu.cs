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
    enum GameOverMenuState
    {
        DOCTORIN,
        DOCTOROUT,
        READY,
        ANIMATEUP,
        ANIMATEDOWN,
        SCORECHECK,
        INITIALS
    }

    class GameOverMenu
    {
        public Texture2D background;
        public Texture2D header;

        public Texture2D emptyStar;
        public Texture2D star;

        public int score = 0;
        public int level = 0;
        int animateTime = 0;
        public GameOverMenuState state = GameOverMenuState.SCORECHECK;
        MenuResult result = MenuResult.None;
        int selectedIndex = 0;
        List<MenuOption> optionList;
        int cooldown = 0;
        int headerX = 100;
        int headerY = 20;
        int headerWidth = 824;
        int headerHeight = 150;
        int doctorX = 625;
        int doctorY = 375;
        int optionListX = 120;
        int optionListY = 200;
        int optionGap = 50;
        int optionWidth = 250;
        int optionHeight = 50;
        int scoreX = 120;
        int scoreY = 390;
        int highScoreX = 120;
        int highScoreY = 500;
        int highScoreLine = 30;
        int speechX = 700;
        int speechY = 625;
        
        int rank = -1;
        String initials = "---";
        int currentCharacter = 0;

        String congratulationsMessage = "Testing...";

        LevelData levelData;

        public GameOverMenu()
        {
            optionList = new List<MenuOption>();
        }

        public void AddMenuItem(MenuResult result, String text)
        {
            optionList.Add(new MenuOption(result, text, "BUG"));
        }

        public void Draw()
        {            
            Game.spriteBatch.Draw(background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);
            Game.spriteBatch.Draw(header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
            for (int i = 0; i < optionList.Count; i++)
            {
                if (i == selectedIndex && state != GameOverMenuState.INITIALS)
                {
                    PuzzleNode p = new PuzzleNode(Color.Blue);
                    p.screenX = optionListX-25;
                    p.screenY = optionListY +optionHeight/2+ i * optionGap;
                    if (state == GameOverMenuState.ANIMATEDOWN)
                        p.screenY -= (int)(optionGap * (1f - (float)animateTime / (float)250));
                    if (state == GameOverMenuState.ANIMATEUP)
                        p.screenY += (int)(optionGap * (1f - (float)animateTime / (float)250));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                    Game.spriteBatch.DrawString(Game.menuFont, optionList[i].optionText, new Vector2(optionListX, optionListY + i * optionGap), Color.LightGreen);
                }
                else
                    Game.spriteBatch.DrawString(Game.menuFont, optionList[i].optionText, new Vector2(optionListX, optionListY + i * optionGap), Color.White);
            }
            if (state == GameOverMenuState.DOCTORIN)
            {
                JellyfishRenderer.DrawJellyfish(doctorX + 800 - 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX + 600 - 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            else if (state == GameOverMenuState.DOCTOROUT)
            {
                JellyfishRenderer.DrawJellyfish(doctorX + 200 + 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX + 600 * animateTime / 250, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            else
            {
                JellyfishRenderer.DrawJellyfish(doctorX+200, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (Game.currentSettings.mode == GameMode.Puzzle)
            {
                TimeSpan t = new TimeSpan(0, 0, 0, 0, score);
                Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME: {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(scoreX, scoreY), Color.LightGreen, 0, Vector2.Zero, 1.6f, SpriteEffects.None, 0);
                for (int i = 0; i < 3; i++)
                {
                    Game.spriteBatch.Draw(emptyStar,
                        new Rectangle(scoreX + 75 * i, scoreY + 50, 64, 64), Color.White);
                }
                Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX, scoreY + 50, 64, 64), Color.White);                
                if (score <= Game.currentSettings.two_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 75, scoreY + 50, 64, 64), Color.White);
                if (score <= Game.currentSettings.three_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 150, scoreY + 50, 64, 64), Color.White);                    
            }
            else
            {
                Game.spriteBatch.DrawString(Game.spriteFont, "SCORE:" + score, new Vector2(scoreX, scoreY), Color.LightGreen, 0, Vector2.Zero, 1.6f, SpriteEffects.None, 0);
                for (int i = 0; i < 3; i++)
                {                    
                    Game.spriteBatch.Draw(emptyStar,
                        new Rectangle(scoreX + 75*i, scoreY + 50, 64, 64), Color.White);                    
                }
                Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX, scoreY + 50, 64, 64), Color.White);                    
                if(score >= Game.currentSettings.two_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 75, scoreY + 50, 64, 64), Color.White);
                if (score >= Game.currentSettings.three_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 150, scoreY + 50, 64, 64), Color.White);                    
            }

            if (levelData != null)
            {
                Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX, highScoreY), Color.LightGreen, 0, Vector2.Zero, 1.6f, SpriteEffects.None, 0);
                for (int i = 1; i < 6; i++)
                {
                    if (i - 1 == rank)
                    {
                        if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            TimeSpan t = new TimeSpan(0, 0, 0, 0, levelData.highScores[i - 1]);
                            Game.spriteBatch.DrawString(Game.spriteFont, string.Format("{0}: {1}:{2:D2}", levelData.playerNames[i - 1], t.Minutes, t.Seconds), new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 4), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                        }
                        else
                            Game.spriteBatch.DrawString(Game.spriteFont, i + ": " + levelData.playerNames[i - 1] + " - " + levelData.highScores[i - 1], new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 4), Color.White, 0, Vector2.Zero, 1.5f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            TimeSpan t = new TimeSpan(0, 0, 0, 0, levelData.highScores[i - 1]);
                            Game.spriteBatch.DrawString(Game.spriteFont, string.Format("{0}: {1}:{2:D2}", levelData.playerNames[i - 1], t.Minutes, t.Seconds), new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 5), Color.LightGreen, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 0);
                        }
                        else
                            Game.spriteBatch.DrawString(Game.spriteFont, i + ": " + levelData.playerNames[i - 1] + " - " + levelData.highScores[i - 1], new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 5), Color.LightGreen, 0, Vector2.Zero, 1.4f, SpriteEffects.None, 0);
                    }
                }
            }

            if (currentCharacter > 2 || rank == -1)
            {
                JellyfishRenderer.DrawSpeechBubble2(speechX, speechY, 100, SpriteEffects.FlipHorizontally);
                Game.spriteBatch.DrawString(Game.spriteFont, congratulationsMessage, new Vector2(speechX - 260, speechY), Color.Black);
            }

            if (rank != -1 && currentCharacter <= 2)
            {
                JellyfishRenderer.DrawSpeechBubble2(speechX, speechY, 100, SpriteEffects.FlipHorizontally);
                Game.spriteBatch.DrawString(Game.spriteFont, "Way to go! You got a high score! Please enter", new Vector2(speechX-260, speechY), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, "your initials: ", new Vector2(speechX - 260, speechY+25), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, "" + initials[2], new Vector2(speechX - 70, speechY + 25), Color.Black,0,Vector2.Zero,1f,SpriteEffects.None,0);
                Game.spriteBatch.DrawString(Game.spriteFont, "" + initials[1], new Vector2(speechX - 95, speechY + 25), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                Game.spriteBatch.DrawString(Game.spriteFont, "" + initials[0], new Vector2(speechX - 120, speechY + 25), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                Game.spriteBatch.DrawString(Game.spriteFont, "^", new Vector2(speechX - 120 + 25 * currentCharacter, speechY + 45), Color.Black, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (state == GameOverMenuState.SCORECHECK)
            {
                currentCharacter = 0;
                rank = -1;
                HighScoreData highScoreData = HighScoreTracker.LoadHighScores();
                if (Game.currentSettings.mode == GameMode.TimeAttack)
                {
                    levelData = highScoreData.timeAttackLevels[level];
                    highScoreData.timeAttackLevels[level].played = true;
                    if (highScoreData.timeAttackLevels[level].rank < 3 && score >= Game.currentSettings.three_star)
                    {
                        congratulationsMessage = "Outstanding job! This patient has made a full \nrecovery, giving you the top ranking! Way to go!";
                        highScoreData.timeAttackLevels[level].rank = 3;
                    }
                    else if (highScoreData.timeAttackLevels[level].rank < 2 && score >= Game.currentSettings.two_star)
                    {
                        congratulationsMessage = "Great job! You've earned two stars! Try to score\n" + Game.currentSettings.three_star + " points to make it to the next rank!";
                        highScoreData.timeAttackLevels[level].rank = 2;
                    }
                    else if (highScoreData.timeAttackLevels[level].rank == 0)
                    {
                        highScoreData.timeAttackLevels[level].rank = 1;
                        congratulationsMessage = "Good job! You've earned one star! Try to score " + Game.currentSettings.two_star + " \npoints to make it to the next rank!";
                    }
                    else if (highScoreData.timeAttackLevels[level].rank == 3)
                    {
                        congratulationsMessage = "This patient has already made a full recovery! \nYou should see if you can beat your high score!";
                    }
                    else if (highScoreData.timeAttackLevels[level].rank == 2)
                    {
                        congratulationsMessage = "Try to score " + Game.currentSettings.three_star + " points to earn three \nstars!";
                    }
                    else if (highScoreData.timeAttackLevels[level].rank == 1)
                    {
                        congratulationsMessage = "Try to score " + Game.currentSettings.two_star + " points to earn two \nstars!";
                    }
                    else
                    {
                        congratulationsMessage = "BUG!";
                    }
                    
                    for (int i = 4; i >= 0; i--)
                    {
                        if (score >= levelData.highScores[i])
                        {
                            if (i < 4)
                            {
                                levelData.highScores[i + 1] = levelData.highScores[i];
                                levelData.playerNames[i + 1] = levelData.playerNames[i];

                            }
                            initials = levelData.playerNames[i];
                            rank = i;
                            levelData.highScores[i] = score;
                        }
                    }
                    if (score >= Game.currentSettings.two_star && level < highScoreData.timeAttackLevels.Count()-1)
                    {
                        if(highScoreData.timeAttackLevels[level+1]!=null)
                            highScoreData.timeAttackLevels[level + 1].unlocked = true;
                    }
                }
                if (Game.currentSettings.mode == GameMode.MoveChallenge)
                {                    
                    levelData = highScoreData.moveChallengeLevels[level];
                    highScoreData.moveChallengeLevels[level].played = true;
                    if (highScoreData.moveChallengeLevels[level].rank < 3 && score >= Game.currentSettings.three_star)
                    {
                        congratulationsMessage = "Outstanding job! This patient has made a full \nrecovery, giving you the top ranking! Way to go!";
                        highScoreData.moveChallengeLevels[level].rank = 3;
                    }
                    else if (highScoreData.moveChallengeLevels[level].rank < 2 && score >= Game.currentSettings.two_star)
                    {
                        congratulationsMessage = "Great job! You've earned two stars! Try to score \n" + Game.currentSettings.three_star + " points to make it to the next rank!";
                        highScoreData.moveChallengeLevels[level].rank = 2;
                    }
                    else if (highScoreData.moveChallengeLevels[level].rank == 0)
                    {
                        highScoreData.moveChallengeLevels[level].rank = 1;
                        congratulationsMessage = "Good job! You've earned one star! Try to score " + Game.currentSettings.two_star + " \npoints to make it to the next rank!";
                    }
                    else if (highScoreData.moveChallengeLevels[level].rank == 3)
                    {
                        congratulationsMessage = "This patient has already made a full recovery! \nYou should see if you can beat your high score!";
                    }
                    else if (highScoreData.moveChallengeLevels[level].rank == 2)
                    {
                        congratulationsMessage = "Try to score " + Game.currentSettings.three_star + " points to earn three \nstars!";
                    }
                    else if (highScoreData.moveChallengeLevels[level].rank == 1)
                    {
                        congratulationsMessage = "Try to score " + Game.currentSettings.two_star + " points to earn two \nstars!";
                    }
                    else
                    {
                        congratulationsMessage = "BUG!";
                    }
                    for (int i = 4; i >= 0; i--)
                    {
                        if (score >= levelData.highScores[i])
                        {
                            if (i < 4)
                            {
                                levelData.highScores[i + 1] = levelData.highScores[i];
                                levelData.playerNames[i + 1] = levelData.playerNames[i];

                            }
                            initials = levelData.playerNames[i];
                            rank = i;
                            levelData.highScores[i] = score;
                        }
                    }
                    if (score >= Game.currentSettings.two_star && level < highScoreData.moveChallengeLevels.Count() - 1)
                    {
                        if (highScoreData.moveChallengeLevels[level + 1] != null)
                            highScoreData.moveChallengeLevels[level + 1].unlocked = true;
                    }
                }
                else if (Game.currentSettings.mode == GameMode.Puzzle)
                {
                    levelData = highScoreData.puzzleLevels[level];
                    highScoreData.puzzleLevels[level].played = true;
                    TimeSpan twoStarTime = new TimeSpan(0, 0, 0, 0, Game.currentSettings.two_star);
                    TimeSpan threeStarTime = new TimeSpan(0, 0, 0, 0, Game.currentSettings.three_star);
                    if (highScoreData.puzzleLevels[level].rank < 3 && score <= Game.currentSettings.three_star)
                    {
                        congratulationsMessage = "Outstanding job! This patient has made a full \nrecovery, giving you the top ranking! Way to go!";
                        highScoreData.puzzleLevels[level].rank = 3;
                    }
                    else if (highScoreData.puzzleLevels[level].rank < 2 && score <= Game.currentSettings.two_star)
                    {
                        congratulationsMessage = string.Format("Great job! You've earned two stars! Try to finish \nin under {0}:{1:D2} to make it to the next rank!",threeStarTime.Minutes,threeStarTime.Seconds);
                        highScoreData.puzzleLevels[level].rank = 2;
                    }
                    else if (highScoreData.puzzleLevels[level].rank == 0)
                    {
                        highScoreData.puzzleLevels[level].rank = 1;
                        congratulationsMessage = string.Format("Good job! You've earned one star! Try to finish \nin under {0}:{1:D2} to make it to the next rank!",twoStarTime.Minutes,twoStarTime.Seconds);
                    }
                    else if (highScoreData.puzzleLevels[level].rank == 3)
                    {
                        congratulationsMessage = "This patient has already made a full recovery! \nSee if you can beat your high score!";
                    }
                    else if (highScoreData.puzzleLevels[level].rank == 2)
                    {
                        congratulationsMessage = string.Format("Try to finish in under {0}:{1:D2} to earn three \nstars!", threeStarTime.Minutes, threeStarTime.Seconds);
                    }
                    else if (highScoreData.puzzleLevels[level].rank == 1)
                    {
                        congratulationsMessage = string.Format("Try to finish in under {0}:{1:D2} to earn two \nstars!",twoStarTime.Minutes,twoStarTime.Seconds);
                    }
                    else
                    {
                        congratulationsMessage = "BUG!";
                    }
                    for (int i = 4; i >= 0; i--)
                    {
                        if (score <= levelData.highScores[i])
                        {
                            if (i < 4)
                            {
                                levelData.highScores[i + 1] = levelData.highScores[i];
                                levelData.playerNames[i + 1] = levelData.playerNames[i];

                            }
                            initials = levelData.playerNames[i];
                            rank = i;
                            levelData.highScores[i] = score;
                        }
                    }
                    
                    if (score <= Game.currentSettings.two_star && level < highScoreData.puzzleLevels.Count() - 1)
                    {
                        List<Settings> puzzleSettings = SettingsLoader.LoadPuzzleLevels();
                        if (Game.currentSettings.difficulty == Difficulty.EASY)
                        {
                            for (int i = 0; i < puzzleSettings.Count(); i++)
                            {
                                if (highScoreData.puzzleLevels[i] != null && (puzzleSettings[i].difficulty == Difficulty.MEDIUM || puzzleSettings[i].difficulty == Difficulty.EASY))
                                    highScoreData.puzzleLevels[i].unlocked = true;
                            }
                        }
                        if (Game.currentSettings.difficulty == Difficulty.MEDIUM)
                        {
                            for (int i = 0; i < puzzleSettings.Count(); i++)
                            {
                                if (highScoreData.puzzleLevels[i] != null && puzzleSettings[i].difficulty == Difficulty.HARD)
                                    highScoreData.puzzleLevels[i].unlocked = true;
                            }
                        }

                    }
                }
                HighScoreTracker.SaveHighScores(highScoreData);    
                state = GameOverMenuState.DOCTORIN;
            }            
            if (state == GameOverMenuState.DOCTORIN || state == GameOverMenuState.DOCTOROUT || state == GameOverMenuState.ANIMATEDOWN || state == GameOverMenuState.ANIMATEUP)
            {
                animateTime+=gameTime.ElapsedGameTime.Milliseconds;                
            }
            if (state == GameOverMenuState.DOCTORIN && animateTime > 250)
            {
                MusicControl.PlayMenuMusic();
                state = GameOverMenuState.READY;
                if(rank!=-1)
                    state = GameOverMenuState.INITIALS;
            }
            if (state == GameOverMenuState.DOCTOROUT && animateTime > 250)
            {
                animateTime = 0;
                state = GameOverMenuState.DOCTORIN;
                return result;
            }
            if (state == GameOverMenuState.ANIMATEDOWN && animateTime > 250)
            {
                animateTime = 0;
                state = GameOverMenuState.READY;
            }
            if (state == GameOverMenuState.ANIMATEUP && animateTime > 250)
            {
                animateTime = 0;
                state = GameOverMenuState.READY;
            }
            if (state == GameOverMenuState.INITIALS && cooldown == 0)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -.95 || rightStick.Y < -.95)
                {
                    SoundEffects.PlayClick();
                    Char[] cArray = initials.ToCharArray();
                    Char c = cArray[currentCharacter];
                    c--;
                    if (c < 'A') c = 'Z';
                    cArray[currentCharacter] = c;
                    initials = new String(cArray);
                    cooldown = 100;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
                    SoundEffects.PlayClick();
                    Char[] cArray = initials.ToCharArray();
                    Char c = cArray[currentCharacter];
                    c++;
                    if (c > 'Z') c = 'A';
                    cArray[currentCharacter] = c; 
                    initials = new String(cArray);
                    cooldown = 100;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) || gamePadState.IsButtonDown(Buttons.A))
                {

                    currentCharacter++;
                    cooldown = 250;
                    if (currentCharacter > 2)
                    {
                        SoundEffects.PlayScore();
                        cooldown = 500;
                        levelData.playerNames[rank] = initials;
                        HighScoreData data = HighScoreTracker.LoadHighScores();
                        levelData.played = true;
                        if (Game.currentSettings.mode == GameMode.TimeAttack)
                        {
                            data.timeAttackLevels[level] = levelData;
                        }
                        else if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            data.puzzleLevels[level] = levelData;
                        }
                        else
                        {
                            data.moveChallengeLevels[level] = levelData;
                        }
                        HighScoreTracker.SaveHighScores(data);
                        state = GameOverMenuState.READY;
                    }
                    else
                    {
                        SoundEffects.PlayClick();
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || gamePadState.IsButtonDown(Buttons.B))
                {
                    SoundEffects.PlayClick();
                    if (currentCharacter > 0)
                    {
                        currentCharacter--;
                        cooldown = 250;
                    }
                }
            }
            if (state == GameOverMenuState.READY && cooldown == 0)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;

                if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                {
                    result = optionList[selectedIndex].result;
                    animateTime = 0;
                    state = GameOverMenuState.DOCTOROUT;
                    SoundEffects.PlayScore();
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -.95 || rightStick.Y < -.95)
                {
                    if (selectedIndex < optionList.Count() - 1)
                    {
                        state = GameOverMenuState.ANIMATEDOWN;
                        animateTime = 0;
                        selectedIndex++;
                        cooldown = 250;
                        SoundEffects.PlayMove();
                    }
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
                    if (selectedIndex > 0)
                    {
                        state = GameOverMenuState.ANIMATEUP;
                        animateTime = 0;

                        selectedIndex--;
                        cooldown = 250;
                        SoundEffects.PlayMove();
                    }
                }
            }
            return MenuResult.None;            
        }
    }
 
}
