# Unity-GridBasedMovement
A simple grid based point and click movement system made using the Grid component in Unity.

There are 2 main scripts in this project.

# DisplayGrid.cs 
This script is what creates the grid over the level. It instantiates prefabs of sprites in a grid
like fashion. This can be a little intensive as you are spawning (width x height) prefabs in the
scene. There are easier ways to do this but it was fast to make and is not neccessary to the game.

# PlayerMovement_GridBased
This script is placed on a player and controls their movement. When you click on a cell on the grid,
the player makes a "Que" of positions that will lead the player to where you click and it then
runs a coroutine that goes through the Que one position at a time.

Email me if you need any help, have any questions or if there are some errors that you have found.
