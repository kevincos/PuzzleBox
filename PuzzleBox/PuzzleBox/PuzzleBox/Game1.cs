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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D orbTexture;
        PuzzleBox puzzleBox;
        float cameraDistance = 50f;
        float theta = 0f;
        float phi = 0f;
        Vector3 cameraBasePos = new Vector3(50f, 0f, 0f);
        Vector3 u = new Vector3(0f, 1f, 0f);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();

            OrbRenderer.Init(spriteBatch, orbTexture);
            puzzleBox = new PuzzleBox();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            orbTexture = Content.Load<Texture2D>("orb");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                theta -= .05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                theta += .05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                phi -= .05f;
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                phi += .05f;
                


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;
            int shiftX = 400 - mouseX;
            int shiftY = 300 - mouseY;

            
            //theta = theta + 2 * (float)Math.PI * gameTime.ElapsedGameTime.Milliseconds / 4000;
            //phi = (float)shiftY/300*(float)Math.PI/2;
            cameraBasePos = new Vector3(cameraDistance * (float)Math.Cos(theta) * (float)Math.Sin(Math.PI/2-phi),
                cameraDistance * (float)Math.Cos(Math.PI / 2 - phi),
                cameraDistance * (float)Math.Sin(theta) * (float)Math.Sin(Math.PI / 2 - phi));
            Vector3 v = (Vector3.Zero - cameraBasePos);
            
            u = Vector3.Cross(v, Vector3.Cross(new Vector3(0f, 1f, 0f), v));
            v.Normalize();
            u.Normalize();

            List<PuzzleNode> zBuffer = new List<PuzzleNode>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        Vector3 p = new Vector3((x - 1) * 100f, (y - 1) * 100f, (z - 1) * 100f);
                        zBuffer.Add(new PuzzleNode(puzzleBox.arr[x, y, z].color,
                            400 + CameraUtils.GetScreenX(v, cameraBasePos, u, p),
                            300 + CameraUtils.GetScreenY(v, cameraBasePos, u, p),
                            CameraUtils.GetDistance(v, cameraBasePos, p)));

                    }
                }
            }

            zBuffer.Sort();
            foreach (PuzzleNode p in zBuffer)
            {
                OrbRenderer.DrawOrb(p.screenX, p.screenY, 2.5f, p.color);
            }

            /*for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    OrbRenderer.DrawOrb(shiftX + 300 + x * 100, shiftY+150 + y * 100, 2.5f, puzzleBox.arr[x, y, 2].color);
                }
            }
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    OrbRenderer.DrawOrb(shiftX/2 + 300 + x * 100, shiftY/2 + 150 + y * 100, 2.5f, puzzleBox.arr[x, y, 2].color);
                }
            }
            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    OrbRenderer.DrawOrb(300+x*100, 150+y*100, 2.5f, puzzleBox.arr[x, y, 0].color);
                }                
            }*/
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
