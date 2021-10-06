using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    [SerializeField]
    ItemHeader itemHeader;
    [SerializeField]
    Sprite icon;

    public virtual string Name => itemHeader.name;
    public virtual string Description => itemHeader.description;
    public Sprite Icon => icon;

    public abstract bool Usable { get; }
    public abstract void Use();
}
