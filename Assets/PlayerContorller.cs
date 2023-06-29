using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContorller : MonoBehaviour
{
    [Header("Player Control Data")]
    InputContorl inputContorl;
    Vector2 movementInptut;
    Vector2 cameraInptut;

    public inputElement northInput = new inputElement();
    [Header("Player Camera Data")]
    public Transform cameraSystem;
    public Transform cameraPivot;
    public Transform cameraObject;
    public Transform cameraFollowTarget;
    [Range(0, 10)]
    public float cameraFollowSpeed;
    [Range(0, 10)]
    public float cameraRotateSpeed;
    public float cameraMaxAngle;
    public float cameraMinAngle;
    public Vector2 cameraAngles;

    [Header("Player Movement Data")]
    public CharacterController controller;
    public Vector3 moveDirction;
    [Range(0,10)]
    public float movementSpeed;
    [Range(0, 10)]
    public float rotationSpeed;
    public bool isThirdPerson;
    #region InputContorl
    private void OnEnable()
    {
        if (inputContorl == null)
        {
            inputContorl=new InputContorl();
            inputContorl.movement.move.performed += inputContorl => movementInptut = inputContorl.ReadValue<Vector2>();
            inputContorl.movement.camera.performed += inputContorl => cameraInptut = inputContorl.ReadValue<Vector2>();

            inputContorl.actions.changeView.started += inputContorl => northInput.risingEdge = true;
            inputContorl.actions.changeView.performed += inputContorl => northInput.longPress = true;
            inputContorl.actions.changeView.canceled += inputContorl => northInput.releaseEdges();
        }
        inputContorl.Enable();
    }
    private void OnDisable()
    {
        inputContorl.Disable();
    }
    #endregion

    void Update()
    {
        HandleMovement();
        MovementRotation();
        CameraChangeView();
        CameraMovement();

    }

    Vector3 normalVector=Vector3.up;
    private void HandleMovement()
    {
        moveDirction = cameraObject.forward * movementInptut.y;
        moveDirction += cameraObject.right * movementInptut.x;
        Vector3 projectedVelocity=Vector3.ProjectOnPlane(moveDirction, normalVector);
        projectedVelocity.Normalize();
        projectedVelocity *= movementSpeed;
        controller.Move(projectedVelocity*Time.deltaTime);
    }
    private void MovementRotation()
    {
        Vector3 targetDir = Vector3.zero;
        if(isThirdPerson) { 
        targetDir=cameraObject.forward*movementInptut.y;
        targetDir+=cameraObject.right*movementInptut.x;
        }
        else
        {
            targetDir = cameraObject.forward;
        }
        if (targetDir == Vector3.zero)
        {
            targetDir=transform.forward;
        }
        Vector3 projectedDirection = Vector3.ProjectOnPlane(targetDir, normalVector);
        projectedDirection.Normalize();
        Quaternion targetDirection=Quaternion.LookRotation(projectedDirection);
        Quaternion smoothRotation=Quaternion.Slerp(transform.rotation,targetDirection,rotationSpeed*Time.deltaTime);
        transform.rotation = smoothRotation;
    }
    private void CameraMovement()
    {
        cameraSystem.position = Vector3.Lerp(cameraSystem.position, cameraFollowTarget.position, Time.deltaTime * cameraFollowSpeed);
        cameraAngles.x += (cameraInptut.x * cameraFollowSpeed) * Time.fixedDeltaTime;
        cameraAngles.y -= (cameraInptut.y * cameraFollowSpeed) * Time.fixedDeltaTime;
        if (isThirdPerson)
        {
            cameraAngles.y = Mathf.Clamp(cameraAngles.y, cameraMinAngle, cameraMaxAngle);
        }
        else
        {
            cameraAngles.y = Mathf.Clamp(cameraAngles.y, cameraMinAngle*1.5f, cameraMaxAngle*1.5f);

        }
        Vector3 rotation = Vector3.zero;
        rotation.y = cameraAngles.x;
        cameraSystem.rotation=Quaternion.Euler(rotation);
        rotation = Vector3.zero;
        rotation.x = cameraAngles.y;
        cameraPivot.localRotation=Quaternion.Euler(rotation);
    }

    private void CameraChangeView()
    {
        if (northInput.longPress)
        {
            isThirdPerson = true;
            Vector3 newPosition = new Vector3(0,5,-15);
            cameraObject.localPosition=Vector3.Lerp(cameraObject.localPosition,newPosition,Time.deltaTime*cameraFollowSpeed*2f);
        }
        else
        {
            isThirdPerson = false;
            Vector3 newPosition = Vector3.zero;
            cameraObject.localPosition = Vector3.Lerp(cameraObject.localPosition, newPosition, Time.deltaTime * cameraFollowSpeed * 2f);
        }
    }
    private void LateUpdate()
    {
        northInput.resetEdges();
    }
}
