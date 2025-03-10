using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class AgentView : EntityView
{   
    [SerializeField] protected Animator _animator;
    [SerializeField] protected ParticleSystem _hitEffect;
    protected Transform _healthSliderPoint;
    protected Transform _armorSliderPoint;
    protected Rigidbody _rigidbody;
    protected GameObject _healthSliderPrefabs;
    protected GameObject _armorSliderPrefabs;

    private Tween _attackTween;
   
    
    public NavMeshAgent NavMeshAgent;
    protected float _moveSpeed;

    public virtual void Initialize(float moveSpeed, Camera camera, GameObject healthSliderPrefabs, GameObject armorSliderPrefabs)
    {
        _moveSpeed = moveSpeed;
        _camera = camera;
        _healthSliderPrefabs = healthSliderPrefabs;
        _armorSliderPrefabs = armorSliderPrefabs;        
    }

     public virtual void Initialize(float moveSpeed, Camera camera, int level)
    {
        _moveSpeed = moveSpeed;
        _camera = camera;       
        SetLevelUpObject(level);
    }

    public override void Attack()
    {
        _animator.SetBool("Moving", false);
        _animator.SetBool("Attacking", true);
        _animator.Play("AttackBegin");

        _attackTween = DOTween.Sequence()
            .AppendInterval(0.5f)
            .AppendCallback(() =>  _hitEffect.Play())
            .AppendInterval(1f)
            .AppendCallback(() => _animator.SetBool("Attacking", false));        
    }
    
    public void MoveTo(Vector3 destination)
    {       
        _animator.SetBool("Moving", true);
        NavMeshAgent.SetDestination(destination);
    }

    public void StopMovement()
    {
        _animator.SetBool("Moving", false);
        if (NavMeshAgent.enabled)
            NavMeshAgent.ResetPath();
    }
    
    public override void Idle()
    {
        _animator.SetBool("Moving", false);
        _animator.SetBool("Attacking", false);
    }

    public override void Die()
    {
        _animator.SetTrigger("Death");
        _animator.SetBool("Attacking", false);     
        _animator.SetBool("Moving", false);  
        SetMaterialDie();
    }

    public override void SetLevelUpObject(int level)
    {
        for (int i = 0; i < _levelUpObjects.Length; i++)
        {
            _levelUpObjects[i].SetActive(false);
        }

         _levelUpObjects[level].SetActive(true);
    }  

    public void LookAtPosition(Vector3 position)
    {
        var lookDirection = position;
        lookDirection.y = transform.position.y;
        transform.LookAt(lookDirection, Vector3.up);
    }

    public void ShowBars()
    {
        healthView.Show();
        armorView.Show();
    }

    public override void SetMaterial(Team team)
    {
        Material material = team == Team.Blue ? _materials[0] : _materials[1];     
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = material;
        }
    }
    public void SetSpeed(float speed)
    {
        NavMeshAgent.speed = speed;
    }  

    public override void SetMaterialDie()
    {
        foreach (var meshRenderer in meshRenderers)
        {
            meshRenderer.material = _materialDie;
        }
    }

    public void ResetVisuals()
    {
        _animator.SetBool("Moving", false);
        _animator.SetBool("Attacking", false);
        _animator.ResetTrigger("Death");      
        _hitEffect.Stop();   
        
        _moveSpeed = 0;

        if  (_attackTween != null)
            _attackTween.Kill();        
    }

    public void ResetNavMeshAgent()
    {
        NavMeshAgent.enabled = true;        
    }

    void OnDestroy()
    {
        DOTween.Kill(transform);
        _attackTween.Kill();
    }
}
