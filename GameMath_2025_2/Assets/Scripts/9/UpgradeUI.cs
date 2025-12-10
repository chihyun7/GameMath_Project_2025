using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI needExpText;
    public Image expBar;
    public TextMeshProUGUI resultText;

    [Header("Buttons")]
    public Button bruteForceBtn;
    public Button minWasteBtn;
    public Button maxEfficiencyBtn;
    public Button bigFirstBtn;

    public Button upgradeBtn;

    int currentLevel = 1;
    int needExp = 0;

    List<MaterialItem> mats;

    private void Start()
    {
        mats = new List<MaterialItem>()
        {
            new MaterialItem("소", 3, 8),
            new MaterialItem("중", 5, 12),
            new MaterialItem("대", 12, 30),
            new MaterialItem("특대", 20, 45)
        };

        bruteForceBtn.onClick.AddListener(() => RunMode(0));
        minWasteBtn.onClick.AddListener(() => RunMode(1));
        maxEfficiencyBtn.onClick.AddListener(() => RunMode(2));
        bigFirstBtn.onClick.AddListener(() => RunMode(3));

        upgradeBtn.onClick.AddListener(UpgradeLevel);

        UpdateUI();
    }

    void UpgradeLevel()
    {
        currentLevel++;
        UpdateUI();
    }

    void UpdateUI()
    {
        needExp = 8 * currentLevel * currentLevel;

        levelText.text = $"+{currentLevel} → +{currentLevel + 1}";
        needExpText.text = $"필요 경험치 {needExp}/{needExp}";
        expBar.fillAmount = 1f;

        resultText.text = "모드를 선택하세요.";
    }

    // ================================================
    // 모드 실행
    // ================================================
    void RunMode(int type)
    {
        List<(MaterialItem, int)> result = null;

        switch (type)
        {
            case 0: result = BruteForce(needExp); break;
            case 1: result = Greedy_MinWaste(needExp); break;
            case 2: result = Greedy_MaxEfficiency(needExp); break;
            case 3: result = Greedy_BigFirst(needExp); break;
        }

        PrintResult(result);
    }

    // ======================================================
    // 알고리즘 구현
    // ======================================================
    List<(MaterialItem, int)> BruteForce(int need)
    {
        int maxCount = need / 3 + 3;
        int minCost = int.MaxValue;
        List<(MaterialItem, int)> best = null;

        for (int a = 0; a < maxCount; a++)
            for (int b = 0; b < maxCount; b++)
                for (int c = 0; c < maxCount; c++)
                    for (int d = 0; d < maxCount; d++)
                    {
                        int exp = a * 3 + b * 5 + c * 12 + d * 20;
                        if (exp < need) continue;

                        int cost = a * 8 + b * 12 + c * 30 + d * 45;

                        if (cost < minCost)
                        {
                            minCost = cost;
                            best = new List<(MaterialItem, int)>()
                {
                    (mats[0], a),
                    (mats[1], b),
                    (mats[2], c),
                    (mats[3], d),
                };
                        }
                    }

        return best;
    }

    List<(MaterialItem, int)> Greedy_MinWaste(int need)
    {
        var sorted = mats.OrderBy(m => m.exp).ToList();
        List<(MaterialItem, int)> result = new();
        int left = need;

        foreach (var m in sorted)
        {
            while (left >= m.exp)
            {
                left -= m.exp;
                result.Add((m, 1));
            }
        }

        if (left > 0)
            result.Add((sorted[0], 1));

        return result;
    }

    List<(MaterialItem, int)> Greedy_MaxEfficiency(int need)
    {
        var sorted = mats.OrderByDescending(m => (float)m.exp / m.cost).ToList();
        List<(MaterialItem, int)> result = new();
        int left = need;

        foreach (var m in sorted)
        {
            while (left >= m.exp)
            {
                left -= m.exp;
                result.Add((m, 1));
            }
        }

        return result;
    }

    List<(MaterialItem, int)> Greedy_BigFirst(int need)
    {
        var sorted = mats.OrderByDescending(m => m.exp).ToList();
        List<(MaterialItem, int)> result = new();
        int left = need;

        foreach (var m in sorted)
        {
            while (left >= m.exp)
            {
                left -= m.exp;
                result.Add((m, 1));
            }
        }

        return result;
    }

    // ======================================================
    void PrintResult(List<(MaterialItem, int)> list)
    {
        string result = "";
        int totalCost = 0;
        int totalExp = 0;

        foreach (var r in list)
        {
            totalCost += r.Item1.cost * r.Item2;
            totalExp += r.Item1.exp * r.Item2;
            result += $"{r.Item1.name} x {r.Item2}\n";
        }

        result += $"\n총 exp: {totalExp}\n총 gold: {totalCost}";
        resultText.text = result;
    }
}

// ======================================================
public class MaterialItem
{
    public string name;
    public int exp;
    public int cost;

    public MaterialItem(string name, int exp, int cost)
    {
        this.name = name;
        this.exp = exp;
        this.cost = cost;
    }
}
