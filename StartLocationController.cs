using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocationController : MonoBehaviour
{
    [SerializeField] private DoorGroupController castleCorridor, hospitalCorridor, schoolCorridor, subwayCorridor;
    [SerializeField] private Transform startCorridorContainer;
    [SerializeField] private TrackMainController trackMain;
    private void Awake()
    {
        GetMapByID();
    }

    private void GetMapByID()
    {
        int id = PlayerPrefs.GetInt(StringCollection.Instance.MapID, 2);
        Debug.Log(id + " id map");
        switch (id)
        {
            case 1:
                ActivateActualCorridor(castleCorridor);
                break;
            case 2:
                ActivateActualCorridor(castleCorridor);
                break;
            case 3:
                ActivateActualCorridor(hospitalCorridor);
                break;
            case 4:
                ActivateActualCorridor(schoolCorridor);
                break;
            case 5:
                ActivateActualCorridor(subwayCorridor);
                break;
            default:
                break;
        }
    }

    private void ActivateActualCorridor(DoorGroupController corridor)
    {
        corridor.gameObject.SetActive(true);
        corridor.transform.parent = trackMain.transform;
        trackMain.AddToActualCorridorsList(corridor);
    }

    // Вовремя ремувить
    public void RemoveAllStartCorridors()
    {
        Destroy(startCorridorContainer);
    }
}
