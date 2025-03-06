using UnityEngine;

public class MoveUnitsCommand : IArmyCommand
{
    private readonly Vector3 _targetPosition;
    private readonly IUnitFormationController _formationController;
    
    public MoveUnitsCommand(Vector3 targetPosition, IUnitFormationController formationController)
    {
        _targetPosition = targetPosition;
        _formationController = formationController;
    }
    
    public void Execute()
    {
        _formationController.MoveUnitsWithTypeOrder(_targetPosition);
    }
}