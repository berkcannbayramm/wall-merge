using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;
    public void UpdateMoney(int value)
    {
        DataHandler.MoneyIndex += value;

        UpdateMoneyText();
    }

    public void UpdateMoneyText()
    {
        moneyText.text = DataHandler.MoneyIndex.ToString();
    }
    
    public int GetMoney()
    {
        return DataHandler.MoneyIndex;
    }
}
