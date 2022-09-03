using System;
using UnityEngine;
using UnityEngine.Events;

public class EnterColliisonComponent : MonoBehaviour
{
    [SerializeField] private string _tag;
    [SerializeField] public EnterEvent _action;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(_tag))
        {
            if (_action != null)
            {
                _action.Invoke(collision.gameObject);
            }
        }
    }
    [Serializable]
    public class EnterEvent: UnityEvent<GameObject>
    {

    }
}
