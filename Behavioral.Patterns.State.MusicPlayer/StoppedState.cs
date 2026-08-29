namespace Behavioral.Patterns.State.MusicPlayer;

public class StoppedState : IPlayerState
{
    private readonly MusicPlayer _player;

    public StoppedState(MusicPlayer player)
    {
        _player = player;
    }

    public void Play()
    {
        Console.WriteLine("▶ Şarkı çalmaya başladı.");

        _player.ChangeState(new PlayingState(_player));
    }

    public void Pause()
    {
        Console.WriteLine("Şarkı zaten durmuş durumda. Pause yapılamaz.");
    }

    public void Stop()
    {
        Console.WriteLine("Şarkı zaten durmuş durumda.");
    }
}