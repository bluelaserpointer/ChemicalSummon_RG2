using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WorldManager : ChemicalSummonManager
{
    public static WorldManager Instance { get; protected set; }

    [SerializeField]
    World world;
    [SerializeField]
    WorldPlayer worldPlayer;
    [SerializeField]
    ItemScreen itemScreen;
    [SerializeField]
    DeckScreen deckScreen;
    [SerializeField]
    ReactionScreen reactionScreen;
    [SerializeField]
    CharacterScreen characterScreen;
    [SerializeField]
    SettingScreen settingScreen;
    [SerializeField]
    Image newReactionSign;

    public static WorldPlayer Player => Instance.worldPlayer;
    public static ItemScreen ItemScreen => Instance.itemScreen;
    public static DeckScreen DeckScreen => Instance.deckScreen;
    public static ReactionScreen ReactionScreen => Instance.reactionScreen;
    public static CharacterScreen CharacterScreen => Instance.characterScreen;
    public static SettingScreen SettingScreen => Instance.settingScreen;
    public static Image NewReactionSign => Instance.newReactionSign;


    private void Awake()
    {
        ManagerInit(Instance = this);
        ItemScreen.gameObject.SetActive(false);
        DeckScreen.gameObject.SetActive(false);
        ReactionScreen.gameObject.SetActive(false);
        CharacterScreen.gameObject.SetActive(false);
        SettingScreen.gameObject.SetActive(false);
        //new reaction sign on the LeftTab
        newReactionSign.gameObject.SetActive(PlayerSave.NewDicoveredReactions.Count > 0);
        //load last player position
        if(PlayerSave.hasLastWorldPositionSave)
        {
            Player.TargetModel.transform.position = PlayerSave.lastWorldPlayerPosition;
            Player.TargetModel.transform.rotation = PlayerSave.lastWorldPlayerRotation;
        }
    }
    public static void PlaySE(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, GameObject.FindGameObjectWithTag("SE Listener").transform.position);
    }
}
