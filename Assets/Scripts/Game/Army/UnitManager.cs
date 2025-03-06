using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Assets.Scripts.PoolSystem;

public class UnitManager
{
    private const int MAX_UNITS_PER_ROW = 9;
    private const float UNIT_SPACING_X = 0.3f; 
    private const float UNIT_SPACING_Z = 0.3f;
    
    private List<AgentController> _agents = new List<AgentController>();
    private List<BuildingController> _buildings = new List<BuildingController>();
    private List<Vector3> _formationPositions = new List<Vector3>();
    private Vector3 _formationTarget;
    private bool _shouldDisplayFormation = false;
    private float _formationDisplayTime = 5f;
    private float _formationEndTime = 0f;
    
    public int UnitCount => _agents.Count;
    public IReadOnlyList<Vector3> FormationPositions => _formationPositions;
    public Vector3 FormationTarget => _formationTarget;
    public bool IsFormationVisible => _shouldDisplayFormation && Time.time < _formationEndTime;
        
    public void MoveUnitsTo(Vector3 targetPosition)
    {
        List<AgentController> units = GetAvailableUnits();
        if (units.Count == 0) return;        
       
        int totalPositions = Mathf.Max(units.Count, MAX_UNITS_PER_ROW);
              
        CalculateFormationPositions(targetPosition, totalPositions);
               
        MoveUnitsToPositions(units, _formationPositions);
    }
    
   
    public void MoveUnitsWithTypeOrder(Vector3 targetPosition)
    {
        List<AgentController> units = GetAvailableUnits();
        if (units.Count == 0) return;
              
        var tanks = new List<AgentController>();
        var melee = new List<AgentController>();
        var ranged = new List<AgentController>();
        var support = new List<AgentController>();
        var other = new List<AgentController>();
              
        foreach (var unit in units)
        {
            if (unit == null || unit.Data == null) continue;
            
            switch (unit.Data.UnitType)
            {
                case AgentUnitType.Tank: tanks.Add(unit); break;
                case AgentUnitType.Melee: melee.Add(unit); break;
                case AgentUnitType.Ranged: ranged.Add(unit); break;
                case AgentUnitType.Support: support.Add(unit); break;
                default: other.Add(unit); break;
            }
        }
        
        var sortedUnits = new List<AgentController>(units.Count);
        sortedUnits.AddRange(tanks);
        sortedUnits.AddRange(melee);
        sortedUnits.AddRange(ranged);
        sortedUnits.AddRange(support);
        sortedUnits.AddRange(other);        
        
        int totalPositions = Mathf.Max(sortedUnits.Count, MAX_UNITS_PER_ROW);
            
        CalculateFormationPositions(targetPosition, totalPositions);
            
        MoveUnitsToPositions(sortedUnits, _formationPositions);
    }    
  
    private void CalculateFormationPositions(Vector3 targetPosition, int totalPositions)
    {        
        _formationPositions.Clear();
        _formationTarget = targetPosition;
        _shouldDisplayFormation = true;
        _formationEndTime = Time.time + _formationDisplayTime;
        
        int numRows = Mathf.CeilToInt((float)totalPositions / MAX_UNITS_PER_ROW);
        
        Vector3 formationCenter = targetPosition;
      
        for (int row = 0; row < numRows; row++)
        {
            
            float rowWidth = (MAX_UNITS_PER_ROW - 1) * UNIT_SPACING_X;
            float startX = formationCenter.x - rowWidth / 2;
            float rowZ = formationCenter.z + row * UNIT_SPACING_Z;
            
            for (int col = 0; col < MAX_UNITS_PER_ROW; col++)
            {
                Vector3 unitPosition = new Vector3(
                    startX + col * UNIT_SPACING_X,
                    formationCenter.y,
                    rowZ
                );
                
                
                _formationPositions.Add(unitPosition);
                
     
                if (_formationPositions.Count >= totalPositions)
                    break;
            }            
           
            if (_formationPositions.Count >= totalPositions)
                break;
        }
    }
       
    private void MoveUnitsToPositions(List<AgentController> units, List<Vector3> positions)
    {
        for (int i = 0; i < units.Count; i++)
        {
            if (i < positions.Count && units[i] != null)
            {                
                if (!IsUnitFighting(units[i]))
                {
                    units[i].MoveTo(positions[i]);
                }
            }
        }
    }    
    
