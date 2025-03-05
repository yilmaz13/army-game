using UnityEngine;

public class BuildingView : EntityView
{  
    [SerializeField] private GameObject _healthSliderPrefabs;
    [SerializeField] private GameObject _armorSliderPrefabs;

    public virtual void Initialize(Camera camera, GameObject healthSliderPrefabs, GameObject armorSliderPrefabs)
    {
        base.Initialize(camera);
        _healthSliderPrefabs = healthSliderPrefabs;
        _armorSliderPrefabs = armorSliderPrefabs;        
    }

    public virtual void Initialize(Camera camera, int level)
    {
        base.Initialize(camera);
        SetLevelUpObject(level);
    }

    public override void SetLevelUpObject(int level)
    {
        foreach (var obj in _levelUpObjects)
        {
            obj.SetActive(false);
        }

        _levelUpObjects[level].SetActive(true);
       
        for(int i = 0; i < _levelUpObjects[level].transform.childCount; i++)
        {
            var child = _levelUpObjects[level].transform.GetChild(i);
            child.gameObject.GetComponent<MeshRenderer>().material = teamMat;    
        }
    }

    public virtual void CloseModel()
    {
         foreach (var obj in _levelUpObjects)
        {
            obj.SetActive(false);
        }
    }

    public virtual void OpenHalfHealthVFX()
    {
        // Open VFX
    }

    public virtual void OpenThreeQuarterHealthVFX()
    {
        // Open VFX
    }

    public virtual void OpenTenPercentHealthVFX()
    {
        // Open VFX
    }
    
}
