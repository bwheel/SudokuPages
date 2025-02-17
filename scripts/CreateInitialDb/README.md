# Create Initial Database

This script will create the initial database and seed sudoku puzzles in it.
The script requires the sudoku puzzes in a csv format with the following columns:

* id
* puzzle
* solution
* clues
* difficulty

I downloaded my dataset online from [kaggle](https://www.kaggle.com/datasets/radcliffe/3-million-sudoku-puzzles-with-ratings). It had the correct formatted data

## Running

The script requires two arguments to run. The first is the path to the input csv file, and the second is the path to the output sqlite database file.

_Example run:_

``` bash
homer@homer-MacBookPro11-1:~/repos/bwheel/SudokuPages/scripts/CreateInitialDb$  dotnet run -- ~/Downloads/archive/sudoku-3m.csv  ~/Downloads/archive/sudoku-3.db
CreateInitialDb: Creating sqlite file. /home/homer/Downloads/archive/sudoku-3.db
CreateInitialDb: Opening csvFileIn: /home/homer/Downloads/archive/sudoku-3m.csv
CreateInitialDb: Opening sqliteFileOut: /home/homer/Downloads/archive/sudoku-3.db
CreateInitialDb: Migrating DB
CreateInitialDb: Inserting Puzzles
CreateInitialDb: Saving Changes
CreateInitialDb: Saved puzzles(3000000) ellapsed00:04:59.3446959 sqliteFileOut: /home/homer/Downloads/archive/sudoku-3.db

```
