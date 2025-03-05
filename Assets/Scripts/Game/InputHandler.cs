using System;
using UnityEngine;

public class InputHandler
{
    private LayerMask _groundLayer;   
    private Camera _mainCamera;
    //TODO: birden fazla çarpışma kontrol ediecekse RaycastNonAlloc kullanılabilir
    // şimdilik gerek yok
    // private readonly RaycastHit[] _raycastResults = new RaycastHit[5];
    public event Action<Vector3> OnGroundClicked;

    //TODO: enemy ve build etkileşimi eklenebilir 
    //public event Action<IDamageable> OnEnemyClicked;
    public event Action<BuildingController> OnBuildingClicked;
    
    public InputHandler(LayerMask groundLayer, Camera mainCamera)
    {
        _groundLayer = groundLayer;
        _mainCamera = mainCamera;
    }   
    
    public void Dispose()
    {        
        OnGroundClicked = null;
       // OnEnemyClicked = null;
        OnBuildingClicked = null;      
    }

    public void ProcessInput(Camera camera)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;                
          
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _groundLayer))
            {
                OnGroundClicked?.Invoke(hit.point);
            }
        }
    }
}