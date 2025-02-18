using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject cubePrefab;
    [SerializeField]
    private List<Material> materials;
    [SerializeField]
    private Text spawnedCubesUI;

    [SerializeField]
    internal int maxCubes = 10;
    internal int cubeNr;
    /* Arbitrary Xmin/Xmax and Zmin/Zmax to randomize. Ground height */
    private float max = 10f;
    private float min = -10f;
    private float h = 0.5f;
    /* Arbitrary cube limit in scene */
    private int maxCubesOnScreen = 100;

    public void SpawnCube()
    {
        if (cubeNr >= maxCubesOnScreen || cubeNr + transform.childCount > maxCubesOnScreen 
            || transform.childCount >= maxCubesOnScreen)
        {
            Debug.LogWarning("Too many cubes at once. Limit is set to " + maxCubesOnScreen);
            return;
        }
        for (int i = 0; i < cubeNr; i++)
        {
            /* Set this as parent to make it easier to remove the cubes later */
            var cube = Instantiate(cubePrefab, new Vector3(Random.Range(min, max), h, Random.Range(min, max)), 
                Quaternion.identity, transform); 
            cube.GetComponent<MeshRenderer>().material = materials[Random.Range(0, materials.Count)];
        }
        Debug.Log("Spawned " + cubeNr.ToString() + " cubes");
    }

    public void DestroyCubes()
    {
        while (transform.childCount > 0)
        {
            /* Destroy(go) only works in play mode */
            DestroyImmediate(transform.GetChild(0).gameObject); 
        }
        Debug.Log("All cubes removed");
    }

    public void UpdateUI()
    {
        spawnedCubesUI.text = "Spawned cubes: " + transform.childCount;
    }
}
