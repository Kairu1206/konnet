using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class AppManagement : MonoBehaviour
{
    //    THESE ARE THE LINKS FOR APACHE24 LOCAL DATABASE

    // string regisurl = "http://localhost/sqlcon/register.php";                          // register new user link
    // string searchurl = "http://localhost/sqlcon/searchid.php";                        // search link
    // string updateurl = "http://localhost/sqlcon/updateinorout.php";                  // sign in/out link
    // string checkpassurl = "http://localhost/sqlcon/checkpass.php";                   // check passcode for edit link
    // string removeuserurl = "http://localhost/sqlcon/removeuser.php";                 // remove user link
    // string addserviceurl = "http://localhost/sqlcon/addservice.php";                 // add service link
    // string removeserviceurl = "http://localhost/sqlcon/removeservice.php";           // remove service link
    // string changepassurl = "http://localhost/sqlcon/changepass.php";                 // change passcode link
    // string updateserviceurl = "http://localhost/sqlcon/updateservice.php";            //update service status link

    //    THESE ARE THE LINKS FOR IONOS WEB HOSTING DATABASE

    string regisurl = "https://kairu1206.com/sqlcon/register.php";                          // register new user link
    string searchurl = "https://kairu1206.com/sqlcon/searchid.php";                        // search link
    string updateurl = "https://kairu1206.com/sqlcon/updateinorout.php";                  // sign in/out link
    string checkpassurl = "https://kairu1206.com/sqlcon/checkpass.php";                   // check passcode for edit link
    string removeuserurl = "https://kairu1206.com/sqlcon/removeuser.php";                 // remove user link
    string addserviceurl = "https://kairu1206.com/sqlcon/addservice.php";                 // add service link
    string removeserviceurl = "https://kairu1206.com/sqlcon/removeservice.php";           // remove service link
    string changepassurl = "https://kairu1206.com/sqlcon/changepass.php";                 // change passcode link
    string updateserviceurl = "https://kairu1206.com/sqlcon/updateservice.php";            //update service status link
    string exiturl = "https://kairu1206.com/sqlcon/exit.php";                            // exit database link
    string reporturl = "https://kairu1206.com/sqlcon/report.php";                        // report link
    string updateemployeeinfourl = "https://kairu1206.com/sqlcon/updateemployeeinfo.php"; // update employee info link
    string updateserviceinfourl = "https://kairu1206.com/sqlcon/updateserviceinfo.php"; // update service info link
    string maxturnurl = "https://kairu1206.com/sqlcon/maxturn.php";                      // max turn link
    string turncounturl = "https://kairu1206.com/sqlcon/turncount.php";                  // turn count link

// THESE ARE FOR READ & WRITE LOCAL FILES
    string dir = "";
    string filename = "";
    

// MAIN SCENE OBJS
    public GameObject content;
    public GameObject mainscreen;
    public GameObject mainscreenview;
    public GameObject turncount;
    public GameObject signinouttemplate;
    public GameObject template2;
    

// EDIT POP-UPS OBJS
    public GameObject editpopup;
    public TMP_InputField passcode;

// EMPLOYEE SEARCH POP-UPS OBJS
    public GameObject nsearchpopup;
    public GameObject sidsearchpopup;
    public TMP_InputField searchid;

// SERVICE SEARCH POP-UPS OBJS
    public GameObject nservicesearchpopup;
    public GameObject servicepopupcontent;
    
    public GameObject servicepopup;
    public GameObject template5;
    public GameObject manicol;
    public GameObject pedicol;
    public GameObject kidcol;
    public GameObject facialcol;
    public GameObject CDT;

// REGISTER SCENE OBJS
    public TMP_InputField fn;
    public TMP_InputField ln;
    public TMP_InputField sid;

// CHANGE MAX TURNS OBJS
    public GameObject changemaxturnpopup;
    public TMP_InputField numchange;

//CHANGE TURN COUNT OBJS
    public GameObject changeturncountpopup;
    public TMP_InputField turncountchange;

// CHANGE PASSCODE OBJS
    public GameObject changepasspopup;
    public TMP_InputField newpass;

// ADD SERVICE SCENE OBJS
    public TMP_Dropdown servicetype;
    public TMP_InputField servicename;
    public TMP_InputField servicetime;
    public TMP_InputField servicecost;

// SERVICE DISPLAY SCENE OBJS
    public GameObject servicemenuview;
    public GameObject servicemenucontent;

// AUTO-REFRESH VARIABLES
    private float auto_refresh_timer = 30f; // AUTO-REFRESH every 10s
    private float auto_refresh_cd;          // variable to keep track of auto refresh timer
    private float timer_after_auto_refresh = 5f; // wait 5s before the next auto refresh timer start
    private float timer_after_auto_refresh_cd; //variable to keep track of the cd of auto refresh
    private bool auto_refresh = true;
    public TMP_Text arcdt;

// VIEW AND EDIT EMPLOYEE OBJS
    public TMP_Text employeeidplaceholder;
    public TMP_InputField salonidinputfield;
    public TMP_InputField firstnameinputfield;
    public TMP_InputField lastnameinputfield;

// VIEW AND EDIT SERVICE OBJS
    public TMP_Text serviceidinputfield;
    public TMP_Dropdown servicetypeinputfield;
    public TMP_InputField servicenameinputfield;
    public TMP_InputField servicetimeinputfield;
    public TMP_InputField servicecostinputfield;


    void Awake()
    {
        auto_refresh_cd = auto_refresh_timer;
        timer_after_auto_refresh_cd = timer_after_auto_refresh;

        if(servicepopup != null)
        {
            GlobalVariable.servicepopup = servicepopup;
        }
        if(manicol != null)
        {
            GlobalVariable.manicol = manicol;
        }
        if(pedicol != null)
        {
            GlobalVariable.pedicol = pedicol;
        }
        if(kidcol != null)
        {
            GlobalVariable.kidcol = kidcol;
        }
        if(facialcol != null)
        {
            GlobalVariable.facialcol = facialcol;
        }
        if(CDT != null)
        {
            GlobalVariable.CDT = CDT;
        }


        if(SceneManager.GetActiveScene().name == "MainScene" && GlobalVariable.firstopen && template5 != null)
        {
            runSearchService(template5);
            runSearchIn(template2);
            GlobalVariable.firstopen = false;
        }
        
    }


    // Start is called before the first frame update
    void Start()
    {
        if(editpopup != null)
        {
            editpopup.SetActive(false);
        }
        if(nsearchpopup != null)
        {
            nsearchpopup.SetActive(false);
        }
        if(sidsearchpopup != null)
        {
            sidsearchpopup.SetActive(false);
        }
        if(mainscreenview != null)
        {
            mainscreenview.SetActive(true);
        }
        if(servicepopup != null)
        {
            servicepopup.SetActive(false);
        }
        if(changemaxturnpopup != null)
        {
            changemaxturnpopup.SetActive(false);
        }
        if(changepasspopup != null)
        {
            changepasspopup.SetActive(false);
        }
        if(nservicesearchpopup != null)
        {
            nservicesearchpopup.SetActive(false);
        }
        if(CDT != null)
        {
            CDT.SetActive(false);
        }
        if(arcdt != null)
        {
            arcdt.gameObject.SetActive(true);
        }
        if(SceneManager.GetActiveScene().name == "MainScene")
        {
            AssignSIO();
        }
        if(SceneManager.GetActiveScene().name == "View&EditEmployee" && (employeeidplaceholder != null && lastnameinputfield != null && firstnameinputfield != null && salonidinputfield != null))
        {
            DisplayViewnEditEmployee();
        }

        if(SceneManager.GetActiveScene().name == "View&EditServices" && (serviceidinputfield != null && servicetypeinputfield != null && servicenameinputfield != null && servicetimeinputfield != null && servicecostinputfield != null))
        {
            DisplayViewnEditService();
        }
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(arcdt != null)
        {
            if(auto_refresh_cd > 0 && auto_refresh)
            {
                auto_refresh_cd -= Time.deltaTime;
                arcdt.text = string.Format("Next Auto-Refresh\n{0}:{1}{2}",
                                            Mathf.RoundToInt(auto_refresh_cd)/60,    //minutes
                                            Mathf.RoundToInt(auto_refresh_cd%60)/10, //seconds in tenth place
                                            Mathf.RoundToInt(auto_refresh_cd%60)%10 //seconds
                                            );

            }
            else if(auto_refresh_cd <= 0 && auto_refresh)
            {
                GameObject.FindWithTag("refreshbutton").GetComponent<Button>().onClick.Invoke();
                auto_refresh = false;
                auto_refresh_cd = auto_refresh_timer;
            }

            if(timer_after_auto_refresh_cd > 0 && !auto_refresh)
            {
                timer_after_auto_refresh_cd -= Time.deltaTime;
                arcdt.text = string.Format("Auto-Refresh CD\n{0}:{1}{2}",
                                            Mathf.RoundToInt(timer_after_auto_refresh_cd)/60,    //minutes
                                            Mathf.RoundToInt(timer_after_auto_refresh_cd%60)/10, //seconds in tenth place
                                            Mathf.RoundToInt(timer_after_auto_refresh_cd%60)%10 //seconds
                                            );
            }
            else if(timer_after_auto_refresh_cd < 0 && !auto_refresh)
            {
                auto_refresh = true;
                timer_after_auto_refresh_cd = timer_after_auto_refresh;
            }
        }

    }

    public void resetAutoRefreshTimer()
    {
        auto_refresh = false;
        auto_refresh_cd = auto_refresh_timer;
        timer_after_auto_refresh_cd = timer_after_auto_refresh;
    }

    public void AssignSIO()
    {
        if(GlobalVariable.metadata2.Count() > 0)
        {
            int.TryParse(GlobalVariable.metadata2[GlobalVariable.metadata2.Count() - 1][5], out int sio);
            GlobalVariable.sign_in_order = sio + 1;
        }
        else
        {
            GlobalVariable.sign_in_order = 1;
        }
        
    }

    public void ControlOptions(GameObject options)
    {
        if(options.activeSelf)
        {
            options.SetActive(false);
        }
        else
        {
            options.SetActive(true);
        }
    }

//-------------------------------------------------------------------------------------------------------------------CLOSE APPLICATION

    //CLOSE APPLICATION

    public void Quit()
    {
        if(mainscreen.transform.childCount <= 0)
        {
            Report();
            runReportDB();
            StartCoroutine(Exit());
            Application.Quit();
        }
        else
        {
            print("PLEASE SIGN OUT ALL EMPLOYEE!");
        }
    }

    public void runReportDB()
    {
        for(int i = 0; i < GlobalVariable.report.Count(); i++)
        {
            for(int o = 0; o < GlobalVariable.report[i].services.Count(); o++)
            {
                StartCoroutine(ReportDB(i, o));
            }
        }
    }

    IEnumerator ReportDB(int i, int o)
    {
        WWWForm form = new WWWForm();
        form.AddField("salonid", GlobalVariable.report[i].salonid);
        form.AddField("userid", GlobalVariable.report[i].userid);
        form.AddField("username", GlobalVariable.report[i].name);
        form.AddField("turnnum", GlobalVariable.report[i].services[o].turnnum);
        form.AddField("serviceid", GlobalVariable.report[i].services[o].serviceid);
        form.AddField("servicename", GlobalVariable.report[i].services[o].servicename);
        form.AddField("servicecost", GlobalVariable.report[i].services[o].servicecost);
        form.AddField("apporreq", GlobalVariable.report[i].services[o].apporreq);
        form.AddField("date", System.DateTime.Now.ToString("MM-dd-yyyy"));

        UnityWebRequest www = UnityWebRequest.Post(reporturl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();


    }

    IEnumerator Exit()
    {
        WWWForm form = new WWWForm();
        
        UnityWebRequest www = UnityWebRequest.Post(exiturl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

    public void Report()
    {
        dir = Application.dataPath + "/Daily Report";
        if(!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }
        if(GlobalVariable.report_time == 0)
        {
            filename = Application.dataPath + "/Daily Report/Report " + System.DateTime.Now.ToString("MM-dd-yyyy") + ".csv";
        }
        else
        {
            filename = Application.dataPath + "/Daily Report/Report " + System.DateTime.Now.ToString("MM-dd-yyyy") + "(" + GlobalVariable.report_time + ")" + ".csv";
        }
        GlobalVariable.report_time++;
        WriteFile();
    }

    public void WriteFile()
    {
        TextWriter tw = new StreamWriter(filename, false);
        tw.WriteLine("Salon ID, Employee ID, Employee Name, Turn #, Service ID, Service Name, Service Cost, Appointment or Request");
        for(int i = 0; i < GlobalVariable.report.Count(); i++)
        {
            for(int o = 0; o < GlobalVariable.report[i].services.Count(); o++)
            {
                tw.WriteLine(GlobalVariable.report[i].salonid + "," + GlobalVariable.report[i].userid + "," +
                            GlobalVariable.report[i].name + "," + GlobalVariable.report[i].services[o].turnnum + "," +
                            GlobalVariable.report[i].services[o].serviceid + "," + GlobalVariable.report[i].services[o].servicename + "," +
                            GlobalVariable.report[i].services[o].servicecost + "," + GlobalVariable.report[i].services[o].apporreq);
            }
        }
        tw.Close();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void BackToEdit()
    {
        SceneManager.LoadScene("EditScene");
    }

//---------------------------------------------------------------------------------------------------------------------EDIT FUNCS

    //EDIT FUNCS

    public void Edit()
    {
        editpopup.SetActive(true);
        turncount.SetActive(false);
    }

    public void closeEdit()
    {
        editpopup.SetActive(false);
        turncount.SetActive(true);
        passcode.text = "";
    }

    public void RunCheckPass()
    {
        StartCoroutine(CheckPass(passcode.text));
        passcode.text = "";
    }

    IEnumerator CheckPass(string s)
    {
        WWWForm form = new WWWForm();
        form.AddField("pass", s);

        UnityWebRequest www = UnityWebRequest.Post(checkpassurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);
        if(www.downloadHandler.text == "match")
        {
            BackToEdit();
        }

        www.Dispose();

    }

//---------------------------------------------------------------------------------------------------------------------SEARCH FUNCS

    //SEARCHING FOR THE EMPLOYEE BY SALON ID
    public void opennsearch()
    {
        nsearchpopup.SetActive(true);
    }

    public void closensearch()
    {
        nsearchpopup.SetActive(false);
        
    }

    public void openSearch()
    {
        sidsearchpopup.SetActive(true);
    }

    public void closeSearch()
    {
        sidsearchpopup.SetActive(false);
    }

    public void opennServiceSearch()
    {
        nservicesearchpopup.SetActive(true);
    }

    public void closenServiceSearch()
    {
        nservicesearchpopup.SetActive(false);
    }

    public void runSearch(GameObject template)
    {
        foreach(Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(Search(1, template));
    }

    public void runSearchwithID(GameObject template)
    {
        foreach(Transform child in content.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(Search(2, searchid.text, template));
        searchid.text = "";
    }

    public void runSearchIn(GameObject template)
    {
        // foreach(Transform child in mainscreen.transform)
        // {
        //     Destroy(child.gameObject);
        // }
        StartCoroutine(Search(3, template));
    }

    public void runSearchService(GameObject template)
    {
        foreach(Transform child in GlobalVariable.manicol.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in GlobalVariable.pedicol.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in GlobalVariable.kidcol.transform)
        {
            Destroy(child.gameObject);
        }
        foreach(Transform child in GlobalVariable.facialcol.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(Search(4, template));
    }
    
    public void runSearchServiceforRemove(GameObject template)
    {
        foreach(Transform child in servicepopupcontent.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(Search(4, template));
    }

    public void runSearchServiceMenu(GameObject template)
    {
        foreach(Transform child in servicemenucontent.transform)
        {
            Destroy(child.gameObject);
        }
        StartCoroutine(Search(4, template));
    }

    public void runSearchServiceCando()
    {
        if(!gameObject.GetComponent<Timer>().currently_count_down && (gameObject.name == (gameObject.transform.parent.transform.parent.transform.parent.name + gameObject.transform.parent.transform.parent.transform.parent.GetComponent<serviceturn>().current_turn)))
        {
            var fn = GlobalVariable.metadata2[gameObject.transform.parent.transform.parent.transform.parent.GetSiblingIndex()][2];
            var id = GlobalVariable.metadata2[gameObject.transform.parent.transform.parent.transform.parent.GetSiblingIndex()][1];
            var name = fn + id;
            StartCoroutine(Search(6, name, null));
        }
    }

    public void runSearchServiceCando2()
    {
        var fn = GlobalVariable.metadata3[GlobalVariable.employee][2];
        var id = GlobalVariable.metadata3[GlobalVariable.employee][1];
        var name = fn + id;
        StartCoroutine(Search(7, name, null));
    }

    void DisplaySignInOutSearch(GameObject template)                                          //Display search with in/out template
    {
        for(int o = 0; o < GlobalVariable.metadata.Length; o++)                           //Displaying the data out
        {
            var p = Instantiate(template, content.transform, false);
            for(int c = 0; c < p.transform.childCount - 2; c++)
            {
                p.transform.GetChild(c).GetComponent<TMP_Text>().text = GlobalVariable.metadata[o][c];
            } 

            switch(GlobalVariable.metadata[o][4])
            {
                case "i":        //case if user is in, inactive the sign in button while active sign out button
                    p.transform.GetChild(p.transform.childCount - 1).GetComponent<Button>().interactable = true;     //sign out button
                    p.transform.GetChild(p.transform.childCount - 2).GetComponent<Button>().interactable = false;    //sign in button
                    break;
                case "o":       // case if user is out, inactive the sign out button while active the sign in button
                    p.transform.GetChild(p.transform.childCount - 1).GetComponent<Button>().interactable = false;    //sign out button
                    p.transform.GetChild(p.transform.childCount - 2).GetComponent<Button>().interactable = true;     //sign in button
                    break;
            }          
        }
    }

    void DisplayRemovenEditSearch(int i, GameObject template)                                           //Display search with remove template
    {
        for(int o = 0; o < GlobalVariable.metadata.Length; o++)                           //Displaying the data out
        {
            
            GameObject p = new GameObject();
            switch(i)
            {
                case 1:
                case 2:
                    p = Instantiate(template, content.transform, false);
                    break;
                case 4:
                    p = Instantiate(template, servicepopupcontent.transform, false);
                    break;
                default:
                    print("ERROR: NO CONTENTS FIT!");
                    break;
            }
                
            for(int c = 0; c < p.transform.childCount - 1; c++)
            {
                p.transform.GetChild(c).GetComponent<TMP_Text>().text = GlobalVariable.metadata[o][c];
            }        
        }
    }

    void DisplayMainScreen(GameObject template)
    {

        for(int o = 0; o < GlobalVariable.metadata2.Count(); o++)                           //Displaying the data
        {
            var user = mainscreen.transform.Find(GlobalVariable.metadata2[o][2] + GlobalVariable.metadata2[o][1]);
            if(GlobalVariable.metadata2[o][4] == "i" && user == null)
            {
                var p = Instantiate(template, mainscreen.transform, false);
                p.transform.GetChild(0).GetComponent<TMP_Text>().text = GlobalVariable.metadata2[o][2];
                p.transform.GetChild(0).transform.GetChild(0).GetComponent<TMP_Text>().text = GlobalVariable.metadata2[o][5];
                p.transform.GetChild(0).transform.GetChild(1).GetComponent<TMP_Text>().text = GlobalVariable.metadata2[o][6];
                p.name = GlobalVariable.metadata2[o][2] + GlobalVariable.metadata2[o][1];
                GlobalVariable.AddEmployee(GlobalVariable.metadata2[o][2] + " " + GlobalVariable.metadata2[o][3], 
                                            GlobalVariable.metadata2[o][1], 
                                            GlobalVariable.metadata2[o][0]);
            }
            else if(GlobalVariable.metadata2[o][4] == "o" && user != null)
            {
                Destroy(user.gameObject);
                    
            }
            else
            {
                print(GlobalVariable.metadata2[o][2] + GlobalVariable.metadata2[o][1] + " has already sign in!");
            }
            

        }           
    }

    void DisplayServiceMenuScreen(GameObject template)
    {
        for(int o = 0; o < GlobalVariable.metadata.Length; o++)                           //Displaying the data out
        {
            var p = Instantiate(template, servicemenucontent.transform, false);
                
            for(int c = 0; c < p.transform.childCount - 1; c++)
            {
                p.transform.GetChild(c).GetComponent<TMP_Text>().text = GlobalVariable.metadata[o][c];
            }        
        }
    }

    void DisplayServiceScreen(GameObject template)
    {
        for(int i = 0; i < GlobalVariable.metadata1.Length; i++)
        {
            GameObject p = new GameObject();
            switch(GlobalVariable.metadata1[i][1])
            {
                case "Manicure":
                    p = Instantiate(template, GlobalVariable.manicol.transform, false);
                    break;
                case "Pedicure":
                    p = Instantiate(template, GlobalVariable.pedicol.transform, false);
                    break;
                case "Kids":
                    p = Instantiate(template, GlobalVariable.kidcol.transform, false);
                    break;
                case "Facial":
                    p = Instantiate(template, GlobalVariable.facialcol.transform, false);
                    break;
                default:
                    print("ERROR: NO MATCHES SERVICE");
                    break;
            }
            p.transform.GetChild(0).GetComponent<TMP_Text>().text = GlobalVariable.metadata1[i][2];
            p.GetComponent<Image>().color = Color.red;
            p.GetComponent<Button>().interactable = false;
            p.name = GlobalVariable.metadata1[i][0];
        }
    }

    void runAlterServiceScreen()
    {
        foreach(var md in GlobalVariable.metadata)
        {
            GameObject.Find(md[0]).GetComponent<Button>().interactable = true;
            GameObject.Find(md[0]).GetComponent<Image>().color = Color.green;

        }
    }

    void runAlterServiceScreen2()
    {
        foreach(Transform t4 in servicemenucontent.transform)
        {
            foreach(var md in GlobalVariable.metadata)
            {
                if(t4.GetChild(0).GetComponent<TMP_Text>().text == md[0])
                {
                    t4.transform.GetChild(5).GetComponent<Toggle>().isOn = true;
                    break;
                }
            }
        }
    }

    void runDisplayScreen(string templateName, GameObject template, int i)
    {
        switch(templateName)
        {
            case "template":
                DisplaySignInOutSearch(template);
                break;
            case "template 1":
            case "template 3":
            case "template 6":
            case "template 7":
                DisplayRemovenEditSearch(i, template);
                break;
            case "template 2":
                DisplayMainScreen(template);
                break;
            case "template 4":
                DisplayServiceMenuScreen(template);
                break;
            case "template 5":
                DisplayServiceScreen(template);
                break;
            default:
                print("ERROR: NO TEMPLATE ACCESS");
                break;
        }
    }

    void runProcessData(int i)
    {
        switch(i)
        {
            case 1:
                GlobalVariable.metadata3 = GlobalVariable.metadata;
                break;
            case 3:
                GlobalVariable.Sorting(GlobalVariable.metadata);
                GlobalVariable.metadata2 = GlobalVariable.metadata.ToList();
                break;
            case 4:
                GlobalVariable.metadata1 = GlobalVariable.metadata;
                break;
            case 6:
                runAlterServiceScreen();
                break; 
            case 7:
                runAlterServiceScreen2();
                break;
            default:
                print("ERROR: NO CASES FIT!");
                break;
        }
    }

    IEnumerator Search(int i, GameObject template)                                           
    {
        WWWForm form = new WWWForm();
        form.AddField("searchtype", i);

        UnityWebRequest www = UnityWebRequest.Post(searchurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        if(www.downloadHandler.text != "0 results")
        {
            string[] tempdata = www.downloadHandler.text.Split("*");
            string[][] data = new string[tempdata.Length - 1][];

            for(int o = 0; o < data.Length; o++)
            {
                data[o] = tempdata[o].Split("_");                    //Setting the data into 2d array
            }

            GlobalVariable.metadata = data;

            runProcessData(i);
            
            
            if(template != null)
            {
                runDisplayScreen(template.name, template, i);
            }
        }

        www.Dispose();
    }

    IEnumerator Search(int i, string s, GameObject template)                                       
    {
        int.TryParse(s, out int id);

        WWWForm form = new WWWForm();
        form.AddField("searchtype", i);
        if(id != 0)
        {
            form.AddField("id", id);
        }
        else
        {
            form.AddField("name", s);
        }
        

        UnityWebRequest www = UnityWebRequest.Post(searchurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        if(www.downloadHandler.text != "0 results")
        {
            string[] tempdata = www.downloadHandler.text.Split("*");
            string[][] data = new string[tempdata.Length - 1][];

            for(int o = 0; o < data.Length; o++)
            {
                data[o] = tempdata[o].Split("_");                    //Setting the data into 2d array
            }

            GlobalVariable.metadata = data;

            runProcessData(i);

            if(template != null)
            {
                runDisplayScreen(template.name, template, i);
            }
        }

        www.Dispose();
    }

//----------------------------------------------------------------------------------------------------------------------SIGN IN/OUT FUNCS

    //SIGN IN AND SIGN OUT

    public void SignOut()
    {
        StartCoroutine(UpdateStatus("o", GlobalVariable.metadata3[gameObject.transform.parent.GetSiblingIndex()][1]));
    }

    public void SignIn()
    {
        StartCoroutine(UpdateStatus("i", GlobalVariable.metadata3[gameObject.transform.parent.GetSiblingIndex()][1]));
    }

    IEnumerator UpdateStatus(string s, string uid)
    {
        int.TryParse(uid, out int intuid);

        WWWForm form = new WWWForm();
        form.AddField("userid", intuid);
        form.AddField("status", s);
        if(s == "i")
        {
            form.AddField("signinorder", GlobalVariable.sign_in_order);
            form.AddField("signintime", System.DateTime.Now.ToString("HH:mm:ss"));
        }
        else
        {
            form.AddField("signinorder", 0);
        }

        UnityWebRequest www = UnityWebRequest.Post(updateurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);
        GameObject.Find("Options").transform.GetChild(1).GetComponent<Button>().onClick.Invoke();
        GameObject.FindWithTag("refreshbutton").GetComponent<Button>().onClick.Invoke();
        

        www.Dispose();
    }

//---------------------------------------------------------------------------------------------------------------------ADD/REMOVE EMPLOYEE FUNCS

    //ADD EMPLOYEE

    public void openRegister()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    public void runAddEmployee()
    {
        GlobalVariable.fn = fn.text.ToUpper();
        GlobalVariable.ln = ln.text.ToUpper();
        GlobalVariable.sid = sid.text;
        StartCoroutine(AddEmployee(fn.text.ToUpper(), ln.text.ToUpper(), sid.text));
        fn.text = "";
        ln.text = "";
        sid.text = "";
        openServiceMenu();
    }

    public void runRemoveEmployee()
    {
        StartCoroutine(RemoveEmployee(GlobalVariable.metadata[gameObject.transform.parent.GetSiblingIndex()][2], GlobalVariable.metadata[gameObject.transform.parent.GetSiblingIndex()][0], GlobalVariable.metadata[gameObject.transform.parent.GetSiblingIndex()][1]));
    }

    IEnumerator AddEmployee(string firstname, string lastname, string salonid)
    {
        int.TryParse(salonid, out int intsid);

        WWWForm form = new WWWForm();
        form.AddField("firstname", firstname);
        form.AddField("lastname", lastname);
        form.AddField("salonid", intsid);

        UnityWebRequest www = UnityWebRequest.Post(regisurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

    IEnumerator RemoveEmployee(string fn, string sid, string uid)
    {
        int.TryParse(uid, out int intuid);


        WWWForm form = new WWWForm();
        form.AddField("userid", intuid);
        form.AddField("fn", fn);
        form.AddField("sid", sid);

        UnityWebRequest www = UnityWebRequest.Post(removeuserurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();

        BackToEdit();
    }
//--------------------------------------------------------------------------------------------------------------------ADD/REMOVE SERVICES

    public void openAddService()
    {
        SceneManager.LoadScene("AddServiceScene");
    }

    public void runAddServices()
    {
        StartCoroutine(AddServices());
        servicetime.text = "";
        servicename.text = "";
    }

    public void runRemoveServices()
    {
        StartCoroutine(RemoveServices(GlobalVariable.metadata[gameObject.transform.parent.GetSiblingIndex()][0]));
    }

    IEnumerator AddServices()
    {
        int.TryParse(servicetime.text, out int intservicetime);
        int.TryParse(servicecost.text, out int intservicecost);

        WWWForm form = new WWWForm();
        form.AddField("typeofservice", servicetype.options[servicetype.value].text);
        form.AddField("servicename", servicename.text.ToUpper());
        form.AddField("time", intservicetime);
        form.AddField("cost", intservicecost);


        UnityWebRequest www = UnityWebRequest.Post(addserviceurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

    IEnumerator RemoveServices(string sid)
    {
        int.TryParse(sid, out int intsid);

        WWWForm form = new WWWForm();
        form.AddField("serviceid", intsid);

        UnityWebRequest www = UnityWebRequest.Post(removeserviceurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();

        BackToEdit();
        runSearchServiceforRemove(null);
    }

//----------------------------------------------------------------------------------------------------------------------CONNECT SERVICES AND EMPLOYEE
    public void openServiceMenu()
    {
        SceneManager.LoadScene("ServiceMenuScene");
    }

    public void runCandoService()
    {
        StartCoroutine(CandoService(GlobalVariable.metadata[gameObject.transform.parent.GetSiblingIndex()][0], GlobalVariable.fn, gameObject.GetComponent<Toggle>().isOn));
    }

    IEnumerator CandoService(string s, string n, bool v)
    {
        var tof = 0;
        int.TryParse(s, out int siint);

        if(v)
        {
            tof = 1;
        }

        WWWForm form = new WWWForm();
        form.AddField("serviceindex", siint);
        form.AddField("name", n);
        form.AddField("tof", tof);

        UnityWebRequest www = UnityWebRequest.Post(updateserviceurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }
//----------------------------------------------------------------------------------------------------------------------ADD TURN
    public void openService()
    {
        if(!gameObject.GetComponent<Timer>().currently_count_down)
        {
            GlobalVariable.servicepopup.SetActive(true);
            GlobalVariable.last_turn_button_press = gameObject;
        }
        
    }

    public void closeService()
    {
        GlobalVariable.servicepopup.SetActive(false);

    }

    public void runTimer()
    {
        int index = 0;
        for(int i = 0; i < GlobalVariable.metadata1.Length; i++)
        {
            if(gameObject.name == GlobalVariable.metadata1[i][0])
            {
                int.TryParse(GlobalVariable.metadata1[i][3], out int timer);
                GlobalVariable.last_turn_button_press.GetComponent<Slider>().maxValue = timer * 60;
                GlobalVariable.last_turn_button_press.GetComponent<Timer>().runSync(1, timer * 60);
                index = i;
                break;
            }
        }

        GlobalVariable.last_turn_button_press.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = GlobalVariable.metadata1[index][2];
        GlobalVariable.last_turn_button_press.transform.GetChild(1).GetComponent<Image>().color = Color.green;
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().startTimer = true;
        GameObject.FindWithTag("refreshbutton").GetComponent<Button>().onClick.Invoke();
    }

    public void Appointment()
    {
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().appointment = 1;
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().request = 0;
        GlobalVariable.last_turn_button_press.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = "APPOINTMENT";
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().runSync(2, -1);

    }

    public void Request()
    {
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().request = 1;
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().appointment = 0;
        GlobalVariable.last_turn_button_press.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = "REQUEST";
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().runSync(2, -1);
    }

    public void Cancel()
    {
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().request = 0;
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().appointment = 0;
        GlobalVariable.last_turn_button_press.transform.GetChild(2).transform.GetChild(0).GetComponent<TMP_Text>().text = "";
        GlobalVariable.last_turn_button_press.GetComponent<Timer>().runSync(2, -1);
    }

    //CHANGE DURING TIMER - CDT

    public void openCDT()
    {
        if(gameObject.GetComponent<Timer>().currently_count_down)
        {
            GlobalVariable.last_turn_button_press = gameObject;
            GlobalVariable.CDT.SetActive(true);
        }
    }

    public void closeCDT()
    {
        GlobalVariable.CDT.SetActive(false);
    }

    public void CDTFunc()
    {
        print(gameObject.name);
        if(int.TryParse(gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text, out int addTime))
        {
            GlobalVariable.last_turn_button_press.GetComponent<Timer>().runSync(1, addTime * 60);
        }
        else
        {
            switch(gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text)
            {
                case "END":
                    GlobalVariable.last_turn_button_press.GetComponent<Timer>().runSync(1, -999999);
                    break;
                default:
                    print("NO MATCHES IN CDTFUNC");
                    break;
            }
        }
    }

//-------------------------------------------------------------------------------------------------------------------------CHANGE TURN COUNT
    public void openChangeTurnCount()
    {
        changeturncountpopup.SetActive(true);
    }

    public void closeChangeTurnCount()
    {
        changeturncountpopup.SetActive(false);
    }

    public void runchangeTurnCount()
    {
        int.TryParse(turncountchange.text, out int intturncountchange);
        StartCoroutine(changeTurnCount(intturncountchange));
        numchange.text = "";
    }

    IEnumerator changeTurnCount(int turncountchange)
    {
        WWWForm form = new WWWForm();
        form.AddField("type", 1);
        form.AddField("numchange", turncountchange);

        UnityWebRequest www = UnityWebRequest.Post(turncounturl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

//-------------------------------------------------------------------------------------------------------------------------CHANGE MAX TURN
    public void openChangeMaxTurn()
    {
        changemaxturnpopup.SetActive(true);
    }

    public void closeChangeMaxTurn()
    {
        changemaxturnpopup.SetActive(false);
    }

    public void runchangeMaxTurn()
    {
        int.TryParse(numchange.text, out int intnumchange);
        StartCoroutine(changeMaxTurn(intnumchange));
        numchange.text = "";
    }

    IEnumerator changeMaxTurn(int numchange)
    {
        WWWForm form = new WWWForm();
        form.AddField("type", 1);
        form.AddField("numchange", numchange);

        UnityWebRequest www = UnityWebRequest.Post(maxturnurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

//-----------------------------------------------------------------------------------------------------------------------CHANGE PASSCODE
    public void openChangePasscode()
    {
        changepasspopup.SetActive(true);
    }

    public void closeChangePasscode()
    {
        changepasspopup.SetActive(false);
    }

    public void runChangePass()
    {
        StartCoroutine(ChangePass(newpass.text));
        newpass.text = "";
    }

    IEnumerator ChangePass(string s)
    {
        int.TryParse(s, out int intnewpass);
        WWWForm form = new WWWForm();
        form.AddField("newpass", intnewpass);

        UnityWebRequest www = UnityWebRequest.Post(changepassurl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

//-------------------------------------------------------------------------------------------------------------------------EDIT/VIEW EMPLOYEE FUNCS
    public void openViewnEditEmployee()
    {
        SceneManager.LoadScene("View&EditEmployee");
    }

    public void setGlobalEmployee()
    {
        GlobalVariable.employee = gameObject.transform.parent.GetSiblingIndex();
    }

    public void DisplayViewnEditEmployee()
    {
        salonidinputfield.text = GlobalVariable.metadata3[GlobalVariable.employee][0];
        GlobalVariable.sid = salonidinputfield.text;
        employeeidplaceholder.text = GlobalVariable.metadata3[GlobalVariable.employee][1];
        firstnameinputfield.text = GlobalVariable.metadata3[GlobalVariable.employee][2];
        GlobalVariable.fn = firstnameinputfield.text;
        lastnameinputfield.text = GlobalVariable.metadata3[GlobalVariable.employee][3];
        GlobalVariable.ln = lastnameinputfield.text;
    }

    public void runUpdateEmployeeInfo()
    {
        StartCoroutine(UpdateEmployeeInfo());
    }

    IEnumerator UpdateEmployeeInfo()
    {
        WWWForm form = new WWWForm();

        form.AddField("userid", employeeidplaceholder.text);
        form.AddField("salonid", salonidinputfield.text != "" ? salonidinputfield.text : GlobalVariable.sid);
        form.AddField("oldfn", GlobalVariable.fn);
        form.AddField("fn", firstnameinputfield.text != "" ? firstnameinputfield.text : GlobalVariable.fn);
        form.AddField("ln", lastnameinputfield.text != "" ? lastnameinputfield.text : GlobalVariable.ln);

        UnityWebRequest www = UnityWebRequest.Post(updateemployeeinfourl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
        
    }
//--------------------------------------------------------------------------------------------------------------------------EDIT/VIEW SERVICES FUNCS
    public void openViewnEditServices()
    {
        SceneManager.LoadScene("View&EditServices");
    }
    
    public void setGlobalService()
    {
        GlobalVariable.service = gameObject.transform.parent.GetSiblingIndex();
    }

    public void DisplayViewnEditService()
    {
        int index = -1;
        serviceidinputfield.text = GlobalVariable.metadata1[GlobalVariable.service][0];
        for(int i = 0; i < servicetypeinputfield.options.Count(); i++)
        {
            if(servicetypeinputfield.options[i].text == GlobalVariable.metadata1[GlobalVariable.service][1])
            {
                index = i;
                break;
            }
        }
        servicetypeinputfield.value = index;
        servicenameinputfield.text = GlobalVariable.metadata1[GlobalVariable.service][2];
        servicetimeinputfield.text = GlobalVariable.metadata1[GlobalVariable.service][3];
        servicecostinputfield.text = GlobalVariable.metadata1[GlobalVariable.service][4];

    }

    public void runUpdateServiceInfo()
    {
        StartCoroutine(UpdateServiceInfo());
    }

    IEnumerator UpdateServiceInfo()
    {
        WWWForm form = new WWWForm();

        form.AddField("serviceid", serviceidinputfield.text);
        form.AddField("servicetype", servicetypeinputfield.options[servicetypeinputfield.value].text);
        form.AddField("servicename", servicenameinputfield.text);
        form.AddField("servicetime", servicetimeinputfield.text);
        form.AddField("servicecost", servicecostinputfield.text);

        UnityWebRequest www = UnityWebRequest.Post(updateserviceinfourl, form);
        yield return www.SendWebRequest();

        GlobalVariable.CheckSQLError(www);

        www.Dispose();
    }

}
