using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FriendlyProjectile : MonoBehaviour
{
    [SerializeField] CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void MoveProjectile(Vector3 targetPosition)
    {
        print("away!");
        _controller.Move(targetPosition);
    }
}
