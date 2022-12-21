Er zijn 2 manieren om een sudoku aan een programma mee te geven.
De eerste manier is om in het bestand Arguments.txt, 81 ints te plaatsen. Er wordt voor de correctheid van het programma vanuit gegaan dat het er exact 81 zijn.
Om dit te laten werken, moet het bestand Arguments.txt gekopieerd worden naar de output directory. Wij hebben dit als het goed is in de solution opgenomen, maar mocht het niet werken:
Rechtermuisknop op Arguments.txt -> Quick Properties -> Copy to output directory.
Als dit tekstbestand leeg is, kunnen er ook 81 ints meegegeven worden via de command line. Ook hier wordt er vanuit gegaan dat het er exact 81 zijn.

In de main functie in program.cs staan 2 functies, Solve en TimeSudoku. Solve is om het algoritme op een normale manier te draaien en de gegeven sudoku op te lossen,
TimeSudoku is er om voor een aantal S waardes de snelheid van het algoritme te testen. TimeSudoku staat er standaard als een comment bij.
