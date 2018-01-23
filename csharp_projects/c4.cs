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
        return (this.Color)[0].ToUpper();
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
        if (this[i,j] != null)
        {
            return this[i,j].ToString();
        }
        else {return  "0";}
    }
   
    //public override string ToString()
    //{
        string str = "";
        string lineSep = new String('-',7*4);
        static string BoardStr(int i, int j)
        {
            switch ( (i,j) )
            {
                case ( (0,0) ):
                    string str = 
                        (lineSep+"\n| "+
                        this.getStr(i,j ) +" ");
                    return str+boardStr(0,1);
                case ( (i,6) ):
                    string str =
                        ("| "+this.getStr(i,6)+" |\n"+
                        lineSep+"\n");
                    return str+getStr(10,10);
                case ( (10,10) ):
                    return "";
                case ( (i,j) ):
                    string str =
                        ("| "+this.getStr(i,j)+" ");
                    return str+boardStr(i,j+1);
            }
        }
    //}
                
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
