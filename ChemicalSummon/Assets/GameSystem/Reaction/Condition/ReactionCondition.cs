using UnityEngine;

[DisallowMultipleComponent]
public abstract class ReactionCondition : MonoBehaviour
{
    string description;
    /// <summary>
    /// ��������˵��
    /// </summary>
    public string Description => description;
    protected abstract string InitDescription();
    private void OnValidate()
    {
        description = InitDescription();
    }
    /// <summary>
    /// ��Ӧʽ�Ƿ����������
    /// </summary>
    /// <param name="reaction">��Ӧʽ</param>
    /// <returns></returns>
    public abstract bool Accept(Reaction reaction);
}
