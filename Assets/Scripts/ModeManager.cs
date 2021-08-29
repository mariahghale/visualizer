using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ModeManager : MonoBehaviour
{
    public GameObject btnPrefab;
    public List<ModeSpecs> modeList = new List<ModeSpecs>();
    public GameObject modeLayout;

    [System.Serializable]
    public class ModeSpecs
    {
        public Sprite icon;
        public GameObject guiPrefab;
    }

    private Dictionary<GameObject, GameObject> modeGuis = new Dictionary<GameObject, GameObject>();


    void Start()
    {
        foreach(ModeSpecs spec in modeList)
        {  
            // setup the mode button
            GameObject modeBtn = Object.Instantiate(btnPrefab, this.gameObject.transform) as GameObject;
            modeBtn.transform.SetParent(this.modeLayout.transform);
            modeBtn.GetComponent<Image>().sprite = spec.icon;
            modeBtn.GetComponent<Button>().onClick.AddListener(this.onBtnClick);

            // setup the mode GUI
            Transform thisTransform = this.gameObject.transform;
            GameObject modeGui = Object.Instantiate(spec.guiPrefab, thisTransform) as GameObject;
            modeGui.transform.SetParent(this.gameObject.transform);
            modeGui.SetActive(false);
            BaseEditor script = modeGui.GetComponent<BaseEditor>();
            if (script != null)
            {
                script.setupGui();
            }
            // store the ui objects
            this.modeGuis.Add(modeBtn, modeGui);
        }
    }

    void onBtnClick()
    {
        GameObject clickedBtn = EventSystem.current.currentSelectedGameObject;
        // hide/unhide the active gui based on the clicked button
        foreach(var item in this.modeGuis)
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
