using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeshEditor : TransformEditor
{
    public Dropdown meshDropdown;
    public Dropdown matDropdown;
    public Dropdown texDropdown;

    private Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D>();
    private Dictionary<string, Material> materials = new Dictionary<string, Material>();
    private Dictionary<string, GameObject> meshes = new Dictionary<string, GameObject>();

    private Vector3 meshPos = new Vector3(0.0f, -.8f, 3.0f);
    private Dictionary<string, GameObject> cachedObjects = new Dictionary<string, GameObject>();
    private Material activeMat;
    private GameObject activeMesh;


    public void setupDefaults()
    {
       onMeshSelection(meshDropdown);
       setupDefaultVecs();
    }

    public override void setupGui()
    {   
        // populate the asset dropdowns
        Object[] texObjs = Resources.LoadAll("FreeSwords/Textures", typeof(Texture2D));
        List<string> texStrs = new List<string>();
        foreach (Object tex in texObjs)
        {   
            bool pickableTexs = tex.name.Contains("Albedo") || tex.name.Contains("Metal");
            if(!textures.ContainsKey(tex.name) && pickableTexs)
            {
                textures.Add(tex.name, (Texture2D)tex);
                texStrs.Add(tex.name);
            }
        }

        List<string> matStrs = new List<string>();
        Object[] matObjs = Resources.LoadAll("FreeSwords/Materials", typeof(Material));
        foreach (Object mat in matObjs)
        {
            if(!materials.ContainsKey(mat.name))
            {
                materials.Add(mat.name, (Material)mat);
                matStrs.Add(mat.name);       
            }
        }

        List<string> meshStrs = new List<string>();
        Object[] meshObjs = Resources.LoadAll("FreeSwords/Meshes", typeof(GameObject));
        foreach (Object mesh in meshObjs)
        {
            if(!meshes.ContainsKey(mesh.name))
            {
                string meshName = mesh.name.Replace("_FBX", "");
                meshes.Add(meshName, (GameObject)mesh);
                meshStrs.Add(meshName);    
            }
        }

        // setup callbacks for the asset dropdowns
        matStrs.Sort();
        meshStrs.Sort();
        texStrs.Sort();
        meshDropdown.AddOptions(meshStrs);
        meshDropdown.onValueChanged.AddListener(delegate{
            onMeshSelection(meshDropdown);
        });
        matDropdown.AddOptions(matStrs);
        matDropdown.onValueChanged.AddListener(delegate{
            onMatSelection(matDropdown);
        });

        texDropdown.onValueChanged.AddListener(delegate{
            onTexSelection(texDropdown);
        });
    
        texDropdown.AddOptions(texStrs);

        setupDefaults();
        base.setupGui();
    }


    private void onMeshSelection(Dropdown dropdown)
    {
        // hide the old mesh
        string selectedMesh = dropdown.captionText.text;
        GameObject mesh;

        // is this cached already? just set active
        if (cachedObjects.ContainsKey(selectedMesh))
        {
            mesh = cachedObjects[selectedMesh];
        } else
        {
            GameObject meshTemplate = meshes[selectedMesh];
            mesh = Object.Instantiate(meshTemplate, meshPos, Quaternion.identity) as GameObject;
            cachedObjects.Add(selectedMesh, mesh);
        }

        mesh.SetActive(true);
        if (activeMesh != null)
        {
            activeMesh.SetActive(false);
        }
        activeMesh = mesh;
        modeTransform = activeMesh.transform;

        onMatSelection(matDropdown);
    }

    private void onMatSelection(Dropdown dropdown)
    {
        string selectedMat = dropdown.captionText.text;
        Material mat = materials[selectedMat];
        if (activeMesh != null)
        {
            activeMesh.GetComponent<MeshRenderer>().material = mat;
        }
        activeMat = mat;
        onTexSelection(texDropdown);
    }

    private void onTexSelection(Dropdown dropdown)
    {
        string selectedText = dropdown.captionText.text;
        Texture2D texture = textures[selectedText];
        
        if (activeMesh != null)
        {
            Material material = activeMesh.GetComponent<MeshRenderer>().material;
            material.mainTexture = texture;

        }
    }
}
