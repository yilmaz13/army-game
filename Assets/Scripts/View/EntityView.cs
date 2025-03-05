using UnityEngine;
using DG.Tweening;

public class EntityView : MonoBehaviour
{ 
    [SerializeField] protected Renderer[] meshRenderers;
    [SerializeField] protected Transform meshRenderersParent;
    [SerializeField]protected HpSliderView healthView;
    protected SliderView armorView;    

    [SerializeField] protected Material _materialDie;
    [SerializeField] protected Material[] _materials;
    [SerializeField] protected GameObject[] _levelUpObjects;
    
    protected  Material teamMat;

    protected Camera _camera;


    public virtual void Initialize(Camera camera)
    { 
        _camera = camera;
    }

    public virtual void Attack()
    {       
    }
   

    public virtual void Idle()
    {        
    }

    public virtual void Die()
    {
        SetMaterialDie();
    }

    public virtual void InitializeHealthBar(float health, float maxHealth, Team team)
    {
        if(healthView != null)
            healthView.Initialize(health, maxHealth, team);
    }

    public virtual void InitializeArmorBar(float armor, float maxArmor)
    {
        if(armorView != null)
            armorView.Initialize(armor, maxArmor);
    }

    public virtual void UpdateHealthBar(float health, float maxHealth)
    {
        if(healthView != null)
            healthView.UpdateValue(health, maxHealth);
    }

    public virtual void UpdateArmorBar(float armor, float maxArmor)
    {
        if(armorView != null)
            armorView.UpdateValue(armor, maxArmor);
    }

    public virtual void SetMaterial(Team team)
    {        
        teamMat = team == Team.Blue ? _materials[0] : _materials[1];
       
      /*  foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = mat;
        }

       */
        for (int i = 0; i < meshRenderersParent.childCount; i++)
        {
            Renderer meshRenderer = meshRenderersParent.GetChild(i).GetComponent<Renderer>();
            meshRenderer.material = teamMat;
        }
    }

    public virtual void SetMaterialDie()
    {
        /*  foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = _materialDie;
        }*/

        for (int i = 0; i < meshRenderersParent.childCount; i++)
        {
            Renderer meshRenderer = meshRenderersParent.GetChild(i).GetComponent<Renderer>();
            meshRenderer.material = _materialDie;
        }
    }

    public virtual void SetLevelUpObject(int level)
    {
        for (int i = 0; i < _levelUpObjects.Length; i++)
        {
            _levelUpObjects[i].SetActive(false);
        }

         _levelUpObjects[level].SetActive(true);
    }
}