using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStatsView : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider expSlider;
    [SerializeField] private TextMeshProUGUI enemiesKilledText;
    [SerializeField] private TextMeshProUGUI levelText;
    
    public void SetHealth(float value)
    {
        healthSlider.value = value;
    }
    
    public void SetEnemiesKilled(int enemiesKilled, int level, float experienceValue)
    {
        enemiesKilledText.text = enemiesKilled.ToString();
        expSlider.value = experienceValue;
        levelText.text = $"Lv.{level.ToString()}";
    }
}
