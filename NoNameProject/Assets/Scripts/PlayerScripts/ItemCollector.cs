using UnityEngine;
using UnityEngine.UI;

public class ItemCollector : MonoBehaviour
{
    [SerializeField] private int coins;
    [SerializeField] private Text coinsText;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Coin"))
        {
            Destroy(col.gameObject);
            coins++;
            coinsText.text = "Coins: " + coins;
        }
    }
}
