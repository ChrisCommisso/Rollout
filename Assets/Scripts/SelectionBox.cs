using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionBox : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Colllision

    private void OnCollisionEnter(Collision collision)
    {
        Units u = collision.gameObject.GetComponent<Units>();
        if (u != null &&
            u.allegiance == Units.Allegiance.Friendly &&
            !CameraController.Instance.selectedUnits.Contains(u))
        {
            CameraController.Instance.selectedUnits.Add(u);
            u.gameObject.GetComponent<HealthDisplay>().displayHealthFor = 0.5f;
               
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //check if Unit is unit
        Units u = collision.gameObject.GetComponent<Units>();
        if( u!=null && 
            u.allegiance == Units.Allegiance.Friendly && 
            !CameraController.Instance.selectedUnits.Contains(u))
        {
            CameraController.Instance.selectedUnits.Add(u);
        }
    }


    private void OnCollisionExit(Collision collision)
    {
        //check if deselecting Unit
        Units u = collision.gameObject.GetComponent<Units>();
        if (u != null &&
            u.allegiance == Units.Allegiance.Friendly &&
            CameraController.Instance.selectedUnits.Contains(u))
        {
            CameraController.Instance.selectedUnits.Remove(u);
        }
    }

    #endregion
}
