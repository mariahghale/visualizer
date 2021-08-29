using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VecEditor : MonoBehaviour
{
    public InputField xEdit;
    public InputField yEdit;
    public InputField zEdit;

    public delegate void VecChangedDelegate(Vector3 vec);
    public VecChangedDelegate vecEditDel;
    

    void Start()
    {
        xEdit.onValueChanged.AddListener(delegate {onEdit(); });
        yEdit.onValueChanged.AddListener(delegate {onEdit(); });
        zEdit.onValueChanged.AddListener(delegate {onEdit(); });
    }

    private void onEdit()
    {
        // query up all the values and return a vec 3 to those who care
        string x =  xEdit.text;
        string y =  yEdit.text;
        string z =  zEdit.text;
        if (validateInput(x) && validateInput(y) && validateInput(z))
        {
            vecEditDel(new Vector3(float.Parse(x) , float.Parse(y), float.Parse(z)));
        }
    }

    private bool validateInput(string input)
    {
        return float.TryParse(input, out _);
    }
}
