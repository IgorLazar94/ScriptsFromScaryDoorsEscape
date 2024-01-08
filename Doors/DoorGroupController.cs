using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class DoorGroupController : MonoBehaviour
{
    public bool IsDoorOpen { get; set; }
    [SerializeField] private Door correctDoor;
    [SerializeField] private Door otherDoor;
    private TrackMainController trackController;
    private Door[] doors;
    private void Start()
    {
        IsDoorOpen = false;
        trackController = transform.parent.GetComponent<TrackMainController>();
        doors = GetComponentsInChildren<Door>();
        RandomChangePosition();
        correctDoor.isCorrectDoor = true;
    }

    private void RandomChangePosition()
    {
        int random = Random.Range(0, 2);
        if (random > 0)
        {
            Vector3 firstPos = correctDoor.transform.position;
            Vector3 secondPos = otherDoor.transform.position;
            correctDoor.transform.position = secondPos;
            otherDoor.transform.position = firstPos;
            if (firstPos.x > secondPos.x)
            {
                correctDoor.SetTypeOfDoor(TypeOfDoor.Left);
                otherDoor.SetTypeOfDoor(TypeOfDoor.Right);
            }
        }
    }

    public void DisableDoorsGroup(TypeOfDoor activateDoor)
    {
        TypeOfDoor otherDoorType = TypeOfDoor.Left;
        switch (activateDoor)
        {
            case TypeOfDoor.Right:
                otherDoorType = TypeOfDoor.Left;
                break;
            case TypeOfDoor.Left:
                otherDoorType = TypeOfDoor.Right;
                break;
            default:
                Debug.LogWarning("Undefined type of door");
                break;
        }
        foreach (var door in doors)
        {
            if (door.GetTypeOfDoor() == otherDoorType)
            {
                door.GetComponent<BoxCollider>().enabled = false;
            }
        }

        InitNewPartOfTrack();
    }

    private void InitNewPartOfTrack()
    {
        trackController.InitNewPartOfTrack();
    }

    public IEnumerator RemoveDoorGroup()
    {
        yield return new WaitForSeconds(1.0f);
        trackController.RemoveDoorGroup(this);
    }

    public Vector3 GetOtherDoorPos()
    {
        return otherDoor.transform.position;
    }

    public void EnableEmission(Color good, Color bad)
    {
        correctDoor.SetDoorMaterial(good);
        otherDoor.SetDoorMaterial(bad);
    }

    public void DisableEmission(Color neutral)
    {
        correctDoor.SetDoorMaterial(neutral);
        otherDoor.SetDoorMaterial(neutral);
    }
}
