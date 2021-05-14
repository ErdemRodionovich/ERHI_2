using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BER_ERHI_c223901b45f74af0a160b6a254574b90
{
    public class Global : MonoBehaviour
    {
        
        public static float bmv = 0.000001f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public static bool Equal(float a, float b)
        {
            return (Mathf.Abs(a - b) < bmv);
        }


    }
}