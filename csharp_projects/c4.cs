using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace C4
{

public class Piece
{
    public string color {get;set;}
    public Piece(string col)
    {
        color = col;
    }
    public override string ToString()
    {
        return (((this.color).ToUpper())[0]).ToString();
    }

}

public abstract class Player
{
    public int playerNumber;
    public string color {get;set;}
    public abstract int NextMove(Board board);
    public static int Rand(int fr,int to)
    {
        Random rnd = new Random();
        return rnd.Next(fr,to);
     }
    public static Double RandDouble(Double fr, Double to){
        Random rnd = new Random();
        return rnd.NextDouble() * (to - fr) + fr;
    }
    public Player(string col)
    {
        color = col;
    }
}

public class Human : Player
{
    //checks whether input is valid move, exists when input is quit with code 9
    public override int NextMove(Board board)
    {
        int tryMove = 8;
        while (((board.AvailableMoves()).Exists 
                (x => x == tryMove-1))==false && ((tryMove == (-99))==false) )
        {
            Console.WriteLine 
                ("Type desired column to drop piece!");
            string input = Console.ReadLine();
            Int32.TryParse(input,out tryMove);
            if (input == "quit"||input=="q") {tryMove = (-99);}
        }
        return tryMove-1;
    }
    public Human(string col): base(col)
    {
        color = col;
    }
}


public class PC0 : Player
{
    public override int NextMove(Board board)
    {
        var moves = board.AvailableMoves();
        return moves[(PC0.Rand(0,moves.Count))]; 
    }
    public PC0(string col):base(col)
    {
        color = col;
    }
}

public class PC1 : Player
{
    public static Double[,] refData = new Double[7,6];
    
    //print the sheet in the object
    public static string sData(){
        string r = "";
        Double sum = 0.0;
        foreach (Double i in refData){
            r = r+i.ToString();
            sum = sum+i;
        }
        r = r+"\n\n"+sum.ToString()+"\n";
        return r;
    }    

    //delete all memory and reset the sheet
    public static void ResetData(){
        try{
            using (StreamWriter file = 
                    new StreamWriter("PC1DATA.txt", false))
            {
                file.Write("");
            }
            
            using (StreamWriter file = 
                    new StreamWriter("PC1DATA.txt", true))
            {
                string text = "1.0";
                for (int i = 0;i<=41;i++){
                    file.WriteLine(text);
                }
                file.Close();
            }
        }
        catch(Exception ex){
            Console.WriteLine("something went wrong:\n"+ex.Message);
        }
        for (int i=0;i<=6;i++){
            for (int j=0;j<=5;j++){
                refData[i,j]=1.0;
            }
        }
    }

    //take finished game as input to update machine's experience
    public static void UpdateData(int[,] data){
        //input is 1 int for all tiles
        //0 if empty, 1 if losing piece, 2 if winning piece    

        //save all previous games in RAM
        List<string> dataTXT = new List<string> {};
        StreamReader rfile =   
            new StreamReader("PC1DATA.txt");  
        //move cursor past the sheet
        for (int i=1;i<=42;i++){
            rfile.ReadLine();
        }
        //each line represents a played game in memory
        string line;
        while ((line=rfile.ReadLine())!=null){
            dataTXT.Add(line);
        }
        rfile.Close();
        
        //update datasheet (brain) with input(finished game)
        //tile rates (winning tile +3%, losing -3%)
        Double w = 1.03;
        Double l = 0.97;
        //go through all tiles and update sheet's (refData's) value
        for (int i =0;i<=6;i++){
            for (int j=0;j<=5;j++){
                Double k = 0.0;
                switch (data[i,j]){
                    //if a piece on the winning team is on this tile
                    case (2):
                        if (refData[i,j]<5.0){
                            k = (42.0-refData[i,j]*w)/(42.0-refData[i,j]);
                            refData[i,j] = refData[i,j]*w;
                            for (int ii=0;ii<=6;ii++){
                                for (int jj=0;jj<=5;jj++){
                                    if (!(ii==i&&jj==j)){
                                        refData[ii,jj]=refData[ii,jj]*k;
                                    }
                                }
                            }
                        }
                    break;
                    //if losing piece is on this tile
                    case (1):
                        if (refData[i,j]>0.2){
                            k = (42.0-refData[i,j]*l)/(42.0-refData[i,j]);
                            refData[i,j] = refData[i,j]*l;
                            for (int ii=0;ii<=6;ii++){
                                for (int jj=0;jj<=5;jj++){
                                    if (!(ii==i&&jj==j)){
                                        refData[ii,jj]=refData[ii,jj]*k;
                                    }
                                }
                            }
                        }
                    break;
                    //if tile is empty then do nothing
                    default:
                    break;
                }
            }
        }
        //update file with newest sheet
        using (StreamWriter file = 
                new StreamWriter("PC1DATA.txt", false))
        {
            //collect all info and put it in the datafile
            for (int i=0;i<=6;i++){
                for (int j=0;j<=5;j++){
                    file.WriteLine(refData[i,j].ToString());
                }
            }
            file.Close();
        }
        using (StreamWriter file = 
                new StreamWriter("PC1DATA.txt", true))
        {
            line="";
            for (int i=0;i<=6;i++){
                for(int j=0;j<=5;j++){
                    line=line+data[i,j].ToString();
                }
            }
            dataTXT.Add(line);
            for (int i=0;i<dataTXT.Count;i++){
                file.WriteLine(dataTXT[i]);
            }
            file.Close();
        }
    }
    
    //take the sheet from memoryfile and store it in the object
    public static void GetLatestSheet(){
        string line = "";
        StreamReader file =   
            new StreamReader("PC1DATA.txt");  
        for (int i=0;i<=6;i++){
            for (int j=0;j<=5;j++){
                Double num =1.0;
                line = file.ReadLine();
                Double.TryParse(line,out num);
                refData[i,j] = num;
            }
        } 
        file.Close();
    }
    
    //chooses next from availablemoves by random
    //with the weighted numbers from the sheet
    public override int NextMove(Board board){
        List<int> moves = board.AvailableMoves2d();
        List<int> moves1d = board.AvailableMoves();
        List<string> pcolors = board.PlayerColors();
        if (pcolors[0] != "" && pcolors[1] != ""){
            Board testBoard = new Board();
            testBoard.playArea = board.Copy();
            string enemy;
            if (this.color == pcolors[0]){enemy = pcolors[1];}
            else{enemy = pcolors[0];}
            int preventLoss = -1;
            for(int i=0;i<moves1d.Count;i++){
                int move = moves1d[i];
                testBoard.Move(this,move);
                if (testBoard.CheckWin()) {return move;}
                testBoard.playArea[moves[i*2],moves[i*2+1]] = null;
                testBoard.Move(new Human(enemy),move);
                if (testBoard.CheckWin()){preventLoss=move;}
                testBoard.playArea[moves[i*2],moves[i*2+1]] = null;
            }
            if (preventLoss!=-1){return preventLoss;}
        }
        List<Double> ladder = new List<Double>();  
        ladder.Add(refData[moves[0],moves[1]]);
        for (int i=2;i<moves.Count;i=i+2){
            ladder.Add(refData[moves[i],moves[i+1]]+ladder[ladder.Count-1]);
        }
        Double num = Player.RandDouble(0.0,ladder[ladder.Count-1]);
        int inc = 0;
        while (num > ladder[inc]){
            inc++;
        }
        return moves[inc*2];
    }

    public PC1(string col):base(col)
    {
        color = col;
    }
}


public class Board{
    int lastI;
    int lastJ;
    string color1 = "";
    string color2 = "";
    public Piece[,] playArea {get;set;} = new Piece[7,6];
    
    //Notation for (i,j) -> (x,y). (0,0) is lower left corner.
    
    public string GetStr(int i , int j)
    {
        if(i>=0 && i <= 6 && j >= 0 && j <= 5){
           if (playArea[i,j] != null){return playArea[i,j].ToString();}
        }
        return " ";
    }
   
    public List<string> PlayerColors(){
        return new List<string> {color1, color2};
    }
    
    public Piece[,] Copy(){
        Piece[,] copy = new Piece[7,6];
        for(int i=0;i<=6;i++){
            for (int j=0;j<=5;j++){
                copy[i,j] = playArea[i,j];
            }
        }
        return copy;
    }
    //Returns list of available columns to play. Indexed 0..6.
    public List<int> AvailableMoves()
    {
        List<int> retList = new List<int>();
        for(int i=0;i<=6;i++){
            if (playArea[i,5] == null){retList.Add(i);}
        }
        return retList;
    }
    
    public List<int> AvailableMoves2d(){
        List<int> retList = new List<int>();
        int num;
        for (int i=0;i<=6;i++){
            num = 6;
            while(num>=1&&playArea[i,num-1]==null){
                num--;
            }
            if (num <6){
                retList.Add(i);
                retList.Add(num);
            }
        }
        return retList;
    }

    //Read-friendly console print of board.
    public override string ToString()
    {
        string str = "";
        string lineSep = new string('-',29);
        for(int j=5;j>=0;j--){
            str = str + "|";

            for(int i=0;i<=6;i++){
            
                string piece = this.GetStr(i,j);
                str = str + $" {piece} |";
            
            }
            str = str + $"\n{lineSep}\n";
        }
        return str + "- 1 - 2 - 3 - 4 - 5 - 6 - 7 -\n";
    }
    
    public void Move( Player player,int column )
    {
        int row = 0;
        while ( playArea[column,row] != null ){row++;}
        playArea[column,row ] = new Piece(player.color);
        lastI = column;
        lastJ = row;
        if (color1 == ""){color1 = player.color;}
        else if (color2 == ""){color2 = player.color;}
    }

    //CheckWin() returns true if a move ends the game. 
    //Only checks for last played piece.
    public bool CheckWin()
    {
        string refStrH = " ";
        string refStrV = " ";
        string refStrDiagUp = " ";
        string refStrDiagDown = " ";

        for(int k=-3;k<=3;k++){
            refStrH += this.GetStr(lastI+k,lastJ);
            refStrDiagUp += this.GetStr(lastI+k,lastJ+k);
            refStrDiagDown += this.GetStr(lastI+k,lastJ-k);
            if(k<=0){
                refStrV += this.GetStr(lastI,lastJ+k);
            }
        }
        string playStr = new string((this.GetStr(lastI,lastJ))[0],4);
        return 
            (refStrH+refStrV+refStrDiagUp+refStrDiagDown).Contains(playStr);
    }
}

/*game class for controlling gameflow beautifully with
userfriendly messages */
public class Game{
    
    Board playBoard;
    
    //List of players initialized 
    List<Player> players = new List<Player>();
    

    //Fills players with user defined players. Can be used to reset players.
    public List<Player> CreatePlayers(){      
        List<Player> players = new List<Player>();
        Console.WriteLine("You will now choose your names and playertypes.\n");

        for (int i=1;i<=2;i++){
            Console.WriteLine
                ("Player "+i+": Please choose 'human' or 'bot1'");
            string player = Console.ReadLine();

            while (player.ToUpper() != "HUMAN" &&
                player.ToUpper() != "BOT1"){
                
                Console.WriteLine("You have to write 'human' or 'bot1'!");
                player = Console.ReadLine();
            }
            
            switch (player.ToUpper()) {
                
                case ("HUMAN"): 
                    players.Add(new Human(""));
                    break;
                
                default: 
                    players.Add(new PC1(""));
                    break;
            }
        }
        //Trash code. Manually specifying colors until we decide whether the
        //user can decide colors.
        players[0].color = "Red";
        players[1].color = "Blue";
        Console.WriteLine("Player 1 is: "+ players[0].color);
        Console.WriteLine("Player 2 is: "+ players[1].color);

        return players;
    }

    //Resets board.
    public void ResetBoard(){
        playBoard = new Board();
    }

    //Initializes players and creates board
    public void StartGameUI(){
        Console.WriteLine("Welcome to Connect Four by Asger and Elias!");
        players = this.CreatePlayers();
        this.ResetBoard();

    }

    //Starts gameflow meant for human players. AI training is another method
    public void StartGameFlow()
    {
        string playOrEnd = "";
        while(playOrEnd.ToUpper() != "Q" && playOrEnd.ToUpper() != "QUIT")
        {
            Console.WriteLine("New Game is starting:");
            this.ResetBoard();

            Console.WriteLine(playBoard.ToString());
            
            //While TakeMove() doesn't report game ending, print board
            //and take next move.
            while(this.TakeMove()==false)
            {
                Console.WriteLine(playBoard.ToString());
            }
            Console.WriteLine("Final board:\n" + playBoard.ToString());
            Console.WriteLine("'q' to quit:");
            playOrEnd = Console.ReadLine();
        } 
    }

    //Take move for all players returning true if game ends.
    bool TakeMove(){
        for (int i = 0; i < players.Count;i++)
        {
            Console.WriteLine(players[i]);
            playBoard.Move(players[i], players[i].NextMove(playBoard));
            if(playBoard.CheckWin()==true ||
                    playBoard.AvailableMoves().Count == 0 )
            {
                return true;
            }
        }
        return false;       
    }

    public void TrainingSessionPC1(int n){
        PC1 winner;
        List<PC1> playersT = new List<PC1> {new PC1("red"),new PC1("blue")};
        int[,] intBoard = new int[7,6];
        PC1.GetLatestSheet();
        for (int k=1;k<=n;k++){
            Console.WriteLine("Starting game "+k+" of "+n);
            this.ResetBoard();
            int winNum = this.TrainingGamePC1(playersT);
            Console.WriteLine(winNum);
            if (winNum < 42) {
                winner = playersT[winNum%2];
                for (int i=0;i<=6;i++){
                    for (int j=0;j<=5;j++){
                        if(playBoard.GetStr(i,j)==
                            ((winner.color).ToUpper())[0].ToString()){
                            intBoard[i,j] = 2;
                        }
                        else{if (playBoard.GetStr(i,j) == " "){
                           intBoard[i,j] = 0;
                           }
                        else {intBoard[i,j] = 1;}
                        }
                    }
                }
                PC1.UpdateData(intBoard);
                PC1.GetLatestSheet();
            }
        }
    }
    
    //plays game with two PC1s against eachother and returns winner
    int TrainingGamePC1(List<PC1> playersT){
        int turn = 1;
        playBoard.Move(playersT[0],playersT[0].NextMove(playBoard));
        while(!(playBoard.CheckWin()) && turn<42 ){
            PC1 pTurn = playersT[turn%2];
            System.Threading.Thread.Sleep(1);
            playBoard.Move(pTurn,pTurn.NextMove(playBoard));
            turn++;
            Console.WriteLine(playBoard);
        }
            //Console.WriteLine(playBoard);
        return turn;
    }
}

class MainClass
{
    public static void Main()
    {
        Game trainingGame = new Game();
        //trainingGame.TrainingSessionPC1(1);
        //PC1.ResetData();
        trainingGame.StartGameUI();
        trainingGame.StartGameFlow();


//<<<<<<< HEAD
        /* Example of new gameflow controls:
         *
         * Game gameFlow = new Game();
         * gameFlow.StartGameUI();
         * gameFlow.StartGameFlow();
        
=======
        Board board = new Board();
        Player elias = new Human("red");
        Player asger = new PC1("blue");
        while(true){
            int mv = elias.NextMove(board);
            board.Move(elias,mv);
            Console.WriteLine(board.ToString());
            Console.WriteLine(board.CheckWin());

            int mv2 = asger.NextMove(board);
            board.Move(asger,mv2);
            Console.WriteLine(board.ToString());
            Console.WriteLine(board.CheckWin());
        }
//>>>>>>> refs/remotes/origin/master*/
    }
}
}
