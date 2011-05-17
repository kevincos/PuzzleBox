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
        public int numColors = 6;
        public int toggleFreq = 0;
        public int counterFreq = 0;
        public int timerFreq =0;
        public int grayOrbStart = 18;
        public bool randomOrbs = true;
        public string levelfilename = "";
        public GameMode mode = GameMode.TimeAttack;
        public bool allowResets = true;
        public bool displayScore = true;
        public bool displayTimer = true;
        public int timerLimit = 0;
        public int initialTime = 120000;
        public bool countdownTimer = true;
        public string name = "unnamed";
        public bool displayMoveCount = false;
        public int availableMoves = 100;
        public Texture2D texture = JellyfishRenderer.orangeJelly;
        public string instructions;
    }

    class MenuOption
    {
        public MenuResult result;
        public MenuType type;
        public Texture2D optionText;
        public string optionString;


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

        public MenuOption(MenuResult result, string optionString)
        {
            this.result = result;
            this.optionString = optionString;
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

    public enum MenuClass
    {
        MainMenu,
        PauseMenu,
        ResultsMenu
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
        GoToPuzzle,
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

        MenuClass type;
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
        int toggleFreq = 0;
        int counterFreq = 0;
        int timerFreq = 0;

        MenuState state = MenuState.Ready;

        public Menu(MenuClass type)
        {
            this.type = type;
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
                        else if (j == 5)
                            p = new PuzzleNode(Color.DarkOrange);
                        else if (j == 6)
                            p = new PuzzleNode(Color.GreenYellow);
                        else if (j == 7)
                            p = new PuzzleNode(Color.DarkViolet);
                        else if (j == 8)
                            p = new PuzzleNode(Color.DarkTurquoise);
                        else if (j == 9)
                            p = new PuzzleNode(Color.White);
                        else
                            p = new PuzzleNode(Color.Black);

                        p.screenX = optionListX + 250 + j * 45;
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
            JellyfishRenderer.DrawJellyfish(600, 300, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.None); 
            JellyfishRenderer.DrawJellyfish(400, 300, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);
            
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
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                Vector2 leftStick = gamePadState.ThumbSticks.Left;
                Vector2 rightStick = gamePadState.ThumbSticks.Right;
                if (Keyboard.GetState().IsKeyDown(Keys.Down) || leftStick.Y < -.95 || rightStick.Y < -.95)
                {
                    SelectDown();                    
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Up) || leftStick.Y > .95 || rightStick.Y > .95)
                {
                    SelectUp();                    
                }
                else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (optionList[selectedIndex].type == MenuType.ColorSelect)
                    {
                        numColors++;
                        if (numColors > 10) numColors = 1;
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
                else if (Keyboard.GetState().IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.A) || gamePadState.IsButtonDown(Buttons.Start))
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
