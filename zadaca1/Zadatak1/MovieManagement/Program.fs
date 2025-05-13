open Microsoft.Data.Sqlite
open DatabaseAPI
open System.Collections.Generic
open NanoidDotNet

let createDB = "CREATE TABLE IF NOT EXISTS Movies (
  Id TEXT PRIMARY KEY,
  Title TEXT NOT NULL,
  Genre TEXT NOT NULL,
  ReleaseYear INTEGER NOT NULL,
  Rating REAL NOT NULL
);"

let db = new Database("movies.db", createDB);

// types

type Movie (db : Database, id : string, title : string, genre : string, releaseYear : int, rating : double) =
  inherit DatabaseEntity(db)

  let mutable _id = id
  let mutable _title = title
  let mutable _genre = genre
  let mutable _releaseYear = releaseYear
  let mutable _rating = rating

  member this.Id
    with get () = _id
    and set value = _id <- value

  member this.Title
    with get () = _title
    and set value = _title <- value

  member this.Genre 
    with get () = _genre
    and set value = _genre <- value

  member this.ReleaseYear 
    with get () = _releaseYear
    and set value = _releaseYear <- value

  member this.Rating 
    with get () = _rating
    and set value = _rating <- value

  override this.BindCommitParameters (cmd : SqliteCommand) =
    cmd.Parameters.AddWithValue("@Id", this.Id) |> ignore
    cmd.Parameters.AddWithValue("@Title", this.Title) |> ignore
    cmd.Parameters.AddWithValue("@Genre", this.Genre) |> ignore
    cmd.Parameters.AddWithValue("@ReleaseYear", this.ReleaseYear) |> ignore
    cmd.Parameters.AddWithValue("@Rating", this.Rating) |> ignore
    ()

  interface ISaveable with 
    member this.Save () =
      let sqlQuery = @"INSERT OR REPLACE INTO Movies(Id, Title, Genre, ReleaseYear, Rating)
      VALUES(@Id, @Title, @Genre, @ReleaseYear, @Rating);"
      this.Commit(sqlQuery)
      ()

  interface IDeletable with
    member this.Delete () =
      let sqlQuery = @"DELETE FROM Movies WHERE Id = @Id;"
      this.Commit(sqlQuery)
      ()

  interface ISearchable with
    member this.FromReader (reader : SqliteDataReader) =
      _id <- reader.GetString 0
      _title <- reader.GetString 1
      _genre <- reader.GetString 2
      _releaseYear <- reader.GetInt32 3
      _rating <- reader.GetDouble 4
      ()

  override this.ToString () =
    this.Id + ", " + this.Title + ", " + this.Genre + ", " + (string this.ReleaseYear) + ", " + (string this.Rating)

  new () = Movie (db, "", "", "", 0, 0.0)


type MovieByGenre (title : string, genre : string) =
  let mutable _title = title
  let mutable _genre = genre

  member this.Title = _title
     
  member this.Genre = _genre

  new () = MovieByGenre ("", "")

  interface ISearchable with 
    member this.FromReader (reader : SqliteDataReader) =
      _title <- reader.GetString 0                    
      _genre <- reader.GetString 1
      ()


type MovieByRating (title : string, rating : double) =
  let mutable _title = title
  let mutable _rating = rating

  member this.Title = _title
  member this.Rating = _rating

  new () = MovieByRating ("", 0.0)

  interface ISearchable with
    member this.FromReader (reader : SqliteDataReader) =
      _title <- reader.GetString 0
      _rating <- reader.GetDouble 1
      ()


type MovieByTitle (title : string) =
  let mutable _title = title

  member this.Title = _title
  
  new () = MovieByTitle ("")

  interface ISearchable with
    member this.FromReader (reader : SqliteDataReader) =
      _title <- reader.GetString 0
      ()

// helper functions

let tryParseDouble (str : string) =
  try
    double str
  with
  | :? System.FormatException as e -> -1.0

let tryParseInt (str : string) = 
  try
    int str
  with
  | :? System.FormatException as e -> -1

let searchMoviesByGenre (database : Database) (genre : string) =
  let sqlQuery = @"SELECT Title, Genre FROM Movies WHERE Genre LIKE @Genre;"
  let parameters = Dictionary<string, obj>()
  parameters.Add("@Genre", genre)
  database.Search<MovieByGenre>(sqlQuery, parameters)

let searchMoviesByRating (database : Database) (rating : double) =
  let sqlQuery = @"SELECT Title, Rating FROM Movies WHERE Rating >= @Rating;"
  let parameters = Dictionary<string, obj>()
  parameters.Add("@Rating", rating)
  database.Search<MovieByRating>(sqlQuery, parameters)

