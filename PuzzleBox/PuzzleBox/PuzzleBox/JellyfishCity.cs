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
    enum JellyfishCityState
    {
        NURSEIN,
        NURSEOUT,
        HELLO,
        VIEW,
        GOODBYE,
        SPAWN
    }

    public class Jellyfish
    {
        public Jellyfish(Texture2D tex)
        {
            posX = JellyfishCity.r.Next(100,Game.screenSizeX-100);
            posY = JellyfishCity.r.Next(100, Game.screenSizeY - 100);            
            
            texture = tex;
        }

        public void Update(GameTime gameTime)
        {
            posX += velX * gameTime.ElapsedGameTime.Milliseconds;
            posY += velY * gameTime.ElapsedGameTime.Milliseconds;
            if (texture == JellyfishRenderer.doctorJellyfish)
                velX += 2*(((float)(JellyfishCity.r.NextDouble())) * .001f - .0005f) * gameTime.ElapsedGameTime.Milliseconds;
            else
                velX += (((float)(JellyfishCity.r.NextDouble()))*.001f-.0005f)*gameTime.ElapsedGameTime.Milliseconds;
            velY += (((float)(JellyfishCity.r.NextDouble()))*.001f-.0005f)* gameTime.ElapsedGameTime.Milliseconds;
            if (posX > Game.screenSizeX - 200)
                velX -= .01f;
            if (posX <150)
                velX += .01f;
            
            if (posY < -100)
                posY = Game.screenSizeY+100;
            if (velY > 0)
                velY = 0;
            if (posX < -60f)
                posX = Game.screenSizeX+50;
            if (posX > Game.screenSizeX+80)
                posX = -50f;
            if (velY < -.2f)
                velY = -.2f;
            if (velX < -.2f)
                velX = -.2f;
            if (velX > .2f)
                velX = .2f;
            if (velX < -.05f)
                left = true;
            if (velX > .05f)
                left = false;
        }

        public Texture2D texture;
        public float posX;
        public float posY;
        public float velX;
        public float velY = -.1f;
        public bool left = false;
    }

    public class JellyfishCity
    {
        int nurseX = 150;
        int nurseY = 400;
        int speechX = 512;
        int speechY = 600;

        int cooldown = 0;
        int animateTime = 0;
        bool intro = true;
        JellyfishCityState state = JellyfishCityState.SPAWN;
        List<Jellyfish> jellyList;
        int jelliesSaved = 0;
        int totalJellies = 0;

        public static Random r;
        
        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            r = new Random();
            if (cooldown < 0) cooldown = 0;
            if (state == JellyfishCityState.SPAWN)
            {
                HighScoreData data = HighScoreTracker.LoadHighScores();
                jellyList = new List<Jellyfish>();
                // Load TimeAttack Jellies
                List<Settings> timeAttackSettingsList = SettingsLoader.LoadTimeAttackLevels();
                for (int i = 0; i < timeAttackSettingsList.Count; i++)
                {
                    totalJellies++;
                    if (data.timeAttackLevels[i].rank == 3)
                    {
                        jelliesSaved++;
                        jellyList.Add(new Jellyfish(timeAttackSettingsList[i].texture));
                    }
                }
                // Load MoveChallenge Jellies
                List<Settings> moveChallengeSettingsList = SettingsLoader.LoadMoveCountLevels();
                for (int i = 0; i < moveChallengeSettingsList.Count; i++)
                {
                    totalJellies++;
                    if (data.moveChallengeLevels[i].rank == 3)
                    {
                        jelliesSaved++;
                        jellyList.Add(new Jellyfish(moveChallengeSettingsList[i].texture));
                    }
                }
                // Load Puzzle Jellies
                List<Settings> puzzleSettingsList = SettingsLoader.LoadPuzzleLevels();
                for (int i = 0; i < puzzleSettingsList.Count; i++)
                {
                    totalJellies++;
                    if (data.puzzleLevels[i].rank == 3)
                    {
                        jelliesSaved++;
                        jellyList.Add(new Jellyfish(puzzleSettingsList[i].texture));
                    }
                }
                if (totalJellies == jelliesSaved)
                {
                    jellyList.Add(new Jellyfish(JellyfishRenderer.doctorJellyfish));
                }
                state = JellyfishCityState.NURSEIN;
            }
            if (state == JellyfishCityState.NURSEIN || state == JellyfishCityState.NURSEOUT)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
            }
            if (state == JellyfishCityState.NURSEIN && animateTime > 250)
            {
                if (intro)
                {
                    state = JellyfishCityState.HELLO;                    
                }
                else
                    state = JellyfishCityState.GOODBYE;
            }
            if (state == JellyfishCityState.NURSEOUT && animateTime > 250)
            {
                if (intro)
                {
                    intro = false;
                    animateTime = 0;
                    state = JellyfishCityState.VIEW;
                }
                else
                {
                    return MenuResult.GoToMainMenu;
                }
            }
            if (state == JellyfishCityState.HELLO)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(Game.playerIndex).IsButtonDown(Buttons.A))
                {
                    state = JellyfishCityState.NURSEOUT;
                    animateTime = 0;
                }
            }
            if (state == JellyfishCityState.GOODBYE)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(Game.playerIndex).IsButtonDown(Buttons.A))
                {
                    state = JellyfishCityState.NURSEOUT;
                    animateTime = 0;
                }
            }
            if (state == JellyfishCityState.VIEW)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space) || GamePad.GetState(Game.playerIndex).IsButtonDown(Buttons.A))
                {
                    state = JellyfishCityState.NURSEIN;
                    animateTime = 0;
                }
            }
            if (state != JellyfishCityState.SPAWN)
            {
                foreach (Jellyfish j in jellyList)
                {
                    j.Update(gameTime);
                }
            }
            return MenuResult.None;
        }

        public void Draw()
        {
            if (state != JellyfishCityState.SPAWN)
            {
                foreach (Jellyfish j in jellyList)
                {
                    if (j.texture == JellyfishRenderer.doctorJellyfish)
                    {
                        if(j.left)
                            JellyfishRenderer.DrawJellyfish((int)(j.posX), (int)(j.posY), 100, j.texture, .5f,SpriteEffects.FlipHorizontally);
                        else
                            JellyfishRenderer.DrawJellyfish((int)(j.posX), (int)(j.posY), 100, j.texture, .5f, SpriteEffects.None);
                    }
                    else
                        JellyfishRenderer.DrawJellyfish((int)(j.posX), (int)(j.posY), 100, j.texture, .35f);
                }
            }
            if (state == JellyfishCityState.NURSEIN)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 + 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == JellyfishCityState.NURSEOUT)
            {
                JellyfishRenderer.DrawJellyfish(nurseX - 300 * animateTime / 250, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == JellyfishCityState.HELLO || state == JellyfishCityState.GOODBYE)
            {
                JellyfishRenderer.DrawJellyfish(nurseX, nurseY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawSpeechBubble(speechX, speechY, 100, SpriteEffects.None);
                if (state == JellyfishCityState.HELLO)
                {
                    if(jelliesSaved==totalJellies)
                        Game.spriteBatch.DrawString(Game.spriteFont, "Wow! Way to go! You've saved all the Jellyfish!\nEven Dr. Jellyfish is ready to join the parade!", new Vector2(speechX - 265, speechY), Color.Black);
                    else if(jelliesSaved==totalJellies-1)
                        Game.spriteBatch.DrawString(Game.spriteFont, "You've rescued " + jelliesSaved + " Jellyfish! Just one more to go!\nTry to score 3 stars!", new Vector2(speechX - 265, speechY), Color.Black);
                    else
                        Game.spriteBatch.DrawString(Game.spriteFont, "You've rescued " + jelliesSaved + " jellyfish! " + (totalJellies - jelliesSaved) + " more patients still\nneed your help. Score 3 stars on each Jellyfish!", new Vector2(speechX - 265, speechY), Color.Black);     
                }
                if(state == JellyfishCityState.GOODBYE)
                    Game.spriteBatch.DrawString(Game.spriteFont, "Come back soon!", new Vector2(speechX - 270, speechY), Color.Black);
            }            
        }
    }
}
