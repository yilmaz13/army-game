using System.Collections.Generic;
using UnityEngine;

public class MoveSpecificUnitsCommand : IArmyCommand
{
    private readonly Vector3 _targetPosition;
    private readonly List<int> _unitIDs;
    private readonly IUnitFormationController _formationController;
    
    public MoveSpecificUnitsCommand(Vector3 targetPosition, System.Collections.Generic.List<int> unitIDs, 
                                   IUnitFormationController formationController)
    {
        _targetPosition = targetPosition;
        _unitIDs = unitIDs;
        _formationController = formationController;
    }
    
    public void Execute()
    {
        _formationController.MoveSpecificUnitsTo(_unitIDs, _targetPosition);
    }
}