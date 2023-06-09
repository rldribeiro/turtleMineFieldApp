# Turtle MineField Challenge
The Turtle MineField Challenge is a program that simulates a turtle walking through a minefield. The program reads the initial game settings from one file and one or more sequences of moves from a different file. For each move sequence, the program outputs whether the sequence leads to the success or failure of the little turtle. The program can handle scenarios where the turtle doesn't reach the exit point or doesn't hit a mine.

## Running the App in Sequence mode
To run the Turtle MineField Challenge app, use the following command-line syntax:

`TurtleMineFieldApp.exe <path-to-settings> <path-to-actions>`
For example, to run the app with the "game-settings.json" settings file and the "actions.txt" actions file, use the following command:

`TurtleMineFieldApp.exe game-settings.json actions.txt`

### Settings Format
The settings should be a Json File. They can model a MineField with a random number of mines or with mines at predefined coordinates.

### Random Mines Example

```
{
  "FieldWidth": 5,
  "FieldHeight": 5,
  "ExitCoordinate": {
    "X": 4,
    "Y": 4
  },
  "RandomMines": true,
  "NumberOfMines": 2,
  "InitCoordinate": {
    "X": 0,
    "Y": 0
  },
  "InitDirection": "S"
}
```

### Predefined Mines Example

```
{
  "FieldWidth": 5,
  "FieldHeight": 5,
  "ExitCoordinate": {
    "X": 4,
    "Y": 4
  },
  "RandomMines": false,
  "NumberOfMines": 0,
  "MineCoordinates": [
    {
      "X": 2,
      "Y": 2
    },
    {
      "X": 3,
      "Y": 3
    }
  ],
  "InitCoordinate": {
    "X": 0,
    "Y": 0
  },
  "InitDirection": "S"
}
```

### Actions File
The action files are text files with the char 'm' to indicate MOVE and 'r' to indicate ROTATE 90 DEGREES CLOCKWISE. The actions file can have whitespace characters to organize the actions.

Example:

`mmm rrr mm rr m r`

This would be interpreted as "MOVE 3 TIMES, ROTATE 3 TIMES, MOVE 2 TIMES, ROTATE 2 TIMES, MOVE 1 TIME, ROTATE 1 TIME"

## Running the app in interactive mode

In interactive mode you do not provide an action file. You control the movement with the keyboard.
This was a request from my son.

To run in interactive mode, simply add --interactive to the end of the command:

`TurtleMineFieldApp.exe game-settings.json --intearctive`

You do not need to specify actions file.

## Limitations

The Mine Field size a limit of 1000x1000, but only renders to the terminal if is smaller than 100x100.
The exit coordinates, mine coordinates and turtle coordinates must be valid (inside the field).
Negative mine numbers are considered zero.
More mines than the size of the field simply fill the field.
