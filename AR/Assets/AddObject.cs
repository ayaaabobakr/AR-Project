using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore.Examples.ObjectManipulation;

public class AddObject : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject prefab;
    public GameObject ObjectGenerator;
    public GameObject closePanel;
    PawnManipulator manipulator;
    public string url;
    public string prefabName;

    void Start()
    {
        manipulator = ObjectGenerator.GetComponent<PawnManipulator>();

    }

    public void viewInSpace()
    {
        manipulator.chosenPrefab = true;
        manipulator.PawnPrefab = prefab;
        // yield return StartCoroutine(GetAssetBundle());
        // StartCoroutine(manipulator.GetAssetBundle());
        closePanel.GetComponent<openPanel>().OpenPanel();
    }




}
