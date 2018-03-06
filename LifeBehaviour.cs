using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LifeBehaviour : MonoBehaviour {
    [SerializeField] int lifes;
    [SerializeField] Text lifetext;

    // Use this for initialization
    void Start () { 
    }

    private void Update()
    {
       
    }

    public void UpdateLife()
    {   
        lifes--;
        if(lifes == 0 ) SceneManager.LoadScene("GameOver");
        lifetext.GetComponent<Text>().text = lifes.ToString();
    }
}
