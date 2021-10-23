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
    AudioSource audioSource;
    [SerializeField]
    Transform worldParentTf;
    [SerializeField]
    WorldPlayer worldPlayer;
    [SerializeField]
    Transform playerCameraAnchor;
    [SerializeField]
    [Range(0, 1)]
    float cameraTracingRate;
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

    World generatedWorld;
    public static World World => Instance.generatedWorld;
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
        if(PlayerSave.SelectedCharacter.models.Count <= PlayerSave.CurrentCharacterModelIndex)
        {
            Debug.LogWarning("Current character doesn't have models for index " + PlayerSave.CurrentCharacterModelIndex);
        }
        else
            worldPlayer.SetModel(PlayerSave.SelectedCharacter.models[PlayerSave.CurrentCharacterModelIndex]);
        EnterWorld(PlayerSave.CurrentWorldLink, Resources.Load<WorldEnterPortIDSO>("PortIDSO/DefaultPort")); 
        ItemScreen.gameObject.SetActive(false);
        DeckScreen.gameObject.SetActive(false);
        ReactionScreen.gameObject.SetActive(false);
        CharacterScreen.gameObject.SetActive(false);
        SettingScreen.gameObject.SetActive(false);
        //new reaction sign on the LeftTab
        newReactionSign.gameObject.SetActive(PlayerSave.NewDicoveredReactions.Count > 0);
    }
    private void Update()
    {
        playerCameraAnchor.transform.position = Vector3.Lerp(playerCameraAnchor.transform.position, Player.Model.transform.position, cameraTracingRate);
    }
    public static void PlaySE(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, GameObject.FindGameObjectWithTag("SE Listener").transform.position);
    }
    public World _EnterWorld(World world, WorldEnterPortIDSO enterPortIdso = null)
    {
        World previousWorld = PlayerSave.CurrentWorldLink;
        if (generatedWorld != null)
        {
            Destroy(generatedWorld.gameObject);
        }
        generatedWorld = Instantiate(PlayerSave.CurrentWorldLink = world, Instance.worldParentTf);
        WorldEnterPort enterPort = enterPortIdso == null ? generatedWorld.FindEnterPort(previousWorld) : generatedWorld.FindEnterPort(enterPortIdso);
        if(enterPort == null)
        {
            Player.Model.transform.position = Vector3.zero;
        }
        else
        {
            Player.Model.transform.position = enterPort.transform.position;
            Player.Model.transform.rotation = enterPort.transform.rotation;
            Player.Rotater.StopAnimation();
        }
        playerCameraAnchor.position = Player.Model.transform.position;
        Physics.SyncTransforms(); //CharacterControll doesnt see above change
        if (audioSource.clip == null || !audioSource.clip.Equals(PlayerSave.CurrentWorldLink.BGM))
        {
            audioSource.clip = PlayerSave.CurrentWorldLink.BGM;
            audioSource.Play();
        }
        return generatedWorld;
    }
    public static World EnterWorld(World world, WorldEnterPortIDSO enterPortIdso = null)
    {
        return Instance._EnterWorld(world, enterPortIdso);
    }
}
