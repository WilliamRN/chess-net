using ChessNet.ConsoleGame;

GameManager gameManager = new(new ConsoleDisplay());

gameManager.Configure();
gameManager.StartMainGameLoop();
