using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace KoitanLib
{
    public class KoitanDebug : MonoBehaviour
    {
        private static KoitanDebug instance;
        [SerializeField]
        private DebugTextManager dtm;
        [SerializeField]
        private GameObject mainCanvasObj;
        [SerializeField]
        bool defaultCanvasActiveSelf;
        // Start is called before the first frame update
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this);
                DontDestroyOnLoad(dtm);
                mainCanvasObj.SetActive(defaultCanvasActiveSelf);
            }
            else
            {
                Destroy(this);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || (Input.touchCount == 5 && Input.touches[4].phase == TouchPhase.Began))
            {
                mainCanvasObj.SetActive(!mainCanvasObj.activeSelf);
            }
        }

        [Conditional("KOITAN_DEBUG")]
        public static void Display(string str)
        {
            instance.dtm.Display(str);
        }

        [Conditional("KOITAN_DEBUG")]
        public static void DisplayBox(string str, MonoBehaviour mono)
        {
            instance.dtm.DisplayBox(str, mono);
        }

        [Conditional("KOITAN_DEBUG")]
        public static void DrawCircle(Vector3 center, float radius, Color color, float duration = 0f)
        {
            int cnt = 24;
            Vector3 start = center + new Vector3(radius, 0);
            Vector3 end = start;
            for (int i = 1; i < cnt + 1; i++)
            {
                start = end;
                end = center + new Vector3(radius * Mathf.Cos(Mathf.PI * 2 * i / cnt), radius * Mathf.Sin(Mathf.PI * 2 * i / cnt));
                UnityEngine.Debug.DrawLine(start, end, color, duration);
            }
        }
    }
}