using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _3D_Rubiks_Cube
{
    class Program
    {
        static void Main(string[] args)
        {
            //Start a game loop using the main class, which acts as the main interface for the other classes interacting
            while (true)
            {
                Main main = new Main();
            }                        
        }
    }

    class Main
    {
        //Main is a class which acts as the main interface between the camera and the cube

        //declare an instance of the cube class, so that it can be assigned throughout main
        private Cube myCube;

        //As the Colours needs to passed into each camera class, there needs to be a variable declared of it at the start of the class.
        int[] MyColours = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
        bool Loop = true;

        public Main()
        {
            //instantiate the Cube
            myCube = new Cube();
            //Activate the "game" loop which keeps the program running forever, until it is exited
            CameraEnvironment();
        }     

        private void CameraEnvironment()
        {
            //Declare an instance of the Menu class, which displays the interface for the user
            Menu myMenu;
            while (true)
            {
                //Attempt to call a class with custom color values
                try { myMenu = new Menu(MyColours); }
                //if color values haven't been instantiated yet, call the class without the colours
                catch { myMenu = new Menu(); }

                //Option system, where you choose from the menu. Each one takes you to the loop environment of the corresponding choice
                if (myMenu.MenuSelection == 0)
                    ThreeDEnvironment_v3();
                else if (myMenu.MenuSelection == 1)
                    TwoDEnvironment();
                else if (myMenu.MenuSelection == 2)
                    InputEnvironment();
                else if (myMenu.MenuSelection == 3)
                    MyColours = myMenu.Settings();
                else if (myMenu.MenuSelection == 4)
                    Environment.Exit(0);
                else
                    myMenu.MenuSelection = 0;
            }
        }

        //the main environment for the two D cube. Where all the functions are called        
        private void TwoDEnvironment()
        {
            TwoDCamera myCamera;
            //instantiate the "camera" for a 2D cube, attempting to do it with custom colors
            try
            {
                myCamera = new TwoDCamera(MyColours);
            }
            catch
            {
                myCamera = new TwoDCamera();
            }
            //Draw the 2D cube to the screen buffer, and then print it.
            update2Dcube(myCamera, myCube);
            //Keep looking for keyboard inputs until 2D camera exited.
            Loop = true;
            while (Loop)
            {
                makeCubeInput(myCamera);
            } 
        }

        //Takes a camera, and then does a function according to a key press
        private void makeCubeInput(TwoDCamera myCamera)
        {
            ConsoleKey myKey = Console.ReadKey().Key;
            switch (myKey)
            {
                //The top 6 are for making the 6 possible moves on the cube. Make the move, and then update the frame to see what the cube now looks like
                case ConsoleKey.B:
                    myCube.MoveCube("B");
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.D:
                    myCube.MoveCube("D");
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.F:
                    myCube.MoveCube("F");
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.L:
                    myCube.MoveCube("L");
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.R:
                    myCube.MoveCube("R");
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.U:
                    myCube.MoveCube("U");
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                //If you press escape, exit the 2D camera loop
                case ConsoleKey.Escape:
                    Loop = false;
                    break;
                //Enter shows the user what the available controls are
                case ConsoleKey.Enter:
                    myCamera.Controls();
                    break;
                //Same controls as with 3D cube, - to undo, 0 to redo move, S to solve, A to shuffle
                case ConsoleKey.OemMinus:
                    myCube.UndoMove();
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.D0:
                    myCube.RedoMove();
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                //Only solve the cube if it isnt solved, so that you don't inadvertently lose a solution algorithm
                case ConsoleKey.S:
                    if (myCube.IsCubeSolved() == false)
                    {
                        //Store the solution separate to the shuffle.
                        MovesDone = myCube.movesDone;
                        myCube.Solve();
                        Solution = myCube.movesDone;
                        myCube.movesDone = "";
                    }
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.A:
                    myCube.Shuffle();
                    MovesDone = myCube.movesDone;
                    update2Dcube(myCamera, myCube);
                    break;
                //Buttons to scroll through the moves done window
                case ConsoleKey.D3:
                    myCamera.movesDoneScrollPos = -1;
                    update2Dcube(myCamera, myCube);
                    break;
                case ConsoleKey.D4:
                    myCamera.movesDoneScrollPos = 1;
                    update2Dcube(myCamera, myCube);
                    break;
            }
        }

        //Put together all the different boxes that create the 2D interface
        private void update2Dcube(TwoDCamera myCamera, Cube myCube)
        {
            //Draw the actual 2D cube onto the frame
            myCamera.DrawScreen(myCube);
            myCamera.DrawCubeBoundary();
            myCamera.DrawMovesDone(MovesDone);
            myCamera.DrawHowToGuide();
            myCamera.DrawSolution(Solution);
            //Once all the cube faces are on the screen buffer, draw it to the console
            myCamera.UpdateFrame();
            //Clear what has just been written, so that you can draw next frame without any picture overlap
            myCamera.ClearFrame();
        }

        //Declare the solution and moves done strings outside any functions, so that both the 2D environment and 3D environment can use them
        private String Solution = "", MovesDone = "";

        //Assign an action to different inputs
        private void makeCubeInput(ThreeDCamera_v2 myCamera)
        {
            ConsoleKey myKey = Console.ReadKey().Key;
            switch (myKey)
            {
                //These keys all rotate the cube in different directions, using different axes
                case ConsoleKey.UpArrow:
                    myCamera.RotateCube(1, 1);
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.DownArrow:
                    myCamera.RotateCube(1, -1);
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.LeftArrow:
                    myCamera.RotateCube(0, 1);
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.RightArrow:
                    myCamera.RotateCube(0, -1);
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.D1:
                    myCamera.RotateCube(2, 1);
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.D2:
                    myCamera.RotateCube(2, -1);
                    update3Dcube_v2(myCamera, myCube);
                    break;
                //All these commands are the same as the 2D cube, just using a 3D camera
                case ConsoleKey.B:
                    myCube.MoveCube("B");
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.D:
                    myCube.MoveCube("D");
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.F:
                    myCube.MoveCube("F");
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.L:
                    myCube.MoveCube("L");
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.R:
                    myCube.MoveCube("R");
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.U:
                    myCube.MoveCube("U");
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.Escape:
                    Loop = false;
                    break;
                case ConsoleKey.OemMinus:
                    myCube.UndoMove();
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.D0:
                    myCube.RedoMove();
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.S:
                    if (myCube.IsCubeSolved() == false)
                    {
                        MovesDone = myCube.movesDone;
                        myCube.Solve();
                        Solution = myCube.movesDone;
                        myCube.movesDone = "";
                    }
                    update3Dcube_v2(myCamera, myCube);
                    break;
                //Reset the orientation of the cube, if the user stops being coordinated
                case ConsoleKey.F4:
                    ThreeDEnvironment_v3();
                    break;
                case ConsoleKey.Enter:
                    myCamera.Controls();
                    break;
                case ConsoleKey.A:
                    myCube.Shuffle();
                    MovesDone = myCube.movesDone;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.D3:
                    myCamera.movesDoneScrollPos = -1;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                case ConsoleKey.D4:
                    myCamera.movesDoneScrollPos = 1;
                    update3Dcube_v2(myCamera, myCube);
                    break;
                default:
                    break;
            }
        }

        //Puts together all the parts the display the 3D UI into one place, and draws it onto the screen.
        private void update3Dcube_v2(ThreeDCamera_v2 myCamera, Cube myCube)
        {
            //Draw the boundary of the cube. This is important for when the face is being drawn, as it allows the code to quickly identify if a point is inside or outside the cube
            myCamera.DrawCubeBoundary();
            //Reset the VertexLocations list, which will help the other functions quickly identify what vertex is where
            myCamera.VertexLocations = new List<int[]>();
            
            //For each vertex, calculate the position of the vertex relative to the 2D plane
            for (int i = 0; i < 8; i++)
            {
                myCamera.calculateVertexV3(myCamera.VertexPositions[i]);
            }
            //Draw the lines that connect the vertices
            myCamera.CalculateLinesBetweenVertices_V2();
            //Draw the lines the segment all the faces, and then fill them in
            myCamera.drawFace(myCube);
            //Draw the other boxes around the cube, using the MovesDone and Solution strings previously defined
            myCamera.DrawMovesDone(MovesDone);
            myCamera.DrawHowToGuide();
            myCamera.DrawSolution(Solution);
            //Update the changes to the cube to the screen, then clear it so you can draw to screen again without overlapping text.
            myCamera.UpdateFrame();
            myCamera.ClearFrame();
        }

        //Loop that contains the 3D Cube Environment
        private void ThreeDEnvironment_v3()
        {
            //Declare an instance of a 3D camera, which then tries to get filled in with a custom colour loadout. If not possible, just create with default colours.
            ThreeDCamera_v2 myCamera;
            try
            {
                myCamera = new ThreeDCamera_v2(MyColours);
            }
            catch
            {
                myCamera = new ThreeDCamera_v2();
            }

            //Rotate the cube correctly so that each move (U, R, F, B, L, D) looks like it would on a real life cube
            myCamera.RotateCube(2, 5);
            myCamera.RotateCube(0, 8);
            myCamera.RotateCube(1, 2);

            //Draw the cube and UI elements to the screen, and then loop reading inputs until user exits to menu
            update3Dcube_v2(myCamera, myCube);
            Loop = true;
            while (Loop)
            {
                makeCubeInput(myCamera);
            }
        }

        //Draw together the insert cube environment UI elements
        private void updateInsertCube(ThreeDCamera_v2 myCamera, Cube myCube, String DirInst, String Instruc, int[] ColoursLeft, int Selection)
        {
            //Using the 3D camera to display the insert cube, draw the cube to the screen.
            myCamera.DrawCubeBoundary();
            //Draw an instruction to the screen
            myCamera.DrawInsertInstructions(Instruc);
            myCamera.VertexLocations = new List<int[]>();
            for (int i = 0; i < 8; i++)
            {
                myCamera.calculateVertexV3(myCamera.VertexPositions[i]);
            }
            myCamera.CalculateLinesBetweenVertices_V2();
            myCamera.drawFace(myCube);
            //Draw the remaining UI elements of the insertion environment
            myCamera.DrawAvailableColours(ColoursLeft);
            myCamera.DrawDirectionInstructions(DirInst);
            myCamera.DrawLetters(Selection);
            //Draw the screen to the console, so that the user can see the current state of the cube, and then clear the frame to make sure nothing overlaps
            myCamera.UpdateFrame();
            myCamera.ClearFrame();
        }

        //Code that changes the colour of a vertex in the input environment
        private int[] AddFace(int[] faceOrder, int Selection, int[] ColoursLeft, int Face, Cube inputCube, int DirecCount)
        {
            //Making sure that the user has actually selected a corner, and isn't just pressing a number randomly
            if (Selection >= 0 && Selection <=3)
            {
                //Once the face has been rotated 4 times, due to rotation, the perspective of what corner is what changes. This adjusts the selection accordingly
                if (DirecCount == 4)
                {
                    //Reshuffle the selection so that it becomes the correct corner
                    Selection = Selection - 2;
                    if (Selection < 0)
                        Selection = 4 + Selection;
                    //If there are still avaiable colours of the colour that the user wants to input, then make the change
                    if (ColoursLeft[Face] > 0)
                    {
                        //If there isn't already a colour on the corner they're trying to input already, just fill it in
                        if (inputCube.cube[faceOrder[DirecCount], Selection % 2, 1 - (int)Math.Floor((double)Selection / 2)] == 7)
                            inputCube.ChangeCubeFace(faceOrder[DirecCount], Selection % 2, 1 - (int)Math.Floor((double)Selection / 2), Face);
                        //Otherwise, increase the number of available colours of the one you just wrote over, and then input that face
                        else
                        {
                            ColoursLeft[inputCube.cube[faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2]] += 1;
                            inputCube.ChangeCubeFace(faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2, Face);
                        }
                        //Decrease the avialable number of that colour, as you have just used one
                        ColoursLeft[Face] -= 1;
                    }
                }

                //Once the face has been rotated 5 times, due to rotation, the perspective of what corner is what changes again. This adjusts the selection accordingly
                else if (DirecCount == 5)
                {
                    //Reshuffle the selection so that it becomes the correct corner
                    Selection = Selection - 2;
                    if (Selection < 0)
                        Selection = 4 + Selection;
                    //If there are still avaiable colours of the colour that the user wants to input, then make the change
                    if (ColoursLeft[Face] > 0)
                    {
                        //If there isn't already a colour on the corner they're trying to input already, just fill it in
                        if (inputCube.cube[faceOrder[DirecCount], 1 - Selection % 2, (int)Math.Floor((double)Selection / 2)] == 7)
                            inputCube.ChangeCubeFace(faceOrder[DirecCount], 1 - Selection % 2, (int)Math.Floor((double)Selection / 2) , Face);
                        //Otherwise, increase the number of available colours of the one you just wrote over, and then input that face
                        else
                        {
                            ColoursLeft[inputCube.cube[faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2]] += 1;
                            inputCube.ChangeCubeFace(faceOrder[DirecCount], (int)Math.Floor((double)Selection / 2), 1 - Selection % 2, Face);
                        }
                        //Decrease the avialable number of that colour, as you have just used one
                        ColoursLeft[Face] -= 1;
                    }
                }
                //For the first rotations, the perspective remains constant throughout, so no need to reshuffle what the selection number is
                else
                {
                    //If there are still avaiable colours of the colour that the user wants to input, then make the change
                    if (ColoursLeft[Face] > 0)
                    {
                        //If there isn't already a colour on the corner they're trying to input already, just fill it in
                        if (inputCube.cube[faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2] == 7)
                            inputCube.ChangeCubeFace(faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2, Face);
                        //Otherwise, increase the number of available colours of the one you just wrote over, and then input that face
                        else
                        {
                            ColoursLeft[inputCube.cube[faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2]] += 1;
                            inputCube.ChangeCubeFace(faceOrder[DirecCount], 1 - (int)Math.Floor((double)Selection / 2), Selection % 2, Face);
                        }
                        //Decrease the avialable number of that colour, as you have just used one
                        ColoursLeft[Face] -= 1;
                    }
                }
            }
            return ColoursLeft;
        }

        private bool ValidCube(Cube cube)
        {
            //Create a timeout exception. If the cube takes too long to solve, it isn't going to be solvable
            var task = Task.Run(() => { cube.Solve(); });
            bool IsCompleted = task.Wait(TimeSpan.FromMilliseconds(1000));

            if (IsCompleted)
            {
                return true;
            }
            else
                return false;
        }



        //Function that contains the important information as well as the loop for the input environment
        private void InputEnvironment()
        {
            //Create an instance of the 3D camera
            ThreeDCamera_v2 myCamera = new ThreeDCamera_v2();
            //Create a new cube to draw onto, as you don't want to lose progress on the existing cube if the input isnt corrent
            Cube inputCube = new Cube();
            //The different instructions for the user to follow, a helpful guide to simplify the process
            String[] Instructions = { "Welcome to the cube inserter. Press [ENTER] to continue...", "Type any letter to select a corner. Type the number to input that colour into that corner. Once you're done, press [ENTER]. Press [BACKSPACE] at any time to go back a face." };
            String[] DirectionInstructions = { "", "Rotate your cube 90 degrees so the current top is the new front", "Rotate your cube 90 degrees again so the current top is the new front", "Rotate your cube 90 degrees one more time so the current top is the new front", "Turn your cube 90 degrees clockwise, so that the current right face is the front face", "Rotate your cube 180 degrees, so that the back face becomes the top face", "The cube should be completed. Press [ENTER] to return to menu", "Sorry, that cube is not valid. Please try again"};
            //Keep track of what instruction should be displayed
            int InstCount = 0, DirecCount = 0;
            //Define how many of each colour is left. At the start there are 4 of each colour
            int[] ColoursLeft = { 4, 4, 4, 4, 4, 4 };
            //Make the selection equal to a value that they can't select before allowing them to make an input, so that the code knows they haven't chosen anything yet
            int Selection = 4;
            //Store how the cube will rotate, as this process is automated when the user is done inputting a face
            double[,] RotationDirections = new double[,] { {1, Math.PI/2}, { 1, Math.PI/2}, { 1, Math.PI /2}, { 0, Math.PI / 2 }, {0, Math.PI }, { 0, 0 } };
            //Stores the index location of what face is being displayed at what point
            int[] faceOrder = new int[]{ 1, 4, 5, 0, 2, 3 };
            //For each corner on the new cube, make the corner empty
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    for (int k = 0; k < 2; k++)
                        inputCube.ChangeCubeFace(i, j, k, 7);
                }
            }
            //keep looping until the user quits
            bool Exit = false;
            while (!Exit)
            {
                //Draw current state of cube to screen at beginning of each loop
                updateInsertCube(myCamera, inputCube, DirectionInstructions[DirecCount], Instructions[InstCount], ColoursLeft, Selection);
                //Read a key input
                ConsoleKey myKey = Console.ReadKey().Key;
                switch (myKey)
                {
                    //These 4 are to select a corner of the face
                    case ConsoleKey.A:
                        Selection = 0;
                        break;
                    case ConsoleKey.B:
                        Selection = 1;
                        break;
                    case ConsoleKey.C:
                        Selection = 2;
                        break;
                    case ConsoleKey.D:
                        Selection = 3;
                        break;
                    //These all allow the user to input a colour into a corner
                    case ConsoleKey.D0:
                        ColoursLeft = AddFace(faceOrder, Selection, ColoursLeft, 0, inputCube, DirecCount);
                        break;
                    case ConsoleKey.D1:
                        ColoursLeft = AddFace(faceOrder, Selection, ColoursLeft, 1, inputCube, DirecCount);
                        break;
                    case ConsoleKey.D2:
                        ColoursLeft = AddFace(faceOrder, Selection, ColoursLeft, 2, inputCube, DirecCount);
                        break;
                    case ConsoleKey.D3:
                        ColoursLeft = AddFace(faceOrder, Selection, ColoursLeft, 3, inputCube, DirecCount);
                        break;
                    case ConsoleKey.D4:
                        ColoursLeft = AddFace(faceOrder, Selection, ColoursLeft, 4, inputCube, DirecCount);
                        break;
                    case ConsoleKey.D5:
                        ColoursLeft = AddFace(faceOrder, Selection, ColoursLeft, 5, inputCube, DirecCount);
                        break;
                    //Allows the user to exit the environment
                    case ConsoleKey.Escape:
                        Exit = true;
                        break;
                    //Enter cycles through the different instructions
                    case ConsoleKey.Enter:
                        if (InstCount < 1)
                            InstCount += 1;
                        //Keep displaying new direction instructions for the user to follow, until they reach the end of the directions
                        else
                        {
                            if (DirecCount < DirectionInstructions.Count()-1)
                            {
                                if (DirecCount < 6)
                                    myCamera.RotateCube(RotationDirections[DirecCount, 0], RotationDirections[DirecCount, 1]);
                                DirecCount += 1;
                            }

                            if (DirecCount == DirectionInstructions.Count())
                            {
                                if (ValidCube(inputCube))
                                    Exit = true;
                                else
                                {
                                    
                                }
                            }
                        }
                        break;
                    //If they press backspace, they can go back a face
                    case ConsoleKey.Backspace:
                        if (DirecCount > 0)
                        {
                            DirecCount -= 1;
                            if(DirecCount < 6)
                                myCamera.RotateCube(RotationDirections[DirecCount, 0], 2 * Math.PI - RotationDirections[DirecCount, 1]);
                        }
                        break;                            
                }

            }
            //If they have fully inputted the cube, assign this cube to the already existing myCube, which is used by the other environments
            if(ColoursLeft[0] + ColoursLeft[1] + ColoursLeft[2] + ColoursLeft[3] + ColoursLeft[4] + ColoursLeft[5] == 0)
                myCube = inputCube;
        }
    }

    //Vertex class is for use with the 3D camera, which stores the x y z coordinates of a point in 3D space
    public class Vertex
    {
        public double x = 0, y = 0, z = 0;
        public int Val = 0;
        //Val dictates what vertex the current vertex is, which is used to draw lines between vertices
        public Vertex(double X, double Y, double Z, int Value)
        {
            x = X; y = Y; z = Z;
            Val = Value;
        }
    }

    //This class is what displays the information on the screen, handles grpahics. Is abstract as nothing uses this class without having a cube or menu system to display
    abstract class Camera
    {
        //Create a size of the screen, and the String is what all changes are drawn to
        protected int screenColumns = 160, screenRows = 46;
        protected String[,,] screen;
        //Store an index of the avaiable colours in this cube
        private ConsoleColor[] Colours = new ConsoleColor[] { ConsoleColor.DarkRed, ConsoleColor.Red, ConsoleColor.DarkYellow, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.DarkGreen, ConsoleColor.Cyan, ConsoleColor.DarkCyan, ConsoleColor.Blue, ConsoleColor.DarkBlue, ConsoleColor.Magenta, ConsoleColor.DarkMagenta, ConsoleColor.White, ConsoleColor.Gray, ConsoleColor.DarkGray, ConsoleColor.Black};
        //Store default values that are used directly in the code
        protected int[] ColorIndexValues = new int[]{ 2, 8, 12, 3, 1, 4, 14, 15};
        //ColorIndex is for use with custom colours
        protected ConsoleColor[] ColorIndex = new ConsoleColor[8];

        //Constructor for if there are no custom colour values
        public Camera()
        {
            //Assign the colours for use specific values
            InstantiateColours();
            //Make the entire screen empty for first use
            //Screen[3,0] will display what the character there is
            //Screen[3,1] will display what colour the character text should be
            //Screen[3,2] will display what colour the background will be
            screen = new String[screenRows, screenColumns, 3];
            for (int row = 0; row < screenRows; row++)
            {
                for (int column = 0; column < screenColumns; column++)
                {
                    screen[row, column, 0] = " ";
                    screen[row, column, 1] = "1";
                    screen[row, column, 2] = "7";
                }
            }
            //Declare the console presets, i.e. Black background, console size and make the cursor invisible
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetWindowSize(screenColumns+1, screenRows+1);
            Console.SetBufferSize(screenColumns+1, screenRows+1);
            Console.CursorVisible = false;
        }

        public Camera(int[] ColorValues)
        {
            //If you have custom colours, assign them to the colorvalues array
            ColorIndexValues = ColorValues;
            //Actually make these changes to the code
            InstantiateColours();
            //Make the entire screen empty for first use
            screen = new String[screenRows, screenColumns, 3];
            for (int row = 0; row < screenRows; row++)
            {
                for (int column = 0; column < screenColumns; column++)
                {
                    screen[row, column, 0] = " ";
                    screen[row, column, 1] = "1";
                    screen[row, column, 2] = "7";
                }
            }
            //Declare the console presets, i.e. Black background, console size and make the cursor invisible.
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetWindowSize(screenColumns, screenRows);
            Console.SetBufferSize(screenColumns, screenRows);
            Console.CursorVisible = false;
        }

        //Assign the colour values to the colour array, so that you can see what colour everything is
        public void InstantiateColours()
        {
            ColorIndex = new ConsoleColor[] { Colours[ColorIndexValues[0]], Colours[ColorIndexValues[1]], Colours[ColorIndexValues[2]], Colours[ColorIndexValues[3]], Colours[ColorIndexValues[4]], Colours[ColorIndexValues[5]], Colours[ColorIndexValues[6]], Colours[ColorIndexValues[7]] };
        }

        //Code that draws the current screen array to the cosnole
        public void UpdateFrame()
        {
            //Make the console empty
            Console.Clear();
            //Go through each part of the cube
            for (int row = 0; row < screenRows; row++)
            {
                for (int column = 0; column < screenColumns; column++)
                {
                    if (screen[row, column, 0] != " ")
                    {
                        //Only draw to the screen if the piece is within the buffer
                        try
                        {
                            //If there is a pixel to draw, go to the pixel location
                            Console.SetCursorPosition(column, row);
                            //Convert a string into an integer value, so that a colour can be assigned to it
                            int PxlColour = int.Parse(screen[row, column, 1]);
                            
                            //If there is a background colour assigned, make that the background colour. Otherwise, make it the default, black
                            try { Console.BackgroundColor = ColorIndex[int.Parse(screen[row, column, 2])]; }
                            catch { Console.BackgroundColor = ConsoleColor.Black; }

                            //If there is a text colour defined, use that. Otherwise, make it the default colour
                            try { Console.ForegroundColor = ColorIndex[PxlColour]; }
                            catch { Console.ForegroundColor = Colours[0]; }
                            //Actually write that colour to the screen
                            Console.Write(screen[row, column, 0]);
                        }
                        catch { }
                    }
                }
            }
        }

        //Make the screen clear, so that the next function will draw to the screen without anything overlapping
        public void ClearFrame()
        {
            for (int row = 0; row < screenRows; row++)
            {
                for (int column = 0; column < screenColumns; column++)
                {
                    screen[row, column, 0] = " ";
                    screen[row, column, 1] = "1";
                }
            }
        }

        //Store a value for the current scroll position of the moves done box, as well as the maximum value in the moves done box.
        //Defined in the parent class as both the 2D and 3D cubes use this
        private int MovesDoneScrollPos = 0;
        private int MovesDoneScrollMax = 1;

        public int movesDoneScrollPos
        {
            set
            {
                //If the moves done is able to be changed, i.e. there is space to scroll, then do so
                if (MovesDoneScrollPos < MovesDoneScrollMax && value > 0)
                {
                    MovesDoneScrollPos += value;
                }
                if (MovesDoneScrollPos > 0 && value < 0)
                {
                    MovesDoneScrollPos += value;
                }
            }
        }

        //Display the avaiable
        public void Controls()
        {
            ClearFrame();
            //Display the controls to the screen
            int StartPoint = (int)(screenRows * 0.1);
            string[,] Controls = { { "          CONTROLS", "" }, {"Upper layer", "U" }, {"Bottom Layer", "D" }, {"Right Layer", "R" }, { "Left Layer", "L" }, {"Front Layer", "F" }, { "Back Layer", "B" } , { "---------------", "" },{ "Rotate Cube L/R", "Left/Right" }, { "Rotate Cube U/D", "Up/Down" }, { "Spin Cube", "0, 1" }, { "Solve Cube", "S" },{ "Shuffle Cube", "A" }, { "Reset Camera", "F4" }, { "Undo Move", "-" }, { "Redo Move", "0" } , { "TYPE ANY CONTROL TO GO BACK", "" } };
            //Draw the border of the controls
            for (int i = (int)(screenColumns / 2 - (screenColumns) * 0.15); i < (int)(screenColumns / 2 + (screenColumns) * 0.15); i++)
            {
                screen[StartPoint, i, 0] = "*";
                screen[StartPoint + Controls.Length + 4, i, 0] = "*";
                screen[StartPoint, i, 1] = "3";
                screen[StartPoint + Controls.Length + 4, i, 1] = "3";
            }
            for(int i = StartPoint; i < StartPoint + Controls.Length + 4; i++)
            {
                screen[i, (int)(screenColumns / 2 - (screenColumns) * 0.15), 0] = "*";
                screen[i, (int)(screenColumns / 2 + (screenColumns) * 0.15), 0] = "*";
                screen[i, (int)(screenColumns / 2 - (screenColumns) * 0.15), 1] = "3";
                screen[i, (int)(screenColumns / 2 + (screenColumns) * 0.15), 1] = "3";
            }

            //Draw the controls to the screen
            for(int i = 0; i < Controls.Length / 2; i++)
            {

                screen[StartPoint + ((i+1) * 2), (int)(screenColumns / 2 - (screenColumns) * 0.15) + 10, 0] = Controls[i,0];
                screen[StartPoint + ((i + 1) * 2), (int)(screenColumns / 2 - (screenColumns) * 0.15) + 35, 0] = Controls[i, 1];
            }
            //Draw screen to the console
            UpdateFrame();
            ClearFrame();
        }

        //Draw the How To Guide on the screen
        public void DrawHowToGuide()
        {
            String HowToInstructions = "HOW TO PLAY \n \n " +
                "Welcome to the 2x2 Rubik's cube solver \n " +
                "Press [ENTER] to view controls \n " +
                "Press [ESC] to go back to menu" +
                " \n " +
                "Any move with a 2 after it means you repeat that move, and move with a ' after it means a prime move, which means that it is the inverse move \n \n " +
                "Any moves you make will be in the Moves Done box, which if it overflows you can scroll through with [3] and [4] \n " +
                "The solution to your cube will be put in the Solution box. Make sure to orient your cube the same way as the program displays, or else the solution won't work. \n " +
                "If you back to menu, your cube progress will be saved in between the 3D and 2D cameras";

            //Draw the box of the how to guide
            for (int j = 0; j < 3; j++)
            {
                for (int i = 1; i < screenColumns / 2 - 44; i++)
                {
                    screen[screenRows / 2 - 16, i, j] = "1";
                    screen[screenRows / 2 + 16, i, j] = "1";
                }
                for (int i = screenRows / 2 - 16; i < screenRows / 2 + 16; i++)
                {
                    screen[i, 1, j] = "1";
                    screen[i, screenColumns / 2 - 45, j] = "1";
                }
            }

            //Split the instructions into an array, so that you can write multiple lines without splitting up words
            string[] myInstruction = HowToInstructions.Split(' ');
            //Define how wide this box is
            int width = screenColumns / 2 - 50;
            //Keep track of where in the box you are writing to
            int CurCounter = 0;
            int RowCounter = 0;
            //for each word in instructions...
            foreach (string s in myInstruction)
            {
                //Go to a new line if there is a \n
                if (s == "\n")
                {
                    RowCounter += 1;
                    CurCounter = 0;
                }
                //If the word wouldn't overflow the width of the box, write that word to the screen
                else if (width - CurCounter - s.Length > 0)
                {
                    screen[RowCounter + screenRows / 2 - 14, CurCounter + 4, 0] = s;
                    screen[RowCounter + screenRows / 2 - 14, CurCounter + 4, 1] = "1";
                    CurCounter += s.Length + 1;
                }
                //If the word would overflow the box, go onto the next line and write to the screen
                else
                {
                    RowCounter += 1;
                    CurCounter = 0;
                    screen[RowCounter + screenRows / 2 - 14, CurCounter + 4, 0] = s;
                    screen[RowCounter + screenRows / 2 - 14, CurCounter + 4, 1] = "1";
                    CurCounter += s.Length + 1;
                }
            }
        }


        //Method to draw a box enclosing the cube. Was originally put there so that if the fill function
        //failed, only a portion of the screen would be filled in, rather than the entire screen.
        //Now, it mostly just provides aesthetics for the cube.
        public void DrawCubeBoundary()
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = screenColumns / 2 - 43; i < screenColumns / 2 + 44; i++)
                {
                    screen[screenRows / 2 - 16, i, j] = "2";
                    screen[screenRows / 2 + 16, i, j] = "2";
                }
                for (int i = screenRows / 2 - 16; i < screenRows / 2 + 16; i++)
                {
                    screen[i, screenColumns / 2 - 43, j] = "2";
                    screen[i, screenColumns / 2 + 43, j] = "2";
                }
            }
        }

        //Code to draw a box with the done moves to the screen
        public void DrawMovesDone(string MovesDone)
        {
            //Define the width of the box
            int LBound = screenColumns / 2 + 45;
            int RBound = screenColumns - 1;
            for (int j = 0; j < 3; j++)
            {
                for (int i = LBound; i < RBound; i++)
                {
                    screen[screenRows / 2 - 16, i, j] = "4";
                    screen[21, i, j] = "4";
                }

                for (int i = screenRows / 2 - 16; i < 22; i++)
                {
                    screen[i, LBound, j] = "4";
                    screen[i, RBound, j] = "4";
                }
            }
            //Write a title
            screen[screenRows / 2 - 15, LBound + 2, 0] = "Moves Done";
            screen[screenRows / 2 - 15, LBound + 2, 1] = "4";

            //Write all the moves that have been done to the screen
            int Counter = 0;
            int Max = RBound - LBound - 3;
            //If the information doesn't overflow, then you can just write the moves as they are
            if (MovesDone.Length < Max * (34 - screenRows / 2))
            {
                foreach (char C in MovesDone)
                {
                    //Make it so that no lines start with a space, just to make it nearter
                    if (!(Counter % Max == 0 && C == ' '))
                    {
                        screen[(int)Math.Floor((double)Counter / (double)Max) + screenRows / 2 - 13, LBound + Counter % Max + 2, 0] = C.ToString();
                        screen[(int)Math.Floor((double)Counter / (double)Max) + screenRows / 2 - 13, LBound + Counter % Max + 2, 1] = "4";
                        Counter += 1;
                    }
                }
            }
            //If the moves done exceeds the box, start scrolling
            else
            {
                //Thinner box avaiable as portion is now needed to display the scroll slider
                Max = RBound - LBound - 5;
                //Calculate how many rows you will need to show all of the moves done
                int RowsNeeded = (int)Math.Ceiling((double)MovesDone.Length / (double)Max);
                //Value to dictate what the maximum scroll position is.
                MovesDoneScrollMax = RowsNeeded - (34 - screenRows / 2);
                //Change how high the scroll slider is depending on how many rows are needed
                double ScrollerHeight = ((1 / ((double)MovesDoneScrollMax + 1)) * (34 - screenRows / 2));
                //If the scroller height is too small, as something needs to be displayed, make it 1 high
                if (ScrollerHeight == 0)
                    ScrollerHeight = 1;
                //Only stores the moves that you are displaying at the time
                string MovesDon2 = "";
                //Start at the beginning of the row you have scrolled to, and write all the possible values to that string
                for (int i = MovesDoneScrollPos * Max; i < Math.Min(MovesDoneScrollPos * Max + Max * (34 - screenRows / 2), MovesDone.Length - 1); i++)
                {
                    MovesDon2 += MovesDone[i];
                }
                //For each value in MovesDon2, write that to the screen, the same as if there wasn't overflowing
                foreach (char C in MovesDon2)
                {
                    if (!(Counter % Max == 0 && C == ' '))
                    {
                        screen[(int)Math.Floor((double)Counter / (double)Max) + screenRows / 2 - 13, LBound + Counter % Max + 2, 0] = C.ToString();
                        screen[(int)Math.Floor((double)Counter / (double)Max) + screenRows / 2 - 13, LBound + Counter % Max + 2, 1] = "4";
                        Counter += 1;
                    }
                }
                //Draw the slider to the screen, getting the position and the height from previously calculated values
                for (double i = Math.Floor(ScrollerHeight * MovesDoneScrollPos); i < Math.Ceiling(ScrollerHeight * MovesDoneScrollPos + ScrollerHeight); i++)
                {
                    //The scroller uses a background colour to display itself, but only pixels that have a character to write get written to the screen.
                    //Therefore, if you make all parts the same colour, it will make it drawn to the screen and also will look like a block
                    for (int j = 0; j < 3; j++)
                        screen[(int)i + screenRows / 2 - 14, RBound - 2, j] = "1";
                }
            }
        }

        //Code to draw the solution to a puzzle if it has been solved
        public void DrawSolution(string Solution)
        {
            //Define the bounds of the box
            int LBound = screenColumns / 2 + 45;
            int RBound = screenColumns - 1;
            for (int j = 0; j < 3; j++)
            {
                for (int i = LBound; i < RBound; i++)
                {
                    screen[23, i, j] = "5";
                    screen[39, i, j] = "5";
                }

                for (int i = 23; i < screenRows / 2 + 17; i++)
                {
                    screen[i, LBound, j] = "5";
                    screen[i, RBound, j] = "5";
                }
            }
            screen[24, LBound + 2, 0] = "Solution";
            screen[24, LBound + 2, 1] = "5";

            int Counter = 0;
            int Max = RBound - LBound - 4;
            //For each move in positions, write that to the box, using a counter to keep track of how far through you are
            foreach (char C in Solution)
            {
                if (!(Counter % Max == 0 && C == ' '))
                {
                    screen[(int)Math.Floor((double)Counter / (double)Max) + 26, LBound + Counter % Max + 2, 0] = C.ToString();
                    screen[(int)Math.Floor((double)Counter / (double)Max) + 26, LBound + Counter % Max + 2, 1] = "5";
                    Counter += 1;
                }
            }

            //Draw a point with a black background to counteract bug where entire screen becomes the wrong background colour
            screen[screenRows / 2 + 17, 0, 0] = ".";
            screen[screenRows / 2 + 17, 0, 1] = "7";
            screen[screenRows / 2 + 17, 0, 2] = "7";
        }
    }

    //Specific child class only for displaying the 3D cube
    partial class ThreeDCamera_v2 : Camera
    {
        //store the locations of various points, firstly the player and then the verticies of the cube
        //Stored in format (x,y,z)

        //introduce constants in order to make changes easy: a is base, b is width of points
        private const int A = -4, B = 8;
        public Vertex[] VertexPositions = new Vertex[8];

        public List<int[]> VertexLocations = new List<int[]>();

        public double FOV = Math.PI / 2;

        //Dictate the position of different vertices in 3D space
        private double[,] vertexPos = new double[,] { { A, A + B, A, 1 }, { A, A + B, A + B, 2 }, { A, A, A, 3 }, { A, A, A + B, 4 }, { A + B, A + B, A, 5 }, { A + B, A + B, A + B, 6 }, { A + B, A, A, 7 }, { A + B, A, A + B, 8 } };

        //Fill out the vertex array with instances of the vertex class, so that each value is neatly encapsulated
        public ThreeDCamera_v2() : base() 
        {
            for(int i = 0; i < 8; i++)
            {
                Vertex myVertex = new Vertex(vertexPos[i, 0], vertexPos[i, 1], vertexPos[i, 2], (int)vertexPos[i, 3]);
                VertexPositions[i] = myVertex;
            }
        }
        //If there are custom colours fill in, call the base with the custom colours. Then, fill out the vertex array with instances of vertex class
        public ThreeDCamera_v2(int[] myColours) : base(myColours)
        {
            for (int i = 0; i < 8; i++)
            {
                Vertex myVertex = new Vertex(vertexPos[i, 0], vertexPos[i, 1], vertexPos[i, 2], (int)vertexPos[i, 3]);
                VertexPositions[i] = myVertex;
            }
        }

        //This code works out if a vertex should be displayed or not, depending on its Z position
        private Vertex hiddenVertex(Vertex[] myArr)
        {
            Vertex myV = new Vertex(0, 0, 0, 0);
            foreach(Vertex v in myArr)
            {
                if (myV.z > v.z)
                    myV = v;
            }
            return myV;
        }
        //This code takes the the furthest away vertex using the hiddenVertex class, and any other vertices on that same plane are also hidden
        private List<Vertex> hiddenVertices(Vertex[] myArr, Vertex furthest)
        {
            List<Vertex> myVs = new List<Vertex>();
            foreach(Vertex v in myArr)
            {
                if (furthest.z == v.z)
                    myVs.Add(v);
            }
            return myVs;
        }

        //Calculate the position of a vertex according to the screen using its x and y coordinates.
        //Called V3, as this was the third attempt that I tried, which was the successful one
        public void calculateVertexV3(Vertex testVertex)
        {
            //Calculate the centre of the screen, as the coordinates act around the origin
            int xCentre = (int)(screenColumns / 2);
            int yCentre = (int)(screenRows / 2);

            //variable to make vertex distance bigger
            double Scale = 2;
            //As the screen is wider than it is high, have an aspect ratio to counteract this so it still looks like a square
            double AspectRatio = screenColumns / screenRows ;
            //If the corner fits on the screen, draw it
            try
            {
                //If the Z coordinate is not the furthest back that it can be, and if the vertex is not on the furthest back plane, draw it
                if (testVertex.z != 1 && !hiddenVertices(VertexPositions, hiddenVertex(VertexPositions)).Contains(testVertex))
                {
                    //Go to the screen position using the x and y coordinate, and draw it to the screen
                    screen[yCentre + (int)(testVertex.x * Scale), xCentre + (int)(testVertex.y * Scale * AspectRatio), 0] = "X";
                    screen[yCentre + (int)(testVertex.x * Scale), xCentre + (int)(testVertex.y * Scale * AspectRatio), 1] = "6";
                    //Get the x y coordinates of the vertex on the screen and store it, for use with other functions such as drawFace
                    int[] VertexLoc = new int[] { yCentre + (int)(testVertex.x * Scale), xCentre + (int)(testVertex.y * Scale * AspectRatio), testVertex.Val };
                    VertexLocations.Add(VertexLoc);
                }
            }
            catch { }
        }

        //Draw a line between two points on an array.
        public void DrawLine(int[] Point1, int[] Point2, int Offset)
        {
            //Take the vertices into a 2D array, and assign to the values inputted
            int[,] myVertices = new int[2, 2];

            myVertices[0, 0] =Point1[1];
            myVertices[0, 1] = Point1[0];
            myVertices[1, 0] = Point2[1];
            myVertices[1, 1] = Point2[0];
            //Calcaulte the gradient of the line
            double myGradient = ((double)myVertices[1, 0] - (double)myVertices[0, 0]) / ((double)myVertices[1, 1] - (double)myVertices[0, 1]);
            //If the two points are on the same y coordinate, draw a straight line between them
            if (myVertices[0, 1] - myVertices[1, 1] == 0)
            {
                for (int i = (int)Math.Min(myVertices[0, 0], myVertices[1, 0]) + Offset; i < (int)Math.Max(myVertices[0, 0], myVertices[1, 0]) + (1 - Offset); i++)
                {
                    screen[i, myVertices[1, 1], 0] = ";";
                    screen[i, myVertices[1, 1], 1] = "6";
                }
            }
            //If the gradient is closer to the x axis, then draw along the x axis, calculating the y value at each point
            else if (Math.Abs(myVertices[0,1] - myVertices[1,1]) > Math.Abs(myVertices[0,0] - myVertices[1,0]))
            {
                for (int i = Math.Min(myVertices[0, 1],myVertices[1,1])+ Offset; i < Math.Max(myVertices[1, 1], myVertices[0,1]) + (1 - Offset); i++)
                {
                    screen[(int)(myGradient * i - myVertices[0, 1] * myGradient + myVertices[0, 0]), i, 0] = ";";
                    screen[(int)(myGradient * i - myVertices[0, 1] * myGradient + myVertices[0, 0]), i, 1] = "6";
                }
            }
            //If the gradient is sloping more towards the y axis, go through all the y values and calculate the x from it
            else
            {
                for (int i = (int)Math.Min(myVertices[0, 0], myVertices[1, 0]) + Offset; i < (int)Math.Max(myVertices[0, 0], myVertices[1, 0]) + (1 - Offset); i++)
                {
                    screen[i, (int)((double)(i - (-myVertices[0, 1] * myGradient + myVertices[0, 0])) / myGradient), 0] = ";";
                    screen[i, (int)((double)(i - (-myVertices[0, 1] * myGradient + myVertices[0, 0])) / myGradient), 1] = "6";
                }
            }
        }

        //Second attempt getting this function to work. Draw the lines between adjacent vertices on the cube
        public void CalculateLinesBetweenVertices_V2()
        {
            int[,] myVerticies = { { 0, 0 }, { 0, 0 }};
            //Store the adjacent vertex values together.
            int[,] Order = new int[,] { { 1, 2 }, { 1, 3 }, { 1, 5 }, { 2, 4 }, { 2, 6 }, { 3, 4 }, { 3, 7 }, { 4, 8 }, { 5, 6 }, { 5, 7 }, { 6, 8 }, { 7, 8 } };
           
            //For each adjecent set of vertices...
            for (int i = 0; i < 12; i++)
            {
                //Work out how many of the two vertices are actually visible on the screen
                int Showing = 0;
                for(int j = 0; j < VertexLocations.Count; j++)
                {
                    if (VertexLocations[j][2] == Order[i, 0])
                    {
                        myVerticies[0, 0] = VertexLocations[j][0];
                        myVerticies[0, 1] = VertexLocations[j][1];
                        Showing += 1;
                    }
                    else if (VertexLocations[j][2] == Order[i, 1])
                    {
                        myVerticies[1, 0] = VertexLocations[j][0];
                        myVerticies[1, 1] = VertexLocations[j][1];
                        Showing += 1;
                    }
                    
                }
                //If they are both on the screen, get the X and Y values of the vertices, and then draw the line between them
                if (Showing == 2)
                {
                    int[] Point1 = new int[2];
                    int[] Point2 = new int[2];
                    Point1[0] = myVerticies[0, 1];
                    Point1[1] = myVerticies[0, 0];
                    
                    Point2[0] = myVerticies[1, 1];
                    Point2[1] = myVerticies[1, 0];

                    if (Point1[0] * Point1[1] * Point2[0] * Point2[1] != 0)
                        DrawLine(Point1, Point2,1);
                }               
            }
        }

        //Coords is to store the x and y difference from an original point, going to the 4 values around the point
        int[,] Coords = new int[,] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        //Recursive fill method
        private void floodFill(int[] Point, int Colour)
        {
            //If the point passed in is on the screen
            if (Point[0] > 0 && Point[0] < screenColumns)
            {
                if (Point[1] > 0 && Point[1] < screenRows)
                {
                    //If the point on the screen is empty
                    if (screen[Point[1], Point[0], 0] == " ")
                    {
                        //Go around the point using the coords to get new position
                        for (int i = 0; i < 4; i++)
                        {
                            //Fill in the empty space
                            screen[Point[1], Point[0], 0] = "#";
                            screen[Point[1], Point[0], 1] = $"{Colour}";
                            int[] NewPoint = { Point[0] + Coords[i, 0], Point[1] + Coords[i, 1] };
                            //Call itself again with the new point, as it needs to keep looping until the space is filled in.
                            //As you don't know how large the space will be, it needs to just be recursive
                            floodFill(NewPoint, Colour);
                        }
                    }
                }
            }
        }

        //Colour in the faces of the cube
        public void drawFace(Cube myCube)
        {
            //This array links together each of the different cube faces by the vertex values, and stores them all in the same from top to bottom as in the Cube class
            int[,] FaceCollection = new int[,] { { 5, 6, 7, 8 }, { 6, 2, 8, 4 }, { 2, 6, 1, 5 }, { 3, 7, 4, 8 }, { 2, 1, 4, 3 }, { 1, 5, 3, 7 } };
            //I will be for going through each array in FaceCollection
            for (int i = 0; i < 6; i++)
            {
                int[,] CornerLocations = new int[4, 2];
                int FaceShowing = 0;
                //K is to go through each value in FaceCollection[i]
                for (int k = 0; k < 4; k++)
                {
                    //J is to go through each value in VertexLocations
                    for (int j = 0; j < VertexLocations.Count(); j++)
                    {
                        if (VertexLocations[j][2] == FaceCollection[i, k])
                        {
                            FaceShowing += 1;
                            CornerLocations[k, 0] = VertexLocations[j][0];
                            CornerLocations[k, 1] = VertexLocations[j][1];
                        }
                    }
                }

                //If every vertex in that face is on the screen, draw the lines between the segments
                if (FaceShowing == 4)
                {
                    //Work out the midpoints of each edge of the face, and draw lines between these midpoints
                    int[] Intersection1 = { (int)((double)(CornerLocations[0, 1] + CornerLocations[1, 1]) / 2), (int)((double)(CornerLocations[0, 0] + CornerLocations[1, 0]) / 2) };
                    int[] Intersection2 = { (int)((double)(CornerLocations[2, 1] + CornerLocations[3, 1]) / 2), (int)((double)(CornerLocations[2, 0] + CornerLocations[3, 0]) / 2) };
                    int[] Intersection3 = { (int)((double)(CornerLocations[0, 1] + CornerLocations[2, 1]) / 2), (int)((double)(CornerLocations[0, 0] + CornerLocations[2, 0]) / 2) };
                    int[] Intersection4 = { (int)((double)(CornerLocations[1, 1] + CornerLocations[3, 1]) / 2), (int)((double)(CornerLocations[1, 0] + CornerLocations[3, 0]) / 2) };


                    DrawLine(Intersection1, Intersection2,0);
                    DrawLine(Intersection3, Intersection4,0);
                }
            }
            //Go through the face again. In a separate loop as if you try to fill in the faces before all the intersections are in, there will be faces "leaking" colour
            for (int i = 0; i < 6; i++)
            {
                int[,] CornerLocations = new int[4, 2];
                int FaceShowing = 0;
                //K is to go through each value in FaceCollection[i]
                for (int k = 0; k < 4; k++)
                {
                    //J is to go through each value in VertexLocations
                    for (int j = 0; j < VertexLocations.Count(); j++)
                    {
                        if (VertexLocations[j][2] == FaceCollection[i, k])
                        {
                            FaceShowing += 1;
                            CornerLocations[k, 0] = VertexLocations[j][0];
                            CornerLocations[k, 1] = VertexLocations[j][1];
                        }
                    }
                }

                //If every corner is showing on that face
                if (FaceShowing == 4)
                {
                    //Calculate where the midpoints of each colour corner will be using arrays of previously known values
                    int[] Intersection1 = { (int)Math.Ceiling((double)(CornerLocations[0, 1] + CornerLocations[1, 1]) / 2), (int)Math.Ceiling((double)(CornerLocations[0, 0] + CornerLocations[1, 0]) / 2) };
                    int[] Intersection2 = { (int)((double)(CornerLocations[2, 1] + CornerLocations[3, 1]) / 2), (int)((double)(CornerLocations[2, 0] + CornerLocations[3, 0]) / 2) };
                    int[] Intersection3 = { (int)((double)(CornerLocations[0, 1] + CornerLocations[2, 1]) / 2), (int)((double)(CornerLocations[0, 0] + CornerLocations[2, 0]) / 2) };
                    int[] Intersection4 = { (int)((double)(CornerLocations[1, 1] + CornerLocations[3, 1]) / 2), (int)((double)(CornerLocations[1, 0] + CornerLocations[3, 0]) / 2) };

                    int[] Centre1 = { (int)(((double)Intersection1[1] + (double)Intersection2[1]) / 2), (int)(((double)Intersection1[0] + (double)Intersection2[0]) / 2) };
                    int[] Centre2 = { (int)(((double)Intersection3[1] + (double)Intersection4[1]) / 2), (int)(((double)Intersection3[0] + (double)Intersection4[0]) / 2) };
                    int[] Centre = { (int)((double)(Centre1[0] + Centre2[0]) / 2), (int)((double)(Centre1[1] + Centre2[1]) / 2) };

                    //Get the midpoint of each corner colour as this will be used as the starting position for the colouring cose
                    int[,] CubieCentres = {
                    { (int)(((double)MinValue(CornerLocations[0, 1], Intersection1[0], Centre[1], Intersection3[0]) + (double)MaxValue(CornerLocations[0, 1], Intersection1[0], Centre[1], Intersection3[0])) / 2),           (int)(((double)MinValue(CornerLocations[0, 0], Intersection1[1], Centre[0], Intersection3[1]) + (double)MaxValue(CornerLocations[0, 0], Intersection1[1], Centre[0], Intersection3[1])) / 2),0,0},
                    { (int)(((double)MinValue(CornerLocations[1, 1], Intersection1[0], Centre[1], Intersection4[0]) + (double)MaxValue(CornerLocations[1, 1], Intersection1[0], Centre[1], Intersection4[0])) / 2),           (int)(((double)MinValue(CornerLocations[1, 0], Intersection1[1], Centre[0], Intersection4[1]) + (double)MaxValue(CornerLocations[1, 0], Intersection1[1], Centre[0], Intersection4[1])) / 2),0,1},
                    { (int)(((double)MinValue(CornerLocations[2, 1], Intersection2[0], Centre[1], Intersection3[0]) + (double)MaxValue(CornerLocations[2, 1], Intersection2[0], Centre[1], Intersection3[0])) / 2),           (int)(((double)MinValue(CornerLocations[2, 0], Intersection2[1], Centre[0], Intersection3[1]) + (double)MaxValue(CornerLocations[2, 0], Intersection2[1], Centre[0], Intersection3[1])) / 2),1,0},
                    { (int)(((double)MinValue(CornerLocations[3, 1], Intersection2[0], Centre[1], Intersection4[0]) + (double)MaxValue(CornerLocations[3, 1], Intersection2[0], Centre[1], Intersection4[0])) / 2),           (int)(((double)MinValue(CornerLocations[3, 0], Intersection2[1], Centre[0], Intersection4[1]) + (double)MaxValue(CornerLocations[3, 0], Intersection2[1], Centre[0], Intersection4[1])) / 2),1,1}};

                    if( i >-1)
                    {
                        for(int m = 0; m < 4; m++)
                        {
                            CubieCentres[m, 3] = Math.Abs(CubieCentres[m, 3] - 1);
                        }
                    }

                    //For each section on the face, fill in recursively
                    for (int l = 0; l < 4; l++) { 
                        int[] Curpos = { CubieCentres[l,0], CubieCentres[l,1] };
                        //If the point is actually inside the cube, as sometimes the face is incredibly flat, then the program will try to fill in outside of the cube
                        if (PointInsideCube(Curpos) == true)
                            floodFill(Curpos, myCube.cube[i, CubieCentres[l, 2], CubieCentres[l, 3]]);
                    }
                }
            }
        }

        //Simple code which identifies if a point is outside or inside the cube, so that you only try to fill in if it is inside
        private bool PointInsideCube(int[] myPoint)
        { 
            bool Inside = true;
            //Keep on going in a row until you hit something that isn't white space in all 4 directions
            for (int i = 0; i < 4; i++) 
            {
                int row = myPoint[1], column = myPoint[0];
                while (screen[row, column, 0] != ";" && screen[row,column, 0] != "O" && screen[row, column, 0] != "X")
                {
                    if (row < screenRows - 1 && row > 0)
                        row = row + Coords[i, 0];
                    else
                    {
                        Inside = false;
                        break;
                    }
                    if (column < screenColumns - 1 && column > 0)
                        column = column + Coords[i, 1];
                    else
                    {
                        Inside = false;
                        break;
                    }
                }
                if (screen[row, column, 0] == "2")
                {
                    Inside = false;
                    break;
                }
            }
            return Inside;
        }

        //Returns the biggest of 4 values 
        private int MaxValue(int val1, int val2, int val3, int val4)
        {
            return Math.Max(Math.Max(val1, val2), Math.Max(val3, val4));
        }
        //Returns the smallest of 4 values
        private int MinValue(int val1, int val2, int val3, int val4)
        {
            return Math.Min(Math.Min(val1, val2), Math.Min(val3, val4));
        }


        /// <summary>
        /// Calculates the distance of two points in a 3D space
        /// </summary>
        /// <param name="point1">First point</param>
        /// <param name="point2">Second point</param>
        /// <returns>Distance</returns>
        public double SigFigConvert(double myNumber, int Power)
        {
            myNumber = myNumber * Math.Pow(10, Power);
            myNumber = (int)myNumber;
            myNumber = myNumber / Math.Pow(10, Power);
            return myNumber;
        }
        //Code to carry out a matrix multiplation between a vertex and a rotation matrix.
        private Vertex MatrixMultiplication(double[,] Rotation, Vertex Vertex)
        {
            //Standard code for matrix multiplication
            Vertex newVertex = new Vertex(
                Rotation[0, 0] * Vertex.x + Rotation[0, 1] * Vertex.y + Rotation[0, 2] * Vertex.z,
                Rotation[1, 0] * Vertex.x + Rotation[1, 1] * Vertex.y + Rotation[1, 2] * Vertex.z,
                Rotation[2, 0] * Vertex.x + Rotation[2, 1] * Vertex.y + Rotation[2, 2] * Vertex.z,
                Vertex.Val
                );

            return newVertex;
        }
        //Rotate a Vertex around the origin, by a specific angle, in a direction
        private Vertex Rotate(double angle, Vertex point, int Axis)
        {
            //Axis 0 = x, 1 = y, 2 = z
            //Rotation matrices for 3D space
            angle = SigFigConvert(angle * 180 / Math.PI, 2) * Math.PI / 180;
            double[,] RotationX = { { 1, 0, 0 }, { 0, Math.Cos(angle), -Math.Sin(angle) }, { 0, Math.Sin(angle), Math.Cos(angle) } };
            double[,] RotationY = { { Math.Cos(angle), 0, Math.Sin(angle) }, { 0, 1, 0 }, { -Math.Sin(angle), 0, Math.Cos(angle) } };
            double[,] RotationZ = { { Math.Cos(angle), -Math.Sin(angle), 0 }, { Math.Sin(angle), Math.Cos(angle), 0 }, { 0, 0, 1 } };
            //Create a new vertex which will store the value of the rotated vertex
            Vertex myVertex = new Vertex(0,0,0,0);
            switch (Axis)
            {
                case 0:
                    myVertex = MatrixMultiplication(RotationX, point);
                    break;
                case 1:
                    myVertex = MatrixMultiplication(RotationY, point);
                    break;
                case 2:
                    myVertex = MatrixMultiplication(RotationZ, point);
                    break;
            }

            return myVertex;
        }
        //For integer inputs rotate all 8 vertices in a direction on an axis
        public void RotateCube(int Axis, int Direction)
        {
            for (int i = 0; i < 8; i++)
            {
                VertexPositions[i] = Rotate(Math.PI / 9 * Direction, VertexPositions[i], Axis);
            }
        }
        //For double values, rotate all 8 vertices by an angular distance on an axis
        public void RotateCube(double Axis, double Distance)
        {
            for (int i = 0; i < 8; i++)
            {
                VertexPositions[i] = Rotate(Distance, VertexPositions[i], (int)Axis);
            }
        }

        //For the input environment, draw the 4 letters to the screen that allow the user to know what button to press for what corner
        public void DrawLetters(int Letter)
        {

            int[] RowPos = { screenRows / 2 - 10, screenRows / 2 + 10 };
            int[] ColPos = { screenColumns / 2 - 28, screenColumns / 2 + 28 };
            string[] Letters = { "A", "B", "C", "D" };
            int Counter = 0;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++) {
                    screen[RowPos[j], ColPos[i], 0] = Letters[Counter];
                    if (Counter == Letter)
                        screen[RowPos[j], ColPos[i], 1] = "0";
                    Counter += 1;
                } 
            }
        }
        //For the input environment, draw how many of what colour is left avaiable to be drawn to the screen
        public void DrawAvailableColours(int[] ColourCounts)
        {
            int LBound = screenColumns / 2 + 45;
            int RBound = screenColumns - 1;
            for (int j = 0; j < 3; j++)
            {
                for (int i = LBound; i < RBound; i++)
                {
                    screen[screenRows / 2 - 16, i, j] = "4";
                    screen[21, i, j] = "4";
                }

                for (int i = screenRows / 2 - 16; i < 22; i++)
                {
                    screen[i, LBound, j] = "4";
                    screen[i, RBound, j] = "4";
                }
            }
            screen[screenRows / 2 - 15, LBound + 2, 0] = "Colours Left";
            screen[screenRows / 2 - 15, LBound + 2, 1] = "4";
            //Get the remaining number of each colour, and write it to the screen
            for (int i = 0; i < 6; i++)
            {
                screen[(screenRows / 2 - 14) + (i+1), LBound + 2, 0] = $"[{i}] {ColorIndex[i]}: {ColourCounts[i]}";
                screen[(screenRows / 2 - 14) + (i + 1), LBound + 2, 1] = $"{i}";
            }
        }
        //For the insertion environment, draw the current instruction to the screen
        public void DrawInsertInstructions(String instruction)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int i = screenColumns / 2 - 43; i < screenColumns / 2 + 44; i++)
                {
                    screen[screenRows / 2 + 18, i, j] = "0";
                    screen[screenRows - 1, i, j] = "0";
                }
                for (int i = screenRows / 2 +18 ; i < screenRows; i++)
                {
                    screen[i, screenColumns / 2 - 43, j] = "0";
                    screen[i, screenColumns / 2 + 43, j] = "0";
                }
            }

            int LBound = screenColumns / 2-41;
            //Write in the instruction, using a similar system to the How To function in the parent class
            string[] myInstruction = instruction.Split(' ');
            int width = 82;
            int CurCounter = 0;
            int RowCounter = 0;
            foreach (string s in myInstruction)
            {
                if (width - CurCounter - s.Length > 0)
                {
                    screen[RowCounter + screenRows / 2 + 19, CurCounter + LBound + 2, 0] = s;
                    screen[RowCounter + screenRows / 2 + 19, CurCounter + LBound + 2, 1] = "0";
                    CurCounter += s.Length + 1;
                }
                else
                {
                    RowCounter += 1;
                    CurCounter = 0;
                    screen[RowCounter + screenRows/2 + 19, CurCounter + LBound + 2, 0] = s;
                    screen[RowCounter + screenRows / 2 + 19, CurCounter + LBound + 2, 1] = "0";
                    CurCounter += s.Length + 1;
                }
            }

            screen[screenRows / 2 + 17, 0, 0] = ".";
            screen[screenRows / 2 + 17, 0, 1] = "7";
            screen[screenRows / 2 + 17, 0, 2] = "7";
        }

        //Draw the direction that the user needs to rotate their real life cube in for the input environment
        public void DrawDirectionInstructions(String instruction)
        {
            int LBound = screenColumns / 2 + 45;
            int RBound = screenColumns - 1;
            for (int j = 0; j < 3; j++)
            {
                for (int i = LBound; i < RBound; i++)
                {
                    screen[23, i, j] = "5";
                    screen[screenRows - 1, i, j] = "5";
                }

                for (int i = 23; i < screenRows - 1; i++)
                {
                    screen[i, LBound, j] = "5";
                    screen[i, RBound, j] = "5";
                }
            }
            //Take the input and draw this inside the direction box
            String[] myInstruction = instruction.Split(' ');
            int width = RBound - LBound - 2;
            int CurCounter = 0;
            int RowCounter = 0;
            foreach(string s in myInstruction)
            {
                if(width - CurCounter - s.Length > 0)
                {
                    screen[RowCounter + 25, CurCounter + LBound + 2, 0] = s;
                    screen[RowCounter + 25, CurCounter + LBound + 2, 1] = "5";
                    CurCounter += s.Length + 1;
                }
                else
                {
                    RowCounter += 1;
                    CurCounter = 0;
                    screen[RowCounter + 25, CurCounter + LBound + 2, 0] = s;
                    screen[RowCounter + 25, CurCounter + LBound + 2, 1] = "5";
                    CurCounter += s.Length + 1;
                }
            }

            screen[screenRows-1, screenColumns - 1, 0] = ".";
            screen[screenRows-1, screenColumns - 1, 1] = "7";
            screen[screenRows-1, screenColumns -1, 2] = "7";
        }
    }

    //Child class for displaying only the 2D cube
    partial class TwoDCamera : Camera
    {
        private double scale = 1;

        //If there are custom colour values, use this as the colours. Otherwise, just use the default colours
        public TwoDCamera(int[] ColorValues) : base(ColorValues) { }
        public TwoDCamera() : base() { }

        //Draw a cube to the screen in 2D, using a cube as input
        public void DrawScreen(Cube myCube)
        {
            int[] StartPos = { 14, 70 };
            Console.Clear();
            for (int row = 0; row < screenRows; row++)
            {
                for (int column = 0; column < screenColumns; column++)
                {
                    screen[row, column, 0] = " ";
                    screen[row, column, 1] = "1";
                }
            }

            //Draw the face to the screen, getting start positions from the StartPos array
            DrawFace(StartPos, myCube, 2);
            int[] Pos0 = { StartPos[0] + (int)(7 * scale), StartPos[1] - (int)(14 * scale) };
            DrawFace(Pos0, myCube, 0);
            int[] Pos5 = { StartPos[0] + (int)(7 * scale), StartPos[1] };
            DrawFace(Pos5, myCube, 5);
            int[] Pos4 = { StartPos[0] + (int)(7 * scale), StartPos[1] + (int)(14 * scale) };
            DrawFace(Pos4, myCube, 4);
            int[] Pos1 = { StartPos[0] + (int)(7 * scale), StartPos[1] + (int)(28 * scale) };
            DrawFace(Pos1, myCube, 1);
            int[] Pos3 = { StartPos[0] + (int)(14 * scale), StartPos[1] };
            DrawFace(Pos3, myCube, 3);
        }

        //Draw the face to the screen, using a cube for the colours
        private void DrawFace(int[] Position, Cube myCube, int Face)
        {
            //⬛
            DrawSquare(Position, myCube, (myCube.cube[Face, 0, 0]).ToString());
            int[] PosTR = { Position[0] + (int)(3 * scale), Position[1] };
            DrawSquare(PosTR, myCube, (myCube.cube[Face, 1, 0].ToString()));
            int[] PosBL = { Position[0], Position[1] + (int)(6 * scale) };
            DrawSquare(PosBL, myCube, myCube.cube[Face, 0, 1].ToString());
            int[] PosBR = { Position[0] + (int)(3 * scale), Position[1] + (int)(6 * scale) };
            DrawSquare(PosBR, myCube, myCube.cube[Face, 1, 1].ToString());
        }

        //Draw a square symbol to a specific part of the screen, which is being used by the DrawFace function
        private void DrawSquare(int[] Position, Cube myCube, String Colour)
        {
            for (int j = 0; j < 3 * scale; j++)
            {
                for (int i = 0; i < 2 * scale; i++)
                {
                    screen[Position[0] + i, Position[1] + j, 0] = "■";
                    screen[Position[0] + i, Position[1] + j, 1] = Colour;
                }
            }
        }
    }

    //Child class for the menu
    partial class Menu : Camera
    {
        //Stores the value of the current selection
        public int MenuSelection = 0;
        
        //If there are custom colours, or if there aren't, apply differently
        public Menu() : base()
        {
            StartMenu();
        }

        public Menu(int[] ColorValues) : base(ColorValues)
        {
            StartMenu();
        }

        //Contains the different elements of the start menu
        public String StartMenu()
        {
            bool Start = false;
            Logo();
            Options();
            UpdateFrame();
            //Keep on reading inputs until enter gets pressed
            do
            {
                ConsoleKey Option = Console.ReadKey().Key;
                Start = ReadOption(Option);
                //Draw the point of where the selection box is on the screen
                for (int i = 0; i < 5; i++)
                {
                    Console.SetCursorPosition(51, i * 2 + 15);
                    Console.Write(" ]");
                }
                Console.SetCursorPosition(51, MenuSelection * 2 + 15);
                Console.Write("■]");
            } while (Start == false);
            return "";
        }

        //Show the options that the user can choose between
        private void Options()
        {
            String[] Options =
            {
                "3D Cube",
                "2D Cube",
                "Input Cube",
                "Colour Options",
                "Exit"
            };
            //Write these options to the screen, along with a box that shows what the current selection is
            int CurRow = 15;
            foreach (String s in Options)
            {
                int CurCol = 10;
                foreach (char c in s)
                {
                    screen[CurRow, CurCol, 0] = c.ToString();
                    CurCol += 1;
                }
                screen[CurRow, CurCol + (40 - s.Length), 0] = "[";
                screen[CurRow, CurCol + (42 - s.Length), 0] = "]";
                CurRow += 2;
            }
            screen[MenuSelection * 2 + 15, 51, 0] = "■";
            screen[MenuSelection * 2 + 15, 51, 1] = $"{MenuSelection+2}";
        }

        //Take a key input and assign a function to it
        public bool ReadOption(ConsoleKey Option)
        {
            bool Selected = false;
            switch (Option)
            {
                //UP and DOWN to scroll up and down through the options
                case ConsoleKey.UpArrow:
                    if (MenuSelection > 0)
                        MenuSelection -= 1;
                    break;
                case ConsoleKey.DownArrow:
                    if (MenuSelection < 4)
                        MenuSelection += 1;
                    break;
                //If enter is pressed, return this value so that the program knows to start an environment
                case ConsoleKey.Enter:
                    Selected = true;
                    break;
                default:
                    break;
            }
            return Selected;
        }

        //Draw the colour option interface
        private void DrawColorSettings(int Selection)
        {
            ClearFrame();
            //Logo and draw logo to the screen
            String[] ColorLogo = {
            "  ____   ____   _      ____   _   _   ____   ____",
            " |  __| | __ | | |    | __ | | | | | | __ | |  __|",
            " | |__  | || | | |__  | || | | |_| | | || | |___ |",
            " |____| |____| |____| |____| |_____| |_|\\_\\ |____|",
            "---------------------------------------------------¬" };

            int Counter = 4;
            int Left = 20;
            foreach (String s in ColorLogo)
            {
                screen[Counter, Left, 0] = s;
                screen[Counter, Left, 1] = "0";
                Counter += 1;
            }
            //Write all of the colours. If the current colour is selected, draw arrows around it to show that the user can scroll left and right through them
            Counter += 2;
            for (int i = 0; i < 6; i++)
            {
                int Black = ColorIndex[i].ToString().Length + 2;
                screen[Counter, Left, 0] = $"Color {i + 1}: ";
                screen[Counter, Left, 1] = "6";
                if (i == Selection)
                {
                    screen[Counter, Left + 9, 0] = $"< {ColorIndex[i]} >";
                    Black = ColorIndex[i].ToString().Length + 4;
                }
                else
                    screen[Counter, Left + 9, 0] = $" {ColorIndex[i]} ";
                screen[Counter, Left + 9, 1] = $"{7}";
                screen[Counter, Left + 9, 2] = $"{i}";
                screen[Counter, Left + Black + 10, 0] = "!";
                screen[Counter, Left + Black + 10, 1] = "7";
                screen[Counter, Left + Black + 10, 2] = "7";
                Counter += 2;
            }
            //Draw some instructions to the screen
            screen[Counter + 2, Left, 0] = "Press [ENTER] or [ESC] at any time to return to the menu";
            screen[Counter + 2, Left, 1] = "4";
            UpdateFrame();
        }
        //Keep on running the colour option code until exit case
        public int[] Settings() 
        {
            int Selection = 0;
            DrawColorSettings(Selection);
            //Assign a value to a button press
            ConsoleKey myOption = Console.ReadKey().Key;
            while (myOption != ConsoleKey.Enter && myOption != ConsoleKey.Escape )
            {
                switch (myOption)
                {
                    //UP and DOWN scroll through the available colours
                    case ConsoleKey.UpArrow:
                        if(Selection > 0)
                            Selection = Selection - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        if(Selection < 5)
                            Selection = Selection + 1;
                        break;
                    //RIGHT and LEFT scrolls through the colour of that option
                    case ConsoleKey.RightArrow:
                        if (ColorIndexValues[Selection] < 14)
                            ColorIndexValues[Selection] += 1;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (ColorIndexValues[Selection] > 0)
                            ColorIndexValues[Selection] -= 1;
                        break;
                }
                //Assign the new colour values to the colour index
                InstantiateColours();
                DrawColorSettings(Selection);
                //Read a key input
                myOption = Console.ReadKey().Key;
            }
            
            UpdateFrame();
            ClearFrame();
            return ColorIndexValues;

        }
        //Draw the program logo to the screen
        private void Logo()
        {
            ClearFrame();
            Console.ForegroundColor = ConsoleColor.Red;
            String[] myLogo = {"____________________________________________________________",
            "|  ______________________________________________________  |",
            "| |     ___       __   _ __          _____     __        | |",
            "| |    / _ \\__ __/ /  (_) /__ ___   / ___/_ __/ /  __    | |",
            "| |   / , _/ // / _ \\/ /  '_/(_-<  / /__/ // / _ \\/ -_)  | |",
            "| |  /_/|_|\\_,_/_.__/_/_/\\_\\/___/  \\___/\\_,_/_.__/\\__/   | |",
            "| |         _____ ____  __ _    ____________             | |",
            "| |        / ___// __ \\/ /| |  / / ____/ __ \\            | |",
            "| |        \\__ \\/ / / / / | | / / __/ / /_/ /            | |",
            "| |       ___/ / /_/ / /__| |/ / /___/ _, _/             | |",
            "| |      /____/\\____/_____/___/_____/_/ |_|              | |",
            "| |______________________________________________________| |",
            "|__________________________________________________________|"};



            int curRow = 1;
            foreach (String s in myLogo)
            {
                int curCol = 5;
                foreach (char c in s)
                {
                    screen[curRow, curCol, 0] = c.ToString();
                    curCol += 1;
                }
                curRow += 1;
            }
        }
    }
}
