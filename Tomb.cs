using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tomb : MonoBehaviour
{
    [SerializeField] private TextMeshPro nicknameText;

    public void SetNickname(string name)
    {
        nicknameText.text = name;
    }
}
