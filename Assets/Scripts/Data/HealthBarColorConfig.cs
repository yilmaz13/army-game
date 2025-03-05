using UnityEngine;

[CreateAssetMenu(fileName = "HealthBarColorConfig", menuName = "Game/Health Bar Color Config", order = 1)]
public class HealthBarColorConfig : ScriptableObject
{    
    [Header("Team Colors")]
   [SerializeField] private Color _blueTeamColor;
   [SerializeField]  private Color _redTeamColor;
   [SerializeField] private Color _yellowTeamColor;  
   [SerializeField] private Color _purpleTeamColor;
   [SerializeField] private Color _greenTeamColor;
   [SerializeField] private Color _orangeTeamColor;
   [SerializeField] private Color _pinkTeamColor;    

    public Color BlueTeamColor => _blueTeamColor;
    public Color RedTeamColor => _redTeamColor;
    public Color YellowTeamColor => _yellowTeamColor;

    public Color PurpleTeamColor => _purpleTeamColor;
    public Color GreenTeamColor => _greenTeamColor;
    public Color OrangeTeamColor => _orangeTeamColor;
    public Color PinkTeamColor => _pinkTeamColor;

    public Color GetColor(Team team)
    {
        switch (team)
        {
            case Team.Blue:
                return _blueTeamColor;
            case Team.Red:
                return _redTeamColor;
            case Team.Yellow:
                return _yellowTeamColor;
            case Team.Purple:
                return _purpleTeamColor;
            case Team.Green:
                return _greenTeamColor;
            case Team.Orange:
                return _orangeTeamColor;
            case Team.Pink:
                return _pinkTeamColor;
            default:
                return Color.white;
        }
    }

}