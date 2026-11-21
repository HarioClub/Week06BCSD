using UnityEngine;
using TMPro;

public class TextTMPViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textPlayerHP;
    [SerializeField]
    private TextMeshProUGUI textPlayerGold;
    [SerializeField]
    private TextMeshProUGUI textWave;
    [SerializeField]
    private TextMeshProUGUI textEnemyCount;
    [SerializeField]
    private PlayerHP playerHP;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private EnemySpawner enemySpawner;

    private void Update()
    {
        textPlayerHP.text = playerHP.CurrentHP + "/" + playerHP.MaxHP;
        textPlayerGold.text = playerGold.CurrentGold.ToString();
        WaveSystem wave = WaveSystem.Instance;
        textWave.text = wave.CurrentWave + "/" + wave.MaxWave;
        textEnemyCount.text = enemySpawner.CurrentEnemyCount + "/" + enemySpawner.MaxEnemyCount;
    }
}
