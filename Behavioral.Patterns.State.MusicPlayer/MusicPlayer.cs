namespace Behavioral.Patterns.State.MusicPlayer;
public class MusicPlayer
{
    private IPlayerState _state;

    public MusicPlayer()
    {
        _state = new StoppedState(this);
    }

    public void ChangeState(IPlayerState state)
    {
        _state = state;
    }

    public void Play()
    {
        _state.Play();
    }

    public void Pause()
    {
        _state.Pause();
    }

    public void Stop()
    {
        _state.Stop();
    }
}

