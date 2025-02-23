public interface IUserDataManager
{
    int CurrentLevel();    
    void SetNextLevel();
    void SetCurrentLevel(int level);

    bool IsMusicEnabled();
    void SetMusicEnabled(bool state);
    bool IsSoundEnabled();
    void SetSoundEnabled(bool state);
    bool IsVibrationEnabled();
    void SetVibrationEnabled(bool state);

}
