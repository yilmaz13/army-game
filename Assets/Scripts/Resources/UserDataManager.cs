using UnityEngine;

public class UserDataManager : IUserDataManager
{
    public UserDataManager()
    {
        if (!UserLevelExist())
        {
            SetCurrentLevel(0);
            SetMusicEnabled(true);
            SetSoundEnabled(true);
            SetVibrationEnabled(true);
        }
    }
    public int CurrentLevel()
    {
        return PlayerPrefs.GetInt(PlayerPrefKeys.CURRENTLEVEL);
    }

    public void SetNextLevel()
    {
        var level = (CurrentLevel() + 1);

        PlayerPrefs.SetInt(PlayerPrefKeys.CURRENTLEVEL, level);

    }
    public void SetCurrentLevel(int level)
    {
        PlayerPrefs.SetInt(PlayerPrefKeys.CURRENTLEVEL, level);
    }   

    public bool UserLevelExist()
    {
        return PlayerPrefs.HasKey(PlayerPrefKeys.CURRENTLEVEL);
    }

    public bool IsMusicEnabled()
    {
        int stateInt = 1;
        return PlayerPrefs.GetInt(PlayerPrefKeys.MUSICENABLED, stateInt) == 1;
    }

    public void SetMusicEnabled(bool state)
    {
        int stateInt = state ? 1 : 0;
        PlayerPrefs.SetInt(PlayerPrefKeys.MUSICENABLED, stateInt);
    }

    public bool IsSoundEnabled()
    {
        int stateInt = 1;
        return PlayerPrefs.GetInt(PlayerPrefKeys.SOUNDENABLED, stateInt) == 1;
    }

    public void SetSoundEnabled(bool state)
    {
        int stateInt = state ? 1 : 0;
        PlayerPrefs.SetInt(PlayerPrefKeys.SOUNDENABLED, stateInt);
    }

    public bool IsVibrationEnabled()
    {  
        int stateInt = 1;
        return PlayerPrefs.GetInt(PlayerPrefKeys.VIBRATIONENABLED, stateInt) == 1;       
    }

    public void SetVibrationEnabled(bool state)
    {
        int stateInt = state ? 1 : 0;
        PlayerPrefs.SetInt(PlayerPrefKeys.VIBRATIONENABLED, stateInt);
    }
}
