using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public static class GlobalVariable
{
    public static string[][] metadata;

    public static string[][] metadata1;   //Store all service
    public static List<string[]> metadata2;   //Store signed in user
    public static string[][] metadata3;   //Store all user

    public static List<Employee> report = new List<Employee>();


    public static string fn;
    public static string ln;
    public static string sid;

    public static GameObject servicepopup;
    public static GameObject manicol;
    public static GameObject pedicol;
    public static GameObject kidcol;
    public static GameObject facialcol;
    public static GameObject CDT;

    public static bool firstopen = true;

    public static GameObject last_turn_button_press; //THIS ONLY USE FOR ADD TURN ONLY

    public static int sign_in_order;

    public static bool firstopenmenu = false;

    public static int report_time = 0;

    public static int employee;
    public static int service;

    public static void CheckSQLError(UnityWebRequest www)
    {
        switch(www.result)
        {
            case UnityWebRequest.Result.ConnectionError:
                Debug.Log("Error: Connection Error!");
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.Log("Error: Protocol Error!");
                break;
            case UnityWebRequest.Result.DataProcessingError:
                Debug.Log("Error: Data Processing Error!");
                break;
            case UnityWebRequest.Result.InProgress:
                Debug.Log("In Progress...Please wait a moment...");
                break;
            default:
                Debug.Log(www.downloadHandler.text);
                break;
        }
    }

    public class Turn
    {
        public int turnnum;
        public int serviceid;
        public string servicename;
        public int servicecost;
        public string apporreq;
    }

    public class Employee
    {
        public string name;
        public int salonid;
        public int userid;
        public List<Turn> services = new List<Turn>();
    }

    public static void AddEmployee(string n, string uid, string sid)
    {
        int.TryParse(uid, out int uidint);
        int.TryParse(sid, out int sidint);
        Employee e = new Employee();
        e.name = n;
        e.userid = uidint;
        e.salonid = sidint;
        report.Add(e);
    }

    public static void AddTurn(int tn, string servid, string servname, int servcost, string aor, int index)
    {
        int.TryParse(servid, out int servidint);
        Turn t = new Turn();
        t.turnnum = tn;
        t.serviceid = servidint;
        t.servicename = servname;
        t.servicecost = servcost;
        t.apporreq = aor;
        report[index].services.Add(t);
    }

    public static void Sorting(string[][] arr)
    {
        for(int o = 1; o < arr.Length; o++)
        {
            var y = o - 1;
            while(y >= 0 &&  string.Compare(arr[y][5], arr[o][5]) > 0)
            {
                var temp = arr[o];
                arr[o] = arr[y];
                arr[y] = temp;
                y--;
            }
            
        }
    }

}
