using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    // Text fields

    public Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    //Logic


    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite;
    public Image weaponSprite;
    public RectTransform xpBar;

    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            //if we went too far away

            if (currentCharacterSelection == GameManager.instance.playerSprite.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();

        }
        else
        {
            currentCharacterSelection--;

            //if we went too far away

            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprite.Count - 1;



            OnSelectionChanged();

        }


    }
    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprite[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // weapon Upgrade

    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    // update the character information

    public void UpdateMenu()
    {
        // weapon;
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();



        // meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // XP bar
        int currLevel = GameManager.instance.GetCurrentLevel();
        if(currLevel == GameManager.instance.XpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points"; // displays total xp
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int prevlevelXp = GameManager.instance.GetXptoLevel(currLevel - 1);
            int currLevelXp = GameManager.instance.GetXptoLevel(currLevel);
            int diff = currLevelXp - prevlevelXp;

            int currXpIntoLevel = GameManager.instance.experience - prevlevelXp;

            float completionRatio = (float)currXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currXpIntoLevel.ToString() + " / " + diff;
        }

        

    }
}
