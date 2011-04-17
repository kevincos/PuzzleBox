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
    public class Settings
    {
        public int numColors = 5;
        public int toggleFreq = 0;
        public int counterFreq = 2;
        public int timerFreq =2;
        public int totalTime = 120000;        
        public GameMode mode = GameMode.TimeAttack;
    }

    class MenuOption
    {
        public MenuResult result;
        public MenuType type;
        public Texture2D optionText;

        public static Texture2D menu_off;
        public static Texture2D menu_low;
        public static Texture2D menu_medium;
        public static Texture2D menu_high;

        public MenuOption(MenuResult result, Texture2D optionText)
        {
            this.result = result;
            this.optionText = optionText;
            this.type = MenuType.Normal;
        }

        public MenuOption(MenuType type, Texture2D optionText)
        {
            this.result = MenuResult.None;
            this.optionText = optionText;
            this.type = type;
        }
    }

    public enum MenuType
    {
        Normal,
        ColorSelect,
        ToggleFreq,
        CounterFreq,
        TimerFreq
    }

    public enum MenuState
    {
        Ready,
        Wait,
        AnimateDown,
        AnimateUp,
    }

    public enum MenuResult
    {
        None,
        MenuAction,
        GoToTimeAttack,
        GoToSurvival,
        StartTimeAttack,
        StartSurvival,
        StartCollect,
        StartPuzzle,
        Replay,
        NewGame,
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
        int score = 0;
        int scoreX = -1;
        int scoreY = -1;
        int numColors = 6;
        int toggleFreq = 2;
        int counterFreq = 2;
        int timerFreq = 2;

        MenuState state = MenuState.Ready;

        public Menu()
        {
            optionList = new List<MenuOption>();
        }

        public void AddScore(int score, int drawX, int drawY)
        {
            this.scoreY = drawY;
            this.scoreX = drawX;
            this.score = score;
        }

        public void AddMenuItem(MenuResult result, Texture2D image)
        {
            optionList.Add(new MenuOption(result, image));
        }

        public void AddMenuItem(MenuType type, Texture2D image)
        {
            optionList.Add(new MenuOption(type, image));
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
                if (optionList[i].type == MenuType.ColorSelect)
                {
                    for (int j = 0; j < numColors; j++)
                    {
                        PuzzleNode p;
                        if(j==0)
                            p = new PuzzleNode(Color.Blue);
                        else if (j == 1)
                            p = new PuzzleNode(Color.Yellow);
                        else if (j == 2)
                            p = new PuzzleNode(Color.Red);
                        else if (j == 3)
                            p = new PuzzleNode(Color.Green);
                        else if (j == 4)
                            p = new PuzzleNode(Color.Magenta);
                        else
                            p = new PuzzleNode(Color.Orange);
                        
                        p.screenX = optionListX + 250 + j * 50;
                        p.screenY = optionListY + 25 + i * 50;

                        p.distance = 50;
                        p.scale = 1f;
                        OrbRenderer.DrawOrb(p, State.READY, 0f);
                    }
                }
                if (optionList[i].type == MenuType.ToggleFreq)
                {
                    if(toggleFreq == 0)
                        Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (toggleFreq == 1)
                        Game.spriteBatch.Draw(MenuOption.menu_low, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (toggleFreq == 2)
                        Game.spriteBatch.Draw(MenuOption.menu_medium, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (toggleFreq == 3)
                        Game.spriteBatch.Draw(MenuOption.menu_high, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                }
                if (optionList[i].type == MenuType.CounterFreq)
                {
                    if (counterFreq == 0)
                        Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (counterFreq == 1)
                        Game.spriteBatch.Draw(MenuOption.menu_low, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (counterFreq == 2)
                        Game.spriteBatch.Draw(MenuOption.menu_medium, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (counterFreq == 3)
                        Game.spriteBatch.Draw(MenuOption.menu_high, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                }
                if (optionList[i].type == MenuType.TimerFreq)
                {
                    if (timerFreq == 0)
                        Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (timerFreq == 1)
                        Game.spriteBatch.Draw(MenuOption.menu_low, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (timerFreq == 2)
                        Game.spriteBatch.Draw(MenuOption.menu_medium, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                    if (timerFreq == 3)
                        Game.spriteBatch.Draw(MenuOption.menu_high, new Rectangle(optionListX + 230, optionListY + i * 50, 200, 50), Color.White);
                }
            }
            if (scoreX != -1)
            {
                String message = "Score: " + score;
                Game.spriteBatch.DrawString(Game.spriteFont, message, new Vector2(scoreX, scoreY), Color.LightGreen);
            }
        }

        public void UpdateSettings(Settings s)
        {
            s.counterFreq = counterFreq;
            s.timerFreq = timerFreq;
            s.toggleFreq = toggleFreq;
            s.numColors = numColors;            
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
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (optionList[selectedIndex].type == MenuType.ColorSelect)
                    {
                        numColors++;
                        if (numColors > 6) numColors = 1;
                    }
                    if (optionList[selectedIndex].type == MenuType.TimerFreq)
                    {
                        timerFreq++;
                        timerFreq %= 4;
                    }
                    if (optionList[selectedIndex].type == MenuType.CounterFreq)
                    {
                        counterFreq++;
                        counterFreq %= 4;
                    }
                    if (optionList[selectedIndex].type == MenuType.ToggleFreq)
                    {
                        toggleFreq++;
                        toggleFreq %= 4;
                    }
                    state = MenuState.Wait;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (optionList[selectedIndex].type == MenuType.ColorSelect)
                    {
                        numColors--;
                        if (numColors < 1) numColors = 6;
                    }
                    if (optionList[selectedIndex].type == MenuType.TimerFreq)
                    {
                        timerFreq--;
                        if (timerFreq < 0) timerFreq = 3;
                    }
                    if (optionList[selectedIndex].type == MenuType.CounterFreq)
                    {
                        counterFreq--;
                        if (counterFreq < 0) counterFreq = 3;
                    }
                    if (optionList[selectedIndex].type == MenuType.ToggleFreq)
                    {
                        toggleFreq--;
                        if (toggleFreq < 0) toggleFreq = 3;
                    }
                    state = MenuState.Wait;
                    state = MenuState.Wait;
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (optionList[selectedIndex].type == MenuType.ColorSelect)
                    {
                        numColors++;
                        if (numColors > 6) numColors = 1;
                    }
                    if (optionList[selectedIndex].type == MenuType.TimerFreq)
                    {
                        timerFreq++;
                        timerFreq %= 4;
                    }
                    if (optionList[selectedIndex].type == MenuType.CounterFreq)
                    {
                        counterFreq++;
                        counterFreq %= 4;
                    }
                    if (optionList[selectedIndex].type == MenuType.ToggleFreq)
                    {
                        toggleFreq++;
                        toggleFreq %= 4;
                    }
                    state = MenuState.Wait;
                    state = MenuState.Wait;
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