    private List<AgentController> GetAvailableUnits()
    {
        return _agents.Where(a => a != null && a.IsActive).ToList();
    }    

    private bool IsUnitFighting(AgentController agent)
    {       
        return agent.IsUnitAttacking();              
    }    

    private int GetAgentTypePriority(AgentUnitType type)
    {
        switch (type)
        {
            case AgentUnitType.Tank:     return 0;
            case AgentUnitType.Melee:    return 1; 
            case AgentUnitType.Ranged:   return 2; 
            case AgentUnitType.Support:  return 3; 
            default:                     return 4; 
        }
    }    
   
    public List<AgentController> GetUnits()
    {
        return new List<AgentController>(_agents);
    }
    
    public List<BuildingController> GetBuildings()
    {
        return _buildings;
    }
    
    public void AddAgent(AgentController agent)
    {
        _agents.Add(agent);
    }
    
    public void AddBuilding(BuildingController building)
    {
        _buildings.Add(building);
    }
    
    public void RemoveAllUnit()
    {
        foreach (var agent in _agents)
        {
            if (agent != null)
            {
                PoolObject poolObject = agent.GetComponent<PoolObject>();
                if (poolObject != null)
                {
                    poolObject.GoToPool(2);
                }
                else
                {
                    Object.Destroy(agent.gameObject, 2f);
                }
            }
        }
        _agents.Clear();
    }
    
    public void RemoveAgent(AgentController agent)
    {
        if (agent == null) return;
        
        _agents.Remove(agent);        
        
        PoolObject poolObject = agent.GetComponent<PoolObject>();
        if (poolObject != null)
        {
            poolObject.GoToPool(2);
        }
        else
        {
            Debug.LogWarning("dont have poolObject component");
            Object.Destroy(agent.gameObject, 2f);
        }
    }
    
    public void RemoveBuilding(BuildingController building)
    {
        if (building == null) return;
        
        _buildings.Remove(building);
       
        PoolObject poolObject = building.GetComponent<PoolObject>();
        if (poolObject != null)
        { 
            poolObject.GoToPool(2);
        }
        else
        {
            Object.Destroy(building.gameObject, 2f);
        }
    }
    
    public void ClearAll()
    {
        RemoveAllUnit();
        
        _agents.Clear();
        _buildings.Clear();
    }

    public void MoveSpecificUnitsTo(List<AgentController> specificUnits, Vector3 targetPosition)
    {
        if (specificUnits == null || specificUnits.Count == 0) return;        
      
        _formationPositions.Clear();
        _formationTarget = targetPosition;
        _shouldDisplayFormation = true;
        _formationEndTime = Time.time + _formationDisplayTime;        
       
        int totalUnits = specificUnits.Count;
        int numRows = Mathf.CeilToInt((float)totalUnits / MAX_UNITS_PER_ROW);        
        
        int unitsInLastRow = totalUnits % MAX_UNITS_PER_ROW;
        if (unitsInLastRow == 0 && totalUnits > 0) unitsInLastRow = MAX_UNITS_PER_ROW;        
     
        Vector3 formationCenter = targetPosition;
        List<Vector3> positions = new List<Vector3>();
        
        for (int row = 0; row < numRows; row++)
        {
            int unitsInThisRow = (row == numRows - 1) ? unitsInLastRow : MAX_UNITS_PER_ROW;
            float rowWidth = (unitsInThisRow - 1) * UNIT_SPACING_X;
            float startX = formationCenter.x - rowWidth / 2;
            float rowZ = formationCenter.z + row * UNIT_SPACING_Z;
            
            for (int col = 0; col < unitsInThisRow; col++)
            {
                Vector3 unitPosition = new Vector3(
                    startX + col * UNIT_SPACING_X,
                    formationCenter.y,
                    rowZ
                );
                
                positions.Add(unitPosition);
                _formationPositions.Add(unitPosition);
            }
        }
           
        for (int i = 0; i < specificUnits.Count; i++)
        {
            AgentController unit = specificUnits[i];
            if (unit != null && i < positions.Count)
            {                
                if (!unit.IsUnitAttacking())
                {
                    unit.MoveTo(positions[i]);
                }              
            }
        }
    }
  
}