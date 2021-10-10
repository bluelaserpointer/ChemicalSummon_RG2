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
        worldPlayer.SetModel(PlayerSave.SelectedCharacter.models[PlayerSave.CurrentCharacterModelIndex]);
        Player.Model.transform.position = PlayerSave.CurrentCharacterPosition;
        playerCameraAnchor.position = Player.Model.transform.position;
        GenerateWorld(PlayerSave.CurrentWorld);
        if(audioSource.clip == null || !audioSource.clip.Equals(PlayerSave.CurrentWorld.BGM))
        {
            audioSource.clip = PlayerSave.CurrentWorld.BGM;
            audioSource.Play();
        }
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
            Player.Model.transform.position = PlayerSave.lastWorldPlayerPosition;
            Player.Model.transform.rotation = PlayerSave.lastWorldPlayerRotation;
        }
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
    public void GenerateWorld(World world)
    {
        if(generatedWorld != null)
            Destroy(generatedWorld);
        generatedWorld = Instantiate(PlayerSave.CurrentWorld, worldParentTf);
    }
}
