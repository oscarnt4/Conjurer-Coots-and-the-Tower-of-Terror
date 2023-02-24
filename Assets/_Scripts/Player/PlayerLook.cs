using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    [SerializeField] float xSensitivity = 30f;
    [SerializeField] float ySensitivity = 30f;
    private Camera _camera;
    private float xRotation = 0f;

    private void Start()
    {
        _camera = Camera.main;
    }

    public void Look(Vector2 input)
    {
        //calculate camera rotation for looking up and down
        xRotation -= (input.y * Time.deltaTime) * ySensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //apply locally to camera rotation
        _camera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate player body left and right
        transform.Rotate(Vector3.up * (input.x * Time.deltaTime) * xSensitivity);
    }
}
