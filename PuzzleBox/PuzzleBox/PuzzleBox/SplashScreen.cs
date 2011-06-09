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
    public enum SplashScreenState{
        FADE,
        READY,
        SLIDE,
        WAIT
    }

    public class SplashScreen
    {
        SplashScreenState state = SplashScreenState.FADE;
        int cooldown = 0;
        int animateTime = 0;
        int maxAnimateTime = 1000;

        int headerX;
        int headerY;
        int headerWidth;
        int headerHeight;
        int doctorX;
        int doctorY;
        int doctorEndX;
        
        List<Vector2> startPositions;
        List<Vector2> endPositions;
        List<Texture2D> textures;

        public SplashScreen()
        {
            Game.SetSplashResolution();
            headerX = 225;
            headerY = 20;
            headerWidth = 824;
            headerHeight = 150;
            doctorX = 525;
            doctorY = 325;
            doctorEndX = 725;
            startPositions = new List<Vector2>();
            endPositions = new List<Vector2>();
            textures = new List<Texture2D>();

            startPositions.Add(new Vector2(300, 310));
            endPositions.Add(new Vector2(-100, -150));
            textures.Add(JellyfishRenderer.mogulJelly);
            startPositions.Add(new Vector2(475, 300));
            endPositions.Add(new Vector2(350, -150));
            textures.Add(JellyfishRenderer.libraryJelly);
            startPositions.Add(new Vector2(650, 290));
            endPositions.Add(new Vector2(-200, 400));
            textures.Add(JellyfishRenderer.ninjaJelly);
            startPositions.Add(new Vector2(825, 300));
            endPositions.Add(new Vector2(900, -150));
            textures.Add(JellyfishRenderer.fortuneJelly);
            startPositions.Add(new Vector2(1000, 310));
            endPositions.Add(new Vector2(1300, -150));
            textures.Add(JellyfishRenderer.officerJelly); 

            startPositions.Add(new Vector2(200, 450));
            endPositions.Add(new Vector2(-100, 900));
            textures.Add(JellyfishRenderer.baseballJelly2);
            startPositions.Add(new Vector2(360, 440));
            endPositions.Add(new Vector2(300, 900));
            textures.Add(JellyfishRenderer.karateJelly);
            startPositions.Add(new Vector2(580, 430));
            endPositions.Add(new Vector2(400, 900));
            textures.Add(JellyfishRenderer.profJelly);
            startPositions.Add(new Vector2(1100, 450));
            endPositions.Add(new Vector2(1300, 900));
            textures.Add(JellyfishRenderer.queenJelly);
            startPositions.Add(new Vector2(950, 440));
            endPositions.Add(new Vector2(1000, 900));
            textures.Add(JellyfishRenderer.kingJelly);
            startPositions.Add(new Vector2(780, 430));
            endPositions.Add(new Vector2(800, 900));
            textures.Add(JellyfishRenderer.explorerJelly);
            

            
        }

        public void Draw()
        {
            if (state == SplashScreenState.FADE || state == SplashScreenState.READY)
            {
                Game.spriteBatch.Draw(MainMenu.background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);

                for (int i = 0; i < startPositions.Count; i++)
                {
                    JellyfishRenderer.DrawJellyfish((int)startPositions[i].X, (int)startPositions[i].Y, 100, textures[i], .5f);
                }

                Game.spriteBatch.Draw(MainMenu.header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);                

                JellyfishRenderer.DrawJellyfish(doctorX + 200, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);                
            }
            if (state == SplashScreenState.SLIDE)
            {
                Game.spriteBatch.Draw(MainMenu.background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);
                Game.spriteBatch.Draw(MainMenu.header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
                
                for (int i = 0; i < startPositions.Count; i++)
                {
                    int startX = (int)startPositions[i].X;
                    int startY = (int)startPositions[i].Y;
                    int endX = (int)endPositions[i].X;
                    int endY = (int)endPositions[i].Y;
                    JellyfishRenderer.DrawJellyfish(startX + (endX - startX) * animateTime / maxAnimateTime, startY + (endY - startY) * animateTime / maxAnimateTime,100, textures[i], .5f);
                }

                JellyfishRenderer.DrawJellyfish(doctorX + 200 + ((doctorEndX - doctorX) * animateTime) / maxAnimateTime, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX + ((doctorEndX - doctorX) * animateTime) / maxAnimateTime, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            if (state == SplashScreenState.READY)
            {
                Game.spriteBatch.Draw(MainMenu.header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
                JellyfishRenderer.DrawJellyfish(doctorX + 200, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
                Game.spriteBatch.DrawString(Game.menuFont, "Press START to begin!", new Vector2(400,550), Color.White);                
            }
            if (state == SplashScreenState.FADE)
            {
                Color fadeColor = new Color();
                fadeColor.A = (Byte)(255 * (maxAnimateTime - animateTime) / maxAnimateTime);
                fadeColor.R = (Byte)(255 * (maxAnimateTime - animateTime) / maxAnimateTime);
                fadeColor.G = (Byte)(255 * (maxAnimateTime - animateTime) / maxAnimateTime);
                fadeColor.B = (Byte)(255 * (maxAnimateTime - animateTime) / maxAnimateTime);
                Game.spriteBatch.Draw(MainMenu.background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), fadeColor);
            }
            if (state == SplashScreenState.WAIT)
            {
                Game.spriteBatch.Draw(MainMenu.header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
                JellyfishRenderer.DrawJellyfish(doctorEndX + 200, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(doctorEndX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            if (state == SplashScreenState.FADE && animateTime == 0)
                MediaPlayer.Play(MusicControl.music_menu);

            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            animateTime += gameTime.ElapsedGameTime.Milliseconds;
            PlayerIndex controllingPlayer = Game.playerIndex;
            
            if (state == SplashScreenState.FADE && animateTime > maxAnimateTime)
            {                
                state = SplashScreenState.READY;
                animateTime = maxAnimateTime;
            }
            if (state == SplashScreenState.SLIDE && animateTime > maxAnimateTime)
            {
                animateTime = maxAnimateTime;
                state = SplashScreenState.WAIT;
                return MenuResult.GoToMainMenu;
            }
            if (state == SplashScreenState.READY)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Game.playerIndex = PlayerIndex.One;
                    state = SplashScreenState.SLIDE;
                    animateTime = 0;
                }
                for (PlayerIndex index = Game.playerIndex; index <= PlayerIndex.Four; index++)
                {
                    if (GamePad.GetState(index).Buttons.Start == ButtonState.Pressed)
                    {
                        Game.playerIndex = index;
                        state = SplashScreenState.SLIDE;
                        animateTime = 0;
                        break;
                    }
                }
            }

            return MenuResult.None;
        }
    }
}
