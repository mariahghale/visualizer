using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CamEditor : BaseEditor
{

    public override void setupGui()
    {   
    }



    private void onOrthoChange()
    {
        bool isOrtho = false;
        this.gameObject.GetComponent<Camera>().orthographic = isOrtho;
    }
}
