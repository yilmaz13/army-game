using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Army.Popup;
using DG.Tweening;

public class LevelUpCardPopupView : PopupView
{
    [Header("Popup Elements")]
    [SerializeField] private RectTransform _cardsContainer;
   // [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private Image _titleFirst;
    [SerializeField] private Image _titleUpdate;
    [SerializeField] private LevelUpCard _cardPrefab;
    [SerializeField] private int _cardCount = 3;

    [Header("Visual Effects")]
    [SerializeField] private ParticleSystem _levelUpParticles;
    [SerializeField] private AudioClip _openSound;
    [SerializeField] private AudioClip _selectSound;
    
    private ArmyController _currentArmy;
    private List<LevelUpCard> _activeCards = new List<LevelUpCard>();
    private AudioSource _audioSource;
    
    private const float CARD_ANIMATION_DELAY = 0.5f;

    public override void Initialize()
    {       
        _audioSource = gameObject.AddComponent<AudioSource>();
    }
    
    public override void Open()
    {     
        EnableInput();

       _audioSource.PlayOneShot(_openSound);       
       _levelUpParticles.Play();  

        Time.timeScale = 0f;        
    }
    
    public override void Close()
    {        
        ClearCards();       
    
        Time.timeScale = 1f;
       
    }
    
    public override void Hidden()
    {
        base.Hidden();
    }
    
    public void SetupCards(ArmyController army)
    {
        _currentArmy = army;       
     
        ClearCards();        
       
        _titleFirst.gameObject.SetActive(true);                
       
        AgentType[] availableUpgrades = army.GetUpgradeOptionsForCurrentLevel();
        int optionsCount = Mathf.Min(availableUpgrades.Length, _cardCount);
    
        for (int i = 0; i < optionsCount; i++)
        {
            LevelUpCard card = Instantiate(_cardPrefab, _cardsContainer);               
      
            card.SetupCard(availableUpgrades[i], 1);
            card.OnCardSelected += HandleCardSelected;
            
            _activeCards.Add(card);            
          
            card.AnimateIn();
        }
    }
    
    private void HandleCardSelected(AgentType selectedType)
    {
      
        _audioSource.PlayOneShot(_selectSound);      
     
        foreach (var card in _activeCards)
        {
            card.SetInteractable(false);
        }             
     

        DOVirtual.DelayedCall(CARD_ANIMATION_DELAY , () =>
        {
           ProcessSelection(selectedType);
        });
    }
    
    private void ProcessSelection(AgentType selectedType)
    { 
        _currentArmy.AddAvailableAgentType(selectedType);
        PopupManager.Instance.HideAllPopups();
    }
    
    private void ClearCards()
    {
        foreach (var card in _activeCards)
        {
            if (card != null)
            {
                card.OnCardSelected -= HandleCardSelected;
                Destroy(card.gameObject);
            }
        }
        
        _activeCards.Clear();
    }
    
    private void OnDestroy()
    {
        ClearCards();
    }
}