using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collide : MonoBehaviour
{

    void OnTriggerEnter(Collider col)
    {
        int idx = col.gameObject.transform.GetComponent<CellProperties>().text.GetItemNum();
        SpawnManager.spawnedText[idx].score -= 2;
        SpawnManager.spawnedText[idx] = SpawnManager.spawnedText[idx].Destroy();
        TypingManager.score -= 15;
        Destroy(col.gameObject);  
    }
}
