using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 战斗 - 我方游戏者固定
/// </summary>
public class FixedCharacterMatch : Match
{
    [SerializeField]
    Character mySideCharacter;
    /// <summary>
    /// 我方游戏者
    /// </summary>
    public override Character MySideCharacter => mySideCharacter;
}
