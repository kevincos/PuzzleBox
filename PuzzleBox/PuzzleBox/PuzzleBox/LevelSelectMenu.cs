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
            SELECTING
        }

        public List<Settings> levelList;

        int animateTime;

        int currentLevel = 0;

        int cooldown = 0;

        string[] textPieces;
        int currentTextPiece;

        public SelectMenuState state = SelectMenuState.READY;

        public Settings GetCurrentSetttings()
        {
            return levelList[currentLevel];
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
                    state = SelectMenuState.SELECTING;
                    currentTextPiece = 0;
                }
            }
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown <= 0)
            {
                cooldown = 0;
                if (state == SelectMenuState.TEXT)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
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
                    if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    {
                        if (currentLevel > 0)
                        {
                            state = SelectMenuState.SWAPLEFT;
                            currentLevel--;
                            animateTime = 0;
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    {
                        if (currentLevel < levelList.Count - 1)
                        {
                            state = SelectMenuState.SWAPRIGHT;
                            currentLevel++;
                            animateTime = 0;                            
                        }
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.B))
                    {
                        return MenuResult.GoToMainMenu;
                    }
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {

                        textPieces = levelList[currentLevel].instructions.Split('-');
                        if (textPieces.Count() <= 1)
                            state = SelectMenuState.SELECTING;
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
                int y = t*t/100;
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
                        y = (int)(-50f * (animateTime / 1000f));
                        
                    }
                }
                if(i==currentLevel || state==SelectMenuState.DOCTORIN || state==SelectMenuState.READY || state == SelectMenuState.SWAPRIGHT || state == SelectMenuState.SWAPLEFT)
                    JellyfishRenderer.DrawJellyfish(x+405, 208 - y, 100,levelList[i].texture,scale);
            }
            if (state == SelectMenuState.DOCTORIN)
            {
                JellyfishRenderer.DrawJellyfish(-150 + (int)(270*animateTime/500f), 208, 100, JellyfishRenderer.doctorJellyfish, 1f);
                JellyfishRenderer.DrawJellyfish(950 - (int)(270 * animateTime / 500f), 208, 100, JellyfishRenderer.nurseJellyfish, 1f);
            }
            if (state == SelectMenuState.TEXT)
            {                
                JellyfishRenderer.DrawJellyfish(120, 208, 100, JellyfishRenderer.doctorJellyfish, 1f);
                JellyfishRenderer.DrawJellyfish(680, 208, 100, JellyfishRenderer.nurseJellyfish, 1f);
            }
            if (state == SelectMenuState.DOCTOROUT)
            {
                JellyfishRenderer.DrawJellyfish(120 - (int)(270 * animateTime / 500f), 208, 100, JellyfishRenderer.doctorJellyfish, 1f);
                JellyfishRenderer.DrawJellyfish(680 + (int)(270 * animateTime / 500f), 208, 100, JellyfishRenderer.nurseJellyfish, 1f);
            }
            if (state == SelectMenuState.READY)
            {
                Game.spriteBatch.DrawString(Game.spriteFont, levelList[currentLevel].name, new Vector2(350, 35), Color.LightGreen);
                if(currentLevel > 0)
                    Game.spriteBatch.DrawString(Game.spriteFont, "<", new Vector2(300, 35), Color.LightGreen);
                if (currentLevel < levelList.Count-1)
                    Game.spriteBatch.DrawString(Game.spriteFont, ">", new Vector2(500, 35), Color.LightGreen);
                if (levelList[currentLevel].mode == GameMode.TimeAttack)
                {
                    Game.spriteBatch.DrawString(Game.spriteFont, "Goal: Score as many points as possible!", new Vector2(200, 335), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "Orb Density: "+levelList[currentLevel].grayOrbStart, new Vector2(200, 355), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "Toggle Orb %: " + levelList[currentLevel].toggleFreq, new Vector2(200, 375), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "Countdown Orb %: " + levelList[currentLevel].counterFreq, new Vector2(200, 395), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "Timer Orb %: " + levelList[currentLevel].timerFreq, new Vector2(200, 415), Color.LightGreen);
                }
                else
                {
                    Game.spriteBatch.DrawString(Game.spriteFont, "Goal: Score as many points as possible!", new Vector2(200, 335), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "Difficulty: Medium", new Vector2(200, 355), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "Grade: B", new Vector2(200, 375), Color.LightGreen);
                    Game.spriteBatch.DrawString(Game.spriteFont, "High Score: 5000", new Vector2(200, 395), Color.LightGreen);
                }
            }
            if (state == SelectMenuState.TEXT)
            {
                if(textPieces[currentTextPiece].Split(':')[0]=="D")
                    JellyfishRenderer.DrawSpeechBubble(420, 410, 100, SpriteEffects.None);
                else
                    JellyfishRenderer.DrawSpeechBubble(420, 410, 100, SpriteEffects.FlipHorizontally);
                String text = textPieces[currentTextPiece].Split(':')[1];
                Game.spriteBatch.DrawString(Game.spriteFont, text, new Vector2(180, 390), Color.Black);
            }
            
        }
    }
}
