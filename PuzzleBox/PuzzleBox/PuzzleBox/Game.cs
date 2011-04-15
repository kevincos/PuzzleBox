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
        GamePlay
    }
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        Engine engine;
        Menu mainMenu;
        Menu pauseMenu;
        MetaState metaState = MetaState.MainMenu;
        public static SpriteBatch spriteBatch;
        public static int screenSizeX = 800;
        public static int screenSizeY = 400;
        public static int screenCenterX;
        public static int screenCenterY;        
        GraphicsDeviceManager graphics;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            engine = new Engine();
            mainMenu = new Menu();
            pauseMenu = new Menu();
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
            pauseMenu.background = Content.Load<Texture2D>("mainmenu");
            pauseMenu.header = Content.Load<Texture2D>("paused");
            pauseMenu.AddMenuItem(MenuResult.ResumeGame, Content.Load<Texture2D>("resume"));
            pauseMenu.AddMenuItem(MenuResult.GoToMainMenu, Content.Load<Texture2D>("returntomenu"));
            mainMenu.background = Content.Load<Texture2D>("mainmenu");
            mainMenu.header = Content.Load<Texture2D>("title");
            mainMenu.AddMenuItem(MenuResult.StartTimeAttack, Content.Load<Texture2D>("timeattack"));
            mainMenu.AddMenuItem(MenuResult.StartSurvival, Content.Load<Texture2D>("survival"));
            mainMenu.AddMenuItem(MenuResult.StartCollect, Content.Load<Texture2D>("collect"));
            mainMenu.AddMenuItem(MenuResult.StartPuzzle, Content.Load<Texture2D>("puzzle"));
            mainMenu.AddMenuItem(MenuResult.GoToHelpMenu, Content.Load<Texture2D>("help"));
            OrbRenderer.spriteFont = Content.Load<SpriteFont>("SpriteFont1");
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
                this.Exit();

            if (metaState == MetaState.GamePlay)
            {
                GameStopCause cause = engine.Update(gameTime);
                if (cause == GameStopCause.PAUSE)
                    metaState = MetaState.Paused;
            }
            else if (metaState == MetaState.Paused)
            {
                MenuResult result = pauseMenu.Update(gameTime);
                if (result == MenuResult.GoToMainMenu)
                    metaState = MetaState.MainMenu;
                if (result == MenuResult.ResumeGame)
                    metaState = MetaState.GamePlay;
            }
            else if (metaState == MetaState.MainMenu)
            {
                MenuResult result = mainMenu.Update(gameTime);
                if (result == MenuResult.StartTimeAttack)
                {
                    engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.StartCollect)
                {
                    engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.StartPuzzle)
                {
                    engine = new Engine();
                    metaState = MetaState.GamePlay;
                }
                if (result == MenuResult.StartSurvival)
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
                engine.Draw(gameTime);
            else if (metaState == MetaState.MainMenu)
                mainMenu.Draw();
            base.Draw(gameTime); 
            Game.spriteBatch.End();
        }
    }
}
