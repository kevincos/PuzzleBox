using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuzzleBox
{

    public enum TutorialPhase
    {
        None,
        Intro,
        Pass,
        Fail
    }

    public enum ControlRestrictions
    {
        None,
        StickOnly,
        TriggersOnly,
        ShouldersOnly
    }

    class TutorialStage
    {
        public static TutorialPhase phase = TutorialPhase.None ;
        //Intro Text
        public static String introText = "Intro1:Intro2";
        //Success Text
        public static String successText = "Success1:Success2";
        //Failure Text
        public static String failureText = "Failure1:Failure2";

        public static int introIndex = 0;
        public static int successIndex = 0;
        public static int failureIndex = 0;

        public static bool active = true;
        public static bool loaded = false;
        public static int lessonIndex = 0;
        public static int maxLesson = 10;

        public static ControlRestrictions restrictions = ControlRestrictions.None;

        public static String IntroText()
        {
            String[] parts = introText.Split(':');
            if (introIndex >= parts.Count())
                return null;
            introIndex++;
            return parts[introIndex - 1];
        }

        public static String SuccessText()
        {
            String[] parts = successText.Split(':');
            if (successIndex >= parts.Count())
                return null;
            successIndex++;
            return parts[successIndex - 1];
        }

        public static String FailureText()
        {
            String[] parts = failureText.Split(':');
            if (failureIndex >= parts.Count())
                return null;

            failureIndex++;
            return parts[failureIndex - 1];
        }

        public static bool IsEndOfSection()
        {
            if (phase == TutorialPhase.Fail)
            {
                String[] parts = failureText.Split(':');
                return failureIndex == parts.Count();
            }
            if (phase == TutorialPhase.Pass)
            {
                String[] parts = successText.Split(':');
                return successIndex == parts.Count();
            }
            return false;
        }

        //Puzzle Load
        public static void LoadLesson(int index, PuzzleBox box, MasterGrid grid)
        {
            active = true;
            introIndex = 0;
            successIndex = 0;
            failureIndex = 0;
            phase = TutorialPhase.Intro;
            switch (index)
            {
                case(0):
                    Lesson1(box, grid);
                    break;
                case(1):
                    Lesson2(box, grid);
                    break;
                case (2):
                    Lesson3(box, grid);
                    break;
                case (3):
                    Lesson4(box, grid);
                    break;
                case (4):
                    Lesson5(box, grid);
                    break;
                case (5):
                    Lesson6(box, grid);
                    break;
                case (6):
                    Lesson7(box, grid);
                    break;
                case (7):
                    Lesson8(box, grid);
                    break;
                case (8):
                    Lesson9(box, grid);
                    break;
                case (9):
                    Lesson10(box, grid);
                    break;
                default:
                    return;
            }
            if (loaded == true)
            {
                Mark(box, grid);
            }
            else
                loaded = true;

        }

        public static void Clear(PuzzleBox box, MasterGrid grid)
        {
            box.Blank();
            grid.Blank();
        }

        public static void Mark(PuzzleBox box, MasterGrid grid)
        {
            box.Mark();
            grid.Mark();
        }

        public static void Lesson0(PuzzleBox box, MasterGrid grid)
        {
            Clear(box, grid);
            box[1, 2, 2] = new PuzzleNode(Color.Red);
            box[0, 2, 2] = new PuzzleNode(Color.Red);
            grid[3,0] = new PuzzleNode(Color.Red);            
        }

        public static void Lesson1(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "Look at all these bubbles inside Mister\n"+
            "Jellyfish! The colored ones are toxic! Lets\n" +
            "see if we can get rid of them.:"+
            "Use the Left Control Stick to rotate the\n" +
            "bubbles in Mister Jellyfish's body.:" +
            "Try to match up the colored bubbles in\n" +
            "his body with the bubbles in his\n" +
            "tentacles!";
            successText = "" +
            "Great job! When you aligned the bubbles in\n" +
            "his body with the bubbles in his tentacles,\n" +
            "the bubbles popped!";
            Clear(box, grid);
            restrictions = ControlRestrictions.StickOnly;
            grid[0, 1] = new PuzzleNode(Color.Yellow);
            grid[3, 4] = new PuzzleNode(Color.Green);
            grid.queues[0, 1][1] = new PuzzleNode(Color.Magenta);
            grid.queues[0, 1][0] = new PuzzleNode(Color.Red);
            grid.queues[3, 4][1] = new PuzzleNode(Color.Red);
            grid.queues[3, 4][0] = new PuzzleNode(Color.Magenta);
            grid[2, 4] = new PuzzleNode(Color.Orange);
            grid.queues[2, 4][1] = new PuzzleNode(Color.Blue);
            box[0, 0, 2] = new PuzzleNode(Color.Yellow);
            box[0, 0, 0] = new PuzzleNode(Color.Green);
            box[0, 1, 2] = new PuzzleNode(Color.Orange);
            box[0, 1, 0] = new PuzzleNode(Color.Blue);
        }


        public static void Lesson2(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "Instead of using the Left Control Stick, try\n" +
            "using the Left and Right Shoulder Buttons.:" +
            "This will perform a different type of\n" +
            "rotation. Use the Shoulder Buttons to match\n" +
            "the bubbles!";
            successText = "" +
            "Nice! If you can master all 6 types of these\n" +
            "rotations, you'll do great!";
            restrictions = ControlRestrictions.ShouldersOnly;
            Clear(box,grid);
            grid[0, 1] = new PuzzleNode(Color.Yellow);
            grid[3, 4] = new PuzzleNode(Color.Green);
            grid.queues[0, 1][1] = new PuzzleNode(Color.Magenta);
            grid.queues[0, 1][0] = new PuzzleNode(Color.Red);
            grid.queues[3, 4][1] = new PuzzleNode(Color.Red);
            grid.queues[3, 4][0] = new PuzzleNode(Color.Magenta);
            grid[2, 4] = new PuzzleNode(Color.Orange);
            grid.queues[2, 4][1] = new PuzzleNode(Color.Blue);
            box[0, 0, 2] = new PuzzleNode(Color.Yellow);
            box[0, 0, 0] = new PuzzleNode(Color.Green);
            box[0, 1, 2] = new PuzzleNode(Color.Orange);
            box[0, 1, 0] = new PuzzleNode(Color.Blue);

        }

        public static void Lesson3(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "In addition to rotations, you can use the Left\n" +
            "Right Triggers to push and pull the bubbles in\n" +
            "the Jellyfish's body.:" +
            "That way, you can change which layer of bubbles\n" +
            "aligns with the tentacles.:" +
            "Also, you can use the Right Control Stick at any\n" +
            "time to shift the camera angle.:" +
            "I like to set the camera so that all 27 bubbles\n" +
            "in the body are visible at the same time.:" +
            "But you should do whatever feels comfortable!\n"+
            "Now, use the Left and Right Triggers to match\n"+
            "the bubbles!";
            successText = "" +
            "Great! With the Left Control Stick, Shoulder\n" +
            "Buttons, and Triggers, you can reorient the\n" +
            "bubbles however you want!:"+
            "If you need a reminder of the controls, take a\n" +
            "look around the top and bottom left of the\n"+
            "of the screen for a review.";
            Clear(box, grid);
            restrictions = ControlRestrictions.TriggersOnly;
            grid[0, 1] = new PuzzleNode(Color.Red);
            grid.queues[0, 1][1] = new PuzzleNode(Color.Magenta);
            grid[3, 0] = new PuzzleNode(Color.Yellow);
            grid[4, 3] = new PuzzleNode(Color.Blue);            
            grid[1, 4] = new PuzzleNode(Color.Green);
            box[0, 0, 0] = new PuzzleNode(Color.Magenta);
            box[1, 0, 0] = new PuzzleNode(Color.Red);
            box[2, 2, 0] = new PuzzleNode(Color.Green);
            box[1, 0, 2] = new PuzzleNode(Color.Yellow);
            box[2, 2, 2] = new PuzzleNode(Color.Blue);
        }

        public static void Lesson4(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "Now let's try and put it together! To match\n" +
            "these bubbles, you'll need to use the Left and\n" +
            "Right Triggers to push and pull them.:" +
            "But, you'll also need to use the Left Control\n" +
            "Stick and Shoulder Buttons to rotate them!:" +
            "Try and match all the bubbles!";
            successText = "" +
            "Way to go! You're getting the hang of it!\n" +
            "Let's talk about some more advanced topics!";
            failureText = "BUG!";
            Clear(box, grid);
            restrictions = ControlRestrictions.None;
            grid[0, 1] = new PuzzleNode(Color.Green);
            grid[2, 0] = new PuzzleNode(Color.Orange);
            grid[4, 2] = new PuzzleNode(Color.Magenta);
            grid[3, 4] = new PuzzleNode(Color.Blue);
            grid[4, 1] = new PuzzleNode(Color.Yellow);
            grid[1, 4] = new PuzzleNode(Color.Red);
            box[0, 0, 0] = new PuzzleNode(Color.Red);
            box[2, 2, 2] = new PuzzleNode(Color.Yellow);
            box[1, 2, 2] = new PuzzleNode(Color.Magenta);
            box[0, 2, 0] = new PuzzleNode(Color.Blue);
            box[0, 0, 1] = new PuzzleNode(Color.Green);
            box[2, 1, 1] = new PuzzleNode(Color.Orange);
        }

        public static void Lesson5(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "This next lesson is tricky. Notice the two\n" +
            "yellow bubbles in Mister Jellyfish's body?:" +
            "They're right next to each other, but they\n" +
            "haven't popped!:" +
            "That's because to pop bubbles, you need to\n" +
            "match bubbles from the Jellyfish's body with\n" +
            "the bubbles in the tentacles.:" +
            "Try to align these two yellow bubbles with the\n" +
            "yellow bubble in the tentacle to form a\n"+
            "straight line.";
            successText = "" +
            "You got it. Remember, sets of bubbles will\n" +
            "only pop if they form a straight line. So be\n" +
            "careful!";
            failureText = "" +
            "Almost! Only sets of bubbles that form a\n" +
            "straight line will match.";
            Clear(box,grid);
            grid[0, 1] = new PuzzleNode(Color.Yellow);
            box[0, 2, 2] = new PuzzleNode(Color.Yellow);
            box[0, 2, 1] = new PuzzleNode(Color.Yellow);
            restrictions = ControlRestrictions.None;
        }

        public static void Lesson6(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "Scoring big sets is tricky. Lets practice some\n" +
            "more. Try to match all of the bubbles using sets\n" +
            "of 3 or more!";
            successText = "" +
            "Awesome! These combos are worth way more points\n" +
            "than just matching two bubbles at a time.:" +
            "If you want to be a great Jellyfish doctor,\n" +
            "you'll need to master spotting when you can\n" +
            "match these kinds of sets!";
            failureText = "" +
            "Oops! Remember, you need to rotate the bubbles\n" +
            "so that all of the same colored bubbles are in\n" +
            "a straight line!:" +
            "Use the Right Control Stick to look around and\n" +
            "get a better view of things!:"+
            "Finally, don't forget about using the Shoulder\n" +
            "Buttons for rotations.";
            Clear(box,grid);
            restrictions = ControlRestrictions.None;
            grid[1, 0] = new PuzzleNode(Color.Green);
            grid[0, 2] = new PuzzleNode(Color.Blue);
            grid[4, 3] = new PuzzleNode(Color.Yellow);
            box[2, 0, 2] = new PuzzleNode(Color.Green);
            box[2, 1, 2] = new PuzzleNode(Color.Green);
            box[2, 2, 2] = new PuzzleNode(Color.Green);
            box[1, 2, 0] = new PuzzleNode(Color.Blue);
            box[1, 2, 1] = new PuzzleNode(Color.Blue);
            box[0, 0, 2] = new PuzzleNode(Color.Yellow);
            box[0, 1, 2] = new PuzzleNode(Color.Yellow);


            
        }

        public static void Lesson7(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "Another advanced topic is bonus bubbles! You\n" +
            "may have noticed that when you match bubbles,\n" +
            "new ones slide in to take their place.:" +
            "But what happens if you match against two\n" +
            "bubbles in different tentacles at the same\n" +
            "time?:" +
            "This will create a bonus bubble of the same\n" +
            "color. Matching bonus bubbles will multiply\n" +
            "the score of the set by 2!:" +
            "Try rotating this orange bubble into the\n" +
            "lower left corner!";
            successText = "" +
            "Great! In addition to this bubble being\n" +
            "worth bonus points, you popped 3 outer\n" +
            "bubbles with just one inner bubble.:" +
            "That's a useful skill!";
            failureText = "" +
            "Oops! Try popping the bubbles in the lower\n" +
            "left corner first. This will give you a bonus\n" +
            "bubble!";
            restrictions = ControlRestrictions.None;
            Clear(box,grid);
            grid[0, 3] = new PuzzleNode(Color.Orange);
            grid[1, 4] = new PuzzleNode(Color.Orange);
            grid[4, 1] = new PuzzleNode(Color.Orange);
            box[0, 2, 2] = new PuzzleNode(Color.Orange);

        }

        public static void Lesson8(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "This technique will be especially important in\n" +
            "Challenge Mode. Try to match all the bubbles\n" +
            "here.:"+
            "To do so, you'll need to get the bonus bubbles\n" +
            "first again! Focus on the corners that contain\n"+
            "two bubbles of the same color.:"+
            "Also, don't forget the basics. Remember you can\n"+
            "use the left and right triggers to push and pull\n"+
            "the bubbles.";
            successText = "" +
            "Perfect! You're almost ready to get started\n" +
            "for real.";
            failureText = "" +
            "Oops! Remember, if you match one bubble against\n" +
            "two tentacles at the same time, it will be\n" +
            "replaced by a bonus bubble.:" +
            "You can reuse the bonus bubbles to make more\n" +
            "matches. Try getting the bonus bubbles first!" +            
            "to push and pull the bubbles.";
            restrictions = ControlRestrictions.None;
            Clear(box, grid);
            box[0, 1, 2] = new PuzzleNode(Color.Yellow);            
            box[0, 0, 0] = new PuzzleNode(Color.Magenta);
            grid[3, 0] = new PuzzleNode(Color.Magenta);
            grid[4, 1] = new PuzzleNode(Color.Magenta);
            grid[1, 4] = new PuzzleNode(Color.Magenta);
            grid[0, 1] = new PuzzleNode(Color.Yellow);
            grid[1, 0] = new PuzzleNode(Color.Yellow);
            grid[3, 4] = new PuzzleNode(Color.Yellow);
        }

        public static void Lesson9(PuzzleBox box, MasterGrid grid)
        {
            introText = "" +
            "Okay! Let's combine what we've learned about sets\n" +
            "and bonus bubbles. See those three green bubbles\n" +
            "in the upper right?:" +
            "If we can match our two green bubbles at the\n" +
            "bottom with them, we can pop 5 bubbles at once!:" +
            "They'll also get replaced by two bonus bubbles,\n" +
            "which we can use to get another set!";
            successText = "Nice! That was worth a lot of points!";
            failureText = "" +
            "Oops! This one is tricky. Don't forget to use\n"+
            "The shoulder buttons for rotations and the\n"+
            "triggers to push and pull the bubbles.:"+
            "Try and get match the bubbles in the upper\n"+
            "right first!";
            restrictions = ControlRestrictions.None;
            Clear(box, grid);
            grid[3, 0] = new PuzzleNode(Color.Green);
            grid[4, 1] = new PuzzleNode(Color.Green);
            grid[4, 2] = new PuzzleNode(Color.Green);
            grid[0, 2] = new PuzzleNode(Color.Green);
            box[1, 2, 1] = new PuzzleNode(Color.Green);
            box[1, 2, 2] = new PuzzleNode(Color.Green);
        }

        public static void Lesson10(PuzzleBox box, MasterGrid grid)
        {
            introText = "Almost done! Lets do one more.:" +
            "There are three blue bubbles in the back right \n" +
            "of Mister Jellyfish. There also 5 blue bubbles \n" +
            "in the right tentacles.:" +
            "If you can bring all three of those blue bubbles \n" +
            "to the front, you can get 3 bonus bubbles to use \n" +
            "for a second set!";
            successText = ""+
            "Super! Looks like you're ready to get to work.\n" +
            "First, lets go over a few more quick things!:"+
            "There are three different types of Jellyfish\n" +
            "surgery to choose from, each of which poses\n" +
            "unique challenges.:"+
            "In Emergency mode, you'll race against the clock\n"+
            "to pop as many bubbles as you can.:" + 
            "Scoring sets of 3 or more bubbles at a time is\n"+
            "key to getting a high score.:" +
            "Operation mode is similar, but you can relax and\n"+
            "take your time. However, you'll have a limited\n"+
            "number of moves to work with.:" +
            "Time isn't a factor, but you'll need to plan\n"+
            "your moves carefully and be efficient to get\n"+
            "a good score.:"+
            "Finally, Challenge mode will give you some\n"+
            "tricky puzzles to solve.:"+
            "You'll need to use what you've learned here to\n"+
            "pop all of the bubbles in each patient. If you\n"+
            "make a wrong move, you might get stuck!:"+
            "Luckily, in Challenge mode, you can press B to\n"+
            "undo your last move if you make a mistake.:"+
            "You'll be awarded a rating after each patient.\n"+
            "If you earn 3 stars, your patient will make a\n"+
            "full recovery and return to the sea!:"+
            "Try to save them all!\n";
            failureText = "" +
            "Oops! Try to rotate all three blue bubbles in\n" +
            "back to the front right. Then use the three\n" +
            "bonus bubbles to make a second set.:" +
            "Don't forget you can use the Right Control Stick\n" +
            "to get a better view of bubbles in the back!";
            restrictions = ControlRestrictions.None;
            Clear(box, grid);
            box[2, 0, 2] = new PuzzleNode(Color.Blue);
            box[2, 1, 2] = new PuzzleNode(Color.Blue);
            box[2, 2, 2] = new PuzzleNode(Color.Blue);
            grid[3, 0] = new PuzzleNode(Color.Blue);
            grid[3, 4] = new PuzzleNode(Color.Blue);
            grid[4, 1] = new PuzzleNode(Color.Blue);
            grid[4, 2] = new PuzzleNode(Color.Blue);
            grid[4, 3] = new PuzzleNode(Color.Blue);
            grid[1, 4] = new PuzzleNode(Color.Blue);            

        }
                
    }
}
