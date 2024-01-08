using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackMainController : MonoBehaviour
{
    public static System.Action OnUpdateDoorSpeed;
    public static float defaultAnimationSpeed { get; private set; }
    [SerializeField] private List<GameObject> oldCorridors;
    [SerializeField] private List<GameObject> castleMapPrefabs, hospitalMapPrefabs, schoolMapPrefabs, subwayMapPrefabs;
    [SerializeField] private Tomb[] tombPrefabs;
    [SerializeField] private PlayerCameraController cameraController;
    [SerializeField] private List<DoorGroupController> actualDoorsGroupControllers = new List<DoorGroupController>();
    [SerializeField] private Color colorGoodDoor, colorBadDoor, colorNeutral;
    [SerializeField] private ElevatorController elevatorSimplePrefab, elevatorVIPPrefab;
    private List<GameObject> availableDoorGroupPrefabs = new List<GameObject>();
    private int counter = 1;
    private int zOffset = 60;
    private int maxCounterToUpdateSpeed = 140;
    private Dictionary<string, int> characterRecords = new Dictionary<string, int>();
    private bool isActivateCameraDrop;
    private bool isElevatorMode;
    private bool isVIPElevator;
    private bool isSpawnCorridorAfterElevator = false;

    private void Start()
    {
        defaultAnimationSpeed = 1f;
        GetMapByID();

    }

    private void GetMapByID()
    {
        int id = PlayerPrefs.GetInt(StringCollection.Instance.MapID);
        switch (id)
        {
            case 1:
                AddMapToAvailableList(oldCorridors);
                break;
            case 2:
                AddMapToAvailableList(castleMapPrefabs);
                break;
            case 3:
                AddMapToAvailableList(hospitalMapPrefabs);
                break;
            case 4:
                AddMapToAvailableList(schoolMapPrefabs);
                break;
            case 5:
                AddMapToAvailableList(subwayMapPrefabs);
                break;
            default:
                AddMapToAvailableList(castleMapPrefabs);
                break;
        }
    }

    private void AddMapToAvailableList(List<GameObject> newCorridorsList)
    {
        availableDoorGroupPrefabs.AddRange(newCorridorsList);
    }

    public void InitNewPartOfTrack()
    {
        if (!isElevatorMode)
        {
            ChooseAndInitRandomTrack();
        }
        else
        {
            if (isVIPElevator)
            {
                InitVIPElevator();
            }
            else
            {
                InitSimpleElevator();
            }
        }
    }

    public void ChooseAndInitRandomTrack()
    {
        int randomNumber = Random.Range(0, availableDoorGroupPrefabs.Count);
        var newTrack = Instantiate(availableDoorGroupPrefabs[randomNumber], transform.position, Quaternion.identity);
        if (isActivateCameraDrop)
        {
            newTrack.GetComponent<DoorGroupController>().EnableEmission(colorGoodDoor, colorBadDoor);
        }
        actualDoorsGroupControllers.Add(newTrack.GetComponent<DoorGroupController>());
        newTrack.transform.parent = transform;
        if (isSpawnCorridorAfterElevator)
        {
            isSpawnCorridorAfterElevator = false;
        }
        newTrack.transform.position += new Vector3(transform.position.x,
                                                   transform.position.y,
                                                   transform.position.z + (counter * zOffset));
        CheckTombInstantiate(newTrack.GetComponent<DoorGroupController>().GetOtherDoorPos());
        counter++;
        CheckUpdateCameraSpeed();
    }

    private void InitSimpleElevator()
    {
        var elevator = Instantiate(elevatorSimplePrefab, transform.position, Quaternion.identity);
        elevator.transform.position += new Vector3(transform.position.x,
                                                   transform.position.y + 6f,
                                                   transform.position.z + 9f + ((counter - 1) * zOffset)); // править позицию лифта относительно коридора
        isSpawnCorridorAfterElevator = true;
    }

    private void InitVIPElevator()
    {
        var elevator = Instantiate(elevatorVIPPrefab, transform.position, Quaternion.identity);
        elevator.transform.position += new Vector3(transform.position.x,
                                                   transform.position.y + 6f,
                                                   transform.position.z + 9f + ((counter - 1) * zOffset)); // править позицию VIP лифта относительно коридора
        isSpawnCorridorAfterElevator = true;
    }

    private void CheckUpdateCameraSpeed()
    {
        if (counter % 5 == 0 && counter < maxCounterToUpdateSpeed)
        {
            cameraController.UpdateCameraSpeed();
            defaultAnimationSpeed += 0.15f;
        }
        if (counter % 50 == 0)
        {
            cameraController.UpdateCameraOffsetSpeed();
            if (counter <= maxCounterToUpdateSpeed)
            {
                CountDownTimerFlashLight.OnShortenTheTimer?.Invoke();
            }
        }
    }

    public void RemoveDoorGroup(DoorGroupController doorGroup)
    {
        actualDoorsGroupControllers.Remove(doorGroup);
        Destroy(doorGroup.gameObject);
    }

    private void CheckTombInstantiate(Vector3 spawnPos)
    {
        foreach (var record in characterRecords)
        {
            if (counter == record.Value)
            {
                var newTomb = Instantiate(GetRandomTomb(), new Vector3(spawnPos.x, spawnPos.y + 0.5f, spawnPos.z - 2f), Quaternion.identity);
                newTomb.SetNickname(record.Key);
                break;
            }
        }

    }

    private Tomb GetRandomTomb()
    {
        int random = Random.Range(0, tombPrefabs.Length);
        return tombPrefabs[random];
    }

    public void SetCharacterRecords(Dictionary<string, int> records)
    {
        characterRecords = records;
    }

    public IEnumerator EnableDoorEmissionFromCameraDrop(float time)
    {
        yield return new WaitForSeconds(0.25f);
        foreach (var doorGroup in actualDoorsGroupControllers)
        {
            isActivateCameraDrop = true;
            doorGroup.EnableEmission(colorGoodDoor, colorBadDoor);
            StartCoroutine(DisableDoorEmissionFromCameraDrop(time));
        }
    }

    private IEnumerator DisableDoorEmissionFromCameraDrop(float time)
    {
        yield return new WaitForSeconds(time);
        UIController.OnActivateCameraFadeFX.Invoke(false);
        yield return new WaitForSeconds(0.25f);
        isActivateCameraDrop = false;
        foreach (var doorGroup in actualDoorsGroupControllers)
        {
            doorGroup.DisableEmission(colorNeutral);
        }
    }

    public void ActivateElevatorMode(bool value, bool isVIP)
    {
        isElevatorMode = value;
        isVIPElevator = isVIP;
    }

    public void AddToActualCorridorsList(DoorGroupController actualCorridor)
    {
        actualDoorsGroupControllers.Add(actualCorridor);
    }
}
