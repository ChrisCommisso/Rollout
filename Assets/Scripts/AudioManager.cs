using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Agent unit in CameraController.Instance.availableUnits)
        {
            if(unit.myAgent.velocity.magnitude>0.1f)
            {
                
            }
        }

        if(Input.GetKeyDown(KeyCode.N))
        {
            PlayVoiceLine();
        }
    }



    public void PlayVoiceLine()
    {
        CameraController.Instance.voiceline.Play();
    }
}
