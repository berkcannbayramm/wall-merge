using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BBGameStudios.Managers
{
    public class Intro : MonoBehaviour
    {
        [SerializeField] private string sceneName;

        private void Start()
        {
            StartCoroutine(OpenGameTimer());
        }
        IEnumerator OpenGameTimer()
        {
            yield return new WaitForSecondsRealtime(4f);
            SceneManager.LoadScene(sceneName);
        }
    }
}
