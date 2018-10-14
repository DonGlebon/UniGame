using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public TextMeshProUGUI coins;
    private int count = 0;

    public void UpdateCoins()
    {
        coins.text = string.Empty + count++;
    }


}
