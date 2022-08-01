using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    [SerializeField] private bool _loop;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private UnityEvent _onComplete; 

    private SpriteRenderer _spriteRenderer;
    private float _secondsPerFrame;
    private int _currentFrameIndex;
    private float _nextFrameTime;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        _secondsPerFrame = 1f / _frameRate;
        _nextFrameTime = Time.time + _secondsPerFrame;
        _currentFrameIndex = 0;
    }
    private void Update()
    {
        if (Time.time < _nextFrameTime) return;

        if (_currentFrameIndex >= _sprites.Length)
        {
            if (_loop)
            {
                _currentFrameIndex = 0;
            }
            else
            {
                enabled = false;
                _onComplete?.Invoke();
                return;
            }
        }

        _spriteRenderer.sprite = _sprites[_currentFrameIndex];
        _nextFrameTime += _secondsPerFrame;
        _currentFrameIndex++;
    }
}
