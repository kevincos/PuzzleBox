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
using Microsoft.Xna.Framework.Storage;

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
        Settings_Move,
        Settings,
        Summary,
        JellyfishCity,
        Tutorial,
        SplashScreen,
        GetDevice,
        InitialLoad
    }

    public enum GameMode
    {
        TimeAttack,
        MoveChallenge,
        Puzzle,
        Tutorial
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static Engine p1engine;
        
        public static Settings currentSettings;
        public static GameSettings gameSettings;

        public static Color jellyBlue;

        public static MainMenu mainMenu;
        public static GameOverMenu gameOverMenu;
        public static LevelSelectMenu selectMenu;
        public static PauseMenu pauseMenu;
        public static SummaryMenu summaryMenu;
        public static JellyfishCity jellyCity;
        public static TutorialLauncher tutorialLauncher;
        public static Menu settingsMenu;
        public static SplashScreen splashScreen;

        public static MetaState metaState = MetaState.InitialLoad;
        public static SpriteBatch spriteBatch;
        public static int screenSizeX = 800;
        public static int screenSizeY = 400;
        bool reselectDevice = false;
        bool showRetry = false;
        bool backToRetry = false;

        public static int screenCenterX;
        public static int screenCenterY;        
        public static GraphicsDeviceManager graphics;
        public static SpriteFont spriteFont;
        public static SpriteFont menuFont;
        public static Game JellyfishMD;

        public bool deviceSelected = false;

        public static PlayerIndex playerIndex;

        void NoStorageCallback(IAsyncResult result)
        {
            if (0 == Guide.EndShowMessageBox(result))
                deviceSelected = true;
            else
            {
                if (metaState == MetaState.SplashScreen)
                {
                    splashScreen = new SplashScreen();
                    metaState = MetaState.SplashScreen;
                }
                else
                {
                    backToRetry = true;
                    showRetry = false;
                }
            }
        }

        void GetDevice(IAsyncResult result)
        {
            try
            {
                HighScoreTracker.device = StorageDevice.EndShowSelector(result);
            }
            catch
            {
                HighScoreTracker.device = null;
            }
            if (HighScoreTracker.device == null)
            {
                try
                {
                    while (Guide.IsVisible == true)
                        System.Threading.Thread.Sleep(500);
                    if (Guide.IsVisible == false)
                    {
                        Guide.BeginShowMessageBox("No Storage Device Selected",
                            "You will be unable to save your game progress or high scores.",
                            new string[] { "OK" },
                            0, MessageBoxIcon.None, NoStorageCallback, null);
                    }
                }
                catch
                {
                    splashScreen = new SplashScreen();
                    metaState = MetaState.SplashScreen;
                }
            }
            else
                deviceSelected = true;
        }

        void RetryDevice(IAsyncResult result)
        {
            try
            {
                HighScoreTracker.device = StorageDevice.EndShowSelector(result);
            }
            catch
            {
                HighScoreTracker.device = null;
            }
            if (HighScoreTracker.device == null)
            {
                try
                {
                    while (Guide.IsVisible == true)
                        System.Threading.Thread.Sleep(500);
                    if (Guide.IsVisible == false)
                    {
                        Guide.BeginShowMessageBox("No Storage Device Selected",
                            "You will be unable to save your game progress or high scores.",
                            new string[] { "OK" },
                            0, MessageBoxIcon.None, NoStorageCallback, null);
                    }
                }
                catch
                {
                }
            }
            else
            {
                deviceSelected = true;
                HighScoreData data = HighScoreTracker.LoadHighScores();
            }
        }


        public Game()
        {            
            jellyBlue = new Color(40, 75, 255);
            JellyfishMD = this;
            graphics = new GraphicsDeviceManager(this);
            
            this.Components.Add(new GamerServicesComponent(this));
            Content.RootDirectory = "Content";            
        }

        public static void SetSplashResolution()
        {
            graphics.IsFullScreen = false;
            Game.graphics.PreferredBackBufferWidth = 1280;
            Game.graphics.PreferredBackBufferHeight = 720;
            JellyfishMD.Window.BeginScreenDeviceChange(false);
            JellyfishMD.Window.EndScreenDeviceChange(JellyfishMD.Window.ScreenDeviceName, 1280, 720);
            Game.graphics.ApplyChanges();
            screenSizeX = Game.graphics.GraphicsDevice.Viewport.Width;
            screenSizeY = Game.graphics.GraphicsDevice.Viewport.Height;
            screenCenterX = screenSizeX / 2;
            screenCenterY = screenSizeY / 2;

        }

        public static void UpdateResolution()
        {
            graphics.IsFullScreen = gameSettings.fullScreen;
            if (Game.gameSettings.wideScreen)
            {
                Game.graphics.PreferredBackBufferWidth = 1280;
                Game.graphics.PreferredBackBufferHeight = 720;
                JellyfishMD.Window.BeginScreenDeviceChange(gameSettings.fullScreen);
                JellyfishMD.Window.EndScreenDeviceChange(JellyfishMD.Window.ScreenDeviceName, 1280, 720);
                Game.graphics.ApplyChanges();
            }
            else
            {
                Game.graphics.PreferredBackBufferWidth = 1024;
                Game.graphics.PreferredBackBufferHeight = 768;
                JellyfishMD.Window.BeginScreenDeviceChange(gameSettings.fullScreen);
                JellyfishMD.Window.EndScreenDeviceChange(JellyfishMD.Window.ScreenDeviceName, 1024, 768);
                Game.graphics.ApplyChanges();
            }
            screenSizeX = Game.graphics.GraphicsDevice.Viewport.Width;
            screenSizeY = Game.graphics.GraphicsDevice.Viewport.Height;
            screenCenterX = screenSizeX / 2;
            screenCenterY = screenSizeY / 2;

            mainMenu.ApplyResolutionChanges();
            settingsMenu.ApplyResolutionChanges();
            tutorialLauncher.ApplyResolutionChanges();
            pauseMenu.ApplyResolutionChanges();
            summaryMenu.ApplyResolutionChanges();
            gameOverMenu.ApplyResolutionChanges();
        }

        protected override void Initialize()
        {
            base.Initialize();
            spriteBatch = new SpriteBatch(GraphicsDevice);            
            Logger.Init();
        }

        protected override void LoadContent()
        {            
            MainMenu.background = Content.Load<Texture2D>("background");
            MainMenu.header = Content.Load<Texture2D>("title");
            Menu.background = Content.Load<Texture2D>("mainmenu");
            Menu.header = Content.Load<Texture2D>("title");            
            GameOverMenu.background = Content.Load<Texture2D>("background");
            GameOverMenu.header = Content.Load<Texture2D>("title");

            LevelSelectMenu.star = Content.Load<Texture2D>("star");
            LevelSelectMenu.emptyStar = Content.Load<Texture2D>("emptyStar");                 
            GameOverMenu.emptyStar = Content.Load<Texture2D>("emptyStar");
            GameOverMenu.star = Content.Load<Texture2D>("star");
            HelpOverlay.help_hide = Content.Load<Texture2D>("help_hide");
            HelpOverlay.help_show = Content.Load<Texture2D>("help_show");
            HelpOverlay.help_sound = Content.Load<Texture2D>("help_sound");
            HelpOverlay.help_music = Content.Load<Texture2D>("help_music");
            HelpOverlay.help_undo = Content.Load<Texture2D>("help_undo");
            HelpOverlay.help_leftstick = Content.Load<Texture2D>("help_leftstick");
            HelpOverlay.help_lefttrigger = Content.Load<Texture2D>("help_lefttrigger");
            HelpOverlay.help_righttrigger = Content.Load<Texture2D>("help_righttrigger");
            HelpOverlay.help_leftbutton = Content.Load<Texture2D>("help_leftbutton");
            HelpOverlay.help_rightbutton = Content.Load<Texture2D>("help_rightbutton");
            HelpOverlay.help_x = Content.Load<Texture2D>("help_x");
            HelpOverlay.summary_confirm = Content.Load<Texture2D>("summary_confirm");
            spriteFont = Content.Load<SpriteFont>("SpriteFont1");
            menuFont = Content.Load<SpriteFont>("SpriteFont2");
            OrbRenderer.orbTexture = Content.Load<Texture2D>("orb");
            OrbRenderer.orbCrackedLeftTexture = Content.Load<Texture2D>("orb-cracked-left");
            OrbRenderer.orbCrackedRightTexture = Content.Load<Texture2D>("orb-cracked-right");
            OrbRenderer.orbCrackedTopTexture = Content.Load<Texture2D>("orb-cracked-top");
            OrbRenderer.orbFragmentLeftTexture = Content.Load<Texture2D>("orb-fragment-left");
            OrbRenderer.orbFragmentRightTexture = Content.Load<Texture2D>("orb-fragment-right");
            OrbRenderer.orbFragmentTopTexture = Content.Load<Texture2D>("orb-fragment-top");
            OrbRenderer.doubleTexture = Content.Load<Texture2D>("double");
            OrbRenderer.toggleInnerTexture = Content.Load<Texture2D>("toggle_inner");
            OrbRenderer.toggleOuterTexture = Content.Load<Texture2D>("toggle_outer");
            OrbRenderer.highlightTexture = Content.Load<Texture2D>("highlight");
            OrbRenderer.haloTexture = Content.Load<Texture2D>("halo");
            OrbRenderer.backgroundTexture = Content.Load<Texture2D>("background");
            OrbRenderer.bubbleTexture = Content.Load<Texture2D>("bubble");
            JellyfishRenderer.jellytexture = Content.Load<Texture2D>("baseballhat");
            JellyfishRenderer.orangeJelly = Content.Load<Texture2D>("patientzero");
            JellyfishRenderer.mysteryJelly = Content.Load<Texture2D>("mysterypatient");
            JellyfishRenderer.agentJelly = Content.Load<Texture2D>("agentjellyfish");
            JellyfishRenderer.baseballJelly = Content.Load<Texture2D>("baseballhat");
            JellyfishRenderer.mustacheJelly = Content.Load<Texture2D>("mustashjellyfish");
            JellyfishRenderer.transparentBody = Content.Load<Texture2D>("transparentbell");
            JellyfishRenderer.transparentRing = Content.Load<Texture2D>("tentaclering");
            JellyfishRenderer.doctorJellyfish = Content.Load<Texture2D>("drjelly");
            JellyfishRenderer.nurseJellyfish = Content.Load<Texture2D>("nursejelly");
            JellyfishRenderer.speechBubble = Content.Load<Texture2D>("speechbubble");
            JellyfishRenderer.speechBubble2 = Content.Load<Texture2D>("speechbubble2");
            JellyfishRenderer.clownJelly = Content.Load<Texture2D>("clownjelly");
            JellyfishRenderer.officerJelly = Content.Load<Texture2D>("policejelly");
            JellyfishRenderer.profJelly = Content.Load<Texture2D>("professorjelly");
            JellyfishRenderer.firemanJelly = Content.Load<Texture2D>("firejelly");
            JellyfishRenderer.libraryJelly = Content.Load<Texture2D>("librarianjelly");
            JellyfishRenderer.baseballJelly2 = Content.Load<Texture2D>("baseballjelly");
            JellyfishRenderer.mogulJelly = Content.Load<Texture2D>("mogoljelly");
            JellyfishRenderer.artistJelly = Content.Load<Texture2D>("artistjelly");
            JellyfishRenderer.bikerJelly = Content.Load<Texture2D>("bikerjelly");
            JellyfishRenderer.birthdayJelly = Content.Load<Texture2D>("birthdayjelly");
            JellyfishRenderer.capnJelly = Content.Load<Texture2D>("capnjelly");
            JellyfishRenderer.chefJelly = Content.Load<Texture2D>("chefjelly");
            JellyfishRenderer.explorerJelly = Content.Load<Texture2D>("explorerjelly");
            JellyfishRenderer.fortuneJelly = Content.Load<Texture2D>("fortunejelly");
            JellyfishRenderer.karateJelly = Content.Load<Texture2D>("karatejelly");
            JellyfishRenderer.kingJelly = Content.Load<Texture2D>("kingjelly");
            JellyfishRenderer.hookerJelly = Content.Load<Texture2D>("jellyofthenight");
            JellyfishRenderer.ballerinaJelly = Content.Load<Texture2D>("ballerinajelly");
            JellyfishRenderer.cowboyJelly = Content.Load<Texture2D>("cowboyjelly");
            JellyfishRenderer.greenJelly = Content.Load<Texture2D>("greenjelly");
            JellyfishRenderer.yellowJelly = Content.Load<Texture2D>("yellowjelly");
            JellyfishRenderer.redJelly = Content.Load<Texture2D>("redjelly");
            JellyfishRenderer.ninjaJelly = Content.Load<Texture2D>("ninjajelly");
            JellyfishRenderer.queenJelly = Content.Load<Texture2D>("queenjelly");
            JellyfishRenderer.newsieJelly = Content.Load<Texture2D>("newsiejelly");            
            Preview.level1 = Content.Load<Texture2D>("level1");
            Preview.level2 = Content.Load<Texture2D>("level2");
            Preview.level3 = Content.Load<Texture2D>("level3");
            Preview.level4 = Content.Load<Texture2D>("level4");
            Preview.level5 = Content.Load<Texture2D>("level5");
            Preview.level6 = Content.Load<Texture2D>("level6");
            Preview.level7 = Content.Load<Texture2D>("level7");
            Preview.level8 = Content.Load<Texture2D>("level8");
            Preview.level9 = Content.Load<Texture2D>("level9");
            Preview.level10 = Content.Load<Texture2D>("level10");
            Preview.level11 = Content.Load<Texture2D>("level11");
            Preview.level12 = Content.Load<Texture2D>("level12");
            Preview.level13 = Content.Load<Texture2D>("level13");
            Preview.level14 = Content.Load<Texture2D>("level14");
            Preview.level15 = Content.Load<Texture2D>("level15");
            Preview.level16 = Content.Load<Texture2D>("level16");
            Preview.level17 = Content.Load<Texture2D>("level17");
            Preview.level18 = Content.Load<Texture2D>("level18");
            Preview.level19 = Content.Load<Texture2D>("level19");


            SoundEffects.soundSwoosh = Content.Load<SoundEffect>("swoosh");
            SoundEffects.soundBloop = Content.Load<SoundEffect>("bloop");
            SoundEffects.soundClick = Content.Load<SoundEffect>("click");
            SoundEffects.soundBeep = Content.Load<SoundEffect>("beep");
            MusicControl.music_menu = Content.Load<Song>("music_menu");
            MusicControl.music_game = Content.Load<Song>("music_game");            
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (metaState == MetaState.InitialLoad)
            {
                splashScreen = new SplashScreen();
                metaState = MetaState.SplashScreen;
            }

            // Controls
            //if (GamePad.GetState(Game.playerIndex).Buttons.Back == ButtonState.Pressed)
              //  this.Exit();
            if (HighScoreTracker.device!=null && Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                HighScoreData data = HighScoreTracker.LoadHighScores();
                data.soundEffectsEnabled = gameSettings.soundEffectsEnabled;
                data.musicEnabled = gameSettings.musicEnabled;
                data.displayHelp = gameSettings.displayControls;
                data.wideScreen = gameSettings.wideScreen;
                data.keyboardControls = gameSettings.keyboardControls;
                HighScoreTracker.SaveHighScores(data);
                Logger.CloseLogger();
                this.Exit();
            }
            if (metaState == MetaState.SplashScreen)
            {
                if (MenuResult.GoToMainMenu == splashScreen.Update(gameTime))
                {
                    base.Initialize();
                    try
                    {
                        if (Guide.IsVisible == true)
                            splashScreen.stillActive = true;
                        else
                            StorageDevice.BeginShowSelector(this.GetDevice, "Select Storage Device");                        
                    }
                    catch
                    {
                        splashScreen.stillActive = true;                        
                    }
                }

            }
            if (HighScoreTracker.device != null && HighScoreTracker.device.IsConnected == true)
            {
                showRetry = false;
            }
            if (backToRetry == true || (HighScoreTracker.device != null && showRetry == false && HighScoreTracker.device.IsConnected == false))
            {
                if (Guide.IsVisible == false)
                {
                    try
                    {
                        StorageDevice.BeginShowSelector(this.RetryDevice, "Select Storage Device");
                        backToRetry = false;
                        showRetry = true;
                    }
                    catch
                    {
                    }
                }
            }
            if (metaState == MetaState.SplashScreen && deviceSelected==true)
            {                
                HighScoreData data = HighScoreTracker.LoadHighScores();
                gameSettings = new GameSettings();
                gameSettings.displayControls = data.displayHelp;
                gameSettings.musicEnabled = data.musicEnabled;
                gameSettings.soundEffectsEnabled = data.soundEffectsEnabled;
                gameSettings.fullScreen = data.fullScreen;
                gameSettings.wideScreen = data.wideScreen;
                gameSettings.keyboardControls = data.keyboardControls;
                currentSettings = new Settings();
                p1engine = new Engine(-1);
                mainMenu = new MainMenu();
                pauseMenu = new PauseMenu();
                summaryMenu = new SummaryMenu(false);
                gameOverMenu = new GameOverMenu();
                selectMenu = new LevelSelectMenu();
                tutorialLauncher = new TutorialLauncher();
                settingsMenu = new Menu(MenuClass.SettingsMenu);
                
                currentSettings = new Settings();
                p1engine = new Engine(-1);
                mainMenu = new MainMenu();
                pauseMenu = new PauseMenu();
                summaryMenu = new SummaryMenu(false);
                gameOverMenu = new GameOverMenu();
                selectMenu = new LevelSelectMenu();
                tutorialLauncher = new TutorialLauncher();
                settingsMenu = new Menu(MenuClass.SettingsMenu);
                
                if(Guide.IsTrialMode)
                    mainMenu.AddMenuItem(MenuResult.GoToTutorial, "Tutorial", "Learn to play Jellyfish, MD");
                mainMenu.AddMenuItem(MenuResult.GoToTimeAttack, "Emergency Room", "Score as many points as you can within the \ntime limit.");
                mainMenu.AddMenuItem(MenuResult.GoToMoveChallenge, "Operation", "Score as many points as you can with a \nlimited number of moves.");
                mainMenu.AddMenuItem(MenuResult.GoToPuzzle, "Challenge", "Solve a series of unique challenges.");
                if(Guide.IsTrialMode==false)
                    mainMenu.AddMenuItem(MenuResult.GoToTutorial, "Tutorial", "Learn to play Jellyfish, MD");
                mainMenu.AddMenuItem(MenuResult.BuyFullGame, "Unlock Full Game", "Purchase the full version of Jellyfish, MD"); 
                mainMenu.AddMenuItem(MenuResult.GoToJellyfishCity, "Jellyfish Parade", "Check in on your former patients!");                
                mainMenu.AddMenuItem(MenuResult.GoToSettings, "Settings", "Change settings for Jellyfish, MD");
                mainMenu.AddMenuItem(MenuResult.Quit, "Quit", "Quit Jellyfish, MD??");
                gameOverMenu.AddMenuItem(MenuResult.StartTimeAttack, "Replay");
                gameOverMenu.AddMenuItem(MenuResult.GoToMainMenu, "Main Menu");
                gameOverMenu.AddMenuItem(MenuResult.GoToLevelSelect, "Level Select");
                settingsMenu.AddMenuItem(MenuResult.GoToMainMenu, "Return to Menu");
                settingsMenu.AddMenuItem(MenuType.SoundToggle, "Sound Effects");
                settingsMenu.AddMenuItem(MenuType.MusicToggle, "Music");
                settingsMenu.AddMenuItem(MenuType.HelpToggle, "Help Overlay");

                #if WINDOWS
                    settingsMenu.AddMenuItem(MenuType.FullScreenToggle, "Full Screen");
                    settingsMenu.AddMenuItem(MenuType.WideScreenToggle, "Wide Screen");
                #endif
                UpdateResolution();

                metaState = MetaState.MainMenu;
            }
            if (metaState == MetaState.GamePlay && IsActive==true)
            {
                GameStopCause cause = p1engine.Update(gameTime);
                if (cause == GameStopCause.PAUSE)
                {
                    pauseMenu = new PauseMenu();
                    metaState = MetaState.Paused;
                }
                if (cause == GameStopCause.END)
                {
                    gameOverMenu.score = p1engine.currentScore;
                    gameOverMenu.level = currentSettings.level;
                    gameOverMenu.state = GameOverMenuState.SCORECHECK;
                    summaryMenu = new SummaryMenu(true);
                    if(currentSettings.mode==GameMode.MoveChallenge)
                        summaryMenu.text = "Looks like you're out of moves, Doctor! \nLet's see how you did...";
                    if(currentSettings.mode == GameMode.TimeAttack)
                        summaryMenu.text = "Time's up, Doctor! Let's see \nhow you did...";
                    MusicControl.PlayMenuMusic();
                    metaState = MetaState.Summary;                    
                }
                if (cause == GameStopCause.WIN)
                {
                    gameOverMenu.score = Engine.clock.timeElapsed;
                    gameOverMenu.level = currentSettings.level;
                    gameOverMenu.state = GameOverMenuState.SCORECHECK;
                    summaryMenu = new SummaryMenu(true);
                    summaryMenu.text = "Way to go, Doctor! You did it!";
                    metaState = MetaState.Summary;
                    MusicControl.PlayMenuMusic();
                }
                if (cause == GameStopCause.TUTORIAL_TEXT)
                {
                    TutorialStage.phase = TutorialPhase.Intro;
                    summaryMenu = new SummaryMenu(true);
                    summaryMenu.text = TutorialStage.IntroText();
                    if (summaryMenu.text == null)
                        metaState = MetaState.GamePlay;
                    summaryMenu.state = SummaryMenuState.READY;

                    metaState = MetaState.Summary;
                }
                if (cause == GameStopCause.TUTORIAL_PASS)
                {
                    TutorialStage.phase = TutorialPhase.Pass;
                    String text = TutorialStage.SuccessText();
                    summaryMenu = new SummaryMenu(true);
                    summaryMenu.text = text;
                    metaState = MetaState.Summary;
                }
                if (cause == GameStopCause.TUTORIAL_FAIL)
                {
                    TutorialStage.phase = TutorialPhase.Fail;
                    String text = TutorialStage.FailureText();                    
                    summaryMenu = new SummaryMenu(false);
                    summaryMenu.text = text;
                    metaState = MetaState.Summary;
                }                
                if (cause == GameStopCause.LOSE_STUCK)
                {
                    summaryMenu = new SummaryMenu(false);
                    summaryMenu.text = "Oh no! Looks like you're stuck! Try to be more \ncareful next time!";
                    gameOverMenu.state = GameOverMenuState.READY;
                    metaState = MetaState.Summary;
                }
                if (cause == GameStopCause.LOSE_ERROR)
                {
                    summaryMenu = new SummaryMenu(false);
                    summaryMenu.text = "Oh no! You burst a " + currentSettings.dangerColorDisplay + " bubble! Try to be more \ncareful next time!";
                    gameOverMenu.state = GameOverMenuState.READY;
                    metaState = MetaState.Summary;
                }
            }
            else if (metaState == MetaState.Paused)
            {
                MenuResult result = pauseMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {
                    TutorialStage.phase = TutorialPhase.None;
                    metaState = MetaState.MainMenu;
                }
                if (result == MenuResult.ResumeGame)
                {
                    summaryMenu.state = SummaryMenuState.NURSEIN;                    
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.GoToLevelSelect)
                {
                    MusicControl.PlayMenuMusic();
                    if (currentSettings.mode == GameMode.TimeAttack)
                        metaState = MetaState.Settings_TimeAttack;
                    if (currentSettings.mode == GameMode.Puzzle)
                        metaState = MetaState.Settings_Puzzle;
                    if (currentSettings.mode == GameMode.MoveChallenge)
                        metaState = MetaState.Settings_Move;
                }
                if (result == MenuResult.Replay)
                {
                    p1engine = new Engine(-1);
                    metaState = MetaState.GamePlay;
                }
            }
            else if (metaState == MetaState.Summary)
            {
                if (Game.currentSettings.mode==GameMode.Puzzle && gameOverMenu.state != GameOverMenuState.SCORECHECK)
                {
                    Engine.clock.Update(gameTime);
                }
                MenuResult result = summaryMenu.Update(gameTime);
                if (TutorialStage.phase != TutorialPhase.None && TutorialStage.introIndex-1 == TutorialStage.controlLessonIndex)
                    p1engine.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {
                    TutorialStage.phase = TutorialPhase.None;
                    metaState = MetaState.MainMenu;
                }
                if (result == MenuResult.Replay)
                {
                    if (TutorialStage.phase == TutorialPhase.Fail)
                    {
                        p1engine.LoadTutorial(TutorialStage.lessonIndex);
                        TutorialStage.phase = TutorialPhase.Intro;
                        TutorialStage.failureIndex = 0;
                        TutorialStage.introIndex = 0;
                        TutorialStage.successIndex = 0;
                        p1engine.firstResume = true;
                        metaState = MetaState.GamePlay;
                    }
                    else if (TutorialStage.phase == TutorialPhase.Pass)
                    {
                        p1engine.LoadTutorial(TutorialStage.lessonIndex);
                        TutorialStage.phase = TutorialPhase.Intro;
                        TutorialStage.failureIndex = 0;
                        TutorialStage.introIndex = 0;
                        TutorialStage.successIndex = 0;
                        p1engine.firstResume = false;
                        metaState = MetaState.GamePlay;
                    }
                    else
                    {
                        p1engine = new Engine(-1);
                        MusicControl.PlayGameMusic();
                        metaState = MetaState.GamePlay;
                    }
                }
                if (result == MenuResult.Undo)
                {
                    p1engine.Back();
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.GoToResults)
                {
                    if (currentSettings.mode == GameMode.Tutorial)
                    {
                        if (TutorialStage.phase == TutorialPhase.Intro)
                        {
                            metaState = MetaState.GamePlay;
                        }
                        else if (TutorialStage.phase == TutorialPhase.Pass)
                        {                           
                            TutorialStage.lessonIndex++;
                            if (TutorialStage.lessonIndex == TutorialStage.maxLesson)
                            {
                                TutorialStage.phase = TutorialPhase.None;
                                summaryMenu = new SummaryMenu(false);
                                metaState = MetaState.MainMenu;
                            }
                            else
                            {
                                //p1engine = new Engine(TutorialStage.lessonIndex);
                                p1engine.LoadTutorial(TutorialStage.lessonIndex);
                                p1engine.firstResume = true;
                                //p1engine.gameState = State.VANISH;
                                TutorialStage.phase = TutorialPhase.Intro;
                                metaState = MetaState.GamePlay;
                            }
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        metaState = MetaState.GameOver_TimeAttack;
                    }
                }
                if (result == MenuResult.GoToLevelSelect)
                {
                    MusicControl.PlayMenuMusic();
                    if (currentSettings.mode == GameMode.TimeAttack)
                        metaState = MetaState.Settings_TimeAttack;
                    if (currentSettings.mode == GameMode.Puzzle)
                        metaState = MetaState.Settings_Puzzle;
                    if (currentSettings.mode == GameMode.MoveChallenge)
                        metaState = MetaState.Settings_Move;
                }
            }
            else if (metaState == MetaState.Settings_TimeAttack || metaState == MetaState.Settings_Puzzle || metaState == MetaState.Settings_Move)
            {
                MenuResult result = selectMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {
                    System.Threading.Thread.Sleep(100);
                    metaState = MetaState.MainMenu;
                }
                if (result == MenuResult.StartTimeAttack)
                {
                    currentSettings = selectMenu.GetCurrentSettings();
                    currentSettings.level = selectMenu.currentLevel;
                    p1engine = new Engine(-1);
                    MusicControl.PlayGameMusic();
                    metaState = MetaState.GamePlay;
                }
            }
            else if (metaState == MetaState.GameOver_TimeAttack)
            {
                if (Engine.mode == ControlMode.AUTOMATED)
                {
                    p1engine = new Engine(-1);
                    metaState = MetaState.GamePlay;
                }
                else
                {
                    MenuResult result = gameOverMenu.Update(gameTime);
                    if (result == MenuResult.GoToMainMenu)
                    {
                        metaState = MetaState.MainMenu;
                        System.Threading.Thread.Sleep(100);
                    }
                    if (result == MenuResult.GoToLevelSelect)
                    {
                        selectMenu = new LevelSelectMenu();
                        selectMenu.state = LevelSelectMenu.SelectMenuState.LOAD;
                        
                        if (currentSettings.mode == GameMode.MoveChallenge)
                        {
                            metaState = MetaState.Settings_Move;
                            selectMenu.cooldown = 250;
                            selectMenu.currentLevel = gameSettings.moveChallengeViewLevel;
                            selectMenu.levelList = SettingsLoader.LoadMoveCountLevels();
                            currentSettings.mode = GameMode.MoveChallenge;
                        }
                        if (currentSettings.mode == GameMode.TimeAttack)
                        {
                            metaState = MetaState.Settings_TimeAttack;
                            selectMenu.cooldown = 250;
                            selectMenu.currentLevel = gameSettings.timeAttackViewLevel;
                            selectMenu.levelList = SettingsLoader.LoadTimeAttackLevels();
                            currentSettings.mode = GameMode.TimeAttack;
                        }
                        if (currentSettings.mode == GameMode.Puzzle)
                        {
                            metaState = MetaState.Settings_Puzzle;
                            selectMenu.cooldown = 250;
                            selectMenu.levelList = SettingsLoader.LoadPuzzleLevels();
                            selectMenu.currentLevel = gameSettings.puzzleViewLevel;
                            currentSettings.mode = GameMode.Puzzle;
                        }
                    }
                    if (result == MenuResult.StartTimeAttack)
                    {
                        p1engine = new Engine(-1);
                        MusicControl.PlayGameMusic();
                        metaState = MetaState.GamePlay;
                    }
                }
            }
            else if (metaState == MetaState.Settings)
            {
                MenuResult result = settingsMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {                    
                    metaState = MetaState.MainMenu;
                }
            }
            else if (metaState == MetaState.JellyfishCity)
            {
                MenuResult result = jellyCity.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                {
                    metaState = MetaState.MainMenu;
                }
            }
            else if (metaState == MetaState.Tutorial)
            {
                MenuResult result = tutorialLauncher.Update(gameTime);
                if (result == MenuResult.StartTutorial)
                {
                    summaryMenu.state = SummaryMenuState.READY;
                    currentSettings = SettingsLoader.Tutorial();
                    p1engine = new Engine(0);
                    p1engine.firstResume = true;
                    metaState = MetaState.GamePlay;

                }
            }
            else if (metaState == MetaState.MainMenu)
            {
                MenuResult result = mainMenu.Update(gameTime);
                if (result == MenuResult.ReturnToSplashScreen)
                {
                    HighScoreData data = HighScoreTracker.LoadHighScores();
                    data.soundEffectsEnabled = gameSettings.soundEffectsEnabled;
                    data.musicEnabled = gameSettings.musicEnabled;
                    data.displayHelp = gameSettings.displayControls;
                    data.fullScreen = gameSettings.fullScreen;
                    data.wideScreen = gameSettings.wideScreen;
                    data.keyboardControls = gameSettings.keyboardControls;
                    HighScoreTracker.SaveHighScores(data);
                    
                    deviceSelected = false;                    
                    HighScoreTracker.device = null;
                    if (HighScoreTracker.container != null)
                        HighScoreTracker.container.Dispose();
                    HighScoreTracker.container = null;
                    HighScoreTracker.cachedData = null;
                    splashScreen = new SplashScreen();
                    metaState = MetaState.SplashScreen;
                }
                if (result == MenuResult.BuyFullGame)
                {
                    try { Guide.ShowMarketplace(Game.playerIndex); }
                    catch (GamerPrivilegeException)
                    {
                        Guide.BeginShowMessageBox("Oops!",
                            "The current controller is either not signed in or is unable to purchase games on XBox Live.",
                            new string[] { "OK" },
                            0, MessageBoxIcon.None, null, null);
                    }
                    catch
                    {
                    }
                        
                    
                }
                if (result == MenuResult.GoToSettings)
                {
                    metaState = MetaState.Settings;                    
                }
                if (result == MenuResult.GoToTimeAttack)
                {
                    metaState = MetaState.Settings_TimeAttack;
                    selectMenu = new LevelSelectMenu();
                    selectMenu.levelList = SettingsLoader.LoadTimeAttackLevels();
                    selectMenu.currentLevel = gameSettings.timeAttackViewLevel;
                    selectMenu.state = LevelSelectMenu.SelectMenuState.LOAD;
                    currentSettings.mode = GameMode.TimeAttack;
                    System.Threading.Thread.Sleep(100);
                }
                if (result == MenuResult.GoToPuzzle)
                {
                    metaState = MetaState.Settings_Puzzle;
                    selectMenu = new LevelSelectMenu();
                    selectMenu.levelList = SettingsLoader.LoadPuzzleLevels();
                    selectMenu.state = LevelSelectMenu.SelectMenuState.LOAD;
                    selectMenu.currentLevel = gameSettings.puzzleViewLevel;
                    currentSettings.mode = GameMode.Puzzle;
                    System.Threading.Thread.Sleep(100);
                }
                if (result == MenuResult.GoToJellyfishCity)
                {
                    metaState = MetaState.JellyfishCity;
                    jellyCity = new JellyfishCity();
                }
                if (result == MenuResult.GoToTutorial)
                {
                    metaState = MetaState.Tutorial;
                    TutorialStage.phase = TutorialPhase.Intro;
                    TutorialStage.lessonIndex = 0;
                    TutorialStage.loaded = false;
                    tutorialLauncher = new TutorialLauncher();
                }
                if (result == MenuResult.GoToMoveChallenge)
                {
                    metaState = MetaState.Settings_Move;
                    selectMenu = new LevelSelectMenu();
                    selectMenu.levelList = SettingsLoader.LoadMoveCountLevels();
                    selectMenu.state = LevelSelectMenu.SelectMenuState.LOAD;
                    selectMenu.currentLevel = gameSettings.moveChallengeViewLevel;
                    currentSettings.mode = GameMode.TimeAttack;
                }
                if (result == MenuResult.StartCollect)
                {
                    p1engine = new Engine(-1);
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.StartPuzzle)
                {
                    p1engine = new Engine(-1);

                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.GoToSurvival)
                {
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.Quit)
                {
                    HighScoreData data = HighScoreTracker.LoadHighScores();
                    data.soundEffectsEnabled = gameSettings.soundEffectsEnabled;
                    data.musicEnabled = gameSettings.musicEnabled;
                    data.displayHelp = gameSettings.displayControls;
                    data.fullScreen = gameSettings.fullScreen;
                    data.wideScreen = gameSettings.wideScreen;
                    data.keyboardControls = gameSettings.keyboardControls;
                    HighScoreTracker.SaveHighScores(data);
                    Logger.CloseLogger();
                    this.Exit();
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
            spriteBatch.Draw(MainMenu.background, new Rectangle(0, 0, screenSizeX, screenSizeY), Color.White);

            if (metaState == MetaState.SplashScreen || metaState == MetaState.GetDevice || metaState == MetaState.InitialLoad)
            {
                splashScreen.Draw();                
            }
            if (metaState == MetaState.JellyfishCity)
                jellyCity.Draw();
            if (metaState == MetaState.GamePlay || metaState == MetaState.Paused || metaState == MetaState.Summary)            
                p1engine.Draw(gameTime);

            if (metaState == MetaState.Settings)
                settingsMenu.Draw();

            if (metaState == MetaState.Paused)
                pauseMenu.Draw();
            if (metaState == MetaState.Summary || (summaryMenu!=null && summaryMenu.state==SummaryMenuState.READY))
                summaryMenu.Draw();
            if (metaState == MetaState.Tutorial)
                tutorialLauncher.Draw();

            else if (metaState == MetaState.Settings_TimeAttack || metaState == MetaState.Settings_Puzzle || metaState == MetaState.Settings_Move)
            {
                selectMenu.Draw();
            }
            else if (metaState == MetaState.GameOver_TimeAttack)
                gameOverMenu.Draw();
            else if (metaState == MetaState.MainMenu)
                mainMenu.Draw();

            base.Draw(gameTime); 
            Game.spriteBatch.End();
        }
    }
}
