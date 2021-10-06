using UnityEngine;

/// <summary>
/// 可攻击部位
/// </summary>
public class AttackableGamer : MonoBehaviour, IAttackable
{
    Field field;
    /// <summary>
    /// 所属场地
    /// </summary>
    public Field Field => field;
    /// <summary>
    /// 游戏者
    /// </summary>
    public Gamer Gamer => Field.Gamer;
    private void Awake()
    {
        field = GetComponent<Field>();
    }
    public bool AllowAttack(SubstanceCard card)
    {
        return true;
    }
    public void Attack(SubstanceCard card)
    {
        Gamer.HP -= card.ATK;
    }
}
