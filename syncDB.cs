using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class syncDB : MonoBehaviour
{
    string syncDBurl = "https://kairu1206.com/sqlcon/syncDB.php";

    float timer = 1f;
    public float timer_cd;
    // Start is called before the first frame update
    void Start()
    {
        timer_cd = timer;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.name == (gameObject.transform.parent.transform.parent.transform.parent.name + gameObject.transform.parent.transform.parent.transform.parent.GetComponent<serviceturn>().current_turn))
        {
            if(timer_cd > 0)
            {
                timer_cd -= Time.deltaTime;
            }
            else
            {
                runSync();
                timer_cd = timer;
            }
        }
    }

    void runSync()
    {
        StartCoroutine(Sync(0, gameObject.name));
    }

    void setTurnObj(string[][] data)
    {
        foreach(var md1 in GlobalVariable.metadata1)
        {
            if(md1[0] == data[0][1])
            {
                gameObject.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = md1[2];
                gameObject.GetComponent<Timer>().serviceid = md1[0];
                gameObject.GetComponent<Timer>().servicename = md1[2];
                int.TryParse(md1[4], out int intcost);
                gameObject.GetComponent<Timer>().servicecost = intcost;
                int.TryParse(md1[3], out int maxTime);
                gameObject.GetComponent<Slider>().maxValue = maxTime * 60;
                break;
            }
        }
    }

    IEnumerator Sync(int type, string n)                                           //Type for sync: 0 --> pull/get data from DB, 1 --> push/insert data to DB, 2 --> set stage
    {
        WWWForm form = new WWWForm();
        form.AddField("type", type);
        form.AddField("name", n);

        UnityWebRequest www = UnityWebRequest.Post(syncDBurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        if(www.downloadHandler.text != "0 data")
        {
            string[] tempdata = www.downloadHandler.text.Split("*");
            string[][] data = new string[tempdata.Length - 1][];

            for(int o = 0; o < data.Length; o++)
            {
                data[o] = tempdata[o].Split("_");                    //Setting the data into 2d array
            }

            GlobalVariable.metadata = data;
            
            int.TryParse(data[0][2], out int tl);

            setTurnObj(data);
            if(tl > 0)
            {
                gameObject.transform.GetChild(1).GetComponent<Image>().color = Color.green;
            }
            else if(tl == 0)
            {
                gameObject.transform.GetChild(1).GetComponent<Image>().color = gameObject.GetComponent<Timer>().transparent;
            }

            gameObject.GetComponent<Timer>().timeleft = tl;
            gameObject.GetComponent<Timer>().startTimer = true;

            int.TryParse(data[0][3], out int app);
            gameObject.GetComponent<Timer>().appointment = app == 1 ?  1 : 0;

            int.TryParse(data[0][4], out int req);
            gameObject.GetComponent<Timer>().request = req == 1 ?  1 : 0;

            
        }

        www.Dispose();
    }

}
