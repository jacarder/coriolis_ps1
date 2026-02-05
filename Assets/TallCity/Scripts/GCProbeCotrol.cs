using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GCProbeCotrol : MonoBehaviour
{
    Animator _Animator;
    float RandomStart;
    float Speed;

    void Awake()
    {
        UnityEngine.Random.InitState((int)System.DateTime.Now.Ticks * 1000);
    }

    // Start is called before the first frame update
    void Start()
    {
        _Animator = GetComponent<Animator>();
        RandomStart = UnityEngine.Random.Range(0f, 2f);
        Speed = UnityEngine.Random.Range(0.3f, 1f);
        StartCoroutine("StartDelay");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartDelay()
    {
        yield return new WaitForSeconds(RandomStart);
        if (UnityEngine.Random.Range(0, 100) < 50)
        {
            _Animator.CrossFade("Base Layer.Action", 0.1f);
        }
        else
        {
            _Animator.CrossFade("Base Layer.Action Reverse", 0.1f);
        }
        _Animator.speed = Speed;
    }
}
