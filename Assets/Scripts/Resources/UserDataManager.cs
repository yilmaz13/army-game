using UnityEngine;

public class UserDataManager : IUserDataManager
{
    public UserDataManager()
    {
        if (!UserLevelExist())
        {
            SetCurrentLevel(0);          
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

}
