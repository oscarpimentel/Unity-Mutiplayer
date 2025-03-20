using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerSettings : NetworkBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    NetworkVariable<FixedString32Bytes> networkPlayerName = new NetworkVariable<FixedString32Bytes>("unknown", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()  // called each time a new client spawn
    {
        //Debug.Log("new player");
        if (IsOwner){
            networkPlayerName.Value = GameObject.Find("UIManager").GetComponent<UIManager>().nameInput.text;  // we defined the value as the text added to the input field
        }
        playerName.text = networkPlayerName.Value.ToString();  // render name text
        networkPlayerName.OnValueChanged += networkPlayerName_OnValueChanged; // subscribe to a method
    }

    void networkPlayerName_OnValueChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        //Debug.Log(playerName.text.ToString() + "->" + newValue.Value.ToString());
        playerName.text = newValue.Value;  // when the name change, we set
    }
}
