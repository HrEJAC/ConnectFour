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
                (x => x == tryMove))==false && ((tryMove == (-99))==false) )
        {
            Console.WriteLine 
                ("Type desired column to drop piece!");
            string input = Console.ReadLine();
            Int32.TryParse(input,out tryMove);
            if (input == "quit") {tryMove = (-99);}
        }
        return tryMove;
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
    public Double[,] refData = new Double[7,6];
    
    //print the sheet in the object
    public string sData(){
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
    public void resetData(){
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
    public void UpdateData(int[,] data){
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
                        k = (42.0-refData[i,j]*w)/(42.0-refData[i,j]);
                        refData[i,j] = refData[i,j]*w;
                        for (int ii=0;ii<=6;ii++){
                            for (int jj=0;jj<=5;jj++){
                                if (!(ii==i&&jj==j)){
                                    refData[ii,jj]=refData[ii,jj]*k;
                                }
                            }
                        }
                    break;
                    //if losing piece is on this tile
                    case (1):
                        k = (42.0-refData[i,j]*l)/(42.0-refData[i,j]);
                        refData[i,j] = refData[i,j]*l;
                        for (int ii=0;ii<=6;ii++){
                            for (int jj=0;jj<=5;jj++){
                                if (!(ii==i&&jj==j)){
                                    refData[ii,jj]=refData[ii,jj]*k;
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
    public void GetLatestSheet(){
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

    public override int NextMove(Board board){
        return 3;
    }

    public PC1(string col):base(col)
    {
        color = col;
    }
}


public class Board{
    int lastI;
    int lastJ;

    public Piece[,] playArea = new Piece[7,6];
    //Notation for (i,j) -> (x,y). (0,0) is lower left corner.
    
    public string GetStr(int i , int j)
    {
        if(i>=0 && i <= 6 && j >= 0 && j <= 5){
           if (playArea[i,j] != null){return playArea[i,j].ToString();}
        }
        return " ";
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
    public List<Player> players = new List<Player>();
    public List<Player> CreatePlayers(){      
        List<Player> players = new List<Player>();
        Console.WriteLine
            ("You will now choose your names and playertypes.\n");
        for (int i=1;i<=2;i++){
            Console.WriteLine 
                ("Please choose 1 of following; 'human', 'bot1'");
            string player = Console.ReadLine();
            while (player.ToUpper() != "human".ToUpper() ||
                player.ToUpper() != "bot1".ToUpper()){
                Console.WriteLine("You have to write 'human' or 'bot1'!");
                player = Console.ReadLine();
            }
            Console.WriteLine("Type the player's name!");
            string playerName = Console.ReadLine();
            switch (player.ToUpper()) {
                case ("HUMAN"): players.Add((Player)(new Human(playerName)));
                break;
                default: players.Add((Player)(new PC0(playerName)));
                break;
            }
        }
        return players;
    }
    public string StartGameGUI(){
        Console.WriteLine
            ("Welcome to Connect Four by Asger and Elias!");
        players = this.CreatePlayers();
        return "";
    }
    
    
}

class MainClass
{
    public static void Main()
    {
        Piece testPiece = new Piece("red");    
        Board board = new Board();
        Player elias = new Human("red");
        Player asger = new PC0("blue");
        PC1 els = new PC1("green");
        els.resetData();
        Console.WriteLine(els.sData());
        els.UpdateData(new int[,] {{1,2,9,9,5,6},{9,9,9,9,5,6},{3,3,3,3,5,6},{4,4,4,4,5,6},{9,9,3,4,5,6},{9,9,3,4,5,6},{9,9,3,4,5,6}});
        els.UpdateData(new int[,] {{1,2,9,9,5,6},{9,9,9,9,5,6},{3,3,3,3,5,6},{4,4,4,4,5,6},{9,9,3,4,5,6},{9,9,3,4,5,6},{9,9,3,4,5,6}});
        els.GetLatestSheet();
        Console.WriteLine(els.sData());
        Console.WriteLine("I'm alive " + testPiece.color);
        Console.WriteLine(board.ToString() );

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
    }
}}
