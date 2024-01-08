using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreamerController : MonoBehaviour
{
    public static Action<Vector3> SetMonsterToPos;
    [SerializeField] private Monster clownPrefab, maniacPrefab, gastaroidPrefab, slugPrefab, undertakerPrefab, vampirePrefab;
    [SerializeField] private Transform monsterContainer, spawnPosition;
    private Monster placedMonster;

    private void Start()
    {
        //ChooseRandomMonster();
        GetMonsterByID();
    }

    private void GetMonsterByID()
    {
        int id = PlayerPrefs.GetInt(StringCollection.Instance.MonsterID);
        switch (id)
        {
            case 1:
                placedMonster = CreateMonsterFromPrefab(clownPrefab);
                break;
            case 2:
                placedMonster = CreateMonsterFromPrefab(maniacPrefab);
                break;
            case 3:
                placedMonster = CreateMonsterFromPrefab(gastaroidPrefab);
                break;
            case 4:
                placedMonster = CreateMonsterFromPrefab(slugPrefab);
                break;
            case 5:
                placedMonster = CreateMonsterFromPrefab(undertakerPrefab);
                break;
            case 6:
                placedMonster = CreateMonsterFromPrefab(vampirePrefab);
                break;
            default:
                placedMonster = CreateMonsterFromPrefab(vampirePrefab);
                break;
        }
    }

    private Monster CreateMonsterFromPrefab(Monster monster)
    {
        var newMonster = Instantiate(monster, spawnPosition.position, monster.transform.rotation, monsterContainer);
        return newMonster;
    }

    private void OnEnable()
    {
        SetMonsterToPos += SetMonsterPosition;
    }

    private void OnDisable()
    {
        SetMonsterToPos -= SetMonsterPosition;
    }

    //public void ChooseRandomMonster()
    //{
    //    int random = UnityEngine.Random.Range(0, activeMonsters.Length);
    //    placedMonster = activeMonsters[random];
    //}

    public void SetMonsterPosition(Vector3 pos)
    {
        if (placedMonster.GetTypeOfMonster() == TypeOfMonster.One)
        {
            placedMonster.transform.position = new Vector3(pos.x + 5f, pos.y - 5f, pos.z + 8f);
        }
        else if (placedMonster.GetTypeOfMonster() == TypeOfMonster.Two)
        {
            placedMonster.transform.position = new Vector3(pos.x, pos.y - 5f, pos.z + 4f);
        }
        else if (placedMonster.GetTypeOfMonster() == TypeOfMonster.Slug)
        {
            placedMonster.transform.position = new Vector3(pos.x - 1.5f, pos.y - 5f, pos.z + 4f);
        }
        else if (placedMonster.GetTypeOfMonster() == TypeOfMonster.Lacodon)
        {
            placedMonster.transform.position = new Vector3(pos.x - 1.5f, pos.y - 5f, pos.z + 10f);
        }
        else if (placedMonster.GetTypeOfMonster() == TypeOfMonster.Undertaker)
        {
            placedMonster.transform.position = new Vector3(pos.x, pos.y - 5f, pos.z + 5f);
        }
        else if (placedMonster.GetTypeOfMonster() == TypeOfMonster.Gastaroid)
        {
            placedMonster.transform.position = new Vector3(pos.x, pos.y - 5f, pos.z + 10f);
        }
        else if (placedMonster.GetTypeOfMonster() == TypeOfMonster.Vampire)
        {
            placedMonster.transform.position = new Vector3(pos.x, pos.y - 5f, pos.z + 6f);
        }
        placedMonster.gameObject.SetActive(true);
    }

    public void ActivateAttackMonster()
    {
        ActivateMonster(true);
        placedMonster.ActivateAttackMonster();
    }

    public void PrepareToAttackMonster()
    {
        ActivateMonster(true);
        placedMonster.PrepareMonster();
    }

    public void RetreatTheMonster()
    {
        ActivateMonster(true);
        placedMonster.SetReadyRetreatMonster();
    }

    public void ActivateMonster(bool isActive)
    {
        placedMonster.gameObject.SetActive(isActive);
    }
}
