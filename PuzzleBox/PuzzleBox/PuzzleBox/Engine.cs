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
    public enum State
    {
        READY,
        VERIFY,
        DESTROY,
        VANISH,
        REGENERATE,
        MOVECOMPLETE,
        NEWSET,
        PUSH,
        PULL,
        ROTATEPOSX,
        ROTATENEGX,
        ROTATEPOSY,
        ROTATENEGY,
        ROTATEPOSZ,
        ROTATENEGZ,
        PAUSING,
        RESUMING
    }

    public enum AnimationState
    {
        NORMAL,
        EXPLODING,
        APPEARING
    }

    public enum GameStopCause
    {
        NONE,
        PAUSE,
        END,
        WIN,
        LOSE_STUCK,
        LOSE_ERROR,
        TUTORIAL_TEXT,
        TUTORIAL_PASS,
        TUTORIAL_FAIL
    }

    public enum ControlMode
    {
        NORMAL,
        AUTOMATED,
        EDITOR
    }

    class Engine
    {

        #region Member Variables

        public static ControlMode mode = ControlMode.NORMAL;
        Random automator;
        public PuzzleNode selectedNode = null;
        public List<PuzzleNode> selectedQueue = null;
        int selectedDepth = 0;
        int editorCooldown = 0;

        GameStopCause pendingResult = GameStopCause.NONE;
            
        // Game dimensions
        public static int gridSize = 5;
        public static int boxSize = 3;
        public static int boxOffset = 1;
        public static int centerBoxIndex = (boxSize - 1) / 2;
        public static int centerGridIndex = boxOffset + centerBoxIndex;

        // Effects
        List<Fragment> fragmentList;
        List<ScoringSet> scoreList;

        int jellyX = 517;
        int jellyY = 400;
        int jellyBodyY = 437; //420

        // Game state
        int cooldown = 0;
        public static Countdown timer = new Countdown(0,0,0);
        public static Countup clock = new Countup(0,0,0);
        public int movesRemaining = 50;
        PuzzleBox puzzleBox;
        MasterGrid masterGrid;
        PuzzleBox prevPuzzleBox;
        MasterGrid prevMasterGrid;
        int prevCubeDistance;
        public State gameState = State.RESUMING;
        int animateTime = 0;
        int maxAnimateTime = 250;
        int maxSlideDistance = 0;
        public int currentScore = 0;

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
        Vector2 shift;
        Vector2 targetShift;
        Vector2 savedShift;

        // Screen display
        public static int spacing = 60;

        float scale = 1.5f;
        public static float baseDistance = 110f;
        public static int cubeDistance;
        public static int cubeDistanceGoal;
        float tiltScale = 1.5f;

        List<PuzzleNode> zBuffer;
        #endregion

        public void Back()
        {
            puzzleBox = prevPuzzleBox.Copy();
            masterGrid = prevMasterGrid.Copy();
            cubeDistance = prevCubeDistance;
            //gameState = State.NEWSET;
            pendingResult = GameStopCause.NONE;
            gameState = State.VANISH;
            animateTime = 0;
            prevPuzzleBox = null;
            prevMasterGrid = null;
        }

        public Engine(int tutorialStage)
        {
            Logger.ClearLogger();
            automator = new Random();
            cubeDistance = 0;
            cubeDistanceGoal = 0;
            if (Game.currentSettings.displayTimer && Game.currentSettings.countdownTimer)
            {
                timer = new Countdown(Game.currentSettings.initialTime, 850, 640);
                timer.enabled = true;
                clock.enabled = false;
            }
            else if (Game.currentSettings.displayTimer && Game.currentSettings.countdownTimer != true)
            {
                clock = new Countup(0, 850, 640);
                clock.enabled = true;
                timer.enabled = false;

            }
            else
            {
                timer.enabled = false;
                clock.enabled = false;
            }

            targetShift = new Vector2(1000, 1000);
            savedShift = new Vector2(1000, 1000);
            shift = new Vector2(1000, 1000);

            puzzleBox = new PuzzleBox();
            masterGrid = new MasterGrid();
            
            fragmentList = new List<Fragment>();
            scoreList = new List<ScoringSet>();
            if (Game.currentSettings.randomOrbs)
            {
                Matcher.Reset(puzzleBox, masterGrid);
                Matcher.Clear(puzzleBox, masterGrid);
                gameState = State.RESUMING;
                MusicControl.PlayGameMusic();
            }
            else if (tutorialStage!=-1)
            {
                TutorialStage.LoadLesson(tutorialStage, puzzleBox, masterGrid);
                gameState = State.RESUMING;
            }
            else
            {
                LevelLoader.LoadLevel(Game.currentSettings.levelfilename, puzzleBox, masterGrid);
                gameState = State.RESUMING;
            }
            animateTime = 0;
                
        }

        public GameStopCause Update(GameTime gameTime)
        {
            cooldown -= gameTime.ElapsedGameTime.Milliseconds;
            if (cooldown < 0) cooldown = 0;
            if (mode != ControlMode.EDITOR)
            {
                Matcher.UpdateTimeCountdown(puzzleBox, masterGrid, gameTime.ElapsedGameTime.Milliseconds);
                if (false == timer.Update(gameTime))
                {
                    Logger.totalScore = currentScore;
                    Logger.LogGame();
                    return GameStopCause.END;
                }
                clock.Update(gameTime);
            }
            // Game state flow
            if (gameState == State.DESTROY || gameState == State.VANISH || gameState == State.NEWSET)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > maxAnimateTime)
                    animateTime = maxAnimateTime;
            }
            if (gameState == State.PAUSING || gameState == State.RESUMING)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > 2*maxAnimateTime)
                    animateTime = 2*maxAnimateTime;
            }
            if (gameState == State.REGENERATE)
            {
                animateTime += gameTime.ElapsedGameTime.Milliseconds;
                if (animateTime > maxAnimateTime * maxSlideDistance)
                    animateTime = maxAnimateTime * maxSlideDistance;
            }
            if (gameState == State.MOVECOMPLETE)
            {
                if (mode == ControlMode.EDITOR) gameState = State.READY;
                else
                {
                    if(Game.currentSettings.mode==GameMode.MoveChallenge)
                        movesRemaining--;
                    Matcher.UpdateToggleState(puzzleBox, masterGrid);      
                    gameState = State.VERIFY;
                }
            }
            if (gameState == State.VERIFY)
            {
                List<ScoringSet> scoreSet = Matcher.Solve(puzzleBox, masterGrid);
                if (scoreSet.Count > 0)
                    SoundEffects.PlayScore();
                foreach (ScoringSet s in scoreSet)
                {
                    if (Game.currentSettings.loseType == LoseType.BADCOLOR)
                    {
                        if (s.color == Game.currentSettings.dangerColor)
                            pendingResult = GameStopCause.LOSE_ERROR;
                    }                    
                    s.CalculateScore();
                    s.LogScore();
                    currentScore += s.score;
                    scoreList.Add(s);
                }
                if (Matcher.AllGray(puzzleBox, masterGrid))
                {
                    if (Game.currentSettings.mode == GameMode.Tutorial)
                    {
                        TutorialStage.phase = TutorialPhase.Pass;
                        return GameStopCause.TUTORIAL_PASS;
                    }
                    return GameStopCause.WIN;
                }
                maxSlideDistance = Matcher.GetMaxReplaceDistance(puzzleBox, masterGrid);
                if (0 == maxSlideDistance)
                {
                    if (movesRemaining == 0)
                        return GameStopCause.END;
                        
                    if (!Matcher.HasValidMove(puzzleBox, masterGrid))
                    {
                        if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            return GameStopCause.LOSE_STUCK;
                        }
                        else if (Game.currentSettings.mode == GameMode.Tutorial)
                        {
                            return GameStopCause.TUTORIAL_FAIL;
                        }
                        else
                        {
                            Matcher.Reset(puzzleBox, masterGrid);
                            gameState = State.VANISH;
                        }
                    }
                    else
                    {
                        Matcher.UpdateMoveCountdown(puzzleBox, masterGrid);
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
                if (pendingResult != GameStopCause.NONE)
                    return pendingResult;
                for (int x = 0; x < gridSize; x++)
                {
                    for (int y = 0; y < gridSize; y++)
                    {
                        if (masterGrid[x, y].marked)
                        {
                            fragmentList.Add(new Fragment(masterGrid[x, y].screenX, masterGrid[x, y].screenY, Game.screenSizeX, Game.screenSizeY, masterGrid[x, y].scale, masterGrid[x, y].color));
                        }
                    }
                }
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        if (puzzleBox[0, y, z].marked)
                        {
                            fragmentList.Add(new Fragment(puzzleBox[0, y, z].screenX, puzzleBox[0, y, z].screenY, Game.screenSizeX, Game.screenSizeY, puzzleBox[0, y, z].scale, puzzleBox[0, y, z].color));
                        }
                    }
                }
                // Regenerate orbs
                Matcher.Replace(puzzleBox, masterGrid);
                gameState = State.REGENERATE;
                animateTime = 0;
            }
            if (gameState == State.PAUSING && animateTime == 2 * maxAnimateTime)
            {
                gameState = State.RESUMING;
                animateTime = 0;
                SoundEffects.PlayMove();                
                return GameStopCause.PAUSE;                
            }
            if (gameState == State.RESUMING && animateTime == 2 * maxAnimateTime)
            {
                //Matcher.Reset(puzzleBox,masterGrid);
                Matcher.Clear(puzzleBox, masterGrid);
                gameState = State.READY;
                animateTime = 0;
                if (Game.currentSettings.mode == GameMode.Tutorial)
                {
                    return GameStopCause.TUTORIAL_TEXT;
                }
            }
            if (gameState == State.VANISH && animateTime == maxAnimateTime)
            {
                gameState = State.NEWSET;
                animateTime = 0;
            }
            if (gameState == State.NEWSET && animateTime == maxAnimateTime)
            {
                Matcher.Clear(puzzleBox, masterGrid);
                gameState = State.READY;
                animateTime = 0;
                if (Game.currentSettings.mode == GameMode.Tutorial)
                {
                    return GameStopCause.TUTORIAL_TEXT;
                }
            }
            if (gameState == State.REGENERATE && animateTime == maxAnimateTime * maxSlideDistance)
            {
                // Clear orbs                
                Matcher.Clear(puzzleBox, masterGrid);
                gameState = State.VERIFY;
            }

            if (gameState == State.READY)
            {
                if(mode!=ControlMode.EDITOR)               
                    Matcher.UpdateTimeCountdown(puzzleBox, masterGrid, gameTime.ElapsedGameTime.Milliseconds);
                if (mode == ControlMode.NORMAL)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.P) || GamePad.GetState(PlayerIndex.One).IsButtonDown(Buttons.Start))
                    {
                        gameState = State.PAUSING;
                        savedShift = shift;
                        animateTime = 0;                        
                    }
                }
                if (mode == ControlMode.NORMAL || mode == ControlMode.EDITOR)
                {
                    Vector2 stick = GamePad.GetState(PlayerIndex.One).ThumbSticks.Left;
                    GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

                    if (Keyboard.GetState().IsKeyDown(Keys.B) || gamePadState.IsButtonDown(Buttons.B))
                    {
                        if (Game.currentSettings.mode == GameMode.Puzzle)
                        {
                            if (prevPuzzleBox == null)
                                SoundEffects.PlayClick();
                            else
                                Back();
                        }
                    }
                    if ((Keyboard.GetState().IsKeyDown(Keys.E) || gamePadState.IsButtonDown(Buttons.X)))
                    {
                        if (cooldown == 0)
                        {
                            Game.gameSettings.soundEffectsEnabled = !Game.gameSettings.soundEffectsEnabled;
                            cooldown = 250;
                        }
                    }
                    if (cooldown==0 && (Keyboard.GetState().IsKeyDown(Keys.M) || gamePadState.IsButtonDown(Buttons.Y)))
                    {
                        if (Game.gameSettings.musicEnabled)
                        {
                            Game.gameSettings.musicEnabled = false;
                            MusicControl.Stop();
                        }
                        else
                        {
                            Game.gameSettings.musicEnabled = true;
                            MusicControl.PlayGameMusic();
                        }
                        cooldown = 250;
                    }
                    if(cooldown==0 && (Keyboard.GetState().IsKeyDown(Keys.OemPlus) || gamePadState.IsButtonDown(Buttons.Back)))
                    {
                        Game.gameSettings.displayControls = !Game.gameSettings.displayControls;
                        cooldown = 250;
                    }
                    if ((TutorialStage.phase == TutorialPhase.None || TutorialStage.restrictions == ControlRestrictions.None || TutorialStage.restrictions == ControlRestrictions.StickOnly))
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Left) || stick.X < -.95f)
                        {
                            SoundEffects.PlayMove();
                            prevMasterGrid = masterGrid.Copy();
                            prevPuzzleBox = puzzleBox.Copy();
                            prevCubeDistance = cubeDistance;
                            gameState = State.ROTATEPOSY;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Right) || stick.X > .95f)
                        {
                            SoundEffects.PlayMove();
                            prevMasterGrid = masterGrid.Copy();
                            prevPuzzleBox = puzzleBox.Copy();
                            prevCubeDistance = cubeDistance;
                            gameState = State.ROTATENEGY;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Up) || stick.Y > .95f)
                        {
                            SoundEffects.PlayMove();
                            prevMasterGrid = masterGrid.Copy();
                            prevPuzzleBox = puzzleBox.Copy();
                            prevCubeDistance = cubeDistance;
                            gameState = State.ROTATEPOSX;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Down) || stick.Y < -.95f)
                        {
                            SoundEffects.PlayMove();
                            prevMasterGrid = masterGrid.Copy();
                            prevPuzzleBox = puzzleBox.Copy();
                            prevCubeDistance = cubeDistance;
                            gameState = State.ROTATENEGX;
                        }
                    }
                    if (TutorialStage.phase==TutorialPhase.None || TutorialStage.restrictions == ControlRestrictions.None || TutorialStage.restrictions == ControlRestrictions.ShouldersOnly)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.S) || gamePadState.IsButtonDown(Buttons.RightShoulder))
                        {
                            SoundEffects.PlayMove();
                            prevMasterGrid = masterGrid.Copy();
                            prevPuzzleBox = puzzleBox.Copy();
                            prevCubeDistance = cubeDistance;
                            gameState = State.ROTATEPOSZ;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.A) || gamePadState.IsButtonDown(Buttons.LeftShoulder))
                        {
                            SoundEffects.PlayMove();
                            prevMasterGrid = masterGrid.Copy();
                            prevPuzzleBox = puzzleBox.Copy();
                            prevCubeDistance = cubeDistance;
                            gameState = State.ROTATENEGZ;
                        }
                    }
                    if (TutorialStage.phase == TutorialPhase.None || TutorialStage.restrictions == ControlRestrictions.None || TutorialStage.restrictions == ControlRestrictions.TriggersOnly)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.Q) || gamePadState.IsButtonDown(Buttons.LeftTrigger) || gamePadState.IsButtonDown(Buttons.LeftTrigger))
                        {
                            if (cubeDistance < spacing * 2)
                            {
                                SoundEffects.PlayMove();
                                prevMasterGrid = masterGrid.Copy();
                                prevPuzzleBox = puzzleBox.Copy();
                                prevCubeDistance = cubeDistance;
                                cubeDistanceGoal = cubeDistance + spacing;
                                gameState = State.PUSH;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.W) || gamePadState.IsButtonDown(Buttons.RightTrigger) || gamePadState.IsButtonDown(Buttons.RightTrigger))
                        {
                            if (cubeDistance > 0)
                            {
                                SoundEffects.PlayMove();
                                prevMasterGrid = masterGrid.Copy();
                                prevPuzzleBox = puzzleBox.Copy();
                                prevCubeDistance = cubeDistance;
                                cubeDistanceGoal = cubeDistance - spacing;
                                gameState = State.PULL;
                            }
                        }
                    }
                }
                if (mode == ControlMode.AUTOMATED)
                {
                    int x = automator.Next(0, 8);
                    if (x == 0) gameState = State.ROTATEPOSY;
                    if (x == 1) gameState = State.ROTATEPOSZ;
                    if (x == 2) gameState = State.ROTATEPOSX;
                    if (x == 3) gameState = State.ROTATENEGX;
                    if (x == 4) gameState = State.ROTATENEGY;
                    if (x == 5) gameState = State.ROTATENEGZ;
                    if (x == 6)
                    {
                        if (cubeDistance > 0)
                        {
                            cubeDistanceGoal = cubeDistance - spacing;
                            gameState = State.PULL;
                        }
                    }
                    if (x == 7)
                    {
                        if (cubeDistance < spacing * 2)
                        {
                            cubeDistanceGoal = cubeDistance + spacing;
                            gameState = State.PUSH;
                        }
                    }
                }
                if (mode == ControlMode.EDITOR)
                {
                    editorCooldown-=gameTime.ElapsedGameTime.Milliseconds;
                    if (editorCooldown < 0) editorCooldown = 0;
                    if (editorCooldown == 0)
                    {
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad1))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 2, 0];
                            selectedQueue = null;
                            selectedNode.selected = true;

                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad2))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 2, 1];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad3))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 2, 2];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad4))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 1, 0];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad5))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 1, 1];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad6))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 1, 2];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad7))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 0, 0];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad8))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 0, 1];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.NumPad9))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = puzzleBox[puzzleBox.activeZ, 0, 2];
                            selectedQueue = null;
                            selectedNode.selected = true;
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.Insert))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[1, 0];
                            selectedQueue = masterGrid.queues[1, 0]; ;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Home))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[2, 0];
                            selectedQueue = masterGrid.queues[2, 0]; ;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.PageUp))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[3, 0];
                            selectedQueue = masterGrid.queues[3, 0]; ;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Delete))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[1, 4];
                            selectedQueue = masterGrid.queues[1, 4]; ;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.End))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[2, 4];
                            selectedQueue = masterGrid.queues[2, 4]; ;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.PageDown))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[3, 4];
                            selectedQueue = masterGrid.queues[3, 4]; ;
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemCloseBrackets))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[4, 1];
                            selectedQueue = masterGrid.queues[4, 1];
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemQuotes))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[4, 2];
                            selectedQueue = masterGrid.queues[4, 2];
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemQuestion))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[4, 3];
                            selectedQueue = masterGrid.queues[4, 3];
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemOpenBrackets))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[0, 1];
                            selectedQueue = masterGrid.queues[0,1];
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemSemicolon))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[0, 2];
                            selectedQueue = masterGrid.queues[0, 2];
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemPeriod))
                        {
                            puzzleBox.ClearSelection();
                            masterGrid.ClearSelection();
                            selectedNode = masterGrid[0, 3];
                            selectedQueue = masterGrid.queues[0, 3];
                            selectedNode.selected = true;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.M))
                        {
                            if (selectedQueue != null)
                            {
                                puzzleBox.ClearSelection();
                                masterGrid.ClearSelection();
                                selectedDepth++;
                                if (selectedDepth >= selectedQueue.Count)
                                    selectedDepth = selectedQueue.Count - 1;
                                selectedNode = selectedQueue[selectedDepth];
                                selectedNode.selected = true;
                                editorCooldown = 250;
                            }
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.N))
                        {
                            if (selectedQueue != null)
                            {
                                puzzleBox.ClearSelection();
                                masterGrid.ClearSelection();
                                selectedDepth--;
                                if (selectedDepth < 0) selectedDepth = 0;
                                selectedNode = selectedQueue[selectedDepth];
                                selectedNode.selected = true;
                                editorCooldown = 250;
                            }
                        }
                         
                        if (Keyboard.GetState().IsKeyDown(Keys.R))
                        {
                            selectedNode.color = Color.Red;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.G))
                        {
                            selectedNode.color = Color.Green;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.B))
                        {
                            selectedNode.color = Color.Blue;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.Y))
                        {
                            selectedNode.color = Color.Yellow;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.P))
                        {
                            selectedNode.color = Color.Magenta;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.O))
                        {
                            selectedNode.color = Color.DarkOrange;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.X))
                        {
                            selectedNode.color = Color.Gray;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.F10))
                        {
                            LevelLoader.SaveLevel(puzzleBox, masterGrid);
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.F5))
                        {
                            mode = ControlMode.NORMAL;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.F))
                        {
                            if (selectedNode.toggleOrb == false)
                            {
                                selectedNode.toggleColor = Color.Gray;
                            }
                            selectedNode.toggleOrb = true;
                        }
                        
                        if (Keyboard.GetState().IsKeyDown(Keys.C))
                        {
                            selectedNode.moveCountdownOrb = true;
                            selectedNode.countdown = 10;
                        }
                        if(Keyboard.GetState().IsKeyDown(Keys.LeftShift)) {
                            if(Keyboard.GetState().IsKeyDown(Keys.F))
                            {   
                                if (selectedNode.toggleOrb)
                                {
                                    Color temp = selectedNode.color;
                                    selectedNode.color = selectedNode.toggleColor;
                                    selectedNode.toggleColor = temp;
                                    editorCooldown = 250;
                                }
                            }
                        }

                        if (Keyboard.GetState().IsKeyDown(Keys.T))
                        {
                            selectedNode.timeCountdownOrb = true;
                            selectedNode.countdown = 10000;                        
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
                        {
                            if(selectedNode.moveCountdownOrb)
                                selectedNode.countdown++;
                            if (selectedNode.timeCountdownOrb)
                                selectedNode.countdown += 1000;
                            editorCooldown = 250;
                        }
                        if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
                        {
                            if (selectedNode.moveCountdownOrb)
                                selectedNode.countdown--;
                            if (selectedNode.timeCountdownOrb)
                                selectedNode.countdown -= 1000; 
                            editorCooldown = 250;
                        }
                    }


                }
            }

            // Update fragment animation positions
            for (int i = 0; i < fragmentList.Count; i++)
            {
                if (false == fragmentList[i].Update(gameTime))
                {
                    fragmentList.RemoveAt(i);
                }
            }
            // Update scoring animation positions
            for (int i = 0; i < scoreList.Count; i++)
            {
                if (false == scoreList[i].Update(gameTime))
                {
                    scoreList.RemoveAt(i);
                }
            }
            return GameStopCause.NONE;

        }

        public void Draw(GameTime gameTime)
        {
            
            OrbRenderer.DrawBackground();

            // Calculate tilt data
            if (gameState != State.PAUSING && gameState != State.RESUMING && GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Length() > .95f)
            {
                //shift = new Vector2(Game.screenCenterX - Mouse.GetState().X, Game.screenCenterY - Mouse.GetState().Y);
                targetShift = new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X * Game.screenCenterX,
                    -GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y * Game.screenCenterY);
                if (shift != targetShift)
                {
                    Vector2 shiftDelta = targetShift - shift;
                    if (shiftDelta.Length() > 100)
                    {
                        shiftDelta.Normalize();
                        shiftDelta *= 100;
                    }
                    shift += shiftDelta;
                }
            }
            else
            {
                if (gameState == State.PAUSING)
                {
                    if (animateTime < maxAnimateTime)
                    {
                        float f = (1f - ((float)animateTime / maxAnimateTime));
                        shift.X = savedShift.X * f;
                        shift.Y = (savedShift.Y + 348) * f - 348;
                    }
                    else
                        shift = new Vector2(0, -348);
                }
                if (gameState == State.RESUMING)
                {
                    if (animateTime > maxAnimateTime)
                    {
                        float f = (float)(animateTime - maxAnimateTime) / maxAnimateTime;
                        // Mouse Camera
                        //int targetX = (Game.screenCenterX - Mouse.GetState().X);
                        //int targetY = (Game.screenCenterY - Mouse.GetState().Y);
                        // Gamepad Camera                       
                        if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Length() > .95)
                        {
                            targetShift = new Vector2(GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.X * Game.screenCenterX,
                                -GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y * Game.screenCenterY);
                            savedShift = targetShift;
                        }
                        else
                            targetShift = savedShift;
                        shift.X = (targetShift.X) * f;
                        shift.Y = (targetShift.Y + 348) * f - 348;
                    }
                    else
                        shift = new Vector2(0, -348);
                }
            }

            if (shift.Length() > (1.0f * Game.screenSizeX) / 4f * tiltScale)
            {
                shift.Normalize();
                shift = shift * (1.0f * Game.screenSizeX) / 4f * tiltScale;
            }
            
            int vortexShiftX = (int)(shift.X / Game.screenSizeX * -90f);
            int vortexShiftY = (int)(shift.Y / Game.screenSizeX * -90f);

            int opacity = 100;
            if (gameState == State.PAUSING)
            {
                opacity = 100-(int)(100f * animateTime / 2f/maxAnimateTime);
            }
            if (gameState == State.RESUMING)
            {
                opacity = (int)(100f * animateTime / 2f / maxAnimateTime);
            }

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                    {
                        if (masterGrid.queues[x, y] != null)
                        {
                            for (int z = 0; z < 40; z++)
                            {
                                float distance = baseDistance - 5 * (z + 1);
                                int screenX = Game.screenCenterX + (x - centerGridIndex) * spacing + (int)(shift.X / Game.screenSizeX * distance);
                                int screenY = Game.screenCenterY + (y - centerGridIndex) * spacing + (int)(shift.Y / Game.screenSizeX * distance);
                                int modifier = (int)(Math.Sqrt(baseDistance - distance) * spacing / 10);
                                if (x > centerGridIndex)
                                    screenX += modifier;
                                if (x < centerGridIndex)
                                    screenX -= modifier;
                                if (y > centerGridIndex)
                                    screenY += modifier;
                                if (y < centerGridIndex)
                                    screenY -= modifier;

                                float scaleModifier = 1f;
                                if (baseDistance > distance)
                                    scaleModifier = 90 / (75 + baseDistance - distance);
                                JellyfishRenderer.DrawTransparentRing(screenX, screenY, Math.Min(opacity,10), scaleModifier);                                                                
                            }
                        }
                    }
                }
            }

            #region RotationControls
            switch (gameState)
            {
                case State.PULL:
                    cubeDistance -= 5;
                    if (cubeDistance <= cubeDistanceGoal)
                    {
                        cubeDistance = cubeDistanceGoal;
                        puzzleBox.activeZ--;
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.PUSH:
                    cubeDistance += 5;
                    if (cubeDistance >= cubeDistanceGoal)
                    {
                        cubeDistance = cubeDistanceGoal;
                        puzzleBox.activeZ++;
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.ROTATENEGY:
                    theta += rotateSpeed;
                    if (theta > Math.PI / 2)
                    {
                        theta = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSY);
                        animateTime = 0;
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.ROTATEPOSY:
                    theta -= rotateSpeed;
                    if (theta < -Math.PI / 2)
                    {
                        theta = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGY);
                        animateTime = 0;
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.ROTATENEGX:
                    phi += rotateSpeed;
                    if (firstHalf && phi > Math.PI / 4)
                    {
                        phi = -(float)Math.PI / 4;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGZ);
                        firstHalf = false;
                    }
                    if (!firstHalf && phi > 0)
                    {
                        phi = 0;
                        firstHalf = true;
                        animateTime = 0;
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.ROTATEPOSX:
                    phi -= rotateSpeed;
                    if (firstHalf && phi < -Math.PI / 4)
                    {
                        phi = (float)Math.PI / 4;
                        firstHalf = false;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSZ);
                    }
                    if (!firstHalf && phi < 0)
                    {
                        phi = 0;
                        firstHalf = true;
                        animateTime = 0;
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.ROTATENEGZ:
                    psi -= rotateSpeed;
                    if (psi < -Math.PI / 2)
                    {
                        psi = (float)-Math.PI / 2;
                        animateTime = 0;
                        psi = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.POSX);
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                case State.ROTATEPOSZ:
                    psi += rotateSpeed;
                    if (psi > Math.PI / 2)
                    {
                        psi = (float)Math.PI / 2;
                        animateTime = 0;
                        psi = 0;
                        puzzleBox.Rotate(PuzzleBox.ROTATION.NEGX);
                        gameState = State.MOVECOMPLETE;
                    }
                    break;
                default:
                    break;
            }
            #endregion

            #region CameraProcessing
            cameraBasePos = new Vector3(cameraDistance * (float)Math.Cos(theta) * (float)Math.Sin(Math.PI / 2 - phi),
                cameraDistance * (float)Math.Cos(Math.PI / 2 - phi),
                cameraDistance * (float)Math.Sin(theta) * (float)Math.Sin(Math.PI / 2 - phi));

            v = (Vector3.Zero - cameraBasePos);
            u_0 = Vector3.Cross(v, Vector3.Cross(new Vector3(0f, 1f, 0f), v));
            u_0.Normalize();
            v.Normalize();
            left = Vector3.Cross(u_0, v);
            left.Normalize();
            u = (float)Math.Sin(psi) * left + (float)Math.Cos(psi) * u_0;
            u.Normalize();
            left = Vector3.Cross(u, v);
            left.Normalize();
            if (gameState == State.READY)
                baseDistance = CameraUtils.GetDistance(v, cameraBasePos, new Vector3(-spacing, 0, 0));

            #endregion

            #region AddOrbsToZBuffer
            zBuffer = new List<PuzzleNode>();
            for (int x = 0; x < boxSize; x++)
            {
                for (int y = 0; y < boxSize; y++)
                {
                    for (int z = 0; z < boxSize; z++)
                    {
                        float animatedX = (float)x;
                        float animatedZ = (float)z;
                        float animatedY = (float)y;
                        if (gameState == State.REGENERATE)
                        {
                            animatedZ = OrbRenderer.GetSlideX(z, puzzleBox[x, y, z], 1f * animateTime / maxAnimateTime);
                            animatedY = OrbRenderer.GetSlideY(y, puzzleBox[x, y, z], 1f * animateTime / maxAnimateTime);
                        }
                        //if (!(animatedY < -2f || animatedY > 4f || animatedZ < -2f || animatedZ > 4f))
                        {
                            Vector3 p = new Vector3((animatedX - centerBoxIndex) * spacing, (animatedY - centerBoxIndex) * spacing, (animatedZ - centerBoxIndex) * spacing);
                            int distance = (int)CameraUtils.GetDistance(v, cameraBasePos, p) + cubeDistance;
                            puzzleBox[x, y, z].screenX = Game.screenCenterX + CameraUtils.GetScreenX(v, cameraBasePos, u, p) + (int)(shift.X / Game.screenSizeX * distance);
                            puzzleBox[x, y, z].screenY = Game.screenCenterY + CameraUtils.GetScreenY(v, cameraBasePos, u, p) + (int)(shift.Y / Game.screenSizeX * distance);
                            puzzleBox[x, y, z].distance = distance;
                            puzzleBox[x, y, z].scale = scale;

                            zBuffer.Add(puzzleBox[x, y, z]);
                        }
                    }
                }
            }
            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    // Avoid rendering orbs in corners and center.
                    if ((x < boxOffset && y < boxOffset) ||
                        (x < boxOffset && y >= boxOffset + boxSize) ||
                        (x >= boxOffset + boxSize && y < boxOffset) ||
                        (x >= boxOffset + boxSize && y >= boxOffset + boxSize))
                        continue;
                    if (x >= boxOffset && x < boxOffset + boxSize && y >= boxOffset && y < boxOffset + boxSize)
                        continue;

                    {
                        float animatedX = (float)x;
                        float animatedY = (float)y;
                        if (gameState == State.REGENERATE)
                        {
                            animatedX = OrbRenderer.GetSlideX(x, masterGrid[x, y], 1f * animateTime / maxAnimateTime);
                            animatedY = OrbRenderer.GetSlideY(y, masterGrid[x, y], 1f * animateTime / maxAnimateTime);
                        }
                        //if (!(animatedX > 6f || animatedX < 0f || animatedY < 0f || animatedY > 6f))
                        {
                            masterGrid[x, y].screenX = (int)(Game.screenCenterX + (animatedX - centerGridIndex) * spacing + (shift.X / Game.screenSizeX * baseDistance));
                            masterGrid[x, y].screenY = (int)(Game.screenCenterY + (animatedY - centerGridIndex) * spacing + (shift.Y / Game.screenSizeX * baseDistance));
                            masterGrid[x, y].distance = baseDistance;
                            masterGrid[x, y].scale = scale;
                            zBuffer.Add(masterGrid[x, y]);
                        }
                    }
                    if (x == 0 || x == gridSize - 1 || y == 0 || y == gridSize - 1)
                    {
                        if (masterGrid.queues[x, y] != null)
                        {
                            for (int z = 0; z < Math.Min(masterGrid.queues[x, y].Count, 50); z++)
                            {
                                float distance = baseDistance - 40 * (z + 1);
                                if (masterGrid[x, y].marked && gameState == State.REGENERATE)
                                {
                                    distance = distance + 40 * Math.Min(1f * animateTime / maxAnimateTime, masterGrid[x,y].replace_distance);
                                }
                                if (distance <= baseDistance && distance >= baseDistance - 40 * 5)
                                {
                                    masterGrid.queues[x, y][z].screenX = Game.screenCenterX + (x - centerGridIndex) * spacing + (int)(shift.X / Game.screenSizeX * distance);
                                    masterGrid.queues[x, y][z].screenY = Game.screenCenterY + (y - centerGridIndex) * spacing + (int)(shift.Y / Game.screenSizeX * distance);
                                    int modifier = (int)(Math.Sqrt(baseDistance - distance) * spacing / 10);
                                    if (x > centerGridIndex)
                                        masterGrid.queues[x, y][z].screenX += modifier;
                                    if (x < centerGridIndex)
                                        masterGrid.queues[x, y][z].screenX -= modifier;
                                    if (y > centerGridIndex)
                                        masterGrid.queues[x, y][z].screenY += modifier;
                                    if (y < centerGridIndex)
                                        masterGrid.queues[x, y][z].screenY -= modifier;

                                    float scaleModifier = 1f;
                                    if (baseDistance > distance)
                                        scaleModifier = 90 / (75 + baseDistance - distance);


                                    masterGrid.queues[x, y][z].distance = distance;
                                    masterGrid.queues[x, y][z].scale = scale * scaleModifier;
                                    zBuffer.Add(masterGrid.queues[x, y][z]);
                                }
                            }
                        }
                    }
                }
            }
            #endregion

            #region OrbRendering
            zBuffer.Sort();
            foreach (PuzzleNode p in zBuffer)
            {
                OrbRenderer.DrawOrb(p, gameState, 1f * animateTime / maxAnimateTime);
            }
            #endregion

            #region EffectsRendering
            foreach (Fragment f in fragmentList)
            {
                OrbRenderer.DrawFragments(f);
            }

            if (Game.currentSettings.displayScore)
            {
                foreach (ScoringSet s in scoreList)
                {
                    OrbRenderer.DrawScoreBonus(s);
                }
            }
            #endregion

            String message = "Score: " + currentScore;

            if (Game.currentSettings.displayScore)            
                Game.spriteBatch.DrawString(Game.spriteFont, message, new Vector2(850,665), Color.LightGreen);

            JellyfishRenderer.DrawTransparentJellyfish(Game.screenCenterX + (int)(shift.X / Game.screenSizeX * baseDistance), jellyBodyY + (int)(shift.Y / Game.screenSizeX * baseDistance), 10, 1f);

            if (gameState == State.PAUSING && animateTime > maxAnimateTime)
            {
                opacity = (int)((100f * (animateTime-maxAnimateTime)) / maxAnimateTime);
                JellyfishRenderer.DrawJellyfish(jellyX, jellyY, opacity,Game.currentSettings.texture,1f);
            }
            if (gameState == State.RESUMING && animateTime < maxAnimateTime)
            {
                opacity = (int)((100f * (animateTime)) / maxAnimateTime);
                JellyfishRenderer.DrawJellyfish(jellyX, jellyY, 100 - opacity, Game.currentSettings.texture,1f);
            }

            timer.Draw();
            clock.Draw();
            if (Game.currentSettings.mode == GameMode.MoveChallenge)
            {
                if(movesRemaining > 10)
                    Game.spriteBatch.DrawString(Game.spriteFont, string.Format("MOVES: {0}", movesRemaining), new Vector2(850, 640), Color.White);
                else
                    Game.spriteBatch.DrawString(Game.spriteFont, string.Format("MOVES: {0}", movesRemaining), new Vector2(850, 640-5), Color.Red,0f,Vector2.Zero,1.3f,SpriteEffects.None,0);
            }

            // Draw controls info
        }
    }
}
