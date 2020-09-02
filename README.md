# MovieDetails
 
<img src="https://github.com/Chlorine-trifluoride/MovieDetails/workflows/.NET%20Core%20Build/badge.svg"/>
<img src="https://github.com/Chlorine-trifluoride/MovieDetails/workflows/MSTests/badge.svg"/>


## Info

This is a .NET Core C# CLI application for searching for information about movies.
The **MovieLib** looks for matches in local JSON cache first, and if not found it uses the Open Movie Database API to search for matches there. If a match is found, the movie information is downloaded in to local JSON DB cache and then displayed to the user.

### Movie search

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/search.gif"/>

## Auto Complete

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/autocomplete.gif"/>

The auto complete works by loading <a href=https://github.com/Chlorine-trifluoride/MovieDetails/blob/predictivetext/MovieCLI/Data/MovieNames.txt>**MovieNames.txt**</a> as an array and matching against that. The database is generated from <a href=https://www.kaggle.com/rounakbanik/the-movies-dataset>this dataset</a> released under Public Domain Dedication license. Thank you for that.

### Multiple matches

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/multiple_choise.png"/>

### Movie Details

All the fields in **MovieLib/Entities/Movie.cs** are printed using reflection.

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/reflection.png"/>

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/movie_info.png"/>

### Logging

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/condition_debug.png"/>

If the program is executed in **DEBUG** mode the Log is printed to the stdout and logfile.
If the program is executed in Release mode, the log is only written in the file.

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/logger.png"/>
