using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class TitleManager : ChemicalSummonManager
{
    public static TitleManager Instance { get; protected set; }
    [SerializeField]
    Text versionText;
    [SerializeField]
    SettingScreen settingScreen;
    public static SettingScreen SettingScreen => Instance.settingScreen;
    // Start is called before the first frame update
    void Awake()
    {
        ManagerInit(Instance = this);
        versionText.text = "Version " + ChemicalSummonManager.Version;
    }
}
