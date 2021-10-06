using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ResultPanel : MonoBehaviour
{
    [SerializeField]
    GameObject winObject, loseObject;

    public bool IsMatchFinish => gameObject.activeSelf;
    public void SetResult(bool isVictory)
    {
        gameObject.SetActive(true);
        MatchManager.Player.EndDefence();
        MatchManager.Player.RemoveAttackButtons();
        if(isVictory)
        {
            winObject.SetActive(true);
            loseObject.SetActive(false);
        }
        else
        {
            winObject.SetActive(false);
            loseObject.SetActive(true);
        }
    }
}
