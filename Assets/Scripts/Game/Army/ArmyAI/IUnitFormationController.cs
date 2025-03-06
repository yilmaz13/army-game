
using UnityEngine;
using System.Collections.Generic;

public interface IUnitFormationController
{
    void MoveUnitsWithTypeOrder(Vector3 targetPosition);
    void MoveSpecificUnitsTo(List<int> unitIDs, Vector3 targetPosition);
}