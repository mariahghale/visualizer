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

    void Start()
    {
        cam = Camera.main;
        camObj = cam.gameObject;

        nearClipEdit.text = cam.nearClipPlane.ToString();
        farClipEdit.text = cam.farClipPlane.ToString();
    }

    public override void setupGui()
    {   
        this.projDropdown.onValueChanged.AddListener(delegate{
            this.onProjectionChange(this.projDropdown);
        });

        fovSlider.onValueChanged.AddListener(delegate {onFovChange();});
        nearClipEdit.onValueChanged.AddListener(delegate {onNearClipChange(); });
        farClipEdit.onValueChanged.AddListener(delegate {onFarClipChange(); });
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
        cam.nearClipPlane = float.Parse(nearClipEdit.text);
    }

    private void onFarClipChange()
    {
        cam.farClipPlane = float.Parse(farClipEdit.text);
    }
}
