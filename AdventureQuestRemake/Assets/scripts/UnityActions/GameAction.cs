using UnityEngine.Events;
using UnityEngine;

[CreateAssetMenu]
public class GameAction : ScriptableObject
{
    public UnityAction raise;

    public void RaiseAction()
    {
        raise?.Invoke();
    }
}
