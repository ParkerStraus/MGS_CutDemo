using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct PlayerData
{
    public bool Walking;
    public bool OnGround;
    public float Attacking;
}

public class PlayerMovement : MonoBehaviour
{
    public Animator animator;
    public Rigidbody rigidbody;
    public GameObject orientation; // Orientation GameObject

    public float HorizontalSpeed= 5;
    public float HorizontalAcc = 8;
    public float VerticalSpeed = 1;

    [SerializeField] private Vector3 moveVelocity; // Changed to Vector3 for 3D
    [SerializeField] private float JumpVelocity;

    [Header("Vertical Handler")]
    public float JumpSpeed = 7;
    public float JumpTime = 1;
    public float JumpTime_Current;
    public bool IsJumping;
    public float FallInterp = 1;

    [SerializeField] private float RideHeight =1;
    [SerializeField] private float RayLength = 2;
    [SerializeField] private float FallSpeed = 5;
    [SerializeField] private float CoyoteTime;
    [SerializeField] private float CoyoteTime_Base = 0.25f;
    public bool OnGround;
    public LayerMask whatIsGround;
    [SerializeField] private PlayerData m_PlayerData;

    void Start()
    {
        m_PlayerData.Attacking = 0;
        m_PlayerData.Walking = false;
        m_PlayerData.OnGround = false;
    }

    void Update()
    {
        HorizontalHandling();
    }

    void FixedUpdate()
    {
        VerticalHandling();
        rigidbody.velocity = new Vector3(moveVelocity.x, JumpVelocity, moveVelocity.z);
    }

    #region Positioning

    void HorizontalHandling()
    {
        /*
        if (!GameHandler.CanThePlayerMove)
        {
            moveVelocity = Vector3.zero;
            m_PlayerData.Walking = false;
            return;
        }*/

        Vector3 control = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        // Transform control direction based on orientation
        control = orientation.transform.TransformDirection(control);
        control.y = 0; // Prevent movement in the vertical direction

        if(CutPath.instance.CutActive) { control = Vector2.zero; }

        moveVelocity = Vector3.Lerp(moveVelocity, control * HorizontalSpeed, HorizontalAcc * Time.deltaTime);

        //m_PlayerData.Walking = true;
    }

    void VerticalHandling()
    {
        float VTarget = 0;
        if (!IsJumping)
        {
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, Vector3.down, out hit, RayLength, whatIsGround))
            {
                Vector3 SnapLocation = new Vector3(transform.position.x, hit.point.y + RideHeight, transform.position.z);
                transform.position = SnapLocation;
                CoyoteTime = CoyoteTime_Base;
                OnGround = true;
                VTarget = 0;
            }
            else
            {
                CoyoteTime -= Time.deltaTime;
                if (CoyoteTime <= 0)
                {
                    OnGround = false;
                    VTarget = -FallSpeed;
                }
            }
        }
        if (IsJumping && JumpTime_Current >= JumpTime)
        {
            IsJumping = false;
        }

        JumpVelocity = Mathf.Lerp(JumpVelocity, VTarget, FallInterp * Time.deltaTime);

        if (OnGround && Input.GetKeyDown(KeyCode.Space))
        {
            OnGround = false;
            JumpVelocity = JumpSpeed;
            IsJumping = true;
            JumpTime_Current = 0;
            CoyoteTime = 0;
        }
        if (IsJumping)
        {
            JumpVelocity = JumpSpeed;
            JumpTime_Current += Time.deltaTime;
        }
        m_PlayerData.OnGround = OnGround;
    }

    #endregion

    #region General Info

    // Additional general methods can be placed here

    #endregion
}
