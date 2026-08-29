namespace Behavioral.Patterns.State.MusicPlayer;

public class PausedState : IPlayerState
{
    private readonly MusicPlayer _player;

    public PausedState(MusicPlayer player)
    {
        _player = player;
    }

    public void Play()
    {
        Console.WriteLine("▶ Şarkı kaldığı yerden devam ediyor.");

        _player.ChangeState(new PlayingState(_player));
    }

    public void Pause()
    {
        Console.WriteLine("Şarkı zaten pause durumunda.");
    }

    public void Stop()
    {
        Console.WriteLine("⏹ Şarkı tamamen durduruldu.");

        _player.ChangeState(new StoppedState(_player));
    }
}