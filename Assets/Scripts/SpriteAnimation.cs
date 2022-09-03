using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteAnimation : MonoBehaviour
{
    [SerializeField] private int _frameRate;
    [SerializeField] private UnityEvent<string> _onComplete;
    [SerializeField] private SpriteAnimationClip[] _clips;

    private SpriteRenderer _spriteRenderer;

    private float _secondsPerFrame;
    private int _currentFrameIndex;
    private float _nextFrameTime;
    private bool _isPlaying=true;

    private int currentClip=0;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _secondsPerFrame = 1f / _frameRate;
        currentClip = 0;
        StartAnimation();
    }
    private void OnBecameVisible()
    {
        enabled = _isPlaying;
    }
    private void OnBecameInvisible()
    {
        enabled = false;   
    }
    public void SetClip(string clipName)
    {
        for (int i=0; i < _clips.Length; i++)
        {
            if (_clips[i].Name == clipName)
            {
                currentClip = i;
                StartAnimation();
                return;
            }
        }
        enabled = _isPlaying = false;
    }

    private void StartAnimation()
    {
        _nextFrameTime = Time.time + _secondsPerFrame;
        _currentFrameIndex = 0;
        _isPlaying = true;
    }

    private void Update()
    {
        if (!enabled || !_isPlaying || Time.time < _nextFrameTime) return;

        var clip = _clips[currentClip];

        if (_currentFrameIndex >= clip.Sprites.Length)
        {
            if (clip.Loop)
            {
                _currentFrameIndex = 0;
            }
            else
            {
                enabled = _isPlaying = false;
                clip.OnComplete?.Invoke();
                return;
            }
        }

        _spriteRenderer.sprite = clip.Sprites[_currentFrameIndex];
        _nextFrameTime += _secondsPerFrame;
        _currentFrameIndex++;
    }


    [Serializable]
    public class SpriteAnimationClip
    {
        [SerializeField] private string _name;
        [SerializeField] private Sprite[] _sprites;
        [SerializeField] private bool _loop;
        [SerializeField] private bool _allowNextClip;
        [SerializeField] private UnityEvent _onComplete;

        public string Name => _name;
        public Sprite[] Sprites => _sprites;
        public bool Loop => _loop;
        public bool AllowNextClip => _allowNextClip;
        public UnityEvent OnComplete => _onComplete;
    }
}
