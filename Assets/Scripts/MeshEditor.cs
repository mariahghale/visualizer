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

    private Vector3 meshPos = new Vector3(0.0f, -.8f, 3.0f);
    private Dictionary<string, GameObject> cachedObjects = new Dictionary<string, GameObject>();
    private GameObject activeMesh = null;
    private Material activeMat = null;


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

        this.texDropdown.onValueChanged.AddListener(delegate{
            this.onTexSelection(this.texDropdown);
        });
    
        this.texDropdown.AddOptions(texStrs);

        this.texDropdown.value = 2;
        this.matDropdown.value = 2;
        this.meshDropdown.value = 2;
        this.onMeshSelection(this.meshDropdown);
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

        this.onMatSelection(this.matDropdown);
    }

    private void onMatSelection(Dropdown dropdown)
    {
        string selectedMat = dropdown.captionText.text;
        Material mat = this.materials[selectedMat];
        if (this.activeMesh != null)
        {
            this.activeMesh.GetComponent<MeshRenderer>().material = mat;
        }
        this.activeMat = mat;
        this.onTexSelection(this.texDropdown);
    }

    private void onTexSelection(Dropdown dropdown)
    {
        string selectedText = dropdown.captionText.text;
        Texture2D texture = this.textures[selectedText];
        
        if (this.activeMesh != null)
        {
            Material material = this.activeMesh.GetComponent<MeshRenderer>().material;
            Debug.Log(material);
            Debug.Log(material.name);
            material.mainTexture = texture;

        }
    }
}
