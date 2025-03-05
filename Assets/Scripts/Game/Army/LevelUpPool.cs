
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelUpPool
{  
    public int minLevel;        
    public List<AgentType> availableTypes = new List<AgentType>();   
    
    public List<int> weights = new List<int>();    

    public AgentType[] GetRandomOptions(int count)
    {
        if (availableTypes.Count == 0) return new AgentType[0];        
       
        List<AgentType> types = new List<AgentType>(availableTypes);
        List<int> typeWeights = new List<int>(weights);        
      
        if (typeWeights.Count != types.Count)
        {
            typeWeights.Clear();
            for (int i = 0; i < types.Count; i++)
            {
                typeWeights.Add(1);
            }
        }        
       
        List<AgentType> selectedTypes = new List<AgentType>();        
   
        count = Mathf.Min(count, types.Count);
        
        for (int i = 0; i < count; i++)
        {            
            int totalWeight = 0;
            for (int j = 0; j < typeWeights.Count; j++)
            {
                totalWeight += typeWeights[j];
            }            
           
            int randomValue = Random.Range(0, totalWeight);
            int currentWeight = 0;            
          
            for (int j = 0; j < types.Count; j++)
            {
                currentWeight += typeWeights[j];
                if (randomValue < currentWeight)
                {
                    selectedTypes.Add(types[j]);
                    types.RemoveAt(j);
                    typeWeights.RemoveAt(j);
                    break;
                }
            }
        }
        
        return selectedTypes.ToArray();
    }
}