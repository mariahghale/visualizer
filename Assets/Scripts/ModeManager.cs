using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ModeManager : MonoBehaviour
{
    public GameObject btnPrefab;
    public List<ModeSpecs> modeList = new List<ModeSpecs>();
    public string defaultModeName;
    public GameObject modeLayout;

    [System.Serializable]
    public class ModeSpecs
    {
        public string name;
        public Sprite icon;
        public GameObject guiPrefab;
    }

    private Dictionary<GameObject, GameObject> modeGuis = new Dictionary<GameObject, GameObject>();


    void Start()
    {
        // go through each mode, generate a button/gui prefab for it
        foreach(ModeSpecs spec in modeList)
        {  
            // setup the mode button
            GameObject modeBtn = Object.Instantiate(btnPrefab, gameObject.transform) as GameObject;
            modeBtn.transform.SetParent(modeLayout.transform);
            modeBtn.GetComponent<Image>().sprite = spec.icon;
            modeBtn.GetComponent<Button>().onClick.AddListener(onBtnClick);

            // setup the mode GUI
            Transform thisTransform = gameObject.transform;
            GameObject modeGui = Object.Instantiate(spec.guiPrefab, thisTransform) as GameObject;
            modeGui.transform.SetParent(gameObject.transform);
            modeGui.SetActive(spec.name == defaultModeName);
            BaseEditor script = modeGui.GetComponent<BaseEditor>();
            if (script != null)
            {
                script.setupGui();
            }
            // store the ui objects
            modeGuis.Add(modeBtn, modeGui);
        }
    }

    void onBtnClick()
    {
        // hide/unhide the active gui based on the clicked button
        GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
        foreach(var item in modeGuis)
        {
            if (item.Key == clickedBtn)
            {
               item.Value.SetActive(true);
            } else {
                item.Value.SetActive(false);
            }
        }
    }
}
