using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float MaxCamSpeed;
    [RangeAttribute(0.000001f,0.999f)]public float ScreenBoundsPercent;

    private Vector3 movementVector;
    private float leftBound;
    private float rightBound;
    private float topBound;
    private float bottomBound;



    #region Unity LifeCycle

    void Start()
    {
        ResetMovementBounds();
    }

    // Update is called once per frame
    void Update()
    {
        MovementCheck();
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

        Debug.Log($"Movement Vector:{movementVector}");

        transform.position += movementVector*Time.deltaTime;
    }

    public void ZoomCheck()
    {
        //check for scroll input



    }
      

    #endregion
}
