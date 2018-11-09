using TMPro;
using UnityEngine;

public class CounterUpdator : MonoBehaviour
{
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = (GameManager.Instance.CoinCount).ToString();
        GameManager.Instance.Event.onCoinPickUp.AddListener(UpdateCoinCount);
    }

    private void UpdateCoinCount(GameObject obj)
    {
        text.text = (GameManager.Instance.CoinCount += 1).ToString();

    }
}
