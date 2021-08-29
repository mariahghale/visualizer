using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LightEditor : TransformEditor
{
    public Dropdown typeDropdown;
    public InputField intensityEdit;

    public GameObject rangeLayout;
    public GameObject angleLayout;

    public InputField rangeEdit;
    public InputField innerAngleEdit;
    public InputField outerAngleEdit;

    private List<GameObject> extraFields;
    private Dictionary<string, List<GameObject>> typeFields;
    private List<string> lightTypes;
    private Light editLight;


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

        // setup all the input field callbacks
        intensityEdit.onValueChanged.AddListener(delegate {onIntensityChange();});
        rangeEdit.onValueChanged.AddListener(delegate {onRangeChange();});
        innerAngleEdit.onValueChanged.AddListener(delegate {onInnerAngleChange();});
        outerAngleEdit.onValueChanged.AddListener(delegate {onOuterAngleChange();});

        // find the light the user will edit
        modeTransform = GameObject.FindWithTag("EditableLight").transform;
        editLight = modeTransform.GetComponent<Light>();
        base.setupGui();

        setupUiDefaults();
    }

    private void setupUiDefaults()
    {
        // set up the pos/rot defaults
        setupDefaultVecs();

        // set up the other field defaults
        intensityEdit.text = editLight.intensity.ToString();
        rangeEdit.text = editLight.range.ToString();
        innerAngleEdit.text = editLight.innerSpotAngle.ToString();
        outerAngleEdit.text = editLight.spotAngle.ToString();
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

        editLight.type = (LightType)System.Enum.Parse(typeof(LightType), selectedType);
    }

    private void onIntensityChange()
    {
        editLight.intensity = float.Parse(intensityEdit.text);
    }

    private void onRangeChange()
    {
        editLight.range = float.Parse(rangeEdit.text);
    }

    private void onInnerAngleChange()
    {
        editLight.innerSpotAngle = float.Parse(innerAngleEdit.text);

    }

    private void onOuterAngleChange()
    {
        editLight.spotAngle = float.Parse(outerAngleEdit.text);
    }
}
