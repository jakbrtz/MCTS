# simple-MCTS
This is a [Monte Carlos Tree Search algorithm](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search) for playing simple games of perfect information. 

The entire algorithm and user interface is in a single file: [Program.cs](UltimateTicTacToe/Program.cs)

In this file, I have programmed it to play the game [Ultimate TicTacToe](https://ultimate-t3.herokuapp.com). You can play by typing the coordinate that you want to play (eg the top-right cell in the top-middle board is 61).

The MCTS algorithm can be adapted to any game by changing the code where ever it says `FIXME`. 
The `PickNextMove` function does not contain any places which say `FIXME`. 
Instead, it uses helper functions (eg `state.MoveIsLegal` and `state.GameOver`) which need to be modified to represent the game. 
It also uses integers to represent possible moves.

You can also control the flow of the game with these variables:

 * `autoRestart = false;`
   
   Does the game automatically restart at the end?
   
 * `Xhuman = true;`
   
   Is a human playing as X?
   
 * `Ohuman = false;`
   
   Is a human playing as O?
   
 * `simulations = 10000;`
   
   How many iterations of the MCTS algorithm are done?
   
 * `tuningParameter = Math.Sqrt(2);`
   
   Lower values favour confirmation that strategies work. Higher values favour exploring new strategies.
   
