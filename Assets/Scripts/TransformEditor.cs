using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformEditor : BaseEditor
{
    public VecEditor posEdit;
    public VecEditor rotEdit;
    public VecEditor scaleEdit;

    protected Transform modeTransform;


    public override void setupGui()
    {   
        // set up the vector gui callbacks
        if(posEdit != null)
        {
            posEdit.vecEditDel += onPosChange;
        }
        if(rotEdit != null)
        {
            rotEdit.vecEditDel += onRotChange;
        }
        if(scaleEdit != null)
        {
            scaleEdit.vecEditDel += onScaleChange;
        }
    }

    public void setupDefaultVecs()
    {
        if(posEdit != null)
        {
            posEdit.setValues(modeTransform.position);
        }

       if(rotEdit != null)
        {
            rotEdit.setValues(modeTransform.eulerAngles);
        }
        
        if(scaleEdit != null)
        {
            scaleEdit.setValues(modeTransform.localScale);
        }
    }

    public void onPosChange(Vector3 posVec)
    {
        modeTransform.position = posVec;
    }

    public void onRotChange(Vector3 rotVec)
    {
        modeTransform.eulerAngles = rotVec;
    }

    public void onScaleChange(Vector3 scaleVec)
    {
        modeTransform.localScale = scaleVec;
    }
}
