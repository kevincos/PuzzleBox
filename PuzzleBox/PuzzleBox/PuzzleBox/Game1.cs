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
    public enum State
    {
        READY,
        ROTATEPOSX,
        ROTATENEGX,
        ROTATEPOSY,
        ROTATENEGY,
        ROTATEPOSZ,
        ROTATENEGZ,
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D orbTexture;
        PuzzleBox puzzleBox;
        State gameState = State.READY;
        float cameraDistance = 50f;
        float theta = 0f;
        float phi = 0f;
        float psi = 0f;
        float rotateSpeed = .05f;
        Vector3 cameraBasePos = new Vector3(50f, 0f, 0f);
        bool firstHalf = true;
        
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
            if (gameState == State.READY)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    gameState = State.ROTATEPOSY;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    gameState = State.ROTATENEGY;
                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                    gameState = State.ROTATEPOSX;
                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                    gameState = State.ROTATENEGX;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    gameState = State.ROTATEPOSZ;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    gameState = State.ROTATENEGZ;
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            int mouseX = Mouse.GetState().X;
            int mouseY = Mouse.GetState().Y;
            Vector2 shift = new Vector2(400-mouseX, 300-mouseY);
            if (shift.Length() > 200f)
            {
                shift.Normalize();
                shift = shift * 200f;
            }

            Vector3 offSet = new Vector3(shift.X, shift.Y, 0);

            switch (gameState)
            {
                case State.ROTATENEGY:
                    theta += rotateSpeed;                    
                    if (theta > Math.PI / 2)
                    {
                        theta = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSY);
                        gameState = State.READY;
                    }
                    break;
                case State.ROTATEPOSY:
                    theta -= rotateSpeed;
                    if (theta < -Math.PI / 2)
                    {
                        theta = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGY);
                        gameState = State.READY;
                    }
                    break;
                case State.ROTATENEGX:
                    phi += rotateSpeed;                    
                    if (firstHalf && phi > Math.PI / 4)
                    {
                        phi = -(float)Math.PI/4;                            
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGZ);
                        firstHalf = false;                        
                    }
                    if (!firstHalf && phi > 0)
                    {
                        phi = 0;
                        firstHalf = true;      
                        gameState = State.READY;
                    }
                    break;
                case State.ROTATEPOSX:
                    phi -= rotateSpeed;
                    if (firstHalf && phi < -Math.PI / 4)
                    {
                        phi = (float)Math.PI/4;
                        firstHalf = false;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSZ);                        
                    }
                    if (!firstHalf && phi < 0)
                    {
                        phi = 0;
                        firstHalf = true;
                        gameState = State.READY;
                    }
                    break;
                case State.ROTATENEGZ:
                    psi -= rotateSpeed;
                    if(psi < -Math.PI/2)
                    {
                        psi = (float)-Math.PI / 2;
                        gameState = State.READY;
                        psi = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSX);
                    }
                    break;
                case State.ROTATEPOSZ:
                    psi += rotateSpeed;
                    if(psi > Math.PI/2)
                    {
                        psi = (float)Math.PI / 2;
                        gameState = State.READY;
                        psi = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGX);
                    }                    
                    break;
                default:
                    break;
            }

            Vector3 v;
            Vector3 u_0;
            Vector3 left;
            Vector3 u;
                       
            cameraBasePos = new Vector3(cameraDistance * (float)Math.Cos(theta) * (float)Math.Sin(Math.PI/2-phi),
                cameraDistance * (float)Math.Cos(Math.PI / 2 - phi),
                cameraDistance * (float)Math.Sin(theta) * (float)Math.Sin(Math.PI / 2 - phi));

            v = (Vector3.Zero - cameraBasePos);
            u_0 = Vector3.Cross(v, Vector3.Cross(new Vector3(0f, 1f, 0f), v));
            u_0.Normalize();
            v.Normalize();
            left = Vector3.Cross(u_0, v);
            left.Normalize();
            u = (float)Math.Sin(psi)*left + (float)Math.Cos(psi)*u_0;
            u.Normalize();
            left = Vector3.Cross(u, v);
            left.Normalize();
            /*cameraBasePos = cameraBasePos +
                (u * shift.Y / 300 * 20f)*(float)Math.Cos(2*phi) +
                (left * shift.X / 400 * 20f) * (float)Math.Cos(2*phi);

            v = (Vector3.Zero - cameraBasePos);
            u_0 = Vector3.Cross(v, Vector3.Cross(new Vector3(0f, 1f, 0f), v));
            u_0.Normalize();
            v.Normalize();
            left = Vector3.Cross(u_0, v);
            left.Normalize();
            u = (float)Math.Sin(psi) * left + (float)Math.Cos(psi) * u_0;*/

            List<PuzzleNode> zBuffer = new List<PuzzleNode>();

            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        Vector3 p = new Vector3((x - 1) * 100f, (y - 1) * 100f, (z - 1) * 100f);
                        int dist = (int)CameraUtils.GetDistance(v, cameraBasePos, p);
                        zBuffer.Add(new PuzzleNode(puzzleBox.arr[x, y, z].color,
                            400 + CameraUtils.GetScreenX(v, cameraBasePos, u, p) + (int)(shift.X/800f*dist),
                            300 + CameraUtils.GetScreenY(v, cameraBasePos, u, p) + (int)(shift.Y/800f*dist),
                            CameraUtils.GetDistance(v, cameraBasePos, p)));

                    }
                }
            }

            zBuffer.Sort();
            foreach (PuzzleNode p in zBuffer)
            {
                OrbRenderer.DrawOrb(p.screenX, p.screenY, 2.5f, p.color);
            }
            float dist2 = 150f;
            OrbRenderer.DrawOrb(200 + (int)(shift.X / 800f * dist2),
                300 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Red);
            OrbRenderer.DrawOrb(100 + (int)(shift.X / 800f * dist2),
                300 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Orange);
            OrbRenderer.DrawOrb(600 + (int)(shift.X / 800f * dist2),
                300 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Red);
            OrbRenderer.DrawOrb(700 + (int)(shift.X / 800f * dist2),
                300 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Orange);
            OrbRenderer.DrawOrb(200 + (int)(shift.X / 800f * dist2),
                200 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Red);
            OrbRenderer.DrawOrb(100 + (int)(shift.X / 800f * dist2),
                200 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Orange);
            OrbRenderer.DrawOrb(600 + (int)(shift.X / 800f * dist2),
                200 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Blue);
            OrbRenderer.DrawOrb(700 + (int)(shift.X / 800f * dist2),
                200 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Yellow);
            OrbRenderer.DrawOrb(200 + (int)(shift.X / 800f * dist2),
                400 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Red);
            OrbRenderer.DrawOrb(100 + (int)(shift.X / 800f * dist2),
                400 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Pink);
            OrbRenderer.DrawOrb(600 + (int)(shift.X / 800f * dist2),
                400 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Red);
            OrbRenderer.DrawOrb(700 + (int)(shift.X / 800f * dist2),
                400 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Green);
            OrbRenderer.DrawOrb(300 + (int)(shift.X / 800f * dist2),
                100 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Orange);
            OrbRenderer.DrawOrb(400 + (int)(shift.X / 800f * dist2),
                100 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Yellow);
            OrbRenderer.DrawOrb(500 + (int)(shift.X / 800f * dist2),
                100 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Green);
            OrbRenderer.DrawOrb(300 + (int)(shift.X / 800f * dist2),
                0 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Magenta);
            OrbRenderer.DrawOrb(400 + (int)(shift.X / 800f * dist2),
                0 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Blue);
            OrbRenderer.DrawOrb(500 + (int)(shift.X / 800f * dist2),
                0 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Green);
            OrbRenderer.DrawOrb(300 + (int)(shift.X / 800f * dist2),
                500 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Yellow);
            OrbRenderer.DrawOrb(400 + (int)(shift.X / 800f * dist2),
                500 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Green);
            OrbRenderer.DrawOrb(500 + (int)(shift.X / 800f * dist2),
                500 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Red);
            OrbRenderer.DrawOrb(300 + (int)(shift.X / 800f * dist2),
                600 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Green);
            OrbRenderer.DrawOrb(400 + (int)(shift.X / 800f * dist2),
                600 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Green);
            OrbRenderer.DrawOrb(500 + (int)(shift.X / 800f * dist2),
                600 + (int)(shift.Y / 800f * dist2),
                2.5f, Color.Magenta);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
