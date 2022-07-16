using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;

public class CameraController : MonoBehaviour
{
    public float MaxCamSpeed;
    [RangeAttribute(0.000001f,0.4f)]public float ScreenBoundsPercent;

    public List<Units> availableUnits;
    public List<Units> selectedUnits;

    public Camera mainCamera;

    public float maxDistFromGround;
    public float minDistFromGround;
    public GameObject SelectionBox;
    private Vector3 selectionStartPoint;
    private Vector3 selectionEndPoint;

    private Vector3 movementVector;
    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;
    private bool isHoldingMouseDown;
    private float SelectionBoxTimer;
    private float SelectionBoxTimeFull = 0.243f;

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

    public void CheckDistanceFromGround()
    {
        Vector3 dest = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                    Input.mousePosition.y, mainCamera.nearClipPlane));
        RaycastHit[] hitInfo = new RaycastHit[0];
        Ray ray = new Ray(mainCamera.transform.position, (dest - mainCamera.transform.position).normalized);

        Debug.DrawRay(ray.origin, ray.direction * 5000f, Color.green, 0.02f);
        hitInfo = Physics.RaycastAll(ray, 5000f);

        if(hitInfo.Length>0)
        {

        }
    }


    #endregion


    #region Input


    //check for inputs that give units commands
   

    public void MovementCheck()
    {
        movementVector = movementVector * 0.97349f;

        //If mouse is in certain bounds of the exterior, move camera in that direction
        Vector3 mousePos = Input.mousePosition;
        bool moving = false;

        if (mousePos.x < leftBound )
            moving = true;// movementVector += Vector3.left;
        else if (mousePos.x > rightBound )
            moving = true;//movementVector += Vector3.right;

        if (mousePos.y < bottomBound)
            moving = true;//movementVector += Vector3.back;
        else if (mousePos.y > topBound)
            moving = true;//movementVector += Vector3.forward;

        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
            moving = false;

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
        Vector3 dest = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                    Input.mousePosition.y, mainCamera.nearClipPlane));
        RaycastHit[] hitInfo = new RaycastHit[0];
        Ray ray = new Ray(mainCamera.transform.position, (dest - mainCamera.transform.position).normalized);

        Debug.DrawRay(ray.origin, ray.direction * 5000f, Color.green, 0.02f);
        hitInfo = Physics.RaycastAll(ray, 5000f);
        float dist = Vector3.Distance(hitInfo[hitInfo.Length - 1].point, transform.position);
        //check for scroll input
        if (Input.mouseScrollDelta.y!=0)
        {
            
            //CHECK IF NO hit
            if(hitInfo.Length==0)
            {

            }
            Debug.Log($"Mouse Scroll Delta x:{Input.mouseScrollDelta.x}, y:{Input.mouseScrollDelta.y}");
            //if hit check if possible to move based on direction
            if (Input.mouseScrollDelta.y<0
                &&dist<maxDistFromGround)
                transform.position += ray.direction * -3f;
            if (Input.mouseScrollDelta.y > 0
                && dist > minDistFromGround)
                transform.position += ray.direction * 3f;




        }

        if(dist<minDistFromGround)
            transform.position += ray.direction * -1f;
        else if (dist>maxDistFromGround)
            transform.position += ray.direction * 1f;


        //Debug.Log($"Mouse Scroll Delta x:{Input.mouseScrollDelta.x}, y:{Input.mouseScrollDelta.y}");

    }





    public void ClickCheck()
    {
        Vector3 dest = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                                                    Input.mousePosition.y, mainCamera.nearClipPlane));


        RaycastHit[] hitInfo = new RaycastHit[0];
        Ray ray = new Ray(mainCamera.transform.position, (dest - mainCamera.transform.position).normalized);

        Debug.DrawRay(ray.origin, ray.direction * 5000f, Color.green, 0.02f);
        hitInfo = Physics.RaycastAll(ray, 5000f);

        //when not hitting the mouse button
        if (!Input.GetMouseButton(0))
        {
            isHoldingMouseDown = false;
            SelectionBox.SetActive(false);
            //have we hit something?

            //are we lifting mouse button up this frame?
            if (Input.GetMouseButtonUp(0) &&hitInfo.Length > 0)
            {

                SelectionBoxTimer = SelectionBoxTimeFull;
                //selectionStartPoint = mainCamera.ScreenToWorldPoint(hitInfo[hitInfo.Length - 1].point);
                GameObject hitGO = hitInfo[hitInfo.Length - 1].collider.gameObject;
                Units unit = hitGO.GetComponent<Units>();
                
                foreach (var agent in selectedUnits)
                {
                    if (agent is Agent)
                    {
                        if (((Agent)agent).attackComponent != null)
                            ((Agent)agent).attackComponent.noAttackMove(hitInfo[hitInfo.Length - 1].point);//try to use the attack component if possible
                        else
                        {
                            ((Agent)agent).setDestIfOnNavMesh(hitInfo[hitInfo.Length - 1].point);
                        }
                    }
                }


                

            }
        }
        else
        {
            
            
            if (isHoldingMouseDown)
            {         
                if(SelectionBoxTimer>0)
                {
                    SelectionBoxTimer -= Time.deltaTime;
                    return;
                }
                SelectionBox.SetActive(true);
                selectionEndPoint = hitInfo[hitInfo.Length - 1].point;
                //selectionEndPoint.y = -40f;
                Vector3 betweenVector = selectionEndPoint - selectionStartPoint;

                //move box to midpoint between
                Vector3 boxPos = selectionStartPoint + (betweenVector * 0.5f);
                boxPos.y = selectionStartPoint.y;
                Debug.DrawLine(selectionStartPoint, selectionStartPoint + Vector3.up * 200f, Color.blue, 0.14f);
                Debug.DrawLine(selectionEndPoint, selectionEndPoint + Vector3.up * 200f, Color.red, 0.14f);


                SelectionBox.transform.position = boxPos;
                Debug.Log($"Selection Starting Point:{selectionStartPoint}");
                SelectionBox.transform.localScale = new Vector3(betweenVector.x, 14, betweenVector.z);
                SelectionBox.GetComponent<Collider>();
                //Check for Units inside Selection Box;

                


            }
            if (Input.GetMouseButtonDown(0))
            {
                isHoldingMouseDown = true;
                //check for units with raycaast


                //RaycastOrigin.z = RaycastOrigin.y;
                //RaycastOrigin.y = transform.position.y;

                
                //have we hit something?
                if (hitInfo.Length > 0)
                {
                    //selectionStartPoint = mainCamera.ScreenToWorldPoint(hitInfo[hitInfo.Length - 1].point);
                    GameObject hitGO = hitInfo[hitInfo.Length - 1].collider.gameObject;
                    Units unit = hitGO.GetComponent<Units>();
                    Attackable attackAttribute = hitGO.GetComponent<Attackable>();
                    //check if we are clicking on a unit
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
                    else if(attackAttribute!=null)
                    {
                        foreach (Units item in selectedUnits)
                        {
                            item.unitParent.GetComponent<AutoAttack>()?.doAttackOrder(attackAttribute);
                        }
                    }
                    else
                    {
                        selectionStartPoint = hitInfo[hitInfo.Length - 1].point;
                        foreach (Units item in selectedUnits)
                        {
                            item.unitParent.GetComponent<AutoAttack>()?.doAttackOrder(selectionStartPoint);
                        }
                        
                    }
                    //selectionStartPoint.y = -40f;
                    
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if(hitInfo.Length>0)
                {
                    foreach (Units unit in selectedUnits)
                    {
                        unit.gameObject.GetComponent<AutoAttack>()?.noAttackMove(hitInfo[hitInfo.Length - 1].point);
                    }


                }
            }

        }        
    }
      

    #endregion
}
