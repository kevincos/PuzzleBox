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
        Settings_Move,
        Settings,
        Summary,
        JellyfishCity,
        Tutorial
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
        Engine p1engine;
        
        public static Settings currentSettings;
        public static GameSettings gameSettings;


        MainMenu mainMenu;
        GameOverMenu gameOverMenu;
        LevelSelectMenu selectMenu;
        PauseMenu pauseMenu;
        SummaryMenu summaryMenu;
        JellyfishCity jellyCity;
        TutorialLauncher tutorialLauncher;
        Menu settingsMenu;

        public static MetaState metaState = MetaState.MainMenu;
        public static SpriteBatch spriteBatch;
        public static int screenSizeX = 800;
        public static int screenSizeY = 400;
        public static int screenCenterX;
        public static int screenCenterY;        
        GraphicsDeviceManager graphics;
        public static SpriteFont spriteFont;

        public Game()
        {
            HighScoreData data = HighScoreTracker.LoadHighScores();
            gameSettings = new GameSettings();
            gameSettings.displayControls = data.displayHelp;
            gameSettings.musicEnabled = data.musicEnabled;
            gameSettings.soundEffectsEnabled = data.soundEffectsEnabled;                 
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += new EventHandler<PreparingDeviceSettingsEventArgs>(graphics_PreparingDeviceSettings);
            //this.graphics.PreferredBackBufferWidth = 800;
            //this.graphics.PreferredBackBufferHeight = 480;
            //this.graphics.IsFullScreen = true;

            Logger.Init();
            Content.RootDirectory = "Content";
            currentSettings = new Settings();
            p1engine = new Engine(-1);
            mainMenu = new MainMenu();
            pauseMenu = new PauseMenu();
            summaryMenu = new SummaryMenu(false);
            gameOverMenu = new GameOverMenu();
            selectMenu = new LevelSelectMenu();
            tutorialLauncher = new TutorialLauncher();
            settingsMenu = new Menu(MenuClass.SettingsMenu);
        }

        void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            DisplayMode displayMode = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = 1024;
            e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = 768;
            //e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = 1280;
            //e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = 720;
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
            MenuOption.menu_on = Content.Load<Texture2D>("on");
            MenuOption.menu_low = Content.Load<Texture2D>("low");
            MenuOption.menu_medium = Content.Load<Texture2D>("medium");
            MenuOption.menu_high = Content.Load<Texture2D>("high");
            mainMenu.background = Content.Load<Texture2D>("mainmenu");
            mainMenu.header = Content.Load<Texture2D>("title");
            settingsMenu.background = Content.Load<Texture2D>("mainmenu");
            settingsMenu.header = Content.Load<Texture2D>("title");            
            mainMenu.AddMenuItem(MenuResult.GoToTimeAttack, Content.Load<Texture2D>("timeattack"),"Score as many points as you can within the \ntime limit.");
            mainMenu.AddMenuItem(MenuResult.GoToPuzzle, Content.Load<Texture2D>("puzzle"), "Solve a series of unique challenges.");
            mainMenu.AddMenuItem(MenuResult.GoToMoveChallenge, Content.Load<Texture2D>("moveChallenge"), "Score as many points as you can with a \nlimited number of moves.");
            mainMenu.AddMenuItem(MenuResult.GoToTutorial, Content.Load<Texture2D>("tutorial"), "Learn to play Jellyfish, MD");
            mainMenu.AddMenuItem(MenuResult.GoToJellyfishCity, Content.Load<Texture2D>("jellycity"), "Check in on your former patients!");
            mainMenu.AddMenuItem(MenuResult.GoToSettings, Content.Load<Texture2D>("settings"), "Change settings for Jellyfish, MD");
            mainMenu.AddMenuItem(MenuResult.Quit, Content.Load<Texture2D>("quit"), "Quit Jellyfish, MD??");
            gameOverMenu.background = Content.Load<Texture2D>("background");
            gameOverMenu.header = Content.Load<Texture2D>("gameover");
            gameOverMenu.AddMenuItem(MenuResult.StartTimeAttack, Content.Load<Texture2D>("replay"));
            gameOverMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));
            gameOverMenu.AddMenuItem(MenuResult.GoToLevelSelect, Content.Load<Texture2D>("returntolevelselect"));
            settingsMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));
            settingsMenu.AddMenuItem(MenuType.SoundToggle, Content.Load<Texture2D>("soundeffects_setting")); 
            settingsMenu.AddMenuItem(MenuType.MusicToggle, Content.Load<Texture2D>("music_setting"));
            settingsMenu.AddMenuItem(MenuType.HelpToggle, Content.Load<Texture2D>("help_setting"));            
            LevelSelectMenu.star = Content.Load<Texture2D>("star");
            LevelSelectMenu.emptyStar = Content.Load<Texture2D>("emptyStar");                 
            gameOverMenu.emptyStar = Content.Load<Texture2D>("emptyStar");
            gameOverMenu.star = Content.Load<Texture2D>("star");
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

            SoundEffects.soundSwoosh = Content.Load<SoundEffect>("swoosh");
            SoundEffects.soundBloop = Content.Load<SoundEffect>("bloop");
            SoundEffects.soundClick = Content.Load<SoundEffect>("click");
            SoundEffects.soundBeep = Content.Load<SoundEffect>("beep");
            MusicControl.track1 = Content.Load<Song>("track1");
            MusicControl.track2 = Content.Load<Song>("track2");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Controls
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
              //  this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                HighScoreData data = HighScoreTracker.LoadHighScores();
                data.soundEffectsEnabled = gameSettings.soundEffectsEnabled;
                data.musicEnabled = gameSettings.musicEnabled;
                data.displayHelp = gameSettings.displayControls;
                HighScoreTracker.SaveHighScores(data);
                Logger.CloseLogger();
                this.Exit();
            }


            if (metaState == MetaState.GamePlay)
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
                }
                if (cause == GameStopCause.TUTORIAL_TEXT)
                {
                    TutorialStage.phase = TutorialPhase.Intro;
                    summaryMenu = new SummaryMenu(true);
                    summaryMenu.text = TutorialStage.IntroText();
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
                    metaState = MetaState.GamePlay;
                if (result == MenuResult.GoToLevelSelect)
                {
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
                if (result == MenuResult.GoToMainMenu)
                {
                    TutorialStage.phase = TutorialPhase.None;
                    metaState = MetaState.MainMenu;
                }
                if (result == MenuResult.Replay)
                {
                    if (TutorialStage.phase == TutorialPhase.Fail || TutorialStage.phase == TutorialPhase.Pass)
                    {
                        p1engine = new Engine(TutorialStage.lessonIndex);
                        p1engine.gameState = State.VANISH;
                        TutorialStage.phase = TutorialPhase.Intro;
                        metaState = MetaState.GamePlay;
                    }
                    else
                    {
                        p1engine = new Engine(-1);
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
                                p1engine = new Engine(TutorialStage.lessonIndex);
                                p1engine.gameState = State.VANISH;
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
                            selectMenu.levelList = SettingsLoader.LoadMoveCountLevels();
                            currentSettings.mode = GameMode.MoveChallenge;
                        }
                        if (currentSettings.mode == GameMode.TimeAttack)
                        {
                            metaState = MetaState.Settings_TimeAttack;
                            selectMenu.cooldown = 250;
                            selectMenu.levelList = SettingsLoader.LoadTimeAttackLevels();
                            currentSettings.mode = GameMode.TimeAttack;
                        }
                        if (currentSettings.mode == GameMode.Puzzle)
                        {
                            metaState = MetaState.Settings_Puzzle;
                            selectMenu.cooldown = 250;
                            selectMenu.levelList = SettingsLoader.LoadPuzzleLevels();
                            currentSettings.mode = GameMode.Puzzle;
                        }
                    }
                    if (result == MenuResult.StartTimeAttack)
                    {
                        p1engine = new Engine(-1);
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
                    currentSettings = SettingsLoader.Tutorial();
                    p1engine = new Engine(0);
                    metaState = MetaState.GamePlay;

                }
            }
            else if (metaState == MetaState.MainMenu)
            {
                MenuResult result = mainMenu.Update(gameTime);
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

            if (metaState == MetaState.JellyfishCity)
                jellyCity.Draw();
            if (metaState == MetaState.GamePlay || metaState == MetaState.Paused || metaState == MetaState.Summary)            
                p1engine.Draw(gameTime);

            if (metaState == MetaState.Settings)
                settingsMenu.Draw();

            if (metaState == MetaState.Paused)
                pauseMenu.Draw();
            if (metaState == MetaState.Summary || summaryMenu.state==SummaryMenuState.READY)
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
