using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private UIController uIController;
    [SerializeField] private BoosterController boosterController;
    private PlayerCameraController playerCamera;
    private float distanceToCheckDoor = 80f;
    private float lastTapTime;
    private bool isAndroid;
    private float doubleTapTimeThreshold = 0.5f;

    private void Start()
    {
        CheckPlatform();
        playerCamera = GetComponent<PlayerCameraController>();
    }
    void Update()
    {
        CheckDoorByTouch();
        if (isAndroid)
        {
            CheckDoubleTouch();
        }
        else
        {
            CheckDoubleClick();
        }
    }

    private void CheckPlatform()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            isAndroid = true;
        }
        else
        {
            isAndroid = false;
        }
    }

    private void CheckDoorByTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0))
        {
            Vector3 touchPos = Vector3.zero;

            if (Input.touchCount > 0)
            {
                touchPos = Input.GetTouch(0).position;
            }
            else if (Input.GetMouseButtonDown(0))
            {
                touchPos = Input.mousePosition;
            }
            Ray ray = Camera.main.ScreenPointToRay(touchPos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, distanceToCheckDoor) && hit.collider.GetComponent<Door>() != null)
            {
                OpenHitDoor(hit.collider.GetComponent<Door>());
            }
        }
    }

    private void OpenHitDoor(Door _door)
    {
        if (!_door.isDoorOpen)
        {
            _door.OpenDoor();
            playerCamera.SetTypeOfDoor(_door.GetTypeOfDoor());
            playerCamera.isReadyToMove = true;
            playerCamera.SwitchFlashlight(true);
            //uIController.ActivateTimer(false);
            CheckCorrectDoor(_door);
        }
    }

    private void CheckCorrectDoor(Door _door)
    {
        if (!_door.isCorrectDoor)
        {
            if (PlayerCameraController.isActiveElevatorKey)
            {
                return;
            }
        }
    }

    private void CheckDoubleTouch()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);
            if (touch.phase == TouchPhase.Began)
            {
                if (Time.time - lastTapTime < doubleTapTimeThreshold && !GameManager.IsActivateFirework)
                {
                    boosterController.UseFirework();
                    lastTapTime = 0f;
                }
                else
                {
                    lastTapTime = Time.time;
                }
            }
        }
    }

    private void CheckDoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastTapTime < doubleTapTimeThreshold && !GameManager.IsActivateFirework)
            {
                boosterController.UseFirework();
                lastTapTime = 0f;
            }
            else
            {
                lastTapTime = Time.time;
            }
        }
    }
}
