using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
class Cheat
{
    public string Name;
    public string charCombination;
    public UnityEvent action;
}

public class CheatController : MonoBehaviour
{
    [SerializeField] private float inputTimeToLive;
    [SerializeField] private Cheat[] cheats;
    
    private float inputTime;
    private string currentText;

    private void Awake()
    {
        Keyboard.current.onTextInput += OnTextInput;
    }

    private void OnTextInput(char inputChars)
    {
        currentText += inputChars;
        inputTime = inputTimeToLive;
        FindAnyCheat();
    }

    private void FindAnyCheat()
    {
        foreach (var cheat in cheats)
        {
            if (cheat.charCombination != string.Empty)
            {
                if (currentText.Contains(cheat.charCombination))
                {
                    cheat.action.Invoke();
                    currentText = string.Empty;
                }
            }
        }
    }

    private void Update()
    {
        if (inputTime < 0)
        {
            currentText = string.Empty;
        }
        else
        {
            inputTime -= Time.deltaTime;
        }
    }
}
