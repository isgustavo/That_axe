using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIRankBehaviour : MonoBehaviour
{

    private int monsterHittedCount = 0;

    [SerializeField]
    private GameObject rankPanel;
    [SerializeField]
    private TextMeshProUGUI rankValue;

    public void OnMonsterHitEvent()
    {
        monsterHittedCount += 1;
    }

    public void OnEndGameEvent()
    {
        rankPanel.SetActive(true);

        if (monsterHittedCount < 2)
        {
            rankValue.text = "D";
        } else if (monsterHittedCount < 4)
        {
            rankValue.text = "C";
        }
        else if (monsterHittedCount < 6)
        {
            rankValue.text = "B";
        }
        else if (monsterHittedCount < 8)
        {
            rankValue.text = "A";
        }
        else
        {
            rankValue.text = "S";
        }

    }

    public void OnStartGameEvent()
    {
        rankPanel.SetActive(false);
        monsterHittedCount = 0;
    }

    private void OnEnable()
    {
        OnStartGameEvent();
    }
}
