using UnityEngine;

[System.Serializable]
public class LevelStats
{
    [Header("Info")]
    public int level;
    public Sprite icon;
    public Sprite cardFrame; 

    public Sprite titleSprite;
    public Sprite descriptionSprite;   
    
    [Header("Stats")]
    public float damage;
    public float moveSpeed;
    public float attackRange;
    public float health;
    public float armor;
    public float attackSpeed;
    public float chaseRange;
    public string upgradeDescription;

    public UnitRank rank;
}