using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    public float timeleft;
    public bool startTimer = false;
    public bool currently_count_down = false;
    public bool finish = false;
    public Color transparent;

    string syncDBurl = "https://kairu1206.com/sqlcon/syncDB.php";
    string turncounturl = "https://kairu1206.com/sqlcon/turncount.php";         
    string updateturnserviceurl = "https://kairu1206.com/sqlcon/updateturnservices.php";

    float update_timer = 1f;
    float update_timer_cd;

    public int appointment = 0;
    public int request = 0;

    public string serviceid = "";
    public string servicename = "";
    public int servicecost = 0;
    public string apporreq = "";

    public bool addTurn = true;

    public int turncount = 0;

    void Start()
    {
        transparent.a = 0;
        update_timer_cd = 0;
        GetTurnCount();
    }
    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.GetSiblingIndex() + 1 == gameObject.transform.parent.transform.parent.transform.parent.GetComponent<serviceturn>().current_turn)
        {
            gameObject.transform.GetChild(2).GetComponent<Button>().interactable = true;
        }

        if(startTimer)
        {
            if(addTurn && gameObject.transform.GetSiblingIndex() + 1 < gameObject.transform.parent.transform.childCount)
            {
                gameObject.transform.parent.GetChild(gameObject.transform.GetSiblingIndex() + 1).transform.GetChild(2).GetComponent<Button>().interactable = true;
                addTurn = false;
            }
            
            if(timeleft > 0)
            {
                timeleft -= Time.deltaTime;
                gameObject.GetComponent<Slider>().value = timeleft;
                currently_count_down = true;
                gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = string.Format("{0}:{1}{2}", 
                                                                                                Mathf.RoundToInt(timeleft)/60,                       //minutes
                                                                                                Mathf.RoundToInt(timeleft%60)/10,                   //seconds
                                                                                                Mathf.RoundToInt(timeleft%60)%10
                                                                                                );
                
            }
            else if(timeleft == 0)
            {
                startTimer = false;
                gameObject.transform.GetChild(2).GetComponent<Button>().interactable = false;
                gameObject.transform.GetChild(3).GetComponent<TMP_Text>().text = "FINISH";
                currently_count_down = false;
                finish = true;
                addTurn = true;
                GlobalVariable.AddTurn(gameObject.transform.parent.transform.parent.transform.parent.GetComponent<serviceturn>().current_turn, 
                                        serviceid, servicename, servicecost, apporreq,
                                        gameObject.transform.parent.transform.parent.transform.parent.GetSiblingIndex());
                StartCoroutine(UpdateTurnService(0, gameObject.name));
                StartCoroutine(UpdateTurnService(1, gameObject.name));
                
            }

            if(!finish)
            {
                if(update_timer_cd > 0)
                {
                    update_timer_cd -= Time.deltaTime;
                }
                else
                {
                    runSync(1, -1); 
                    update_timer_cd = update_timer;
                }
            }

        }

        if(!gameObject.transform.GetChild(2).GetComponent<Button>().interactable && !finish)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.grey;
        }
        else if(gameObject.transform.GetChild(2).GetComponent<Button>().interactable && !finish)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
        }
        else
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.red;
        }

        if(appointment == 1)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.blue;
            apporreq = "appointment";
        }
        if(request == 1)
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.yellow;
            apporreq = "request";
        }

        if(appointment == 0 && request == 0 && gameObject.name == (gameObject.transform.parent.transform.parent.transform.parent.name + gameObject.transform.parent.transform.parent.transform.parent.GetComponent<serviceturn>().current_turn))
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().color = Color.white;
            apporreq = "";
        }
        
    }

    public void runSync(int type, int time)
    {
        StartCoroutine(Sync(type, gameObject.name, time));
    }

    IEnumerator Sync(int type, string n, int t)
    {
        WWWForm form = new WWWForm();
        form.AddField("type", type);
        form.AddField("name", n);


        int i = -1;
        foreach(var md1 in GlobalVariable.metadata1)
        {
            if(md1[2] == gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text)
            {
                int.TryParse(md1[0], out int index);
                i = index;
                break;
            }
        }

        form.AddField("serviceindex", i);
        form.AddField("time", t);
        form.AddField("appointment", appointment);
        form.AddField("request", request);

        UnityWebRequest www = UnityWebRequest.Post(syncDBurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

    IEnumerator GetTurnCount()
    {
        WWWForm form = new WWWForm();
        form.AddField("type", 0);

        UnityWebRequest www = UnityWebRequest.Post(turncounturl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        int.TryParse(www.downloadHandler.text, out int intturncount);
        turncount = intturncount;
        www.Dispose();
    }

    IEnumerator UpdateTurnService(int type, string name)                                //Type 0: pull, 1: push
    {
        WWWForm form = new WWWForm();
        form.AddField("type", type);
        form.AddField("name", name);
        form.AddField("service", servicename);
        form.AddField("cost", servicecost);

        UnityWebRequest www = UnityWebRequest.Post(updateturnserviceurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        if(type == 0)
        {
            string[] tempdata = www.downloadHandler.text.Split("*");
            string[][] data = new string[tempdata.Length - 1][];

            for(int o = 0; o < data.Length; o++)
            {
                data[o] = tempdata[o].Split("_");                    //Setting the data into 2d array
            }

            GlobalVariable.metadata = data;

            int.TryParse(data[0][1], out int totalcost);
            if(totalcost >= turncount)
            {
                gameObject.transform.parent.transform.parent.transform.parent.GetComponent<serviceturn>().current_turn += 1;
            }

            gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = data[0][0];

        }
        

        www.Dispose();
    }
}
