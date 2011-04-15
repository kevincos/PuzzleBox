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
    class MenuOption
    {
        public MenuResult result;
        public Texture2D optionText;

        public MenuOption(MenuResult result, Texture2D optionText)
        {
            this.result = result;
            this.optionText = optionText;
        }
    }

    public enum MenuState
    {
        Ready,
        AnimateDown,
        AnimateUp,
    }

    public enum MenuResult
    {
        None,
        StartTimeAttack,
        StartSurvival,
        StartCollect,
        StartPuzzle,
        GoToHelpMenu,
        GoToMainMenu,
        ResumeGame
    }

    class Menu
    {
        
        public Texture2D background;
        public Texture2D header;

        int headerX = 100;
        int headerY = 20;
        int optionListX = 90;
        int optionListY= 170;
        List<MenuOption> optionList;
        int selectedIndex = 0;
        int animationTime = 0;
        int maxAnimationTime = 150;

        MenuState state = MenuState.Ready;

        public Menu()
        {
            optionList = new List<MenuOption>();
        }

        public void AddMenuItem(MenuResult result, Texture2D image)
        {
            optionList.Add(new MenuOption(result, image));
        }

        public void SelectDown()
        {
            if (selectedIndex < optionList.Count - 1)
            {
                state = MenuState.AnimateDown;
                animationTime = 0;
                selectedIndex++;
            }            
        }
        public void SelectUp()
        {
            if (selectedIndex > 0)
            {
                state = MenuState.AnimateUp;
                animationTime = 0;
                selectedIndex--;
            }            
        }
        public void Draw()
        {            
            Game.spriteBatch.Draw(background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);
            Game.spriteBatch.Draw(header, new Rectangle(headerX, headerY, 600, 100), Color.White);
            for(int i =0; i < optionList.Count; i++)
            {
                if (i == selectedIndex)
                {
                    PuzzleNode p = new PuzzleNode(Color.Blue);
                    p.screenX = optionListX-25;
                    p.screenY = optionListY+25 + i*50;
                    if (state == MenuState.AnimateDown)
                        p.screenY -= (int)(50f * (1f - (float)animationTime / (float)maxAnimationTime));
                    if (state == MenuState.AnimateUp)
                        p.screenY += (int)(50f * (1f - (float)animationTime / (float)maxAnimationTime));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                }
                Game.spriteBatch.Draw(optionList[i].optionText, new Rectangle(optionListX, optionListY + i * 50, 200, 50), Color.White);
            }
        }

        public MenuResult Update(GameTime gameTime)
        {
            if (state == MenuState.Ready)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    SelectDown();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    SelectUp();
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    System.Threading.Thread.Sleep(100);
                    return optionList[selectedIndex].result;
                }
            }
            else
                animationTime += gameTime.ElapsedGameTime.Milliseconds;

            if (animationTime > maxAnimationTime)
            {
                animationTime = 0;
                state = MenuState.Ready;
            }
            return MenuResult.None;
        }

    }
}
