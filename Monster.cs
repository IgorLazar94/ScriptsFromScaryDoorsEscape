using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeOfMonster
{
    One,
    Two,
    Slug,
    Lacodon,
    Vampire,
    Undertaker,
    Gastaroid
}

[SelectionBase]
public class Monster : MonoBehaviour
{
    [SerializeField] private TypeOfMonster typeOfMonster;
    private Animator monsterAnimator;
    private float monsterOffset = 4f;
    private float timeToOffset = 0.25f;
    private bool isReadyToRetreat = false;

    private void Start()
    {
        monsterAnimator = GetComponent<Animator>();
    }
    public void ActivateAttackMonster()
    {
        int randomAttack = ChooseRandomAttack();
        PlayMonsterSound();
        monsterAnimator.SetTrigger(StringCollection.Instance.Attack);
        monsterAnimator.SetInteger(StringCollection.Instance.TypeOfAttack, randomAttack);
        GameManager.OnActivateSlowmod.Invoke();
    }

    public void PrepareMonster()
    {
        monsterAnimator.SetTrigger(StringCollection.Instance.Prepare);
        transform.DOMoveX(transform.position.x - monsterOffset, timeToOffset).OnComplete(() => MonsterRetreat());
    }

    private void PlayMonsterSound()
    {
        switch (typeOfMonster)
        {
            case TypeOfMonster.One:
                AudioManager.instance.PlaySFX(AudioCollection.MonsterSound_1);
                break;
            case TypeOfMonster.Two:
                AudioManager.instance.PlaySFX(AudioCollection.MonsterSound_2);
                break;
            case TypeOfMonster.Slug:
                AudioManager.instance.PlaySFX(AudioCollection.MonsterSound_2);
                break;
            case TypeOfMonster.Undertaker:
                AudioManager.instance.PlaySFX(AudioCollection.MonsterSound_1);
                break;
            case TypeOfMonster.Gastaroid:
                AudioManager.instance.PlaySFX(AudioCollection.MonsterSound_2);
                break;
            case TypeOfMonster.Vampire:
                AudioManager.instance.PlaySFX(AudioCollection.MonsterSound_1);
                break;
            default:
                break;
        }
    }

    private int ChooseRandomAttack()
    {
        return Random.Range(1, 3);
    }

    public TypeOfMonster GetTypeOfMonster()
    {
        return typeOfMonster;
    }

    private void MonsterRetreat()
    {
        if (isReadyToRetreat)
        {
            monsterAnimator.SetTrigger(StringCollection.Instance.Retreat);
            transform.DOMoveX(transform.position.x + monsterOffset, timeToOffset);
        }
    }

    public void SetReadyRetreatMonster()
    {
        isReadyToRetreat = true;
    }

    //private void FixedUpdate()
    //{
    //    if (isReadyToRetreat && isJumpToAttackPos)
    //    {
    //        Debug.Log("monster retreat");
    //        Vector3 newPosition = new Vector3(transform.position.x - monsterOffset, transform.position.y, transform.position.z);
    //        transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * timeToOffset * 5f);
    //    }
    //}
}
