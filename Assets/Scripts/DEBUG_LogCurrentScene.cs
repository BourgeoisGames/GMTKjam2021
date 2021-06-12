using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DEBUG_LogCurrentScene : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
         Debug.Log("current scene: " + SceneManager.GetActiveScene().name);
    }
}
