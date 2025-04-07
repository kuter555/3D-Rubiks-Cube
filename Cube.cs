using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Rubiks_Cube
{
    //This class is what handles all the rubiks cube maths and algorithms 
    public class Cube
    {
        //define a rubiks cube in a 3d array
        //the outermost identifier relates to the face, then the column, then the row
        public int[,,] cube = new int[6, 2, 2];
        private List<String> MovesDone = new List<String>();
        private int SolvingColor, oppositeFace;

        //                    2 2
        //                    2 2
        //
        //            0 0     5 5     4 4    1 1
        //            0 0     5 5     4 4    1 1
        //              
        //                    3 3
        //                    3 3

        //Looking at this net, the face looking at us is the front, with 2 being on top and 3 being bottom

        //For the array, [0,0] is top left, [0,1] is top right, [1,0] us bottom left and [1,1] is bottom right

        //construct the cube
        public Cube()
        {
            //for each face you have to assign different values
            for (int face = 0; face < 6; face++)
            {
                for (int column = 0; column < 2; column++)
                {
                    for (int row = 0; row < 2; row++)
                        //I opted to use numbers to declare the colours of the cube I did this because it would be easier to manipulate numbers
                        //in an array rather than manipulating console colours, which would still be possible but be a lot more complicated
                        cube[face, column, row] = face;
                }
            }
        }

        //Allows a user to change a colour on the cube
        public void ChangeCubeFace(int face, int x, int y, int Color)
        {
            cube[face, x, y] = Color; 
        }
        
        //Accessor function to return the moves that you have done.
        //I have made it so because there is no need for any other class to write the moves done
        public String movesDone
        {
            get 
            {
                String myReturn = ListToString(MovesDone);
                return $"{myReturn}"; 
            }
            set { MovesDone = new List<String>(); }
        }

        //Convert the moves done list to a readable string
        private string ListToString(List<String> myList)
        {
            String alpha = "";
            foreach(String s in myList)
                alpha += s;
            String beta = "";
            for (int i = 0; i < alpha.Length; i++)
                beta += alpha[i];
            //After adding all the values to a string, make this string fit proper cube notation, eg ' is prime, and 2 is double move
            return TidyMoves(beta);
        }

        //Reset the moves done to be empty
        public void ClearMoves()
        {
            MovesDone = new List<String>();
        }

        //Convert a string of moves into proper cube notation
        public string TidyMoves(string myMoves)
        {
            //Regular expressions to convert different letter amounts
            string[,] Patterns = new string[,] {
            { @"U U'", @"D D'",@"B B'",@"L L'",@"R R'",@"F F'", "" },
            { @"U' U",@"D' D",@"B' B",@"L' L",@"R' R",@"F' F", ""},
            { @"U{4}", @"D{4}", @"B{4}", @"L{4}", @"R{4}", @"F{4}", "" } ,
            { @"U{3}", @"D{3}", @"B{3}", @"L{3}", @"R{3}", @"F{3}", "\'" },
            { @"U{2}", @"D{2}", @"B{2}", @"L{2}", @"R{2}", @"F{2}", "2" }};
            //Go through each of the regular expressions
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    //For each match, replace that instance with the correct notation, which is stored in the 6th position of each sub array in Patterns
                    Regex RE = new Regex(Patterns[i, j]);
                    if (i <= 2)
                        myMoves = RE.Replace(myMoves, $"{Patterns[i, 6]}");
                    else
                        myMoves = RE.Replace(myMoves, $"{Patterns[i, j][0]}{Patterns[i, 6]}");
                }
            }
            //For each letter, add a space before that letter, so that it is more legible
            string[] Letters = new string[] { @"U", @"D", @"B", @"L", @"R", @"F" };
            foreach (string f in Letters)
            {
                Regex RE = new Regex(f);
                myMoves = RE.Replace(myMoves, $" {f[0]}");
            }
            return myMoves;
        }

        //Code that shuffles the cube by using random integers for different moves
        public string Shuffle()
        {
            Random r = new Random();

            //Generate a random 10-20 move shuffle which gives north of 9 trillion different shuffle possiblilities
            for (int i = 0; i < r.Next(10, 20); i++)
            {
                int Face = r.Next(0, 6);
                MoveCube(Face.ToString());
            }
            return movesDone;
        }

        //Takes a string of an algorithm and converts it into moves to be done on the cube
        public void CommenceAlgorithm(string Algorithm)
        {
            String[] movesToComplete = Regex.Split(Algorithm, ",");

            //Can be separated by either , or space 
            foreach (string S in movesToComplete)
            {
                String[] movesToComplete2 = Regex.Split(S, " ");
                //Do each of these moves on the cube
                foreach (string M in movesToComplete2)
                    MoveCube(M);
            }
        }


        //Using the MovesDone list as a stack to undo a move
        public void UndoMove()
        {
            if(MovesDone.Count > 0)
            {
                //Find the most recent Move that has been done
                String myMove = MovesDone[MovesDone.Count - 1];

                MoveCube(myMove + "'");
                //Remove all the moves that you have just done, so as to not create a loop
                for(int i = 0; i < 4; i ++)                   
                    MovesDone.RemoveAt(MovesDone.Count - 1);
            }
        }
        //Allows the user to repeat a move they ahve just done
        public void RedoMove()
        {
            if (MovesDone.Count > 0)
            {
                string MyMove = MovesDone[MovesDone.Count - 1];
                MoveCube(MyMove[0].ToString());
            }
        }

        //Code that makes a move on the cube
        public void MoveCube(string Move)
        {
            String[] moveIndex = { "L", "B", "U", "D", "R", "F" };            
            
            //Face indicates the main face that you are moving in the code. Face numbers are commented at start of file.
            int Face = 0;
            //If the move that has been entered is a valid move.
            bool ValidMove = true;
            //PMove, or "prime move" idicates if the rotation of the cube face is clockwise or anticlockwise, or if its a double move
            int PMove = 1;
            if (Move.Length > 2)
            {
                ValidMove = false;
            }
            else if (Move.Length < 1)
                ValidMove = false;
            else
            {
                try
                {
                    //Attempt to change the move into an integer. As it is a character and int.Parse only works for string, convert it into a string first
                    Face = int.Parse(Move[0].ToString());
                }
                catch
                {
                    //If the move isnt already in integer form, check to see if it's a letter
                    switch (Move[0].ToString().ToUpper())
                    {
                        case "L":
                            Face = 0;
                            break;
                        case "B":
                            Face = 1;
                            break;
                        case "U":
                            Face = 2;
                            break;
                        case "D":
                            Face = 3;
                            break;
                        case "R":
                            Face = 4;
                            break;
                        case "F":
                            Face = 5;
                            break;
                        default:
                            ValidMove = false;
                            break;
                    }
                }
            }
            if (ValidMove)
            {
                if (Move.Length > 1)
                {
                    if (Move[1] == Char.Parse("'"))
                        PMove = 3;
                    else if (Move[1] == '2')
                        PMove = 2;
                    else
                        ValidMove = false;
                }

                //add the move to the string that holds all the moves completed, for later inspection              
               
                //Code for making a move in the cube
                //Takes the values at a point and moves it around the cube.
                for (int i = 0; i < PMove; i++)
                {
                    MovesDone.Add(moveIndex[Face]);
                    int tempVal = cube[Face, 0, 0];
                    cube[Face, 0, 0] = cube[Face, 1, 0];
                    cube[Face, 1, 0] = cube[Face, 1, 1];
                    cube[Face, 1, 1] = cube[Face, 0, 1];
                    cube[Face, 0, 1] = tempVal;

                    switch (Face)
                    {
                        case 0:
                            tempVal = cube[5, 0, 0];
                            cube[5, 0, 0] = cube[2, 0, 0];
                            cube[2, 0, 0] = cube[1, 1, 1];
                            cube[1, 1, 1] = cube[3, 0, 0];
                            cube[3, 0, 0] = tempVal;
                            tempVal = cube[5, 1, 0];
                            cube[5, 1, 0] = cube[2, 1, 0];
                            cube[2, 1, 0] = cube[1, 0, 1];
                            cube[1, 0, 1] = cube[3, 1, 0];
                            cube[3, 1, 0] = tempVal;
                            break;
                        case 1:
                            tempVal = cube[2, 0, 0];
                            cube[2, 0, 0] = cube[4, 0, 1];
                            cube[4, 0, 1] = cube[3, 1, 1];
                            cube[3, 1, 1] = cube[0, 1, 0];
                            cube[0, 1, 0] = tempVal;
                            tempVal = cube[2, 0, 1];
                            cube[2, 0, 1] = cube[4, 1, 1];
                            cube[4, 1, 1] = cube[3, 1, 0];
                            cube[3, 1, 0] = cube[0, 0, 0];
                            cube[0, 0, 0] = tempVal;
                            break;
                        case 2:
                            tempVal = cube[0, 0, 0];
                            cube[0, 0, 0] = cube[5, 0, 0];
                            cube[5, 0, 0] = cube[4, 0, 0];
                            cube[4, 0, 0] = cube[1, 0, 0];
                            cube[1, 0, 0] = tempVal;
                            tempVal = cube[0, 0, 1];
                            cube[0, 0, 1] = cube[5, 0, 1];
                            cube[5, 0, 1] = cube[4, 0, 1];
                            cube[4, 0, 1] = cube[1, 0, 1];
                            cube[1, 0, 1] = tempVal;
                            break;
                        case 3:
                            tempVal = cube[0, 1, 0];
                            cube[0, 1, 0] = cube[1, 1, 0];
                            cube[1, 1, 0] = cube[4, 1, 0];
                            cube[4, 1, 0] = cube[5, 1, 0];
                            cube[5, 1, 0] = tempVal;
                            tempVal = cube[0, 1, 1];
                            cube[0, 1, 1] = cube[1, 1, 1];
                            cube[1, 1, 1] = cube[4, 1, 1];
                            cube[4, 1, 1] = cube[5, 1, 1];
                            cube[5, 1, 1] = tempVal;
                            break;
                        case 4:
                            tempVal = cube[5, 0, 1];
                            cube[5, 0, 1] = cube[3, 0, 1];
                            cube[3, 0, 1] = cube[1, 1, 0];
                            cube[1, 1, 0] = cube[2, 0, 1];
                            cube[2, 0, 1] = tempVal;
                            tempVal = cube[5, 1, 1];
                            cube[5, 1, 1] = cube[3, 1, 1];
                            cube[3, 1, 1] = cube[1, 0, 0];
                            cube[1, 0, 0] = cube[2, 1, 1];
                            cube[2, 1, 1] = tempVal;
                            break;
                        case 5:
                            tempVal = cube[2, 1, 1];
                            cube[2, 1, 1] = cube[0, 0, 1];
                            cube[0, 0, 1] = cube[3, 0, 0];
                            cube[3, 0, 0] = cube[4, 1, 0];
                            cube[4, 1, 0] = tempVal;
                            tempVal = cube[2, 1, 0];
                            cube[2, 1, 0] = cube[0, 1, 1];
                            cube[0, 1, 1] = cube[3, 0, 1];
                            cube[3, 0, 1] = cube[4, 0, 0];
                            cube[4, 0, 0] = tempVal;
                            break;

                    }
                }
            }
        }



        // ====================================================== WHERE THE AI BEGINS ============================================================================  \\

        //Three sections to solving the cube
        public void Solve()
        {
            ClearMoves();
            SolveSide1();
            SolveSide2();
            SolveSide3();
        }
        
        //Tells you how many of the desired color is on what face
        private int[] HowManyOnFace(int Face, int Colour)
        {
            int Number = 0;
            int[] FaceDetails = new int[5];

            int Counter = 1;
            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (cube[Face, i, j] == Colour)
                    {
                        FaceDetails[Counter] = 1;
                        Number += 1;
                    }
                    Counter += 1;
                }
            }
            FaceDetails[0] = Number;
            return FaceDetails;
        }

        //Bool which returns if the cube is fully solved or not
        public bool IsCubeSolved()
        {
            bool Solved = true;
            for(int i = 0; i < 6; i++)
            {
                int curSide = cube[i, 0, 0];
                if(!(cube[i,0,1] == curSide && cube[i,1,0] == curSide && cube[i,1,1] == curSide))
                {
                    Solved = false;
                }
            }
            return Solved;

        }

        //Different algorithms that get used in the code
        public void AlignEdgeAlgorithm()
        {
            CommenceAlgorithm("R,D2,R',D',R,D',R',L',D2,L,D,L',D,L");
        }
        public void AlignEdgeAlgorithmPrime()
        {
            CommenceAlgorithm("L',D',L,D',L',D2,L,R,D,R',D,R,D2,R'");
        }
        public void PositionSideAlgorithm1()
        {
            CommenceAlgorithm("R2,U',B2,U2,R2,U',R2");
        }
        public void PositionSideAlgorithm2()
        {
            CommenceAlgorithm("R U R' U' R' F R2 U' R' U' R U R' F'");
        }
        public void PositionSideAlgorithm3()
        {
            CommenceAlgorithm("F R U' R' U' R U R' F' R U R' U' R' F R F'");
        }

        //Solve the first side of the cube
        public void SolveSide1()
        {
            //Look for moves that solve the White face. I am going to solve this for the white face first, and then afterwards depending on how things are going I will make it apply to all faces
            //See how many of the correct pieces are on the top
            //Work out if any white pieces are on the side layers
            SolvingColor = 2;

            bool KeepSolving = true, LowerRow = false;
            int[] fixedCubies = new int[5], BottomFace = new int[5], Frontface = new int[5];

            //See how many are solved
            while (KeepSolving)
            {
                if (IsCubeSolved())
                    KeepSolving = false;

                fixedCubies = HowManyOnFace(SolvingColor, SolvingColor);
                BottomFace = HowManyOnFace(3, SolvingColor);

                if ((fixedCubies[0] < 4) && (BottomFace[0] == 0))
                {
                    for (int Move = 0; Move < 4; Move++)
                    {
                        MoveCube("D");
                        if (cube[5, 1, 0] == SolvingColor || cube[5, 1, 1] == SolvingColor)
                        {
                            LowerRow = true;
                            Move = 4;
                        }
                    }

                    //if the pieces are on the lower level. THE EASY Scenario
                    if (LowerRow)
                    {
                        if (cube[5, 1, 0] == SolvingColor)
                        {
                            while (cube[SolvingColor, 1, 0] == SolvingColor)
                                MoveCube("U");
                            CommenceAlgorithm("D,L,D',L'");
                        }
                        else
                        {
                            while (cube[SolvingColor, 1, 1] == SolvingColor)
                                MoveCube("U");
                            CommenceAlgorithm("D',R',D,R");
                        }
                    }
                    else
                    {
                        while (cube[5, 0, 0] != SolvingColor || cube[5, 0, 0] != SolvingColor)
                            MoveCube("U");
                        if(cube[5,0,0] == SolvingColor)
                        {
                            CommenceAlgorithm("F',D',F,D2,L,D',L'");
                        }
                        else
                        {
                            CommenceAlgorithm("F,D,F',D2,R',D,R");
                        }
                    }
                }

                //If there is one/no solved cubies, or there are two and the two are not diagonal from each other
                else if ((fixedCubies[0] <= 1) || (fixedCubies[0] == 2 && ((fixedCubies[1] != fixedCubies[4]) || (fixedCubies[2] != fixedCubies[3]))) && (BottomFace[0] > 0))
                {
                    BottomFace = HowManyOnFace(3, SolvingColor);
                    //If the bottom face has the right color
                    //while parts [2] and [4] are empty, do a D move
                    //while top face parts [2] and [4] are not empty, do a U move
                    //Do an R2 move;

                    //If there are any pieces on the bottom face
                    if (BottomFace[0] > 0)
                    {
                        if (BottomFace[0] >= 2)
                        {
                            while (BottomFace[2] != 1)
                            {
                                MoveCube("D");
                                //3 will need to be changed to the opposite to the solving color
                                BottomFace = HowManyOnFace(3, SolvingColor);
                            }
                        }
                        else
                        {
                            while (BottomFace[2] != 1)
                            { 
                                MoveCube("D");
                                BottomFace = HowManyOnFace(3, SolvingColor);
                            }
                        }
                        while(fixedCubies[2] == 1 || fixedCubies[4] == 1)
                        {
                            MoveCube("U");
                            fixedCubies = HowManyOnFace(2, SolvingColor);
                        }
                        MoveCube("R2");
                    }
                }
                //If the fixed pieces are diagonal, get pieces up from the bottom face
                else if((fixedCubies[0] == 2 && ((fixedCubies[1] == fixedCubies[4]) || (fixedCubies[2] == fixedCubies[3]))) || fixedCubies[0] == 3)
                {
                    BottomFace = HowManyOnFace(3, SolvingColor);
                    {
                        if(BottomFace[0] > 0)
                        {
                            while(BottomFace[4] != 1)
                            {
                                MoveCube("D");
                                BottomFace = HowManyOnFace(3, SolvingColor);
                            }
                            while(fixedCubies[2] == 1)
                            {
                                MoveCube("U");
                                fixedCubies = HowManyOnFace(2, SolvingColor);
                            }
                            CommenceAlgorithm("R,D2,R',D2,B',D',B");
                        }
                    }
                }

                //Keep solving until the top face is solved
                fixedCubies = HowManyOnFace(2, SolvingColor);
                BottomFace = HowManyOnFace(3, SolvingColor);

                if (fixedCubies[0] == 4)
                    KeepSolving = false;
            }
        }

        public void SolveSide2()
        {
            //Check how many of the pieces on the opposite face are the right way around
            bool side2 = true;
            oppositeFace = 3;
            int CheckFace = 4;
            //Keep on looping until the cube is solved
            while (side2)
            {
                if (IsCubeSolved())
                    side2 = false;
                int[] Side2Count = HowManyOnFace(oppositeFace, oppositeFace);
                //if side 2 is solved, then stop trying to solve it
                if (Side2Count[0] == 4)
                {
                    side2 = false;
                }
                //if side 2 has only one solved face, then do the algorithm to fix it
                else if (Side2Count[0] == 1)
                {
                    int counter = 0;
                    while (cube[oppositeFace, 0, 0] != oppositeFace && counter <= 4)
                    {
                        MoveCube("D");
                        counter += 1;
                    }
                    if(cube[4,1,0] == oppositeFace)
                    {
                        AlignEdgeAlgorithm();
                        MoveCube("D'");
                        AlignEdgeAlgorithmPrime();
                    }
                    else
                    {
                        MoveCube("D'");
                        AlignEdgeAlgorithm();
                        MoveCube("D");
                        AlignEdgeAlgorithmPrime();
                    }
                }
                //if there are two pieces
                else
                {
                    //if they are connected faces
                    if (cube[oppositeFace,0,0] == cube[oppositeFace,1,1] || cube[oppositeFace,0,1] == cube[oppositeFace, 1, 0])
                    {
                        int counter = 0;
                        while (cube[oppositeFace, 0, 0] != oppositeFace && counter <= 4)
                        {
                            counter += 1;
                            MoveCube("D");
                        }
                        if(cube[CheckFace,1,0] == oppositeFace)
                        {
                            AlignEdgeAlgorithm();
                            MoveCube("D'");
                            AlignEdgeAlgorithm();
                        }
                        else
                        {
                            MoveCube("D");
                            AlignEdgeAlgorithm();
                            MoveCube("D");
                            AlignEdgeAlgorithm();
                        }
                    }

                    //if they are not connected
                    else
                    {
                        int counter = 0;
                        while (cube[oppositeFace, 0,0] != oppositeFace && counter <= 4)
                        {
                            MoveCube("D");
                            counter += 1;
                        }
                        if (cube[CheckFace, 1, 0] == oppositeFace)
                            AlignEdgeAlgorithm();
                        else
                            AlignEdgeAlgorithmPrime();
                    }
                }
                
            }
        }
        //Solve the sides inbetween the top and bottom face
        public void SolveSide3()
        {
            //Keep solving until the cube is solved.
            bool KeepSolving = true;
            while (KeepSolving)
            {
                if (IsCubeSolved())
                    KeepSolving = false;

                //Work out which sides are remaining after solving top and bottom
                int[] middleSides = new int[4];
                int pointer = 0;

                for (int i = 0; i < 6; i++)
                {
                    if (i != oppositeFace && i != SolvingColor)
                    {
                        middleSides[pointer] = i;
                        pointer += 1;
                    }

                }



                //Calculate how many of the top and bottom levels are solved
                int validTops = 0, validBottoms = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (cube[middleSides[i], 0, 0] == cube[middleSides[i], 0, 1])
                    {
                        validTops += 1;
                        //whichTop = cube[middleSides[i], 0, 0];
                    }
                    if (cube[middleSides[i], 1, 0] == cube[middleSides[i], 1, 1])
                    {
                        validBottoms += 1;
                        //whichBottom = cube[middleSides[i], 1, 0];
                    }
                }


                //5 Different PBL Situations

                //One top, one bottom
                if (validTops == 1 && validBottoms == 1)
                {
                    while (cube[5, 0, 0] != cube[5, 0, 1])
                        MoveCube("U");
                    while (cube[5, 1, 0] != cube[5, 1, 1])
                        MoveCube("D");
                    PositionSideAlgorithm1();
                }

                //One top
                else if (validTops == 1 && validBottoms == 0 || validTops == 0 && validBottoms == 1)
                {
                    //get it so the top is facing forwards
                    while (cube[5, 0, 0] != cube[5, 0, 1] && cube[5, 1, 0] != cube[5, 1, 1])
                    {
                        MoveCube("U");
                        MoveCube("D");
                    }
                    //make sure the cube is the right way round
                    if (cube[5, 1, 0] == cube[5, 1, 1])
                        CommenceAlgorithm("F2 B2");

                    //Solve the cube
                    CommenceAlgorithm("R U' R F2 R' U R'");
                }

                //None of either
                else if (validTops == 0 && validBottoms == 0)
                {
                    CommenceAlgorithm("R2,F2,R2");
                }

                //One top, four bottoms
                else if ((validTops == 1 && validBottoms == 4) || (validTops == 4 && validBottoms == 1))
                {
                    if (validBottoms == 1)
                        CommenceAlgorithm("F2 B2");
                    while (cube[0, 0, 0] != cube[0, 0, 1])
                        MoveCube("U");
                    while (cube[0, 0, 0] != cube[0, 1, 0])
                        MoveCube("D");
                    PositionSideAlgorithm2();

                }

                //No top, four bottoms
                else if ((validTops == 0 && validBottoms == 4) || (validTops == 4 && validBottoms == 0))
                {
                    if (validBottoms == 0)
                        CommenceAlgorithm("F2 B2");
                    PositionSideAlgorithm3();
                }

                //Four tops, four bottoms
                else if (validTops == 4 && validBottoms == 4)
                {
                    while (cube[middleSides[0], 0, 0] != cube[middleSides[0], 1, 0] || cube[middleSides[0], 0, 1] != cube[middleSides[0], 1, 1])
                        MoveCube("D");
                    KeepSolving = false;
                }

                else if (validTops == 3 && validBottoms == 4)
                {
                    if (cube[2, 1, 0] != cube[2,0,0])
                        cube[2, 1, 0] = cube[2, 0, 0];

                    else if (cube[2, 1, 1] != cube[2,0,0])
                        cube[2, 1, 1] = cube[2, 0, 0];

                    while (cube[5, 0, 0] == cube[5, 0, 1])
                    {
                        MoveCube("U");
                    }
                    if (cube[5, 0, 0] == SolvingColor)
                        cube[5, 0, 0] = cube[5, 0, 1];
                    else
                        cube[5, 0, 1] = cube[5, 0, 0];
                }
            }
        }
    }
}


