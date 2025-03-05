using Army.Game.UI;
using Army.Popup;
using DG.Tweening;
using UnityEngine;

public class GameController : MonoBehaviour, IGameViewListener, IArmyControllerListener
{
    private GameData _data;
    private GameView _view;
    private GameUIView _gameUIView;
    private IGameListener _listener;
    private GameResources _gameResources;
    private Camera _camera;
    private ArmyController[] _armyControllers;
    private SpatialPartition _spatialPartition;    

    public GameView View => _view;

    public bool IsGameStarted;
    [SerializeField] private LayerMask groundLayer = 1 << 6;
    

    private void OnDestroy()
    {
        UnsubscribeEvents();
        Unload();
    }
    
    private void Update()
    {
      //  UpdateAgentPositions();
    }
    
    private void OnDrawGizmos()
    {
        if (_spatialPartition != null)
        {
           // _spatialPartition.DrawGizmos();
        }
    }    

    public GameController()
    {
        _data = new GameData();
    }
    
    public void Initialize(GameView view, GameUIView gameUIView, IGameListener listener, GameResources gameResources, Camera camera)
    {
        _view = view;
        _gameUIView = gameUIView;
        _listener = listener;
        _gameResources = gameResources;
        _camera = camera;
        
        SubscribeEvents();
        Load();        
    }    
    
    public void Load()
    {
        _data.Load();
        _view.Create();
        
        PopupManager.Instance.HideAllPopups();
        InitializeSpatialPartition();
        InitializeArmies();
        
        InvokeRepeating(nameof(UpdateAgentPositions), 0.1f, 0.5f);
        IsGameStarted = true;
    }    
  
    public void Unload()
    {
        if (_data != null)
        {
            _data.Unload();
        }
        
        if (_view != null)
        {
            _view.Clear();
        }

        if (_armyControllers != null)
        {
            foreach (var army in _armyControllers)
            {
                if (army != null)
                {
                    army.Unload();
                }
            }
        }

        if (_spatialPartition != null)
        {  
             CancelInvoke(nameof(UpdateAgentPositions));
            _spatialPartition.Clear();           
        }

        if (_armyControllers != null)
        {
            for (int i = _armyControllers.Length - 1; i >= 0; i--)
            {
                ArmyController army = _armyControllers[i];
                if (army != null)
                {
                     Destroy(army.gameObject);
                }
            }
        } 

        IsGameStarted = false;     
    }    
  
    private void UpdateAgentPositions()
    {
        if (_armyControllers == null) return;
        
        foreach (var army in _armyControllers)
        {
            if (army == null) continue;
            
            foreach (var agent in army.GetUnits())
            {
                Vector3 oldPos = agent.LastPosition;
                _spatialPartition.UpdateAgentPosition(agent, oldPos);
                agent.LastPosition = agent.transform.position;
            }
        }
    }    

    private void InitializeArmies()
    {
        _armyControllers = new ArmyController[2];
        
        if (_view.TeamOneSpawnPoint == null || _view.TeamTwoSpawnPoint == null)
        {
            Debug.LogError("Army spawn points (TeamOneSpawnPoint/TeamTwoSpawnPoint) are not set in GameView.");
            return;
        }
        
        _armyControllers[0] = CreateArmy(Team.Red, _view.TeamOneSpawnPoint);
        _armyControllers[1] = CreateArmy(Team.Blue, _view.TeamTwoSpawnPoint);
    }
    
    private ArmyController CreateArmy(Team team, Transform spawnPoint)
    {
        GameObject armyObj = Instantiate(_gameResources.ArmyControllerPrefab, 
                                        spawnPoint.position, 
                                        spawnPoint.rotation);
        
        ArmyController army = armyObj.GetComponent<ArmyController>();
        
        if (army == null)
        {
            Debug.LogError($"Prefab ArmyController is missing its component ({team}).");
            return null;
        }
        
        army.Initialize(this, team, _gameResources, _gameResources.GetArmyLevelData());

        if (team == Team.Blue)
        {
            _gameUIView.SetupBlueArmyStatPanel(army);
        }

        return army;
    }    
    
    private void InitializeSpatialPartition()
    {
        _spatialPartition = new SpatialPartition(
            new Vector3(-11, 0, -6),  
            new Vector2(6, 12),      
            1,                        
            3                        
        );
    }    
 
    private void SubscribeEvents()
    {        
        if (_gameUIView != null)
        {
          
        }
    }
    
    private void UnsubscribeEvents()
    {       
        if (_gameUIView != null)
        {
            
        }
    }
    
    // IArmyControllerListener Interface
    public IDamageable NearestEnemy(Vector3 position, float range, Team currentTeam)
    {
        return GetNearestEnemy(position, range, currentTeam);
    }
    
    public void HandleAgentSpawned(IDamageable newAgent)
    {
        _spatialPartition.AddAgent(newAgent);
    }
    
    public void HandleAgentDied(IDamageable agent)
    {
        _spatialPartition.RemoveAgent(agent);
    }
    

    public IDamageable GetNearestEnemy(Vector3 position, float range, Team currentTeam)
    {
        var possibleEnemies = _spatialPartition.GetAgentsInCell(position);
        
        IDamageable nearestEnemy = null;
        float minDist = float.MaxValue;
        
        foreach (var enemy in possibleEnemies)
        {
            if (enemy.GetTeam() == currentTeam || !enemy.IsActive())
                continue;
                
            float dist = Vector3.Distance(position, enemy.GetPosition());
            if (dist < minDist)
            {
                minDist = dist;
                nearestEnemy = enemy;
            }
        }
        
        return nearestEnemy;
    }

    public void OnDefeatCastle(Team team)
    {
        if (team == Team.Blue)
        {
            OnGameEnded(false);     
        }     
        else if (IsAllEnemyDefeated())
        {
            OnGameEnded(true);
        }        
    }

    private bool IsAllEnemyDefeated()
    {
        foreach (var army in _armyControllers)
        {
            if (!army.IsDefeated && army.Team != Team.Blue) 
            {
                return false;
            }
        }
        
        return true;
    }

    private void OnGameEnded(bool isWin)
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            GameEvents.EndedGame(isWin);
        });   
    }
}
