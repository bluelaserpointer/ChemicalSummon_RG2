using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class CardCondition : MonoBehaviour
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
    /// �����Ƿ����������
    /// </summary>
    /// <param name="slot">���ڿ���</param>
    /// <param name="substance">����</param>
    /// <returns></returns>
    public abstract bool Accept(SubstanceCard card);
}
