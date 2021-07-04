using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountTimer : MonoBehaviour
{
    [SerializeField]
    private Text cntText;
    [SerializeField]
    private float cntdown = 3f;
    private int cnt;

    private void Start()
    {
        cnt = (int)cntdown;
        cntText.text = cnt.ToString();
    }

    private void Update()
    {
        if (cntdown >= 0)
        {
            cntdown -= Time.deltaTime;
            cnt = (int)cntdown;
            cntText.text = cnt.ToString();
        }
        if (cntdown <= 0)
        {
            gameObject.SetActive(false);
        }
    }

}
