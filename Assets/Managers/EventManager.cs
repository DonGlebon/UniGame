using UnityEngine;
using UnityEngine.Events;

public class EventManager
{


    public UnityGameObjectEvent onCoinPickUp;
    public UnityEvent onJump;

    public EventManager()
    {
        onCoinPickUp = new UnityGameObjectEvent();
        onJump = new UnityEvent();
    }

}

[System.Serializable]
public class UnityGameObjectEvent : UnityEvent<GameObject>
{
}
