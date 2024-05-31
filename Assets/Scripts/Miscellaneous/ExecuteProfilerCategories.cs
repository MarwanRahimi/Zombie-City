using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteProfilerCategories : MonoBehaviour {



    // Start is called before the first frame update
    void Start() {
        ProfilerCategoriesOutput.EnumerateProfilerStats();
    }

    // Update is called once per frame
    void Update() {
        
    }
}
