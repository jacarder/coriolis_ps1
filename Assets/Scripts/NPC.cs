using System;
using System.Collections;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string characterName;
    public AnimationClip idleAnimation;
    public AudioSource audioSource;
    public AudioClip main;
    public Dialogue dialogue;
    public GameObject npcFocusPoint;
    private Quaternion _originalRotation;
    private Coroutine _rotateRoutine;
    void Start()
    {
        PlayMainAudio();
        _originalRotation = transform.rotation;
    }

    public void StopMainAudio()
    {
        audioSource.Stop();
    }

    public void PlayMainAudio()
    {
        audioSource.clip = main;
        audioSource.loop = true;
        audioSource.spatialBlend = 1f;
        audioSource.Play();
    }

    public void StartDialogue()
    {
        dialogue.StartDialogue(this.gameObject);
    }

    public void RotateTowards(Vector3 position)
    {
        StartRotation(GetPositionRotation(position));
    }

    public void ResetRotation()
    {
        StartRotation(_originalRotation);
    }

    private Quaternion GetPositionRotation(Vector3 position)
    {
        Vector3 direction = position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude == 0f)
            return transform.rotation;

        return Quaternion.LookRotation(direction);
    }

    private void StartRotation(Quaternion targetRotation)
    {
        if (_originalRotation != null && _rotateRoutine != null)
            StopCoroutine(_rotateRoutine);

        _rotateRoutine = StartCoroutine(RotateTo(targetRotation));
    }

    private IEnumerator RotateTo(Quaternion targetRotation)
    {
        while (Quaternion.Angle(transform.rotation, targetRotation) > 0.5f)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                360f * Time.deltaTime
            );
            yield return null;
        }

        transform.rotation = targetRotation;
        _rotateRoutine = null;
    }
}
