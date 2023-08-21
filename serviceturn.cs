using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Networking;

public class serviceturn : MonoBehaviour
{
    public GameObject turnobj;
    public GameObject content;

    public GameObject numlabel;
    public GameObject numcontent;

    public Slider slider;

    public Color transparent;

    public int current_turn = 1;

    string maxturnurl = "https://kairu1206.com/sqlcon/maxturn.php";

    float timer = 1f;
    public float timer_cd;

    // Start is called before the first frame update
    void Start()
    {
        transparent.a = 0;
        StartCoroutine(GetMaxTurn());
    }

    // Update is called once per frame
    void Update()
    {
        // if(timer_cd > 0)
        // {
        //     timer_cd -= Time.deltaTime;
        // }
        // else
        // {
        //     StartCoroutine(GetMaxTurn());
        //     timer_cd = timer;
        // }
    }

    void Display(int max_turn)
    {
        if(turnobj != null)
        {
            for(int i = 0; i < max_turn; i++)
            {
                var t = Instantiate(turnobj, content.transform, false);
                t.name = gameObject.name + (i+1);
                t.transform.GetChild(1).GetComponent<Image>().color = transparent;
                t.transform.GetChild(2).GetComponent<Button>().interactable = false;
            }
        }

        if(numlabel != null)
        {
            for(int i = 0; i < max_turn; i++)
            {
                var t = Instantiate(numlabel,numcontent.transform, false);
                t.transform.GetChild(0).GetComponent<TMP_Text>().text = "" + (i+1);
            }
        }
    }

    IEnumerator GetMaxTurn()
    {
        WWWForm form = new WWWForm();
        form.AddField("type", 0);

        UnityWebRequest www = UnityWebRequest.Post(maxturnurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        if(www.downloadHandler.text != "0 data")
        {
            int.TryParse(www.downloadHandler.text, out int intmaxturn);
            Display(intmaxturn);
        }

        www.Dispose();
        
    }

}
