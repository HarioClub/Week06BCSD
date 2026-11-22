using UnityEngine;
using TMPro;

public enum SystemType { Money = 0, Build }

public class SystemTextViewer : MonoBehaviour
{
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;

    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        if (WaveSystem.Instance.IsFinished) return;
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "System : Not enough money...";
                break;
            case SystemType.Build:
                textSystem.text = "System : Invalid build tower...";
                break;
        }

        tmpAlpha.FadeOut();
    }

    public void ShowResult(bool won)
    {
        tmpAlpha.StopAllCoroutines();
        textSystem.alpha = 1f;
        textSystem.text = won ? "Stage clear! Click the wave button to restart." : "Game over. Click the wave button to restart.";
    }
}
