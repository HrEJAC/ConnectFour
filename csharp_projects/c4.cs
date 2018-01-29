using System;
using System.Collections.Generic;

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
        
        Console.WriteLine (board.ToString());
    }
}}
