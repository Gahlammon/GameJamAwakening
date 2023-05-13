using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;


public class MenuAdapter : MonoBehaviour
{
    [SerializeField]
    private GameObject text;
    [SerializeField]
    GameObject canvas;
    private string ip;
    private bool valid = false;

    private void Start() 
    {
        text.SetActive(false);
    }
    public void OnValueChanged(string s)
    {
        ip = s;
        Regex validate = new Regex("^(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$");
        if(validate.IsMatch(ip))
        {
            Debug.Log("valid");
            valid = true;
            text.SetActive(false);
        }
        else
        {
            Debug.Log("invalid");
            valid = false;
            text.SetActive(true);
        }
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
        canvas.SetActive(false);
    }

    public void StartClient()
    {
        if(!valid)
        {
            return;
        }
        NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(ip, 7777);
        NetworkManager.Singleton.StartClient();
        canvas.SetActive(false);
    }
}
