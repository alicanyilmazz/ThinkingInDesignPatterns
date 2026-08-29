using Behavioral.Patterns.State.MusicPlayer;

var spotify = new MusicPlayer();

spotify.Play();
spotify.Pause();
spotify.Play();
spotify.Stop();

Console.WriteLine();

spotify.Pause();
spotify.Play();
spotify.Play();
spotify.Stop();

Console.ReadLine();
