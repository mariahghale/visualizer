using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CamEditor : BaseEditor
{
    public Dropdown projDropdown;
    public Slider fovSlider;
    public InputField nearClipEdit;
    public InputField farClipEdit;

    private GameObject camObj;
    private Camera cam;
    private string orthoText = "Ortho";

    public override void setupGui()
    {   
        cam = Camera.main;
        camObj = cam.gameObject;

        // add listeners for all of the gui elements
        projDropdown.onValueChanged.AddListener(delegate{onProjectionChange(projDropdown);});
        fovSlider.onValueChanged.AddListener(delegate {onFovChange();});
        nearClipEdit.onValueChanged.AddListener(delegate {onNearClipChange();});
        farClipEdit.onValueChanged.AddListener(delegate {onFarClipChange();});

        // setup default ui values
        nearClipEdit.text = cam.nearClipPlane.ToString();
        farClipEdit.text = cam.farClipPlane.ToString();
        fovSlider.value = cam.fieldOfView;
    }

    private void onProjectionChange(Dropdown dropdown) 
    {
        string selectedProj = dropdown.captionText.text;
        bool isOrtho = selectedProj.Contains(orthoText);
        cam.orthographic = isOrtho;
        fovSlider.interactable = !isOrtho;
    }

    private void onFovChange()
    {
        cam.fieldOfView = fovSlider.value;
    }

    private void onNearClipChange()
    {
        string val = nearClipEdit.text;
        if(validateInput(val))
        {
            cam.nearClipPlane = float.Parse(val);
        }
    }

    private void onFarClipChange()
    {
        string val = farClipEdit.text;
        if(validateInput(val))
        {
            cam.farClipPlane = float.Parse(val);
        }
    }

    private bool validateInput(string input)
    {
        return float.TryParse(input, out _);
    }
}
