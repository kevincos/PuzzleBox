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
        Settings_TimeAttack,
        Settings_Puzzle,
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


        MainMenu mainMenu;
        Menu timeAttackGameOverMenu;
        LevelSelectMenu selectMenu;
        PauseMenu pauseMenu;

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
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            //this.graphics.PreferredBackBufferWidth = 800;
            //this.graphics.PreferredBackBufferHeight = 480;
            this.graphics.IsFullScreen = true;

            Logger.Init();
            Content.RootDirectory = "Content";
            currentSettings = new Settings();
            p1engine = new Engine();
            mainMenu = new MainMenu();
            pauseMenu = new PauseMenu();
            timeAttackGameOverMenu = new Menu(MenuClass.ResultsMenu);
            selectMenu = new LevelSelectMenu();
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            //e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
            //e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = 1024;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = 768;
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
            mainMenu.background = Content.Load<Texture2D>("mainmenu");
            mainMenu.header = Content.Load<Texture2D>("title");
            mainMenu.AddMenuItem(MenuResult.GoToTimeAttack, Content.Load<Texture2D>("timeattack"));
            mainMenu.AddMenuItem(MenuResult.GoToPuzzle, Content.Load<Texture2D>("puzzle"));
            timeAttackGameOverMenu.background = Content.Load<Texture2D>("background");
            timeAttackGameOverMenu.header = Content.Load<Texture2D>("gameover");
            timeAttackGameOverMenu.AddMenuItem(MenuResult.StartTimeAttack, Content.Load<Texture2D>("replay"));
            timeAttackGameOverMenu.AddMenuItem(MenuResult.GoToTimeAttack, Content.Load<Texture2D>("newgame"));            
            timeAttackGameOverMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));
            
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            OrbRenderer.orbTexture = Content.Load<Texture2D>("orb");
            OrbRenderer.orbCrackedLeftTexture = Content.Load<Texture2D>("orb-cracked-left");
            OrbRenderer.orbCrackedRightTexture = Content.Load<Texture2D>("orb-cracked-right");
            OrbRenderer.orbCrackedTopTexture = Content.Load<Texture2D>("orb-cracked-top");
            OrbRenderer.orbFragmentLeftTexture = Content.Load<Texture2D>("orb-fragment-left");
            OrbRenderer.orbFragmentRightTexture = Content.Load<Texture2D>("orb-fragment-right");
            OrbRenderer.orbFragmentTopTexture = Content.Load<Texture2D>("orb-fragment-top");
            OrbRenderer.numbersTexture = Content.Load<Texture2D>("numbers");
            OrbRenderer.clocknumbersTexture = Content.Load<Texture2D>("clocknumbers");
            OrbRenderer.doubleTexture = Content.Load<Texture2D>("double");
            OrbRenderer.toggleTexture = Content.Load<Texture2D>("toggle");
            OrbRenderer.highlightTexture = Content.Load<Texture2D>("highlight");
            OrbRenderer.backgroundTexture = Content.Load<Texture2D>("background");
            OrbRenderer.vortexTexture = Content.Load<Texture2D>("vortex");
            JellyfishRenderer.jellytexture = Content.Load<Texture2D>("baseballhat");
            JellyfishRenderer.orangeJelly = Content.Load<Texture2D>("patientzero");
            JellyfishRenderer.agentJelly = Content.Load<Texture2D>("baseballhat");
            JellyfishRenderer.baseballJelly = Content.Load<Texture2D>("agentjellyfish");
            JellyfishRenderer.mustacheJelly = Content.Load<Texture2D>("mustashjellyfish");
            JellyfishRenderer.transparentBody = Content.Load<Texture2D>("transparentbell");
            JellyfishRenderer.transparentRing = Content.Load<Texture2D>("tentaclering");
            JellyfishRenderer.doctorJellyfish = Content.Load<Texture2D>("drjelly");
            JellyfishRenderer.nurseJellyfish = Content.Load<Texture2D>("nursejelly");
            JellyfishRenderer.speechBubble = Content.Load<Texture2D>("speechbubble");
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
                    timeAttackGameOverMenu.AddScore(p1engine.currentScore, 100, 350);
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
            else if (metaState == MetaState.Settings_TimeAttack || metaState == MetaState.Settings_Puzzle)
            {
                MenuResult result = selectMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {
                    System.Threading.Thread.Sleep(100);
                    metaState = MetaState.MainMenu;
                }
                if (result == MenuResult.StartTimeAttack)
                {
                    currentSettings = selectMenu.GetCurrentSetttings();
                    p1engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
            }
            else if (metaState == MetaState.GameOver_TimeAttack)
            {
                if (Engine.mode == ControlMode.AUTOMATED)
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
                    selectMenu.levelList = SettingsLoader.LoadTimeAttackLevels();
                    System.Threading.Thread.Sleep(100);
                }
                if (result == MenuResult.GoToPuzzle)
                {
                    metaState = MetaState.Settings_Puzzle;
                    selectMenu.levelList = SettingsLoader.LoadPuzzleLevels();
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
            Color bgColor = Color.DarkBlue;
            bgColor.R = 14;
            bgColor.B = 84;
            GraphicsDevice.Clear(bgColor);
            Game.spriteBatch.Begin();
            
            if (metaState == MetaState.GamePlay || metaState == MetaState.Paused)            
                p1engine.Draw(gameTime);

            if (metaState == MetaState.Paused)
                pauseMenu.Draw();
            else if (metaState == MetaState.Settings_TimeAttack || metaState == MetaState.Settings_Puzzle)
            {
                selectMenu.Draw();                
            }
            else if (metaState == MetaState.GameOver_TimeAttack)
                timeAttackGameOverMenu.Draw();
            else if (metaState == MetaState.MainMenu)
                mainMenu.Draw();

            base.Draw(gameTime); 
            Game.spriteBatch.End();
        }
    }
}
