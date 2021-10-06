/// <summary>
/// 可攻击接口
/// </summary>
public interface IAttackable
{
    /// <summary>
    /// 允许用该卡攻击
    /// </summary>
    /// <param name="card"></param>
    bool AllowAttack(SubstanceCard card);
    /// <summary>
    /// 用该卡攻击
    /// </summary>
    /// <param name="card"></param>
    void Attack(SubstanceCard card);
}
