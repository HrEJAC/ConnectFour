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

public class Player
{
    public int playerNumber;
    public string color {get;set;}
    public Player(string col)
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
        string refStr1=" ";
        string refStr2=" ";
        string refStr3=" ";
        string refStr4=" ";

        for(int k=-3;k<=3;k++){
            refStr1 += this.GetStr(lastI+k,lastJ);
            refStr2 += this.GetStr(lastI,lastJ+k);
            refStr3 += this.GetStr(lastI+k,lastJ+k);
            refStr4 += this.GetStr(lastI+k,lastJ-k);
        }
        string playStr = new string((this.GetStr(lastI,lastJ))[0],4);
        return (refStr1+refStr2+refStr3+refStr4).Contains(playStr);
    }
}





class MainClass
{
    public static void Main()
    {
        Piece testPiece = new Piece("red");    
        Board board = new Board();
        Player elias = new Player("red");
        Player asger = new Player("blue");
        
        
        Console.WriteLine("I'm alive " + testPiece.color);
        Console.WriteLine(board.ToString() );

        while(true){
            Console.WriteLine("elias turn:");
            board.Move(elias,Int32.Parse(Console.ReadLine()));
            Console.WriteLine(board.ToString());
            Console.WriteLine(board.CheckWin());

            Console.WriteLine("asger turn:");
            board.Move(asger,Int32.Parse(Console.ReadLine()));
            Console.WriteLine(board.ToString());
            Console.WriteLine(board.CheckWin());
        }
        
        Console.WriteLine (board.ToString());
    }
}
}
