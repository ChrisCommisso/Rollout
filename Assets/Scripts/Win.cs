using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using TMPro;

public class Win : MonoBehaviour
{
    public TextMeshProUGUI text;
    public static string EndString;
    public static int friendliesLost;
    public static int friendliesMade;
    public static float time;
    
    // Start is called before the first frame update
    
    void Awake()
    {
        friendliesLost = 0;
        time = 0;
        friendliesMade = 0;
        if (EndString != null && EndString.Length > 0) { 
        text.text = EndString;
        }    
        
    }
    public static void openFirstLevel() {
        SceneManager.LoadScene(1);
    }
    public void tryEnd()
    {
        foreach (Agent a in Agent.agents)
        {
            if (a == null) { continue; }
            if (a.allegiance == Units.Allegiance.Enemy) { continue; }
            if ((a.transform.position - transform.position).sqrMagnitude < 16f)
            {
                End();
                break;
            }
        }
    }
    void End() {
    Agent[] agents =Agent.agents.ToArray();
        int friendlies = 0; 
        for (int i = 0; i < agents.Length; i++) {
            if (agents == null) {
                continue;
            }
            if (agents[i].allegiance == Units.Allegiance.Friendly) {
                friendlies++;
            }

        }
        EndString = "YOU GOT THE GOLD IN SNAKE EYES BARROW\nYOU MADE IT WITH " + friendlies + " FRIENDS IN " + time + " SECONDS.";
        SceneManager.LoadScene(2);
    }
    // Update is called once per frame
    void Update()
    {
        time+=Time.deltaTime;
    }
}
