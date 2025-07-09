using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneHelper : MonoBehaviour
{
    public void ChangeSceneTo(string nameScene) => SceneManager.LoadScene(nameScene);
}
