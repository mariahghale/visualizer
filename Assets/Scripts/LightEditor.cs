using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LightEditor : TransformEditor
{
    public Dropdown typeDropdown;
    public InputField intensityEdit;
    public InputField rangeEdit;
    public InputField innerAngleEdit;
    public InputField outerAngleEdit;

    public GameObject rangeLayout;
    public GameObject angleLayout;

    private List<GameObject> extraFields;
    private Dictionary<string, List<GameObject>> typeFields;
    private List<string> lightTypes;
    private Light editLight;


    public override void setupGui()
    {   
        // populate the mode-specific gui options
        extraFields = new List<GameObject>(){rangeLayout, angleLayout};
        typeFields = new Dictionary<string, List<GameObject>>() 
        {
            {"Spot", new List<GameObject>(){rangeLayout, angleLayout}},
            {"Directional", new List<GameObject>()},
            {"Point", new List<GameObject>(){rangeLayout}}
        };

        // set up the type dropdown        
        lightTypes = new List<string>(typeFields.Keys);
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
        
        // Set up the transform editing delegates
        base.setupGui();

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
        // enable/disable ui elements based on the light type selected
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
        string val = intensityEdit.text;
        if(validateInput(val))
        {
            editLight.intensity = float.Parse(val);
        }
    }

    private void onRangeChange()
    {
        string val = rangeEdit.text;
        if(validateInput(val))
        {
            editLight.range = float.Parse(val);
        }
    }

    private void onInnerAngleChange()
    {
        string val = innerAngleEdit.text;
        if(validateInput(val))
        {
            editLight.innerSpotAngle = float.Parse(val);
        }
    }

    private void onOuterAngleChange()
    {
        string val = outerAngleEdit.text;
        if(validateInput(val))
        {
            editLight.spotAngle = float.Parse(val);
        }
    }
    
    private bool validateInput(string input)
    {
        return float.TryParse(input, out _);
    }
}
