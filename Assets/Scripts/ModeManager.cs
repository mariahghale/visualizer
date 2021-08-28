using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    public GameObject btnPrefab;
    public List<Mode> modeList = new List<Mode>();
    public GameObject editorLayout;
    public GameObject modeLayout;

    private List<GameObject> modeBtns = new List<GameObject>();

    [System.Serializable]
    public class Mode
    {
        public Sprite icon;
        public GameObject guiPrefab;
        public Mode(Sprite modeIcon, GameObject modeEditorPrefab)
        {
            this.icon = modeIcon;
            this.guiPrefab = modeEditorPrefab;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (Mode mode in modeList)
        {
            GameObject modeBtn = Object.Instantiate(btnPrefab) as GameObject;
            modeBtn.transform.SetParent(this.modeLayout.transform);
            modeBtn.GetComponent<Image>().sprite = mode.icon;
            modeBtn.GetComponent<Button>().onClick.AddListener(this.onBtnClick);
            this.modeBtns.Add(modeBtn);
        }
    }

    void onBtnClick()
    {
        GameObject clickedBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log(clickedBtn.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
