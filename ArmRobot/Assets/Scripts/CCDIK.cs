using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCDIK : MonoBehaviour
{
    /*
     * Must be placed on a game object with a non-root articulation body. 
     */

    public float sqrDistError = 0.01f;
    public int maxIterationCount = 10;

    List<Transform> bones = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        // collect bones
        Transform currentBone = transform;
        bool done = false;
        while (!done)
        {
            bones.Add(currentBone);

            Transform parent = currentBone.parent;
            ArticulationBody parentArticulation = parent.gameObject.GetComponent<ArticulationBody>();

            done = parentArticulation.isRoot;
            currentBone = parent;
        }

        // read out bones to test
        for (int i = 0; i < bones.Count; i++)
        {
            Transform bone = bones[i];
            Debug.Log(bone.gameObject.name);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
