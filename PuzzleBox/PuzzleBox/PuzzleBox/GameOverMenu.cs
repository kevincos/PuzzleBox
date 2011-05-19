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
        int doctorX = 600;
        int doctorY = 375;
        int optionListX = 120;
        int optionListY = 180;
        int optionGap = 75;
        int optionWidth = 250;
        int optionHeight = 75;
        int scoreX = 120;
        int scoreY = 340;
        int highScoreX = 120;
        int highScoreY = 450;
        int highScoreLine = 30;
        int speechX = 700;
        int speechY = 625;
        
        int rank = -1;
        String initials = "---";
        int currentCharacter = 0;

        LevelData levelData;

        public GameOverMenu()
        {
            optionList = new List<MenuOption>();
        }

        public void AddMenuItem(MenuResult result, Texture2D image)
        {
            optionList.Add(new MenuOption(result, image));
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
                    if (state == GameOverMenuState.ANIMATEDOWN)
                        p.screenY -= (int)(optionGap * (1f - (float)animateTime / (float)250));
                    if (state == GameOverMenuState.ANIMATEUP)
                        p.screenY += (int)(optionGap * (1f - (float)animateTime / (float)250));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                }
                Game.spriteBatch.Draw(optionList[i].optionText, new Rectangle(optionListX, optionListY + i * optionGap, optionWidth, optionHeight), Color.White);
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
                Game.spriteBatch.DrawString(Game.spriteFont, string.Format("TIME: {0}:{1:D2}", t.Minutes, t.Seconds), new Vector2(scoreX, scoreY), Color.LightGreen, 0, Vector2.Zero, 1.75f, SpriteEffects.None, 0);
                for (int i = 0; i < 3; i++)
                {
                    Game.spriteBatch.Draw(emptyStar,
                        new Rectangle(scoreX + 75 * i, scoreY + 40, 64, 64), Color.White);
                }
                Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX, scoreY + 40, 64, 64), Color.White);
                if (score <= Game.currentSettings.two_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 75, scoreY + 40, 64, 64), Color.White);
                if (score <= Game.currentSettings.three_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 150, scoreY + 40, 64, 64), Color.White);                    
            }
            else
            {
                Game.spriteBatch.DrawString(Game.spriteFont, "SCORE:" + score, new Vector2(scoreX, scoreY), Color.LightGreen, 0, Vector2.Zero, 1.75f, SpriteEffects.None, 0);
                for (int i = 0; i < 3; i++)
                {                    
                    Game.spriteBatch.Draw(emptyStar,
                        new Rectangle(scoreX + 75*i, scoreY + 40, 64, 64), Color.White);                    
                }
                Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX, scoreY + 40, 64, 64), Color.White);                    
                if(score >= Game.currentSettings.two_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 75, scoreY + 40, 64, 64), Color.White);
                if (score >= Game.currentSettings.three_star)
                    Game.spriteBatch.Draw(star,
                        new Rectangle(scoreX + 150, scoreY + 40, 64, 64), Color.White);                    
            }

            if (levelData != null)
            {
                Game.spriteBatch.DrawString(Game.spriteFont, "High Scores", new Vector2(highScoreX, highScoreY), Color.LightGreen, 0, Vector2.Zero, 1.75f, SpriteEffects.None, 0);
                for (int i = 1; i < 6; i++)
                {
                    if (i - 1 == rank)
                    {
                        if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            TimeSpan t = new TimeSpan(0, 0, 0, 0, levelData.highScores[i - 1]);
                            Game.spriteBatch.DrawString(Game.spriteFont, string.Format("{0}: {1}:{2:D2}", levelData.playerNames[i - 1], t.Minutes, t.Seconds), new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 4), Color.White, 0, Vector2.Zero, 1.8f, SpriteEffects.None, 0);
                        }
                        else
                            Game.spriteBatch.DrawString(Game.spriteFont, i + ": " + levelData.playerNames[i - 1] + " - " + levelData.highScores[i - 1], new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 4), Color.White, 0, Vector2.Zero, 1.8f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            TimeSpan t = new TimeSpan(0, 0, 0, 0, levelData.highScores[i - 1]);
                            Game.spriteBatch.DrawString(Game.spriteFont, string.Format("{0}: {1}:{2:D2}", levelData.playerNames[i - 1], t.Minutes, t.Seconds), new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 5), Color.LightGreen, 0, Vector2.Zero, 1.75f, SpriteEffects.None, 0);
                        }
                        else
                            Game.spriteBatch.DrawString(Game.spriteFont, i + ": " + levelData.playerNames[i - 1] + " - " + levelData.highScores[i - 1], new Vector2(highScoreX, highScoreY + highScoreLine * (i + 1) + 5), Color.LightGreen, 0, Vector2.Zero, 1.75f, SpriteEffects.None, 0);
                    }
                }
            }

            if (rank != -1 && currentCharacter <= 2)
            {
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                Game.spriteBatch.DrawString(Game.spriteFont, "Way to go! You got a high score! Please enter", new Vector2(speechX-260, speechY-15), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, "your initials: ", new Vector2(speechX - 260, speechY+5), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, ""+initials[2], new Vector2(speechX - 70, speechY + 5), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, ""+initials[1], new Vector2(speechX - 85, speechY + 5), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, ""+initials[0], new Vector2(speechX - 100, speechY + 5), Color.Black);
                Game.spriteBatch.DrawString(Game.spriteFont, "^", new Vector2(speechX - 100+15*currentCharacter, speechY + 25), Color.Black);
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
                        if (highScoreData.puzzleLevels[level + 1] != null)
                            highScoreData.puzzleLevels[level + 1].unlocked = true;
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
                    Char[] cArray = initials.ToCharArray();
                    Char c = cArray[currentCharacter];
                    c--;
                    if (c < 'A') c = 'Z';
                    cArray[currentCharacter] = c;
                    initials = new String(cArray);
                    cooldown = 50;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
                    Char[] cArray = initials.ToCharArray();
                    Char c = cArray[currentCharacter];
                    c++;
                    if (c > 'Z') c = 'A';
                    cArray[currentCharacter] = c; 
                    initials = new String(cArray);
                    cooldown = 50;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Right) || gamePadState.IsButtonDown(Buttons.A))
                {
                    currentCharacter++;
                    cooldown = 250;
                    if(currentCharacter>2)
                    {
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
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Left) || gamePadState.IsButtonDown(Buttons.B))
                {
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
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -.95 || rightStick.Y < -.95)
                {
                    state = GameOverMenuState.ANIMATEDOWN;
                    animateTime = 0;
                    selectedIndex++;
                    if (selectedIndex >= optionList.Count())
                        selectedIndex = optionList.Count() - 1;
                    cooldown = 250;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
                    state = GameOverMenuState.ANIMATEUP;
                    animateTime = 0;
                    
                    selectedIndex--;
                    if (selectedIndex < 0)
                        selectedIndex = 0;
                    cooldown = 250;
                }
            }
            return MenuResult.None;            
        }
    }
 
}
