using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset : MonoBehaviour
{
    public void ResetEnvironment()
    {
        SceneManager.LoadScene(0);
    }
}
