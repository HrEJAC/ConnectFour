using System;

namespace application
{
public class Field{
    public string State {get; set;}
}

public class Player{
    public string Name {get;set;}
    public int Score {get;set;}

    public Player(string name, int score)
    {
        Name = name;
        Score = score;
    }

}

class MainClass
{
    public static void Main(string[] args)
    {
        Field[,] playBoard = new Field[2,2];
        Player[] playerArr = new Player[1];
        
        string cmd="";
        int winState=0;
        int playerTurn = 2;
        
        for (int i=1; i<=2; i++){
            Console.WriteLine(playerArr.Length);
            Console.WriteLine("Player " + i + " name:");
            playerArr[i-1].Name = Console.ReadLine();
        }   

        string message = "whattup";
        Console.WriteLine(message + " " + playerArr[0].Name);

        while (cmd != "q"){
            while(winState != 1){
                if(playerTurn % 2 == 1){
                    
                }
            
            }       

            Console.WriteLine("Enter q to quit:");
            cmd = Console.ReadLine();
        }
    }
}
}
