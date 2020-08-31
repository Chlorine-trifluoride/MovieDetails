# MovieDetails
 
<img src="https://github.com/Chlorine-trifluoride/MovieDetails/workflows/.NET%20Core%20Build/badge.svg"/>
<img src="https://github.com/Chlorine-trifluoride/MovieDetails/workflows/MSTests/badge.svg"/>


## Info

This is a .NET Core C# CLI application for searching for infomration about movies.
The **MovieLib** looks for matches in local JSON cache first, and if not found it uses the Open Movie Database API to search for matches there. If a match is found, the movie information is downloaded in to local JSON DB cache and then displayed to the user.

### Multiple matches

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/multiple_choise.png"/>

### Movie Details

All the fields in **MovieLib/Movie.cs** are printed using reflection.

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/reflection.png"/>

<img src="https://github.com/Chlorine-trifluoride/MovieDetails/raw/master/Media/movie_info.png"/>
