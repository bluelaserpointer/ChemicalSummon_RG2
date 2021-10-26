using UnityEngine;

[CreateAssetMenu(menuName = "World/Header")]
public class WorldHeader : ScriptableObject
{
    [SerializeField]
    World world;
    public World World => world;
}
