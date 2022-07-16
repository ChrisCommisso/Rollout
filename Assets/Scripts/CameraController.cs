using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MaxCamSpeed;
    [RangeAttribute(0.000001f,0.999f)]public float ScreenBoundsPercent;

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
        SelectionCheck();
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
    public void CommandCheck()
    {

    }


    public void MovementCheck()
    {
        movementVector = movementVector * 0.99349f;

        //If mouse is in certain bounds of the exterior, move camera in that direction
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < leftBound && mousePos.x>0)
            movementVector += Vector3.left;
        else if (mousePos.x > rightBound && mousePos.x<Screen.width)
            movementVector += Vector3.right;

        if (mousePos.y < bottomBound    &&mousePos.y>0)
            movementVector += Vector3.back;
        else if (mousePos.y > topBound  &&  mousePos.y <Screen.height)
            movementVector += Vector3.forward;

        

        movementVector = Vector3.ClampMagnitude(movementVector, MaxCamSpeed);

        //Debug.Log($"Movement Vector:{movementVector}");

        transform.position += movementVector*Time.deltaTime;
    }

    public void ZoomCheck()
    {
        //check for scroll input



    }

    public void SelectionCheck()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isHoldingMouseDown = true;
            //check for units with raycaast
            Vector3 RaycastOrigin = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            //RaycastOrigin.z = RaycastOrigin.y;
            //RaycastOrigin.y = transform.position.y;

            RaycastHit hitInfo;
            Ray ray = new Ray(RaycastOrigin, Vector3.down);
            
            Debug.DrawRay(ray.origin, ray.direction*50f, Color.green,2f);
            bool hit = Physics.Raycast(ray, out hitInfo, 500f);
            if (hit)
            {
                
            }
        }


        if(Input.GetMouseButtonUp(0))
        {
            isHoldingMouseDown = false;
        }
    }
      

    #endregion
}
