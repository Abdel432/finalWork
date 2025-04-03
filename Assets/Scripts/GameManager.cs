using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(textsystemManager.gameObject);
            Destroy(hud);
            Destroy(menu);
            return;
        }
        instance = this;
       // every time this function is fired it is going to call in saveState
        
        SceneManager.sceneLoaded += LoadState;
        SceneManager.sceneLoaded += OnSceneLoaded;
        

    }

    // Ressources to be tracked

    public List<Sprite> playerSprite;
    public List<Sprite> weaponSprites;
    public List<int> weaponPrices;
    public List<int> XpTable;
    public List<int> weaponLevel;

    // refrences

    public Player player;
    public Weapon weapon;
    public RectTransform hitpointBar;
    public GameObject hud;
    public GameObject menu;
    public Animator deathMenuAnim;

    // public weapon
    public TextsystemManager textsystemManager;




    //Logic

    public int pesos;
    public int experience;


    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        textsystemManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Hitpoint bar

    public void OnHitpointChange()
    {
        float ratio = (float)player.hitpoint / (float)player.maxHitpoint;
        hitpointBar.localScale = new Vector3(1, ratio, 1);
    }

    // upgrade weapon

    public bool TryUpgradeWeapon()
    {
        // is the weapon max level

        if (weaponPrices.Count <= weapon.weaponLevel)
            return false;
        if(pesos >= weaponPrices[weapon.weaponLevel])
        {
            pesos -= weaponPrices[weapon.weaponLevel];
            weapon.UpgradeWeapon();
            return true;
        }

        return false;

    }

    private void Update()
    {
       GetCurrentLevel();
    }


    // experience system

    public int GetCurrentLevel()
    {
        int r = 0;
        int add = 0;

        while( experience >= add)
        {
            add += XpTable[r];
            r++;

            if (r == XpTable.Count) // Max level  
                return r;
        }

        return r;
     

    }

    public int GetXptoLevel(int level)
    {
        int r = 0;
        int xp = 0;

        while (r < level)
        {
            xp += XpTable[r];
            r++;

        }
        return xp;
    }

    public void GrantXp(int xp)
    {
        int currLevel = GetCurrentLevel();
        experience += GetCurrentLevel();
        if (currLevel < GetCurrentLevel())
            OnLevelUp();
    }

    public void OnLevelUp()
    {
        Debug.Log("Level up");
        player.OnLevelUp();
        OnHitpointChange();

    }


    // on scene loaded

    public void OnSceneLoaded(Scene s, LoadSceneMode mode)
    {
        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }

    public void Respawn()
    {
        deathMenuAnim.SetTrigger("hide");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
       
            
    }


    // Save state
  
    public void SaveState()
    {
        string s = "";

        s += "0" + "|";
        s += pesos.ToString() + "|";
        s += experience.ToString() + "|";
        s += weapon.weaponLevel.ToString();


        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("save state");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= LoadState;


        if (!PlayerPrefs.HasKey("SaveState"))
            return;
        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // change player skin
        pesos = int.Parse(data[1]);

        // experience 
        experience = int.Parse(data[2]);
        if(GetCurrentLevel() != 1)
            player.SetLevel(GetCurrentLevel());

       
        // change the weapon Level



        weapon.SetWeaponLevel(int.Parse(data[3]));

        Debug.Log("load sate");

        
       

       
    }



}
