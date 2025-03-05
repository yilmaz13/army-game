using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpCard : MonoBehaviour
{
    [Header("Visual Components")]
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _cardFrame;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _titleText;
    [SerializeField] private Image _descriptionText;
    [SerializeField] private Button _cardButton;
    [SerializeField] private CanvasGroup _canvasGroup;
    
    [Header("Animation")]
    [SerializeField] private Animator _animator;    
   
    // Animation State Names
    private const string ANIM_STATE_SLIDE_START = "CardSlideStart";
    private const string ANIM_STATE_SLIDE = "CardSlide";
    private const string ANIM_STATE_SELECT = "CardSelect";
    private const string ANIM_STATE_FADE_OUT = "CardFade";      
    

    [Header("Effects")]
    [SerializeField] private ParticleSystem _selectionParticles;    
    
    private CardAnimState _currentAnimState = CardAnimState.CardSlideStart;
    private AgentType _agentType;
    private bool _isSelected = false;
    
    public event Action<AgentType> OnCardSelected;
    
    private void Awake()
    {       
        _cardButton.onClick.AddListener(OnClick);           
       
        SetInteractable(false);
    }
    
    private void OnDestroy()
    {        
        _cardButton.onClick.RemoveListener(OnClick);        
    }
    
    public void SetupCard(AgentType type, int level)
    {
        _agentType = type;        
       
        AgentStats agentStats = FindAgentStats(type);
        if (agentStats != null)
        {
            if (_iconImage != null)
            {
                _iconImage.sprite = agentStats.GetIconForLevel(level);
            }

            if (_titleText != null)
            {
                _titleText.sprite = agentStats.GetTitleForLevel(level);
            }
           
            if (_descriptionText != null)
            {
                _descriptionText.sprite = agentStats.GetDescriptionForLevel(level);
            }            

            if (_cardFrame != null)
            {                
                Sprite frameSprite = agentStats.GetCardFrameForLevel(level);
                if (frameSprite != null)
                {
                    _cardFrame.sprite = frameSprite;
                }
            }
        }     
      
        if (_animator != null)
        {
            _animator.Play(ANIM_STATE_SLIDE_START);
            _currentAnimState = CardAnimState.CardSlideStart;
        }
    }

    private AgentStats FindAgentStats(AgentType type)
    {
        // TODO: GameManager, ResourceManager gibi bir yerden tüm agent stats verilerini al
        AgentStats[] allStats = Resources.FindObjectsOfTypeAll<AgentStats>();
        
        foreach (var stats in allStats)
        {
            if (stats.agentType == type)
                return stats;
        }
        
        return null;
    }    
    
    public void SetInteractable(bool interactable)
    {
        if (_cardButton != null)
        {
            _cardButton.interactable = interactable;
        }     
    }
    
    private void OnClick()
    {
        if (_isSelected) 
            return;
        
        _isSelected = true;    
      
        _animator.Play(ANIM_STATE_SELECT);
        _currentAnimState = CardAnimState.CardSelect;   
        
        _selectionParticles.Play();
       
        StartCoroutine(TriggerSelectionAfterAnimation());
    }
    
    private IEnumerator TriggerSelectionAfterAnimation()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        
        OnCardSelected?.Invoke(_agentType);
    }
    
    public void AnimateIn()
    {
        _animator.Play(ANIM_STATE_SLIDE);
        _currentAnimState = CardAnimState.CardSlide;

        //TODO 
        SetInteractable(true);
    }     
   
    public void FadeOut()
    {        
        _animator.Play(ANIM_STATE_FADE_OUT);
        _currentAnimState = CardAnimState.FadeOut;
       
        SetInteractable(false);
    }  
}