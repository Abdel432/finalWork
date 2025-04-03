using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextsystemManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;

    private List<TextSystem> textSystem = new List<TextSystem>();

    private void Start()
    {
       
    }


    private void Update()
    {
        foreach (TextSystem txt in textSystem)
            txt.UpdateFloatingText();
            
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        TextSystem textSystem = GetTextSystem();

        textSystem.txt.text = msg;
        textSystem.txt.fontSize = fontSize;
        textSystem.txt.color = color;
        textSystem.go.transform.position = Camera.main.WorldToScreenPoint( position);

        textSystem.motion = motion;      
        textSystem.duration = duration;
        //tranfer world space to screeen space so we can use it in the UI

        textSystem.Show();


    }

    private TextSystem GetTextSystem()
    {
        TextSystem txt = textSystem.Find(t => !t.active);

        if(txt == null)
        {
            txt = new TextSystem();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<Text>();

            textSystem.Add(txt);
        }

        return txt;
    }

}
