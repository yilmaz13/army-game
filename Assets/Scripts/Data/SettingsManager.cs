using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    //  MEMBERS

    public static SettingsManager Instance { get; private set; }

    private IUserDataManager _dataManager;
    public bool IsMusicEnabled { get; private set; }
    public bool IsSoundEnabled { get; private set; }
    public bool IsVibrationEnabled { get; private set; }
    //      Private
    private bool _isInitialized;

    public void Initialize(IUserDataManager dataManager)
    {
        if (_isInitialized == false)
        {
            _dataManager = dataManager;

            _isInitialized = true;

            IsMusicEnabled = _dataManager.IsMusicEnabled();
            IsSoundEnabled = _dataManager.IsSoundEnabled();
            IsVibrationEnabled = _dataManager.IsVibrationEnabled();

            SettingsEvents.OnChangeMusicEnabled += SetMusicEnabled;
            SettingsEvents.OnChangeSoundEnabled += SetSoundEnabled;
            SettingsEvents.OnChangeVibrationEnabled += SetVibrationEnabled;
        }
    }
    private void SetMusicEnabled(bool state)
    {
        IsMusicEnabled = state;      
        SettingsEvents.MusicEnabledChanged(state);
        _dataManager.SetMusicEnabled(state);
    }

    public void SetSoundEnabled(bool state)
    {
        IsSoundEnabled = state;       
        SettingsEvents.SoundEnabledChanged(state);
        _dataManager.SetSoundEnabled(state);
    }

    public void SetVibrationEnabled(bool state)
    {
        IsVibrationEnabled = state;       
        SettingsEvents.VibrationEnabledChanged(state);
        _dataManager.SetVibrationEnabled(state);
    }
}
