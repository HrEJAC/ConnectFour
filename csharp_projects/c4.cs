using System;

namespace c4
{

public class Piece
{
    public string Color {get;set;}
    public Piece(string col)
    {
        Color = col;

    }


}

public class Player
{
    public int playerNumber;
    public string Color {get;set;}
    public Player(string col)
    {
        Color = col;
    }
}

public class Board{
    public Piece[,] playArea = new Piece[7,6];
    
    public string getStr() 
    {
        string boardString = "";
        foreach ( Piece elm in playArea  )
        {
            string tileObj = "" ;
            if (elm != null)
            {
                 tileObj =  elm.Color;
            }
            else {tileObj =  "0";}
            boardString = boardString + $"{tileObj}";
        }
        return boardString;
    }
            
    public void move( Player player,int column )
    {
        int row = 0;
        while ( playArea[column,row] != null )
        {
            row++;
        }
        playArea[column,row ] = new Piece(player.Color ); 
    }
}





class MainClass
{
    public static void Main()
    {
        Piece testPiece = new Piece("red");    
        Board board = new Board();
        Player elias = new Player("purple");
        Console.WriteLine("I'm alive " + testPiece.Color);
        Console.WriteLine (board.getStr() );
        board.move(elias,3);
        Console.WriteLine (board.getStr() );
    }
}
}
