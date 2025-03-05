using System.Collections.Generic;
using UnityEngine;

public class SpatialPartition
{
    private int gridSizeX;
    private int gridSizeY;
    private float cellWidth;
    private float cellHeight;
    private Vector3 originPosition;
    private Dictionary<Vector2Int, List<IDamageable>> gridCells;

    public SpatialPartition(Vector3 originPosition, Vector2 gridSize, int gridCountX, int gridCountY)
    {
        this.originPosition = originPosition;
        this.gridSizeX = gridCountX;
        this.gridSizeY = gridCountY;
        this.cellWidth = gridSize.x / gridCountX;
        this.cellHeight = gridSize.y / gridCountY;
        gridCells = new Dictionary<Vector2Int, List<IDamageable>>();
    }

    public void Clear()
    {
        gridCells.Clear();
    }

    public void AddAgent(IDamageable agent)
    {
        Vector2Int cellCoords = GetCell(agent.GetPosition());
        if (!gridCells.ContainsKey(cellCoords))
            gridCells[cellCoords] = new List<IDamageable>();
        
        gridCells[cellCoords].Add(agent);
    }

    public void RemoveAgent(IDamageable agent)
    {
        Vector2Int cellCoords = GetCell(agent.GetPosition());
        if (gridCells.ContainsKey(cellCoords))
            gridCells[cellCoords].Remove(agent);
    }

    public void UpdateAgentPosition(IDamageable agent, Vector3 oldPos)
    {
        Vector2Int oldCell = GetCell(oldPos);
        Vector2Int newCell = GetCell(agent.GetPosition());

        if (oldCell != newCell)
        {
            RemoveAgent(agent);
            AddAgent(agent);
        }
    }
  
    public List<IDamageable> GetAgentsInCell(Vector3 position)
    {
        Vector2Int cellCoords = GetCell(position);
        if (gridCells.ContainsKey(cellCoords))
            return gridCells[cellCoords];

        return new List<IDamageable>();
    }

    public bool IsSameCell(Vector3 pos1, Vector3 pos2)
    {
        Vector2Int cell1 = GetCell(pos1);
        Vector2Int cell2 = GetCell(pos2);
        return cell1 == cell2;
    }

    private Vector2Int GetCell(Vector3 position)
    {
        int x = Mathf.FloorToInt((position.x - originPosition.x) / cellWidth);
        int z = Mathf.FloorToInt((position.z - originPosition.z) / cellHeight);
        return new Vector2Int(x, z);
    }

    public void DrawGizmos()
    {
        Gizmos.color = Color.green;
        for (int x = 0; x < gridSizeX; x++)        
        {
            for (int z = 0; z < gridSizeY; z++)
            {
                Vector3 cellPosition = originPosition + new Vector3(x * cellWidth, 0, z * cellHeight);
                Gizmos.DrawWireCube(cellPosition + new Vector3(cellWidth / 2, 0, cellHeight / 2), new Vector3(cellWidth, 0, cellHeight));

                Vector2Int cellCoords = new Vector2Int(x, z);
               if (gridCells.ContainsKey(cellCoords))
                {
                    foreach (var agent in gridCells[cellCoords])
                    {
                        if (agent != null && agent.IsActive())
                        {
                            if (agent.GetTeam() == Team.Red)
                            {
                                Gizmos.color = Color.red;
                                Gizmos.DrawSphere(agent.GetPosition(), 0.1f);
                            }
                            else
                            {
                                Gizmos.color = Color.blue;
                                Gizmos.DrawSphere(agent.GetPosition(), 0.1f);
                            }                            
                        } 
                    }
                }
            }
        }
    }
}