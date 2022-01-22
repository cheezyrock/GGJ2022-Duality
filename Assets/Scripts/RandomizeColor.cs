using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MeshRenderer mr = this.GetComponent<MeshRenderer>();
        mr.material.color = new Color(Random.Range(.2f, .99f), Random.Range(.2f, .99f), Random.Range(.2f, .99f));

        this.GetComponent<RandomizeColor>().enabled = false;
    }
    

}
