using System;
using HiloGame.Domain.Services;
using HiLoGame.SinglePlayerPOC.Console;


var consoleIO = new ConsoleIO(); 
var randomNumberService = new RandomNumberService(); 
IGameService gameService = new GameService(randomNumberService);
var gameController = new GameController(consoleIO, gameService);
var applicationController = new ApplicationController(consoleIO, gameController);

applicationController.Run();


/// <summary>
/// Abstraction for console input/output operations to enable testability.
/// </summary>
public interface IConsoleIO
{
    void WriteLine(string message);
    void Write(string message);

    ConsoleKey ReadKey();
    string? ReadLine();
}


/// <summary>
/// Implementation of console I/O operations.
/// </summary>
public class ConsoleIO : IConsoleIO
{
    public void WriteLine(string message) => Console.WriteLine(message);

    public void Write(string message) => Console.Write(message);

    public string? ReadLine() => Console.ReadLine();

    public ConsoleKey ReadKey() => Console.ReadKey(true).Key;
    
}




