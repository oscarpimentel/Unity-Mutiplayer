using System;
using Unity.Netcode;
using UnityEngine;


public class PlayerController : NetworkBehaviour{
    private Vector2 moveInput;
    public float moveSpeed = 5f; // Speed of player movement
    [SerializeField] public GameObject signPrefab;
    private Animator animator;

    private void Initialize(){
        animator = GetComponent<Animator>();
    }

    public override void OnNetworkSpawn(){
        base.OnNetworkSpawn();
        Initialize();
    }

    private void Update(){
        if (!IsOwner || !Application.isFocused) return;

        // Get movement input
        float moveX = Input.GetAxisRaw("Horizontal"); // A (-1) and D (1)
        float moveY = Input.GetAxisRaw("Vertical");   // W (1) and S (-1)
        moveInput = new Vector2(moveX, moveY).normalized;
        
        // Move the player
        Vector3 velocity = (Vector3)(moveInput * moveSpeed * Time.deltaTime);
        transform.position += velocity;

        // Flip sprite based on movement direction
        if (moveX > 0)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else if (moveX < 0)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }

        // Update animation
        if (moveInput.magnitude > 0)
        {
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        // Spawn a sign when SPACE is pressed
        if (Input.GetKeyDown(KeyCode.Space) && signPrefab != null)
        {
            SpawnSignServerRpc();
        }
    }

    [ServerRpc]
    private void SpawnSignServerRpc()
    {
        GameObject sign = Instantiate(signPrefab, transform.position, Quaternion.identity);
        sign.GetComponent<NetworkObject>().Spawn(); // Spawn it across the network
    }
}
