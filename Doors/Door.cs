using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfDoor
{
    Right,
    Left
}
[SelectionBase]
public class Door : MonoBehaviour
{
    public bool isCorrectDoor { get; set; }
    public bool isDoorOpen { get; private set; }
    [SerializeField] private TypeOfDoor typeOfDoor;
    [SerializeField] private DoorGroupController doorGroupController;
    [SerializeField] private GameObject doorModel;
    [SerializeField] private MeshRenderer doorMesh;
    private Animator doorAnimator;

    private void Start()
    {
        isDoorOpen = false;
        doorAnimator = GetComponentInChildren<Animator>();
        UpdateAnimationSpeed(TrackMainController.defaultAnimationSpeed);
    }

    public void SetTypeOfDoor(TypeOfDoor _typeOfDoor)
    {
        typeOfDoor = _typeOfDoor;
    }

    public void OpenDoor()
    {
        doorAnimator.SetTrigger(StringCollection.Instance.isOpen);
        PlayRandomOpenDoorSound();
        doorGroupController.DisableDoorsGroup(typeOfDoor);
        doorGroupController.IsDoorOpen = true;
        isDoorOpen = true;
    }

    private void PlayRandomOpenDoorSound()
    {
        int random = Random.Range(0, 2);
        if (random > 0)
        {
            AudioManager.instance.PlaySFX(AudioCollection.OpenDoor_1);
        }
        else
        {
            AudioManager.instance.PlaySFX(AudioCollection.OpenDoor_2);
        }
    }

    public TypeOfDoor GetTypeOfDoor()
    {
        return typeOfDoor;
    }

    public void RemoveDoor()
    {
        StartCoroutine(doorGroupController.RemoveDoorGroup());
    }

    public void UpdateAnimationSpeed(float doorSpeed)
    {
        doorAnimator.speed = doorSpeed;
    }

    public Transform GetDoorModelTransform()
    {
        return doorModel.transform;
    }

    public void SetDoorMaterial(Color color)
    {
        var material = doorMesh.materials[0];
        material.SetColor("_EmissionColor", color);
    }
}
