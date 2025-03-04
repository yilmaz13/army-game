using Army.Popup;
using UnityEngine;


public class PlayerLevelUpStrategy : ILevelUpStrategy
{
    private ArmyController _army;
    
    public PlayerLevelUpStrategy(ArmyController army)
    {
        _army = army;
    }
    
    public void ApplyLevelUp()
    {       
        Time.timeScale = 0f;        
        
        var popup = PopupManager.Instance.ShowPopup<LevelUpCardPopupView>();
        popup.SetupCards(_army);            
    }
}