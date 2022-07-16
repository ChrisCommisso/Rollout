using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MaxCamSpeed;
    [RangeAttribute(0.000001f,0.4f)]public float ScreenBoundsPercent;

    public List<Units> availableUnits;
    public List<Units> selectedUnits;

    public Camera mainCamera;


    private Vector3 movementVector;
    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;
    private bool isHoldingMouseDown;

    public static CameraController Instance;


    #region Unity LifeCycle
    void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        mainCamera = gameObject.GetComponent<Camera>();
        ResetMovementBounds();
    }

    // Update is called once per frame
    void Update()
    {
        MovementCheck();
        ClickCheck();
        ZoomCheck();
    }
    #endregion


    #region Helpers
    public void ResetMovementBounds()
    {
        leftBound = Screen.width * ScreenBoundsPercent;
        rightBound = Screen.width - leftBound;
        bottomBound = Screen.height * ScreenBoundsPercent;
        topBound = Screen.height - bottomBound;
    }


    #endregion


    #region Input


    //check for inputs that give units commands
   

    public void MovementCheck()
    {
        movementVector = movementVector * 0.99349f;

        //If mouse is in certain bounds of the exterior, move camera in that direction
        Vector3 mousePos = Input.mousePosition;
        bool moving = false;

        if (mousePos.x < leftBound && mousePos.x > 0)
            moving = true;// movementVector += Vector3.left;
        else if (mousePos.x > rightBound && mousePos.x<Screen.width)
            moving = true;//movementVector += Vector3.right;

        if (mousePos.y < bottomBound    &&mousePos.y>0)
            moving = true;//movementVector += Vector3.back;
        else if (mousePos.y > topBound  &&  mousePos.y <Screen.height)
            moving = true;//movementVector += Vector3.forward;


        if(moving)
        {
            Vector3 temp = Vector3.Normalize(Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f));
            temp.z = temp.y;
            temp.y = 0;
            movementVector += temp;
            
        }

        movementVector = Vector3.ClampMagnitude(movementVector, MaxCamSpeed);

        //Debug.Log($"Movement Vector:{movementVector}");

        transform.position += movementVector*Time.deltaTime;
    }

    public void ZoomCheck()
    {
        //check for scroll input



    }

    public void ClickCheck()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isHoldingMouseDown = true;
            //check for units with raycaast
            Vector3 dest = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //RaycastOrigin.z = RaycastOrigin.y;
            //RaycastOrigin.y = transform.position.y;
            
            RaycastHit[] hitInfo=new RaycastHit[0];
            Ray ray = new Ray(mainCamera.transform.position, (dest-mainCamera.transform.position).normalized);
            
            Debug.DrawRay(ray.origin, ray.direction*5000f, Color.green,2f);
            hitInfo = Physics.RaycastAll(ray, 5000f);
            //have we hit something?
            if (hitInfo.Length>0)
            {
                GameObject hitGO = hitInfo[hitInfo.Length-1].collider.gameObject;
                Units unit = hitGO.GetComponent<Units>();
                if (unit != null && unit.allegiance == Units.Allegiance.Friendly)
                {
                    //ssick we have selected a unit
                    if (!selectedUnits.Contains(unit))
                    {
                        selectedUnits.Add(unit);
                    }
                    else//deselecting
                    {
                        selectedUnits.Remove(unit);
                    }
                }
                else {
                    foreach (var agent in selectedUnits)
                    {
                        if (agent is Agent) {
                            ((Agent)agent).setDestIfOnNavMesh(hitInfo[hitInfo.Length - 1].point);
                        }
                    }
                }

            }
        }


        if(Input.GetMouseButtonUp(0))
        {
            isHoldingMouseDown = false;
        }
    }
      

    #endregion
}
