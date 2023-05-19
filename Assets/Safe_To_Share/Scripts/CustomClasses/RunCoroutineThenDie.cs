using System.Collections;
using UnityEngine;

namespace Safe_to_Share.Scripts.CustomClasses
{
    public sealed class RunCoroutineThenDie : MonoBehaviour
    {
        public void Run(IEnumerator routine)
        {
            DontDestroyOnLoad(gameObject);
            StartCoroutine(RunThenDie(routine));
        }

        IEnumerator RunThenDie(IEnumerator routine)
        {
            yield return routine;
            yield return new WaitForEndOfFrame();
            Destroy(gameObject);
        }
    }
}