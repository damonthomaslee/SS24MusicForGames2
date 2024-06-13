using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{

    public GameObject obj;
    public float xOrg = 0f;
    public float zOrg = 0f;
    public float xNum = 100;
    public float zNum = 100;
    public float heightScale = 10f;
    public float totalScale = 2f;

    public int machineNum = 20;

    public GameObject[] machinePrefabs;


    void Start()
    {
        //GenerateNoise();
        GenerateMachines(machineNum);
    }


    void Update()
    {
    }

    void GenerateNoise()
    {

        for (int z = 0; z < zNum; z++)
        {
            for (int x = 0; x < xNum; x++)
            {
                float xCoord = xOrg - (totalScale * (xNum / 2)) + totalScale * x;
                float zCoord = zOrg - (totalScale * (zNum / 2)) + totalScale * z;
                float sample = Mathf.PerlinNoise(x / xNum, z / zNum);
                GameObject cube = GameObject.Instantiate(obj);
                cube.transform.localScale = new Vector3(totalScale, sample * heightScale * totalScale, totalScale);
                cube.transform.position = new Vector3(xCoord, sample * heightScale * totalScale, zCoord);
            }
        }
    }

    void GenerateMachines(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject machine = GameObject.Instantiate(machinePrefabs[Random.Range(0, machinePrefabs.Length)]);
            machine.transform.position = new Vector3(Random.Range(-95f, 95f), 20f, Random.Range(-95f, 95f));
        }
    }

}
