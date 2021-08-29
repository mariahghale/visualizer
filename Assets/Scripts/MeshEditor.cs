using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeshEditor : BaseEditor
{
    public Dropdown meshDropdown;
    public Dropdown matDropdown;
    public Dropdown texDropdown;

    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private Dictionary<string, Material> materials = new Dictionary<string, Material>();
    private Dictionary<string, GameObject> meshes = new Dictionary<string, GameObject>();

    private Dictionary<string, GameObject> cachedObjects = new Dictionary<string, GameObject>();
    private GameObject activeMesh = null;
    private Vector3 meshPos = new Vector3(0.0f, -.8f, 3.0f);


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

        this.meshDropdown.AddOptions(meshStrs);
        this.meshDropdown.onValueChanged.AddListener(delegate{
            this.onMeshSelection(this.meshDropdown);
        });
        this.matDropdown.AddOptions(matStrs);
        this.matDropdown.onValueChanged.AddListener(delegate{
            this.onMatSelection(this.matDropdown);
        });

        this.texDropdown.AddOptions(texStrs);
    }


    private void onMeshSelection(Dropdown dropdown)
    {
        // hide the old mesh
        string selectedMesh = dropdown.captionText.text;
        GameObject mesh;

        // is this cached already? just set active
        if (this.cachedObjects.ContainsKey(selectedMesh))
        {
            mesh = this.cachedObjects[selectedMesh];
        } else
        {
            GameObject meshTemplate = this.meshes[selectedMesh];
            mesh = Object.Instantiate(meshTemplate, meshPos, Quaternion.identity) as GameObject;
            this.cachedObjects.Add(selectedMesh, mesh);
        }

        mesh.SetActive(true);
        if (activeMesh != null)
        {
            activeMesh.SetActive(false);
        }
        activeMesh = mesh;
    }

    private void onMatSelection(Dropdown dropdown)
    {
        string selectedMat = dropdown.captionText.text;
        Material mat = this.materials[selectedMat];
        activeMesh.GetComponent<MeshRenderer>().material = mat;
    }

    private void onTexSelection()
    {


    }


    // Start is called before the first frame update
    void Start()
    {
    }
}
