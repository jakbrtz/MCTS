# MCTS
This is a [Monte Carlos Tree Search algorithm](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search) for playing simple games of perfect information that don't involve chance. 

This project is made of an abstract class ([MCTS.cs](MCTS/MCTS.cs)) and example files that show how to over-write it.

The easiest example is [Connect Four](MCTS/Examples/ConnectFour.cs). You can play by typing the character 1 to 7 to represent which column you want to place a piece in.

Players and moves are represented by 0-based indexes. For example, in Connect Four there are 2 players (labelled 0 and 1) and 7 moves (labelled 0 to 6). In more complicated games you'll need to find ways to translate more complicated moves into indexes. It doesn't matter if not all indexes represent legal moves, so long as every index is between 0 and an easy-to-calculate number.

Not all of the games are played on the console. [Reversi](MCTS/Examples/Reversi.cs) is an example of a game which is played by clicking on a windows form. All clicks are converted into numbers that represent moves.