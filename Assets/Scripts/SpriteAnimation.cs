using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    [SerializeField] private bool _loop;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private UnityEvent _onComplete;
    [SerializeField] private AnimationClip[] _clips;

    private SpriteRenderer _spriteRenderer;
    private float _secondsPerFrame;
    private int _currentFrameIndex;
    private float _nextFrameTime;
    private int currentClip;

    private bool _isPlaying;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnBecameVisible()
    {
        enabled = _isPlaying;
    }
    private void OnBecameInvisible()
    {
        enabled = false;   
    }
    private void OnEnable()
    {
        _secondsPerFrame = 1f / _frameRate;
    }
    public void SetClip(string clipName)
    {
        for (int i=0; i < _clips.Length; i++)
        {
            if (_clips[i].name == clipName)
            {
                currentClip = i;
                StartAnimation();
                return;
            }
        }
    }

    private void StartAnimation()
    {
        _nextFrameTime = Time.time + _secondsPerFrame;
        _currentFrameIndex = 0;
        _isPlaying = true;
    }

    private void Update()
    {
        if (!_isPlaying || Time.time < _nextFrameTime) return;

        if (_currentFrameIndex >= _sprites.Length)
        {
            if (_loop)
            {
                _currentFrameIndex = 0;
            }
            else
            {
                enabled = _isPlaying = false;
                _onComplete?.Invoke();
                return;
            }
        }

        _spriteRenderer.sprite = _sprites[_currentFrameIndex];
        _nextFrameTime += _secondsPerFrame;
        _currentFrameIndex++;
    }
}
