using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeshEditor : BaseEditor
{
    public GameObject meshDropdown;
    public GameObject matDropdown;
    public GameObject texDropdown;

    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private Dictionary<string, Material> materials = new Dictionary<string, Material>();
    private Dictionary<string, GameObject> meshes = new Dictionary<string, GameObject>();


    public override void setupGui()
    {   
        Object[] texObjs = Resources.LoadAll("FreeSwords/Textures", typeof(Texture2D));
        List<string> texStrs = new List<string>();
        foreach (Object tex in texObjs)
        {   
            bool pickableTexs = tex.name.Contains("Albedo") || tex.name.Contains("Metal");
            if(!this.textures.ContainsKey(tex.name) && pickableTexs)
            {
                this.textures.Add(tex.name, (Texture2D)tex);
                texStrs.Add(tex.name);
            }
        }

        List<string> matStrs = new List<string>();
        Object[] matObjs = Resources.LoadAll("FreeSwords/Materials", typeof(Material));
        foreach (Object mat in matObjs)
        {
            if(!this.materials.ContainsKey(mat.name))
            {
                this.materials.Add(mat.name, (Material)mat);
                matStrs.Add(mat.name);       
            }
        }

        List<string> meshStrs = new List<string>();
        Object[] meshObjs = Resources.LoadAll("FreeSwords/Meshes", typeof(GameObject));
        foreach (Object mesh in meshObjs)
        {
            if(!this.meshes.ContainsKey(mesh.name))
            {
                string meshName = mesh.name.Replace("_FBX", "");
                this.meshes.Add(meshName, (GameObject)mesh);
                meshStrs.Add(meshName);    
            }
        }


        this.meshDropdown.GetComponent<Dropdown>().AddOptions(meshStrs);
        this.matDropdown.GetComponent<Dropdown>().AddOptions(matStrs);
        this.texDropdown.GetComponent<Dropdown>().AddOptions(texStrs);
    }

    // Start is called before the first frame update
    void Start()
    {
    }
}
