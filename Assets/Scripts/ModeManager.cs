using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeManager : MonoBehaviour
{
    public GameObject btnPrefab;
    public List<ModeSpecs> modeList = new List<ModeSpecs>();
    public GameObject modeLayout;

    private Dictionary<GameObject, GameObject> modeGuis = new Dictionary<GameObject, GameObject>();

    [System.Serializable]
    public class ModeSpecs
    {
        public Sprite icon;
        public GameObject guiPrefab;
        public ModeSpecs(Sprite modeIcon, GameObject modeEditorPrefab)
        {
            this.icon = modeIcon;
            this.guiPrefab = modeEditorPrefab;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach(ModeSpecs spec in modeList)
        {   
            GameObject modeBtn = Object.Instantiate(btnPrefab,this.gameObject.transform) as GameObject;
            modeBtn.transform.SetParent(this.modeLayout.transform);
            modeBtn.GetComponent<Image>().sprite = spec.icon;
            modeBtn.GetComponent<Button>().onClick.AddListener(this.onBtnClick);

            // instantiate the GUI
            GameObject modeGui = Object.Instantiate(spec.guiPrefab, this.gameObject.transform) as GameObject;
            modeGui.transform.SetParent(this.gameObject.transform);
            modeGui.SetActive(false);

            // Add both the button 
            this.modeGuis.Add(modeBtn, modeGui);
        }
    }

    void onBtnClick()
    {
        GameObject clickedBtn = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;
        Debug.Log(clickedBtn.ToString());

        foreach(var item in this.modeGuis)
        {
            GameObject btn = item.Key;
            if (btn == clickedBtn)
            {
               item.Value.SetActive(true);
            } else {
                item.Value.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
