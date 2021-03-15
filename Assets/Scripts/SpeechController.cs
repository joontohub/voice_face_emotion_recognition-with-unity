using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class SpeechController : MonoBehaviour
{
    public UnityEvent upEvent;
    public UnityEvent downEvent;
    private void OnMouseDown() {
        Debug.Log("Down");
        downEvent?.Invoke();
    }

    private void OnMouseUp() {
        Debug.Log("Up");
        upEvent?.Invoke();
    }
}
