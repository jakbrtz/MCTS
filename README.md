# simple-MCTS
This is a [Monte Carlos Tree Search algorithm](https://en.wikipedia.org/wiki/Monte_Carlo_tree_search) for playing simple games of perfect information that don't involve chance.

This project is made of an abstract class ([MCTS.cs](MCTS/MCTS.cs)) and an example file that shows how to over-write it ([Program.cs](MCTS/Program.cs)).

In the example file, I have programmed it to play the game Connect Four. You can play by typing the character 1 to 7 to represent which column you want to place a piece in.

Players and moves are represented by 0-based indexes. For example, in Connect Four there are 2 players (labelled 0 and 1) and 7 moves (labelled 0 to 6). In more complicated games you'll need to find ways to translate more complicated moves into indexes. It doesn't matter if not all indexes represent legal moves, so long as every index is between 0 and an easy-to-calculate number.
