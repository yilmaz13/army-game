using UnityEngine;
using TMPro;
using System.Collections;

public class FpsCounter : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _updateInterval = 0.5f;
    [SerializeField] private int _targetFrameRate = 60;
    
    [Header("Display")]
    [SerializeField] private TextMeshProUGUI _fpsText;
    [SerializeField] private bool _showAsOverlay = true;
    
    [Header("Colors")]
    [SerializeField] private Color _goodFpsColor = Color.green;
    [SerializeField] private Color _mediumFpsColor = Color.yellow;
    [SerializeField] private Color _badFpsColor = Color.red;
    [SerializeField] private float _goodFpsThreshold = 50f;
    [SerializeField] private float _mediumFpsThreshold = 30f;
    
    private float _fps;
    private float _accum;
    private int _frames;
    private float _timeLeft;
    private GUIStyle _guiStyle = new GUIStyle();
    
    private void Awake()
    {
        if (_targetFrameRate > 0)
        {
            Application.targetFrameRate = _targetFrameRate;
        }
        
        if (_fpsText != null)
        {
            _showAsOverlay = false;
        }
        else if (_showAsOverlay)
        {           
            _guiStyle.fontSize = 48;
            _guiStyle.normal.textColor = _goodFpsColor;
            _guiStyle.fontStyle = FontStyle.Bold;
            _guiStyle.alignment = TextAnchor.UpperLeft;
        }
        
        StartCoroutine(FPSCalculator());
    }
    
    private IEnumerator FPSCalculator()
    {
        _timeLeft = _updateInterval;
        
        while (true)
        {      
            _timeLeft -= Time.deltaTime;
            _accum += Time.timeScale / Time.deltaTime;
            _frames++;
            
            
            if (_timeLeft <= 0.0f)
            {
                _fps = _accum / _frames;
                _timeLeft = _updateInterval;
                _accum = 0.0f;
                _frames = 0;
                              
                if (!_showAsOverlay && _fpsText != null)
                {
                    _fpsText.text = $"FPS: {_fps:0.0}";
                    _fpsText.color = GetFPSColor(_fps);
                }
            }
            
            yield return null;
        }
    }
    
    private void OnGUI()
    {
        if (_showAsOverlay)
        {
            _guiStyle.normal.textColor = GetFPSColor(_fps);
            GUI.Label(new Rect(100, 150, 100, 50), $"FPS: {_fps:0.0}", _guiStyle);
        }
    }
    
    private Color GetFPSColor(float currentFps)
    {
        if (currentFps >= _goodFpsThreshold)
            return _goodFpsColor;
        else if (currentFps >= _mediumFpsThreshold)
            return _mediumFpsColor;
        else
            return _badFpsColor;
    }
}