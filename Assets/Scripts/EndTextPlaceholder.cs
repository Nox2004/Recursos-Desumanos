using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTextPlaceholder : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI fp_text;
    [SerializeField] private TMPro.TextMeshProUGUI corp_text;
    [SerializeField] private TMPro.TextMeshProUGUI moral_text;

    // Start is called before the first frame update
    void Start()
    {
        fp_text.text = Singleton.Instance.future_points.ToString();
        corp_text.text = Singleton.Instance.corporate_points.ToString();
        moral_text.text = Singleton.Instance.moral_points.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
