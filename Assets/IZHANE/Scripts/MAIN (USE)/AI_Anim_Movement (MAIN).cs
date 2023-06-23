using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AI_Anim_Movement : MonoBehaviour
{
    [SerializeField] Animator _animator;
    public bool IsPlayerDetected = true;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveMonster();
    }

    void MoveMonster()
    {
        if (IsPlayerDetected)
        {
            //movement code here
            _animator.SetBool("isWalking", true);
        }
        else
        {
            _animator.SetBool("isWalking", false);
        }
    }
}
