using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

public class NetworkCommandLine : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;

    void Start()
    {
        canvas.SetActive(false);

        // if (Application.isEditor) return;

        var args = GetCommandlineArgs();

        if (args.TryGetValue("-m", out string mode))
        {
            switch (mode)
            {
                case "server":
                    NetworkManager.Singleton.StartServer();
                    break;
                case "host":
                    NetworkManager.Singleton.StartHost();
                    break;
                case "client":
                    args.TryGetValue("-a", out string address);
                    NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(address, 7777);
                    NetworkManager.Singleton.StartClient();
                    break;
            }
        }
        else
        {
            canvas.SetActive(true);
        }
    }

    private Dictionary<string, string> GetCommandlineArgs()
    {
        Dictionary<string, string> argDictionary = new Dictionary<string, string>();

        var args = System.Environment.GetCommandLineArgs();

        for (int i = 0; i < args.Length; ++i)
        {
            var arg = args[i].ToLower();
            if (arg.StartsWith("-"))
            {
                var value = i < args.Length - 1 ? args[i + 1].ToLower() : null;
                value = (value?.StartsWith("-") ?? false) ? null : value;

                argDictionary.Add(arg, value);
            }
        }
        return argDictionary;
    }
}