using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗背景，通过MatchManager生成
/// </summary>
[DisallowMultipleComponent]
public class MatchBackGround : MonoBehaviour
{
    [SerializeField]
    GameObject explosionPrefab;
    [SerializeField]
    Transform opponentEffectAnchor;
    public void OnExplosion()
    {
        Instantiate(explosionPrefab, opponentEffectAnchor);
    }
}
