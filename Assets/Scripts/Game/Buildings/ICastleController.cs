using System;

public interface  ICastleController : IDamageable
{
    void ShowLevelUpNotification(int level);
    void ShowUpgradeNotification(string name, string description);
    void ShowSpawnSlider(float time);
    void SubscribeEvents(Action onDefeat);
    void UnsubscribeEvents(Action onDefeat);
}