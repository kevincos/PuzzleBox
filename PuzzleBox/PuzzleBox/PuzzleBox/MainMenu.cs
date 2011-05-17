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
    enum MainMenuState
    {
        DOCTORIN,
        DOCTOROUT,
        READY,
        ANIMATEUP,
        ANIMATEDOWN
    }

    class MainMenu
    {
        public Texture2D background;
        public Texture2D header;

        int animateTime = 0;
        MainMenuState state = MainMenuState.DOCTORIN;
        MenuResult result = MenuResult.None;
        int selectedIndex = 0;
        List<MenuOption> optionList;
        int cooldown = 0;
        int headerX = 100;
        int headerY = 20;
        int optionListX = 90;
        int optionListY = 170;

        public MainMenu()
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
            Game.spriteBatch.Draw(header, new Rectangle(headerX, headerY, 600, 100), Color.White);
            for (int i = 0; i < optionList.Count; i++)
            {
                if (i == selectedIndex)
                {
                    PuzzleNode p = new PuzzleNode(Color.Blue);
                    p.screenX = optionListX - 25;
                    p.screenY = optionListY + 25 + i * 50;
                    if (state == MainMenuState.ANIMATEDOWN)
                        p.screenY -= (int)(50f * (1f - (float)animateTime / (float)250));
                    if (state == MainMenuState.ANIMATEUP)
                        p.screenY += (int)(50f * (1f - (float)animateTime / (float)250));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                }
                Game.spriteBatch.Draw(optionList[i].optionText, new Rectangle(optionListX, optionListY + i * 50, 200, 50), Color.White);
            }
            if (state == MainMenuState.DOCTORIN)
            {
                JellyfishRenderer.DrawJellyfish(1250 - 600 * animateTime / 250, 300, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(1050 - 600 * animateTime / 250, 300, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            else if (state == MainMenuState.DOCTOROUT)
            {
                JellyfishRenderer.DrawJellyfish(650 + 600 * animateTime / 250, 300, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(450 + 600 * animateTime / 250, 300, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
            else
            {
                JellyfishRenderer.DrawJellyfish(650, 300, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
                JellyfishRenderer.DrawJellyfish(450, 300, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (state == MainMenuState.DOCTORIN || state == MainMenuState.DOCTOROUT || state == MainMenuState.ANIMATEDOWN || state == MainMenuState.ANIMATEDOWN)
            {
                animateTime+=gameTime.ElapsedGameTime.Milliseconds;                
            }
            if (state == MainMenuState.DOCTORIN && animateTime > 250)
            {
                state = MainMenuState.READY;
            }
            if (state == MainMenuState.DOCTOROUT && animateTime > 250)
            {
                animateTime = 0;
                state = MainMenuState.DOCTORIN;
                return result;
            }
            if (state == MainMenuState.READY && cooldown == 0)
            {
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;

                if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
                {
                    result = optionList[selectedIndex].result;
                    animateTime = 0;
                    state = MainMenuState.DOCTOROUT;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -.95 || rightStick.Y < -.95)
                {
                    selectedIndex++;
                    if (selectedIndex >= optionList.Count())
                        selectedIndex = optionList.Count() - 1;
                    cooldown = 250;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
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
