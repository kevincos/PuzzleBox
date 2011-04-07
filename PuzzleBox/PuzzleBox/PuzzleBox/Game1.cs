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
        VERIFY,
        DESTROY,
        REGENERATE,
        ROTATEPOSX,
        ROTATENEGX,
        ROTATEPOSY,
        ROTATENEGY,
        ROTATEPOSZ,
        ROTATENEGZ,
    }

    public enum AnimationState
    {
        NORMAL,
        EXPLODING,
        APPEARING
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Member Variables
        // Graphics
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D orbTexture;
        Texture2D doubleTexture;

        // Game state
        PuzzleBox puzzleBox;
        MasterGrid masterGrid;
        State gameState = State.READY;
        int animateTime = 0;
        int maxAnimateTime = 250;

        // Camera
        float cameraDistance = 50f;
        float theta = 0f;
        float phi = 0f;
        float psi = 0f;
        float rotateSpeed = .05f;
        Vector3 cameraBasePos = new Vector3(50f, 0f, 0f);
        bool firstHalf = true;
        Vector3 v;
        Vector3 u_0;
        Vector3 left;
        Vector3 u;
        
        // Screen display
        int spacing = 60;
        int screenSizeX = 800;
        int screenSizeY = 400;
        float scale = 1.5f;
        float baseDistance;
        float tiltScale = 1f;
        int screenCenterX;
        int screenCenterY;
        List<PuzzleNode> zBuffer;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            base.Initialize();
            screenSizeX = GraphicsDevice.Viewport.Width;
            screenSizeY = GraphicsDevice.Viewport.Height;
            screenCenterX = screenSizeX / 2;
            screenCenterY = screenSizeY / 2;
            OrbRenderer.Init(spriteBatch, orbTexture, doubleTexture);
            puzzleBox = new PuzzleBox();
            masterGrid = new MasterGrid();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            orbTexture = Content.Load<Texture2D>("orb");
            doubleTexture = Content.Load<Texture2D>("double");
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Game state flow
            if (gameState == State.DESTROY || gameState == State.REGENERATE)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > maxAnimateTime)
                    animateTime = maxAnimateTime;
            }
            if (gameState == State.VERIFY)
            {
                if (false == Matcher.Solve(puzzleBox, masterGrid))
                {
                    if (!Matcher.HasValidMove(puzzleBox, masterGrid))
                    {
                        Matcher.Reset(puzzleBox, masterGrid);
                        gameState = State.REGENERATE;
                        animateTime = 0;
                    }
                    else
                    {
                        gameState = State.READY;
                        animateTime = 0;
                    }
                }
                else
                {
                    gameState = State.DESTROY;
                    animateTime = 0;
                }
            }
            if (gameState == State.DESTROY && animateTime == maxAnimateTime)
            {
                // Regenerate orbs
                Matcher.Replace(puzzleBox, masterGrid);

                gameState = State.REGENERATE;
                animateTime = 0;
            }
            if (gameState == State.REGENERATE && animateTime == maxAnimateTime)
            {
                // Clear orbs
                Matcher.Clear(puzzleBox, masterGrid);
                gameState = State.VERIFY;
            }

            // Controls
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkBlue);            
            spriteBatch.Begin();

            // Calculate tilt data
            Vector2 shift = new Vector2(screenCenterX - Mouse.GetState().X, screenCenterY - Mouse.GetState().Y);
            if (shift.Length() > (1.0f * screenSizeX) / 4f * tiltScale)
            {
                shift.Normalize();
                shift = shift * (1.0f * screenSizeX) / 4f * tiltScale;
            }

            #region RotationControls
            switch (gameState)
            {
                case State.ROTATENEGY:
                    theta += rotateSpeed;                    
                    if (theta > Math.PI / 2)
                    {
                        theta = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSY);
                        animateTime = 0;
                        gameState = State.VERIFY;                        
                    }
                    break;
                case State.ROTATEPOSY:
                    theta -= rotateSpeed;
                    if (theta < -Math.PI / 2)
                    {
                        theta = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGY);
                        animateTime = 0;
                        gameState = State.VERIFY;                        
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
                        animateTime = 0;
                        gameState = State.VERIFY;                        
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
                        animateTime = 0;
                        gameState = State.VERIFY;                        
                    }
                    break;
                case State.ROTATENEGZ:
                    psi -= rotateSpeed;
                    if(psi < -Math.PI/2)
                    {
                        psi = (float)-Math.PI / 2;
                        animateTime = 0;
                        psi = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSX);
                        gameState = State.VERIFY;                        
                    }
                    break;
                case State.ROTATEPOSZ:
                    psi += rotateSpeed;
                    if(psi > Math.PI/2)
                    {
                        psi = (float)Math.PI / 2;
                        animateTime = 0;
                        psi = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGX);
                        gameState = State.VERIFY;                        
                    }                    
                    break;
                default:
                    break;
            }
            #endregion

            #region CameraProcessing
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
            if (gameState == State.READY)
                baseDistance = CameraUtils.GetDistance(v, cameraBasePos, new Vector3(-spacing, 0, 0));
            
            #endregion

            #region AddOrbsToZBuffer
            zBuffer = new List<PuzzleNode>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int z = 0; z < 3; z++)
                    {
                        Vector3 p = new Vector3((x - 1) * spacing, (y - 1) * spacing, (z - 1) * spacing);
                        int dist = (int)CameraUtils.GetDistance(v, cameraBasePos, p);
                        puzzleBox.arr[x,y,z].screenX = screenCenterX + CameraUtils.GetScreenX(v, cameraBasePos, u, p) + (int)(shift.X / screenSizeX * dist);
                        puzzleBox.arr[x,y,z].screenY = screenCenterY + CameraUtils.GetScreenY(v, cameraBasePos, u, p) + (int)(shift.Y / screenSizeX * dist);
                        puzzleBox.arr[x,y,z].distance = CameraUtils.GetDistance(v, cameraBasePos, p);
                        puzzleBox.arr[x, y, z].scale = scale;
                        zBuffer.Add(puzzleBox.arr[x,y,z]);
                    }
                }
            }
            zBuffer.Sort();
            #endregion

            #region OrbRendering
            foreach (PuzzleNode p in zBuffer)
            {
                OrbRenderer.DrawOrb(p, gameState, 1f * animateTime / maxAnimateTime);
            }

            for (int x = 0; x < 7; x++)
            {
                for (int y = 0; y < 7; y++)
                {
                    // Avoid rendering orbs in corners and center.
                    if ((x < 2 && y < 2) ||
                        (x < 2 && y > 4) ||
                        (x > 4 && y < 2) ||
                        (x > 4 && y > 4))
                        continue;
                    if (x > 1 && x < 5 && y > 1 && y < 5)
                        continue;
                    
                    {
                        masterGrid.arr[x, y].screenX = screenCenterX + (x - 3) * spacing + (int)(shift.X / screenSizeX * baseDistance);
                        masterGrid.arr[x, y].screenY = screenCenterY + (y - 3) * spacing + (int)(shift.Y / screenSizeX * baseDistance);
                        masterGrid.arr[x, y].distance = baseDistance;
                        masterGrid.arr[x, y].scale = scale;

                        OrbRenderer.DrawOrb(masterGrid.arr[x, y], gameState, 1f * animateTime / maxAnimateTime);
                    }
                }
            }
            #endregion
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
