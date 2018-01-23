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
        return (((this.Color).ToUpper())[0]).ToString();
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
    public string getStr(int i , int j)
    {
        if(i>=0 && i <= 6 && j >= 0 && j <= 5){
           if (playArea[i,j] != null){return playArea[i,j].ToString();}
        }
        return " ";
    }

    public override string ToString()
    {
        string str = "";
        string lineSep = new string('-',29);
        for(int j=5;j>=0;j--){
            str = str + "|";

            for(int i=0;i<=6;i++){
            
                string piece = this.getStr(i,j);
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
        Player elias = new Player("red");
        Player asger = new Player("blue");
        
        
        Console.WriteLine("I'm alive " + testPiece.Color);
        Console.WriteLine(board.ToString() );
        Console.WriteLine("Elias er " + elias.Color + ". Elias mover 3.");
        Console.WriteLine("Asger er " + asger.Color + ". Asger mover 4.");
        Console.WriteLine("Elias er " + elias.Color + ". Elias mover 3.");
        board.move(elias,3);
        board.move(asger,4);
        board.move(elias,3);

        Console.WriteLine (board.ToString());
    }
}
}
