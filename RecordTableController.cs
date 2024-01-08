using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class RecordTableController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI recordListText;
    [SerializeField] private TrackMainController trackMainController;
    private Dictionary<string, int> characterRecords = new Dictionary<string, int>();

    private void OnEnable()
    {
        SetDataTableOfRecords();
        SetPlayerToTableOfRecords();
        UpdateTableRecords();
        trackMainController.SetCharacterRecords(characterRecords);
    }

    public void SetDataTableOfRecords()
    {
        AddCharacterRecord("fr3ddy kru363r", 500);
        AddCharacterRecord("j450n v00rh335", 450);
        AddCharacterRecord("m1ch43l my3r5", 400);
        AddCharacterRecord("p3nnyw153 7h3 d4nc1n6 cl0wn", 350);
        AddCharacterRecord("5l3nd3rm4n", 300);
        AddCharacterRecord("n16h7m4r3 fr3ddy", 250);
        AddCharacterRecord("hu66y wu66y", 200);
        AddCharacterRecord("4lc1n4 d1m17r35cu", 150);
        AddCharacterRecord("6h057f4c3", 100);
        AddCharacterRecord("pyr4m1d h34d", 5);

    }

    public void SetPlayerToTableOfRecords()
    {
        AddCharacterRecord(gameManager.playerNickname, gameManager.GetPlayerScore());
    }

    private void SortingDictionaryUpdateUIRecords()
    {
        var sortedRecords = characterRecords.OrderByDescending(x => x.Value);

        string recordText = "";
        foreach (var record in sortedRecords)
        {
            bool isPlayerRecord = record.Key == gameManager.playerNickname;
            string textColorTag = isPlayerRecord ? "<color=#FFD700>" : "<color=#FFFFFF>";
            string endColorTag = "</color>";
            recordText += textColorTag + record.Key + ": " + record.Value + endColorTag + "\n";
        }
        recordListText.text = recordText;
    }

    public void UpdateTableRecords()
    {
        SortingDictionaryUpdateUIRecords();
    }

    public void AddCharacterRecord(string characterName, int record)
    {
        if (characterName != null)
        {
            if (!characterRecords.ContainsKey(characterName))
            {
                characterRecords.Add(characterName, record);
            }
            else
            {
                characterRecords[characterName] = record;
            }
        }
        else
        {
            Debug.LogWarning("Trying to add a record with a null character name.");
        }
    }

    public int GetCharacterRecord(string characterName)
    {
        if (characterRecords.ContainsKey(characterName))
        {
            return characterRecords[characterName];
        }
        else
        {
            return 0;
        }
    }

    public Dictionary<string, int> GetCharacterRecords()
    {
        return characterRecords;
    }
}
