using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamage;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;

    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    private TowerWeapon currentTower;

    public void OnPanel(Transform towerWeapon)
    {
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        gameObject.SetActive(true);
        UpdateTowerData();
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        imageTower.sprite = currentTower.TowerSprite;
        bool hasBarrel = currentTower.Type == TowerType.Cannon || currentTower.Type == TowerType.Laser;
        imageTower.rectTransform.sizeDelta = new Vector2(hasBarrel ? 88 : 59, 59);
        if (hasBarrel)
            textDamage.text = $"Damage : {currentTower.Damage}+<color=red>{currentTower.BonusDamage:F1}</color>";
        else if (currentTower.Type == TowerType.Slow)
            textDamage.text = $"Slow : {currentTower.Slow * 100:0}%";
        else
            textDamage.text = $"Buff : {currentTower.Buff * 100:0}%";
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel;
    }

    public void OnClickEventTowerUpgrade()
    {
        bool isSuccess = currentTower.Upgrade();

        if (isSuccess == true)
        {
            UpdateTowerData();
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        currentTower.Sell();
        OffPanel();
    }
}
