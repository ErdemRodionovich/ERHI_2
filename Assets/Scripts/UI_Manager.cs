using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Manager : MonoBehaviour
{

    [SerializeField] private GameObject menu;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI cicleText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMenuButtonClicked()
    {
        menu.SetActive(true);
    }

}
