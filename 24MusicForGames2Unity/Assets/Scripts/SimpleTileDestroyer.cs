using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTileDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TileManagerStaticClass.DestroyAllTiles();
    }

  
}
