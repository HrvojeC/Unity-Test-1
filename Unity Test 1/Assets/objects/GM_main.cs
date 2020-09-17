using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_main : MonoBehaviour
{
    public GameObject[] boxPrefabs;

    public float boxCreationRate = 2.0f;

    private float boxLastCreation;

    // Update is called once per frame
    void Update()
    {
        if (Time.time - boxLastCreation > boxCreationRate) CreateBox();
    }

    private void CreateBox ()
    {
        boxLastCreation = Time.time;
        int i = Mathf.FloorToInt(Random.Range(0.0f, 2.0f));
        Vector3 newPosition = new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(2.5f, 9.0f), 0);
        GameObject newBox = Instantiate(boxPrefabs[i], newPosition, Quaternion.identity) as GameObject;
        newBox.name = boxPrefabs[i].name;
    }
}
