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
        public int timerLimit = 0;
        public string name = "unnamed";

        public Texture2D texture = JellyfishRenderer.orangeJelly;
        public Texture2D preview = Preview.level1;
        public string instructions;
        public int level;

        // Endgame Conditions
        public WinType winType;
        public LoseType loseType;
        public int initialTime = 120000;
        public bool countdownTimer = true;
        public int scoreGoal = 1000;
        public bool displayMoveCount = false;
        public int availableMoves = 100; 
        public bool displayMoves = false;
        public bool displayScore = true;
        public bool displayTimer = true;
        public Color dangerColor = Color.Black;
        public String dangerColorDisplay = "BLACK";
        public bool refillQueues = true;

        public int two_star = 2000;
        public int three_star = 3000;

        public Difficulty difficulty = Difficulty.EASY;
    }

    public enum Difficulty
    {
        EASY,
        MEDIUM,
        HARD
    }

    public enum WinType
    {
        CLEAR,
        SCORE,
        TIMER,
        MOVES
    }

    public enum LoseType
    {
        BADCOLOR,
        TIMER,
        NOMOVES
    }

    class MenuOption
    {
        public MenuResult result;
        public MenuType type;
        public String optionText;
        public string optionString;


        public static Texture2D menu_off;
        public static Texture2D menu_low;
        public static Texture2D menu_on;
        public static Texture2D menu_medium;
        public static Texture2D menu_high;

        public MenuOption(MenuResult result, String optionText, String helpText)
        {
            this.result = result;
            this.optionText = optionText;
            this.optionString = helpText;
            this.type = MenuType.Normal;
        }

        public MenuOption(MenuResult result, string optionString)
        {
            this.result = result;
            this.optionString = optionString;
            this.type = MenuType.Normal;
        }

        public MenuOption(MenuType type, String optionText)
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
        TimerFreq,
        MusicToggle,
        SoundToggle,
        HelpToggle,
        FullScreenToggle
    }

    public enum MenuClass
    {
        MainMenu,
        PauseMenu,
        ResultsMenu,
        SettingsMenu
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
        Undo,
        MenuAction,
        GoToSettings,
        GoToTutorial,
        GoToJellyfishCity,
        GoToTimeAttack,
        GoToSurvival,
        GoToPuzzle,
        GoToMoveChallenge,
        GoToLevelSelect,
        StartTutorial,
        StartTimeAttack,
        StartSurvival,
        StartCollect,
        StartPuzzle,
        Replay,
        NewGame,
        GoToHelpMenu,
        GoToMainMenu,
        GoToResults,
        ResumeGame,
        Quit
    }

    class Menu
    {
        
        public Texture2D background;
        public Texture2D header;

        MenuClass type;
        int headerX = 100;
        int headerY = 20;
        int headerWidth = 824;
        int headerHeight = 150;
        int doctorX = 625;
        int doctorY = 375;
        int optionListX = 75;
        int optionListY = 220;
        int optionGap = 60;
        int optionWidth = 250;
        int optionHeight = 60;
        int speechX = 712;
        int speechY = 600; 
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

        public void AddMenuItem(MenuResult result, String text)
        {
            optionList.Add(new MenuOption(result, text, "BUG"));
        }

        public void AddMenuItem(MenuType type, String text)
        {
            optionList.Add(new MenuOption(type, text));
        }

        public void SelectDown()
        {
            if (selectedIndex < optionList.Count - 1)
            {
                SoundEffects.PlayMove();
                state = MenuState.AnimateDown;
                animationTime = 0;
                selectedIndex++;
            }            
        }
        public void SelectUp()
        {
            if (selectedIndex > 0)
            {
                SoundEffects.PlayMove();
                state = MenuState.AnimateUp;
                animationTime = 0;
                selectedIndex--;
            }            
        }
        public void Draw()
        {
            Game.spriteBatch.Draw(background, new Rectangle(0, 0, Game.screenSizeX, Game.screenSizeY), Color.White);
            Game.spriteBatch.Draw(header, new Rectangle(headerX, headerY, headerWidth, headerHeight), Color.White);
            JellyfishRenderer.DrawJellyfish(doctorX + 200, doctorY, 100, JellyfishRenderer.nurseJellyfish, .75f, SpriteEffects.FlipHorizontally);
            JellyfishRenderer.DrawJellyfish(doctorX, doctorY, 100, JellyfishRenderer.doctorJellyfish, .75f, SpriteEffects.FlipHorizontally);

            for (int i = 0; i < optionList.Count; i++)
            {
                if (i == selectedIndex)
                {                    
                    PuzzleNode p = new PuzzleNode(Color.Blue);
                    p.screenX = optionListX - 25;
                    p.screenY = optionListY + optionHeight / 2 + i * optionGap;
                    if (state == MenuState.AnimateDown)
                        p.screenY -= (int)(optionGap * (1f - (float)animationTime / (float)maxAnimationTime));
                    if (state == MenuState.AnimateUp)
                        p.screenY += (int)(optionGap * (1f - (float)animationTime / (float)maxAnimationTime));

                    p.distance = 50;
                    p.scale = 1f;
                    OrbRenderer.DrawOrb(p, State.READY, 0f);
                    Game.spriteBatch.DrawString(Game.menuFont, optionList[i].optionText, new Vector2(optionListX, optionListY + i * optionGap), Color.LightGreen);
                }
                else
                    Game.spriteBatch.DrawString(Game.menuFont,optionList[i].optionText, new Vector2(optionListX, optionListY + i * optionGap), Color.White);
                if (optionList[i].type == MenuType.SoundToggle)
                {
                    if (Game.gameSettings.soundEffectsEnabled)
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_on, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "On", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                    else
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "Off", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                }
                if (optionList[i].type == MenuType.HelpToggle)
                {
                    if (Game.gameSettings.displayControls)
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_on, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "On", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                    else
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "Off", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                }
                if (optionList[i].type == MenuType.FullScreenToggle)
                {
                    if (Game.gameSettings.fullScreen)
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_on, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "On", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                    else
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "Off", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                }
                if (optionList[i].type == MenuType.MusicToggle)
                {
                    if (Game.gameSettings.musicEnabled)
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_on, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "On", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
                    }
                    else
                    {
                        //Game.spriteBatch.Draw(MenuOption.menu_off, new Rectangle(optionListX + 230, optionListY + i * optionGap, 100, optionHeight), Color.White);
                        Game.spriteBatch.DrawString(Game.menuFont, "Off", new Vector2(optionListX + 300, optionListY + i * optionGap), Color.White);
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
                else if (Keyboard.GetState().IsKeyDown(Keys.B) || gamePadState.IsButtonDown(Buttons.B))
                {
                    return MenuResult.GoToMainMenu;
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
                        if (counterFreq < 0)
                            counterFreq = 3;
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
                    if (optionList[selectedIndex].type == MenuType.HelpToggle)
                    {
                        SoundEffects.PlayClick();
                        Game.gameSettings.displayControls = !Game.gameSettings.displayControls;
                    }
                    if (optionList[selectedIndex].type == MenuType.FullScreenToggle)
                    {
                        SoundEffects.PlayClick();
                        if (Game.gameSettings.fullScreen)
                        {
                            Game.graphics.IsFullScreen = false;
                            Game.graphics.ApplyChanges();
                            Game.gameSettings.fullScreen = false;
                        }
                        else
                        {
                            Game.graphics.IsFullScreen = true;
                            Game.graphics.ApplyChanges();
                            Game.gameSettings.fullScreen = true;
                        }
                    }
                    if (optionList[selectedIndex].type == MenuType.MusicToggle)
                    {
                        SoundEffects.PlayClick();
                        if (Game.gameSettings.musicEnabled)
                        {
                            Game.gameSettings.musicEnabled = false;
                            MusicControl.Stop();
                        }
                        else
                        {
                            Game.gameSettings.musicEnabled = true;
                            MusicControl.PlayMenuMusic();
                        }
                    }
                    if (optionList[selectedIndex].type == MenuType.SoundToggle)
                    {
                        Game.gameSettings.soundEffectsEnabled = !Game.gameSettings.soundEffectsEnabled;
                        SoundEffects.PlayClick();
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
