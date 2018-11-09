using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Settings", order = 1)]
[System.Serializable]
public class LevelSettings : ScriptableObject
{
    [Header("Audio manager")]
    [SerializeField]
    public AudioManager audioManager;

}
