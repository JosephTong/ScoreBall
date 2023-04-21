using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    [SerializeField][Range(1,10)] private float m_Speed = 1;
    [SerializeField][Range(1,10)] private float m_Power = 1;
    [SerializeField] private Rigidbody m_Rigidbody;
    void FixedUpdate()
    {
        float rotation = -1;
        bool shouldMove = false;


        if (Input.GetKey(KeyCode.W)){
            shouldMove = true;
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 0;
            }
        }

        if (Input.GetKey(KeyCode.A)){
            shouldMove = true;
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 270;
            }else{
                rotation += rotation == 0? -45:45;
            }
        }

        if (Input.GetKey(KeyCode.S)){
            shouldMove = true;
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 180;
            }else{
                rotation += rotation > 180? -45:45;
            }
        }

        if (Input.GetKey(KeyCode.D)){
            shouldMove = true;
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 90;
            }else{
                rotation += rotation > 90? -45:45;
            }
        }

        this.transform.localEulerAngles = new Vector3(0, rotation ,0);

        if(shouldMove)
            m_Rigidbody.velocity = this.transform.forward * m_Speed;
        
    }
}
