using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class WorldManager : AbstractManager
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
    [SerializeField]
    Transform popUpTransform;

    World generatedWorld;
    public static World World => Instance.generatedWorld;
    public static WorldPlayer Player => Instance.worldPlayer;
    public static ItemScreen ItemScreen => Instance.itemScreen;
    public static DeckScreen DeckScreen => Instance.deckScreen;
    public static ReactionScreen ReactionScreen => Instance.reactionScreen;
    public static CharacterScreen CharacterScreen => Instance.characterScreen;
    public static SettingScreen SettingScreen => Instance.settingScreen;
    public static Image NewReactionSign => Instance.newReactionSign;
    public static Transform PopUpTransform => Instance.popUpTransform;

    private void Awake()
    {
        ManagerInit(Instance = this);
        if(PlayerSave.SelectedCharacter.models.Count <= PlayerSave.CurrentCharacterModelIndex)
        {
            Debug.LogWarning("Current character doesn't have models for index " + PlayerSave.CurrentCharacterModelIndex);
        }
        else
            worldPlayer.SetModel(PlayerSave.SelectedCharacter.models[PlayerSave.CurrentCharacterModelIndex]);
        EnterWorld(PlayerSave.CurrentWorldHeader, Resources.Load<WorldEnterPortIDSO>("PortIDSO/DefaultPort")); 
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
    public World _EnterWorld(WorldHeader worldHeader, WorldEnterPortIDSO enterPortIdso = null)
    {
        WorldHeader previousWorldHeader = PlayerSave.CurrentWorldHeader;
        if (generatedWorld != null)
        {
            Destroy(generatedWorld.gameObject);
        }
        popUpTransform.DestroyAllChildren();
        generatedWorld = Instantiate((PlayerSave.CurrentWorldHeader = worldHeader).World, Instance.worldParentTf);
        WorldEnterPort enterPort = enterPortIdso == null ? generatedWorld.FindEnterPort(previousWorldHeader) : generatedWorld.FindEnterPort(enterPortIdso);
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
        if (audioSource.clip == null || !audioSource.clip.Equals(PlayerSave.CurrentWorld.BGM))
        {
            audioSource.clip = PlayerSave.CurrentWorld.BGM;
            audioSource.Play();
        }
        return generatedWorld;
    }
    public static World EnterWorld(WorldHeader worldHeader, WorldEnterPortIDSO enterPortIdso = null)
    {
        return Instance._EnterWorld(worldHeader, enterPortIdso);
    }
}
