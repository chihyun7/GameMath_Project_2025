using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardComboBruteForce : MonoBehaviour
{
    struct Card
    {
        public string name;
        public int damage;
        public int cost;
        public int count;

        public Card(string name, int damage, int cost, int count)
        {
            this.name = name;
            this.damage = damage;
            this.cost = cost;
            this.count = count;
        }
    }

    void Start()
    {
        // Ä«µå ¸ñ·Ï Á¤ÀÇ
        List<Card> cards = new List<Card>()
        {
            new Card("Äü¼¦", 6, 2, 2),
            new Card("Çìºñ¼¦", 8, 3, 2),
            new Card("¸ÖÆ¼¼¦", 16, 5, 1),
            new Card("Æ®¸®ÇÃ¼¦", 24, 7, 1)
        };

        int maxCost = 15;
        int bestDamage = 0;
        string bestCombo = "";

        // ºê·çÆ®Æ÷½º Å½»ö
        for (int q = 0; q <= 2; q++) // Äü¼¦ ÃÖ´ë 2Àå
        {
            for (int h = 0; h <= 2; h++) // Çìºñ¼¦ ÃÖ´ë 2Àå
            {
                for (int m = 0; m <= 1; m++) // ¸ÖÆ¼¼¦ 0~1Àå
                {
                    for (int t = 0; t <= 1; t++) // Æ®¸®ÇÃ¼¦ 0~1Àå
                    {
                        int cost = q * 2 + h * 3 + m * 5 + t * 7;
                        int dmg = q * 6 + h * 8 + m * 16 + t * 24;

                        if (cost <= maxCost && dmg > bestDamage)
                        {
                            bestDamage = dmg;
                            bestCombo = $"Äü¼¦ x{q}, Çìºñ¼¦ x{h}, ¸ÖÆ¼¼¦ x{m}, Æ®¸®ÇÃ¼¦ x{t}";
                        }
                    }
                }
            }
        }

        Debug.Log($"[°á°ú] ÃÖ´ë µ¥¹ÌÁö: {bestDamage}, Á¶ÇÕ: {bestCombo}");
    }
}
