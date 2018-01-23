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
    public override string ToString()
    {
        return this.Color;
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
    
    public string getStr(int i, int j) 
    {
        if (playArea[i,j] != null)
        {
            return playArea[i,j].ToString();
        }
        else {return  "0";}
    }
   
    public override string ToString()
    {
        string str = "";
        string lineSep = new string('-',29);
        for(int i=5;i>=0;i--){
            str = str + "|";

            for(int j=6;j>=0;j--){
            
                string piece;
                if (playArea[j,i] != null){piece = "o";}else{piece = " ";}
                str = str + $" {piece} |";
            
            }
            str = str + $"\n{lineSep}\n";
        }

        return str;
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
        //Console.WriteLine (board.getStr() );
        board.move(elias,3);
        Console.WriteLine (board.ToString());
    }
}
}
