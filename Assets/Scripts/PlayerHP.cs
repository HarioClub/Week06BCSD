using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHP : MonoBehaviour
{
    [SerializeField]
    private Image imageScreen;
    [SerializeField]
    private float maxHP = 20;
    private float currentHP;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (currentHP <= 0f) return;
        currentHP = Mathf.Max(0f, currentHP - damage);

        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP == 0f) WaveSystem.Instance.GameOver();
    }

    private IEnumerator HitAlphaAnimation()
    {
        Color color = imageScreen.color;
        color.a = 0.4f;
        imageScreen.color = color;

        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;
            imageScreen.color = color;

            yield return null;
        }
    }
}
