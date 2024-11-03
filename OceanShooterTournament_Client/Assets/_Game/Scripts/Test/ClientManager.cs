using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientManager : MonoSingleton<ClientManager>
{
    public UDPClient client;
    private void Start()
    {
        client = new UDPClient(OnHandleResponse);

    }
    public void OnHandleResponse(string data)
    {
        Debug.Log(data);
        string[] dataArr = data.Split(';');

        string idRole = dataArr[0];
        string action = dataArr[1];
        if(idRole.Equals("client"))
        {
            string userID = dataArr[2];
            if(userID.Contains("user"))
            {
                TestManager.instance.UpdateStateColor(true);
                TestManager.instance.UpdateUserID(userID);
            }
        }
        else if(idRole.Contains("user")) {
            if(action.Equals("logout")) {
                TestManager.instance.UpdateStateColor(false);
                TestManager.instance.UpdateUserID(null);
            }
        }
    }

    private void Update()
    {
        client.ProcessMessages();
    }

    

}
