using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace PuzzleBox
{
    public enum MetaState
    {
        MainMenu,
        Paused,
        GamePlay,
        GameOver_TimeAttack,
        Settings_TimeAttack
    }

    public enum GameMode
    {
        TimeAttack,
        Survival,
        Collect,
        Puzzle
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        Engine p1engine;

        public static Settings currentSettings;

        Menu mainMenu;
        Menu pauseMenu;
        Menu timeAttackGameOverMenu;
        Menu timeAttackSettingsMenu;

        MetaState metaState = MetaState.MainMenu;
        public static SpriteBatch spriteBatch;
        public static int screenSizeX = 800;
        public static int screenSizeY = 400;
        public static int screenCenterX;
        public static int screenCenterY;        
        GraphicsDeviceManager graphics;
        public static SpriteFont spriteFont;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Logger.Init();
            Content.RootDirectory = "Content";
            currentSettings = new Settings();
            p1engine = new Engine();
            mainMenu = new Menu();
            pauseMenu = new Menu();
            timeAttackGameOverMenu = new Menu();
            timeAttackSettingsMenu = new Menu();
        }

        protected override void Initialize()
        {
            base.Initialize();
            screenSizeX = GraphicsDevice.Viewport.Width;
            screenSizeY = GraphicsDevice.Viewport.Height;
            screenCenterX = screenSizeX / 2;
            screenCenterY = screenSizeY / 2;            
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            MenuOption.menu_off = Content.Load<Texture2D>("off");
            MenuOption.menu_low = Content.Load<Texture2D>("low");
            MenuOption.menu_medium = Content.Load<Texture2D>("medium");
            MenuOption.menu_high = Content.Load<Texture2D>("high");
            LifeBar.bar = Content.Load<Texture2D>("lifebar");
            LifeBar.pointer = Content.Load<Texture2D>("pointer");
            Rubric.plus = Content.Load<Texture2D>("plus");
            Rubric.minus = Content.Load<Texture2D>("minus");
            pauseMenu.background = Content.Load<Texture2D>("mainmenu");
            pauseMenu.header = Content.Load<Texture2D>("paused");
            pauseMenu.AddMenuItem(MenuResult.ResumeGame, Content.Load<Texture2D>("resume"));
            pauseMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));
            mainMenu.background = Content.Load<Texture2D>("mainmenu");
            mainMenu.header = Content.Load<Texture2D>("title");
            mainMenu.AddMenuItem(MenuResult.GoToTimeAttack, Content.Load<Texture2D>("timeattack"));
            mainMenu.AddMenuItem(MenuResult.GoToSurvival, Content.Load<Texture2D>("survival"));
            mainMenu.AddMenuItem(MenuResult.StartCollect, Content.Load<Texture2D>("collect"));
            mainMenu.AddMenuItem(MenuResult.StartPuzzle, Content.Load<Texture2D>("puzzle"));
            mainMenu.AddMenuItem(MenuResult.GoToHelpMenu, Content.Load<Texture2D>("help"));
            timeAttackGameOverMenu.background = Content.Load<Texture2D>("background");
            timeAttackGameOverMenu.header = Content.Load<Texture2D>("gameover");
            timeAttackGameOverMenu.AddMenuItem(MenuResult.StartTimeAttack, Content.Load<Texture2D>("replay"));
            timeAttackGameOverMenu.AddMenuItem(MenuResult.GoToTimeAttack, Content.Load<Texture2D>("newgame"));            
            timeAttackGameOverMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));
            timeAttackSettingsMenu.background = Content.Load<Texture2D>("background");
            timeAttackSettingsMenu.header = Content.Load<Texture2D>("timeattackheader");
            timeAttackSettingsMenu.AddMenuItem(MenuResult.StartTimeAttack, Content.Load<Texture2D>("newgame"));
            timeAttackSettingsMenu.AddMenuItem(MenuType.ColorSelect, Content.Load<Texture2D>("colors"));
            timeAttackSettingsMenu.AddMenuItem(MenuType.ToggleFreq, Content.Load<Texture2D>("toggleorbs"));
            timeAttackSettingsMenu.AddMenuItem(MenuType.CounterFreq, Content.Load<Texture2D>("counterorbs"));
            timeAttackSettingsMenu.AddMenuItem(MenuType.TimerFreq, Content.Load<Texture2D>("timerorbs"));
            timeAttackSettingsMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));

            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            OrbRenderer.orbTexture = Content.Load<Texture2D>("orb");
            OrbRenderer.orbCrackedLeftTexture = Content.Load<Texture2D>("orb-cracked-left");
            OrbRenderer.orbCrackedRightTexture = Content.Load<Texture2D>("orb-cracked-right");
            OrbRenderer.orbCrackedTopTexture = Content.Load<Texture2D>("orb-cracked-top");
            OrbRenderer.orbFragmentLeftTexture = Content.Load<Texture2D>("orb-fragment-left");
            OrbRenderer.orbFragmentRightTexture = Content.Load<Texture2D>("orb-fragment-right");
            OrbRenderer.orbFragmentTopTexture = Content.Load<Texture2D>("orb-fragment-top");
            OrbRenderer.numbersTexture = Content.Load<Texture2D>("numbers");
            OrbRenderer.doubleTexture = Content.Load<Texture2D>("double");
            OrbRenderer.toggleTexture = Content.Load<Texture2D>("toggle");
            OrbRenderer.highlightTexture = Content.Load<Texture2D>("highlight");
            OrbRenderer.backgroundTexture = Content.Load<Texture2D>("background");
            OrbRenderer.vortexTexture = Content.Load<Texture2D>("vortex");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Controls
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Logger.CloseLogger();
                this.Exit();
            }


            if (metaState == MetaState.GamePlay)
            {
                GameStopCause cause = p1engine.Update(gameTime);                
                if (cause == GameStopCause.PAUSE)
                    metaState = MetaState.Paused;
                if (cause == GameStopCause.END)
                {
                    timeAttackGameOverMenu.AddScore(p1engine.currentScore, 400, 200);
                    metaState = MetaState.GameOver_TimeAttack;
                }
            }
            else if (metaState == MetaState.Paused)
            {
                MenuResult result = pauseMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                    metaState = MetaState.MainMenu;
                if (result == MenuResult.ResumeGame)
                    metaState = MetaState.GamePlay;
            }
            else if (metaState == MetaState.Settings_TimeAttack)
            {
                MenuResult result = timeAttackSettingsMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {
                    System.Threading.Thread.Sleep(100);
                    metaState = MetaState.MainMenu;
                }
                if (result == MenuResult.StartTimeAttack)
                {
                    timeAttackSettingsMenu.UpdateSettings(currentSettings);
                    currentSettings.mode = GameMode.TimeAttack;
                    p1engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
            }
            else if (metaState == MetaState.GameOver_TimeAttack)
            {
                if (Engine.automated == true)
                {
                    p1engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
                else
                {
                    MenuResult result = timeAttackGameOverMenu.Update(gameTime);
                    if (result == MenuResult.GoToMainMenu)
                    {
                        metaState = MetaState.MainMenu;
                        System.Threading.Thread.Sleep(100);
                    }
                    if (result == MenuResult.StartTimeAttack)
                    {
                        p1engine = new Engine();
                        metaState = MetaState.GamePlay;
                    }
                }
            }
            else if (metaState == MetaState.MainMenu)
            {
                MenuResult result = mainMenu.Update(gameTime);
                if (result == MenuResult.GoToTimeAttack)
                {                    
                    metaState = MetaState.Settings_TimeAttack;
                    System.Threading.Thread.Sleep(100);
                }
                if (result == MenuResult.StartCollect)
                {
                    p1engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.StartPuzzle)
                {
                    p1engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.GoToSurvival)
                {
                    metaState = MetaState.GamePlay;
                }

            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);
            Game.spriteBatch.Begin();
            if (metaState == MetaState.Paused)
                pauseMenu.Draw();
            else if (metaState == MetaState.GamePlay)
            {
                p1engine.Draw(gameTime);
            }
            else if (metaState == MetaState.Settings_TimeAttack)
                timeAttackSettingsMenu.Draw();
            else if (metaState == MetaState.GameOver_TimeAttack)
                timeAttackGameOverMenu.Draw();
            else if (metaState == MetaState.MainMenu)
                mainMenu.Draw();
            base.Draw(gameTime); 
            Game.spriteBatch.End();
        }
    }
}
