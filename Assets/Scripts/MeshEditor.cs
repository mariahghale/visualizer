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

    private Vector3 meshPos = new Vector3(0.0f, -.5f, 3.0f);
    private Dictionary<string, GameObject> cachedObjects = new Dictionary<string, GameObject>();
    private Material activeMat;
    private GameObject activeMesh;


    public override void setupGui()
    {   
        // populate the asset dropdowns -- please excuse my code duplication
        // i know it's bad
        Object[] texObjs = Resources.LoadAll("FreeSwords/Textures", typeof(Texture2D));
        List<string> texStrs = new List<string>();
        foreach (Object tex in texObjs)
        {   
            // filter out any textures not containing key words
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
                // remove the fbx suffix from the mesh name
                string meshName = mesh.name.Replace("_FBX", "");
                meshes.Add(meshName, (GameObject)mesh);
                meshStrs.Add(meshName);    
            }
        }

        // Sort up those dropdown options! Doesn't handle the numbers well.. TODO!
        matStrs.Sort();
        matDropdown.AddOptions(matStrs);
        meshStrs.Sort();
        meshDropdown.AddOptions(meshStrs);
        texStrs.Sort();
        texDropdown.AddOptions(texStrs);

        // setup callbacks for the asset dropdowns
        meshDropdown.onValueChanged.AddListener(delegate{
            onMeshSelection(meshDropdown);
        });
        matDropdown.onValueChanged.AddListener(delegate{
            onMatSelection(matDropdown);
        });
        texDropdown.onValueChanged.AddListener(delegate{
            onTexSelection(texDropdown);
        });
    
        // Spawn the default options so the user has something to look at
        onMeshSelection(meshDropdown);

        // Popualte the vec fields with the default transform's values
        setupDefaultVecs();

        // Sets up the transform edit delegate schtuff
        base.setupGui();
    }

    private void onMeshSelection(Dropdown dropdown)
    {
        string selectedMesh = dropdown.captionText.text;
        GameObject mesh;

        // if we've already spawned the mesh, get a ref to it
        if (cachedObjects.ContainsKey(selectedMesh))
        {
            mesh = cachedObjects[selectedMesh];
        } 
        else
        {
            // if not, spawn it and cache it
            GameObject meshTemplate = meshes[selectedMesh];
            mesh = Object.Instantiate(meshTemplate, meshPos, Quaternion.identity) as GameObject;
            cachedObjects.Add(selectedMesh, mesh);
        }

        // enable the selected mesh and disable the old
        mesh.SetActive(true);
        if (activeMesh != null)
        {
            activeMesh.SetActive(false);
        }
        activeMesh = mesh;
        // need to update this for the transform editors!
        modeTransform = activeMesh.transform;

        // if there's a new mesh, the mat must be re-applied
        onMatSelection(matDropdown);
    }

    private void onMatSelection(Dropdown dropdown)
    {
        string selectedMat = dropdown.captionText.text;
        Material mat = materials[selectedMat];
        activeMesh.GetComponent<MeshRenderer>().material = mat;
        activeMat = mat;
        onTexSelection(texDropdown);
    }

    private void onTexSelection(Dropdown dropdown)
    {
        string selectedText = dropdown.captionText.text;
        activeMat.mainTexture = textures[selectedText];
    }
}
