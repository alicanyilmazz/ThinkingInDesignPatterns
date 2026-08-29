namespace Behavioral.Patterns.State.MusicPlayer;

public interface IPlayerState
{
    void Play();
    void Pause();
    void Stop();
}
