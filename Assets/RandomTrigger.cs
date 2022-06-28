using UnityEngine;
using System.Collections;

sealed class RandomTrigger : MonoBehaviour
{
    [SerializeField] FlashGlitch _target = null;
    [SerializeField] int _index = 0;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(Mathf.Pow(0.5f, Random.Range(1, 4)));
            _target.TriggerEffect(_index);
        }
    }
}
