using System.Collections.Generic;
using UnityEngine;

public class ScoreLabel : MonoBehaviour
{
    public UnityEngine.UI.Text textScore;
    public UnityEngine.UI.Text textRate;

    public static uint coinsCount = 0;
    private int lastCoin = 0;
    private int bestRate = 0;
    private Queue<System.DateTime> coins = new Queue<System.DateTime>();

    void CountCoins()
    {
        // remove stalled coins
        {
            var thr = System.DateTime.Now + System.TimeSpan.FromSeconds(-120);
            while (coins.Count > 0 && coins.Peek() < thr)
                coins.Dequeue();
        }

        // add new coins
        {
            var t = System.DateTime.Now;
            while (lastCoin < coinsCount)
            {
                lastCoin++;
                coins.Enqueue(t);
            }
        }
    }

    void Update()
    {
        if (!textScore)
            return;
        if (Input.GetAxis("Restart") > 0)
        {
            coinsCount = 0;
            lastCoin = 0;
            bestRate = 0;
            coins.Clear();
            return;
        }
        CountCoins();
        textScore.text = coinsCount.ToString();
            textRate.text = "- (" + bestRate.ToString() + ")";
        if (coins.Count >= 1)
        {
            float duration = (float)(System.DateTime.Now - coins.Peek()).TotalSeconds;
            if (duration > 30)
            {
                int rate = Mathf.RoundToInt(60f * coins.Count / duration);
                if (rate > bestRate)
                    bestRate = rate;
                textRate.text = rate.ToString() + " (" + bestRate.ToString() + ")";
            }
        }
    }
}
