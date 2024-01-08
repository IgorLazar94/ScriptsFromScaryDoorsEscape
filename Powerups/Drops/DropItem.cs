using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropItem : MonoBehaviour
{
    protected PlayerCameraController playerCamera;
    protected DropGenerator dropGenerator;

    protected void Start()
    {
        StartCoroutine(DestroyWithDelay());
    }
    public void SetPlayerAndGeneratorToDropItem(PlayerCameraController camera, DropGenerator _dropGenerator)
    {
        playerCamera = camera;
        dropGenerator = _dropGenerator;
    }

    private IEnumerator DestroyWithDelay()
    {
        yield return new WaitForSeconds(15f);
        Destroy(this.gameObject);
    }
}

