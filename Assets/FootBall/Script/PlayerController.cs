using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using PlayerNetStruckNamespace;

namespace PlayerNetStruckNamespace
{

    public struct PlayerNetData :INetworkSerializable
    {
        public float Rotation;
        public Vector3 Velocity;
        public Vector3 Position;



        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref Rotation);
            serializer.SerializeValue(ref Velocity);
            serializer.SerializeValue(ref Position);
        }
    }

    public struct BallNetData :INetworkSerializable
    {
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 Position;

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref Rotation);
            serializer.SerializeValue(ref Velocity);
            serializer.SerializeValue(ref Position);
        }
    }
}


public class PlayerController : NetworkBehaviour
{
    [SerializeField] private Rigidbody m_SelfRigidbody;
    [SerializeField][Range(1,10)] private float m_Speed = 1;

    private PlayerNetData m_PlayerNetData;


    public override void OnNetworkSpawn(){
        if(IsOwner){
            this.transform.position = new Vector3(Random.Range(-5,5),2.1f,Random.Range(-5,5));
            m_PlayerNetData = new PlayerNetData{
                Rotation = 0,
                Velocity = Vector3.zero,
                Position = this.transform.position
            };
        }
    }

    private void FixedUpdate() { 
        if(!IsOwner){
            // update using the data given by host
            UpdateMovementByReceiveData();

            return;
        }

        if(IsHost){
            // update ball
            UpdateBallClientRpc(FootBallInGameManager.GetInstacne().getBallData());
        }
            

        MovementHandler();
    }

    private void UpdateMovementByReceiveData(){
        // owner update itself , no need for host
        if(IsOwner)
            return;

        this.transform.localEulerAngles = new Vector3(0, m_PlayerNetData.Rotation ,0);
        this.transform.position = m_PlayerNetData.Position;

        // update physic on host 
        // client will still update Physic if object is owned ( such as player , bullet )
        if(IsHost)
            m_SelfRigidbody.velocity = m_PlayerNetData.Velocity;
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

        m_PlayerNetData = new PlayerNetData{
                Rotation = rotation,
                Velocity = m_SelfRigidbody.velocity,
                Position = this.transform.position
            };

        SentUpdateDataToHostServerRpc(m_PlayerNetData);
    }

    

    [ServerRpc]
    private void SentUpdateDataToHostServerRpc(PlayerNetData playerData){
        // host will do
        // usually call by client 
        UpdateAllClientRpc(playerData);

    }

    [ClientRpc]
    private void UpdateAllClientRpc(PlayerNetData playerData){
        // host will tell all client to do 
        // Only host can use this method

        m_PlayerNetData = playerData;
        UpdateMovementByReceiveData();
    }


    [ClientRpc]
    private void UpdateBallClientRpc(BallNetData ballData){
        FootBallInGameManager.GetInstacne().UpdateBall(ballData);
    }



}
