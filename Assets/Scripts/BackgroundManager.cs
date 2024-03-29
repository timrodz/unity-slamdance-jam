﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackgroundManager : MonoBehaviour {

    public static BackgroundManager Instance { get; private set; }

    public float scale = 10;

    public Transform currentBackground;
    private Material currentBackgroundMat;
    [HideInInspector]
    public Color currentBackgroundColor;

    public Transform middleBackground;
    private Material middleMat;

    private bool firstChange = true;

    private Sequence seq;

    private Tween myTween;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake () {

        if (Instance != null && Instance != this) {
            Destroy (gameObject);
        }

        Instance = this;

        DontDestroyOnLoad (gameObject);

    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start () {

        currentBackgroundMat = currentBackground.GetComponent<Renderer> ().material;
        middleMat = middleBackground.GetComponent<Renderer> ().material;

        seq = DOTween.Sequence ();

    }

    public void ChangeColor (Transform _transform) {

        SmoothKillTween();

        Color objectColor = _transform.GetComponent<Renderer> ().material.GetColor ("_Color");
        currentBackgroundColor = objectColor;

        if (firstChange) {
            middleMat.SetColor ("_Color", Camera.main.backgroundColor);
            firstChange = false;
        } else {
            // Set the middle material to have the color of the current material
            middleMat.SetColor ("_Color", currentBackgroundMat.GetColor ("_Color"));
        }
        middleBackground.localScale = Vector3.one * 5;

        currentBackground.localScale = Vector3.zero;
        currentBackgroundMat.SetColor ("_Color", objectColor);
        currentBackground.position = _transform.position;

        CreateTween(0);

    }

    public void ChangeColor (Vector3 position, Color c, float delay) {

        SmoothKillTween();

        currentBackgroundColor = c;

        if (firstChange) {
            middleMat.SetColor ("_Color", Camera.main.backgroundColor);
            firstChange = false;
        } else {
            // Set the middle material to have the color of the current material
            middleMat.SetColor ("_Color", currentBackgroundMat.GetColor ("_Color"));
        }
        middleBackground.localScale = Vector3.one * 5;

        currentBackground.localScale = Vector3.zero;
        currentBackgroundMat.SetColor ("_Color", c);
        currentBackground.position = position;

        CreateTween(delay);

    }

    public IEnumerator KillAll (float delay) {

        yield return new WaitForSeconds (delay);

        DOTween.KillAll ();

    }

    void CreateTween (float delay) {
        myTween = currentBackground.DOScale (scale, 2).SetDelay(delay);
    }

    void SmoothKillTween () {
        myTween.SetLoops (0);
        myTween.Complete ();
        myTween.Kill (); //?
    }

}