using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LightEditor : TransformEditor
{
    public Dropdown typeDropdown;
    public InputField intensityEdit;
    public InputField multiplierEdit;

    public GameObject rangeLayout;
    public GameObject angleLayout;

    public InputField rangeEdit;
    public InputField innerAngleEdit;
    public InputField outerAngleEdit;

    private List<GameObject> extraFields;
    private Dictionary<string, List<GameObject>> typeFields;
    private List<string> lightTypes;


    public override void setupGui()
    {   
        extraFields = new List<GameObject>(){rangeLayout, angleLayout};
        typeFields = new Dictionary<string, List<GameObject>>() 
        {
            {"Spot", new List<GameObject>(){rangeLayout, angleLayout}},
            {"Directional", new List<GameObject>()},
            {"Point", new List<GameObject>(){rangeLayout}}
        };
        lightTypes = new List<string>(typeFields.Keys);

        // set up the type dropdown        
        typeDropdown.AddOptions(lightTypes);
        typeDropdown.onValueChanged.AddListener(delegate{onTypeChange(typeDropdown);});

        modeTransform = GameObject.FindWithTag("EditableLight").transform;
        setupDefaultVals();
        base.setupGui();
    }

    private void onTypeChange(Dropdown dropdown)
    {
        // based on the mode selected
        string selectedType = dropdown.captionText.text;
        List<GameObject> extraElems = typeFields[selectedType];
        foreach(GameObject uiElem in extraFields)
        {
            bool isNeeded = extraElems.Contains(uiElem);
            uiElem.SetActive(isNeeded);
        }
    }
}
