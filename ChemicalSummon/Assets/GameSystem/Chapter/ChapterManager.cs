using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterManager : MonoBehaviour
{
    [SerializeField]
    Canvas chapterScreen;
    [SerializeField]
    Transform chapterContentRoot;
    [SerializeField]
    GameObject dummyContent;

    //data
    ScrollRect scrollRect;
    // Start is called before the first frame update
    private void Awake()
    {
        scrollRect = GetComponentInChildren<ScrollRect>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void ShowScreen()
    {
        chapterScreen.gameObject.SetActive(true);
    }
    public void HideScreen()
    {
        chapterScreen.gameObject.SetActive(false);
    }
    public void SetChapter(Chapter chapterPrefab)
    {
        //Instantiate(chapterPrefab, chapterContentRoot);
    }
}
