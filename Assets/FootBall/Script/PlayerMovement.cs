using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_SelfRigidbody;
    [SerializeField][Range(1,10)] private float m_Speed = 1;
    [SerializeField] private Button m_ServerBtn ;
    [SerializeField] private Button m_HostBtn ;
    [SerializeField] private Button m_ClientBtn ;


    private void Start() {
        m_ServerBtn.onClick.AddListener(()=>NetworkManager.Singleton.StartServer());
        m_HostBtn.onClick.AddListener(()=>NetworkManager.Singleton.StartHost());
        m_ClientBtn.onClick.AddListener(()=>NetworkManager.Singleton.StartClient());
    }

    public override void OnNetworkSpawn(){
        if(!IsOwner) Destroy(this);
    }

    private void FixedUpdate() { 
        if(!IsOwner)
            return;

        MovementHandler();
    }


    private void MovementHandler(){
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

        if(rotation != -1)
            this.transform.localEulerAngles = new Vector3(0, rotation ,0);

        if(shouldMove)
            m_SelfRigidbody.velocity = this.transform.forward * m_Speed *10;
    }

}
