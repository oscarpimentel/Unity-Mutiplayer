using System;
using Unity.Netcode;
using UnityEngine;

public class Sign : NetworkBehaviour {
    public float lifetime = 2f; // Duration before despawning the object

    private void Start()
    {
        if (IsServer) // Ensure only the server despawns the object
        {
            Invoke(nameof(DespawnSign), lifetime);
        }
    }

    private void DespawnSign()
    {
        if (IsSpawned)
        {
            NetworkObject.Despawn();
        }
    }
}
