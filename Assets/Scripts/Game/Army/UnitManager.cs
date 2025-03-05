using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.PoolSystem;   

public class UnitManager
{
    private List<AgentController> _agents = new List<AgentController>();
    private List<BuildingController> _buildings = new List<BuildingController>();
   
    public int UnitCount => _agents.Count;
    
    public UnitManager()
    {        
    }
    
    public List<AgentController> GetUnits()
    {
        return _agents;
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
    
    public void MoveUnitsTo(Vector3 targetPosition)
    {
        foreach (var agent in _agents)
        {
            if (agent != null && agent.IsActive)
            {
                agent.MoveTo(targetPosition);
            }
        }
    }
    
    public void ClearAll()
    {
        RemoveAllUnit();
        
        _agents.Clear();
        _buildings.Clear();
    }
}