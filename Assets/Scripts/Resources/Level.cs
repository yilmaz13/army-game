using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Level", order = 0)]

public class Level : ScriptableObject
{
    [SerializeField] public int Id => id;

    [SerializeField] private int id;
    
    public void CopyLevel(Level level, Level level2)
    {
        level.id = level2.Id;             
    }
}
