using UnityEngine;
using TMPro;

public class FoodDisplay : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI foodText;

    #endregion

    #region Update Text

    public void UpdateText(int food)
    {
        foodText.text = "Food: " + food;
    }

    public void UpdateTextWithIncrement(int increment, int food)
    {
        foodText.text = "+" + increment + " Food: " + food;
    }

    public void UpdateTextWithDecrement(int decrement, int food)
    {
        foodText.text = "-" + decrement + " Food: " + food;
    }

    #endregion
}
