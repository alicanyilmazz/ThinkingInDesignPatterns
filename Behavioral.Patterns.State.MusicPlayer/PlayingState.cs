namespace Behavioral.Patterns.State.MusicPlayer;

public class PlayingState : IPlayerState
{
    private readonly MusicPlayer _player;

    public PlayingState(MusicPlayer player)
    {
        _player = player;
    }

    public void Play()
    {
        Console.WriteLine("Şarkı zaten çalıyor.");
    }

    public void Pause()
    {
        Console.WriteLine("⏸ Şarkı duraklatıldı.");

        _player.ChangeState(new PausedState(_player));
    }

    public void Stop()
    {
        Console.WriteLine("⏹ Şarkı durduruldu.");

        _player.ChangeState(new StoppedState(_player));
    }
}