let searchMoviesByTitle (database : Database) (title : string) =
  let sqlQuery = @"SELECT Title FROM Movies WHERE Title LIKE @Title"
  let parameters = Dictionary<string, obj>()
  parameters.Add("@Title", "%" + title + "%")
  database.Search<MovieByTitle>(sqlQuery, parameters)

// init
let separator = String.init 50 (fun _ -> "=")

while true do
  printfn "%s" separator
  printfn "MovieManagement Menu"
  printfn "1. Add a movie"
  printfn "2. Edit a movie"
  printfn "3. Delete a movie"
  printfn "4. Search by Title"
  printfn "5. Search by Rating"
  printfn "6. Search by Genre"
  printfn "7. Exit"
  printf  "> "
  let choice = System.Console.ReadLine () |> tryParseInt
  match choice with
  | 1 ->
    let id = Nanoid.Generate (Nanoid.Alphabets.LowercaseLettersAndDigits, 6)
    printf "Enter title: "
    let title = System.Console.ReadLine ()
    printf "Enter genre: "
    let genre = System.Console.ReadLine ()
    printf "Enter release year: "
    let releaseYear = System.Console.ReadLine () |> tryParseInt
    printf "Enter rating: "
    let rating = System.Console.ReadLine () |> tryParseDouble

    let movie = Movie (db, id, title, genre, releaseYear, rating)
    ( movie :> ISaveable ).Save ()
    printfn "Movie saved"
    printfn "%s" separator

  | 2 ->
    let sqlQuery = @"SELECT * FROM Movies;"
    let movies = db.Search<Movie>(sqlQuery, Dictionary<string, obj>())
    if movies.Count = 0 then
      printfn "No movies to edit"
    else
    let mutable i = 0
    for movie in movies do
      printfn "%d -> %A" i movie
      i <- i + 1
    printf "Enter index: "
    let index = System.Console.ReadLine () |> tryParseInt
    if index >= movies.Count || index < 0 then
      printfn "Invalid index"
    else
      let printEditMenu () =
        printfn "Edit menu:"
        printfn "1. Title"
        printfn "2. Genre"
        printfn "3. Release year"
        printfn "4. Rating"
        printfn "5. Done"
        printf  "> "

      printEditMenu ()
      let mutable subchoice = System.Console.ReadLine () |> tryParseInt
      while subchoice <> 5 do
        match subchoice with
        | 1 ->
          printf "Enter title: "
          movies[index].Title <- System.Console.ReadLine ()
        | 2 ->
          printf "Enter genre: "
          movies[index].Genre <- System.Console.ReadLine ()
        | 3 -> 
          printf "Enter release year: "
          movies[index].ReleaseYear <- System.Console.ReadLine () |> tryParseInt
        | 4 ->
          printf "Enter rating: "
          movies[index].Rating <- System.Console.ReadLine () |> tryParseDouble
        | _ -> ()

        printEditMenu ()
        subchoice <- System.Console.ReadLine () |> tryParseInt

      ( movies[index] :> ISaveable).Save ()
      printfn "Movie Edited"

  | 3 ->
    let sqlQuery = @"SELECT * FROM Movies;" 
    let movies = db.Search<Movie>(sqlQuery, Dictionary<string, obj>())
    if movies.Count = 0 then
      printfn "No movies to delete"
    else
    let mutable i = 0
    for movie in movies do
      printfn "%d -> %A" i movie
      i <- i + 1
    printf "Enter index: "
    let index = System.Console.ReadLine () |> tryParseInt 
    if index >= movies.Count || index < 0 then
      printfn "Invalid index"
    else
      (movies[index] :> IDeletable).Delete ()
      printfn "Movie Deleted"

  | 4 ->
    printf "Enter title: "
    let title = System.Console.ReadLine ()
    let movies = searchMoviesByTitle db title
    if movies.Count <> 0 then
      printfn $"Found movies with title like {title}:"
      for movie in movies do
        printfn "%s" movie.Title
    else
      printfn "No results for given query"
    printfn "%s" separator

  | 5 ->
    printf "Enter rating: "
    let rating = System.Console.ReadLine () |> tryParseDouble
    let movies = searchMoviesByRating db rating
    if movies.Count <> 0 then
      printfn $"Found movies with rating > {rating}:"
      for movie in movies do
        printfn "%s, %.1f" movie.Title movie.Rating
    else
      printfn "No results for given query"
    printfn "%s" separator

  | 6 ->
    printf "Enter genre: "
    let genre = System.Console.ReadLine ()
    let movies = searchMoviesByGenre db genre
    if movies.Count <> 0 then
      printfn $"Found movies with genre = {genre}:"
      for movie in movies do
        printfn "%s, %s" movie.Title movie.Genre
    else
      printfn "No results for given query"
    printfn "%s" separator

  | 7 ->
      exit 0
  | _ -> printfn "Invalid option" 

