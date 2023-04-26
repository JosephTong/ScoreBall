using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using PlayerNetStruckNamespace;
using FootBallNameSpace;

namespace PlayerNetStruckNamespace
{

    public struct PlayerNetData :INetworkSerializable
    {
        public float Rotation;
        public Vector3 Position;

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref Rotation);
            serializer.SerializeValue(ref Position);
        }
    }
    
    public struct BallData :INetworkSerializable
    {
        public Vector3 Position;
        public Vector3 Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        
        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Rotation);
            serializer.SerializeValue(ref Velocity);
            serializer.SerializeValue(ref AngularVelocity);
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
            this.transform.position = new Vector3((int)Random.Range(-5,5),2.1f,(int)Random.Range(-5,5));

            FootBallInGameManager.GetInstacne().SetMainPlayer(this.transform);
            m_PlayerNetData= new PlayerNetData{
                Rotation = -1,
                Position = this.transform.position
            };
            
            SentUpdateDataToHostServerRpc(m_PlayerNetData);
        }
    }

    private void FixedUpdate() { 

        if(IsOwner){        
            if(IsHost){
                UpdatBallPosServerRpc(FootBallInGameManager.GetInstacne().GetBallData());
            }
            // input
            InputHandler();
        }

        MovementHandler();
        
    }

    private void MovementHandler(){
        // teleport if too far away from owner pos
        if(Vector3.Distance(this.transform.position , m_PlayerNetData.Position) >0.5f)
            this.transform.position = m_PlayerNetData.Position;

        if(m_PlayerNetData.Rotation != -1){
            this.transform.localEulerAngles = new Vector3(0, m_PlayerNetData.Rotation ,0);
            m_SelfRigidbody.velocity = this.transform.forward * m_Speed *10;
        }

    }


    private void InputHandler(){
        float rotation = -1;

        if (Input.GetKey(KeyCode.W)){
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 0;
            }
        }

        if (Input.GetKey(KeyCode.A)){
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 270;
            }else{
                rotation += rotation == 0? -45:45;
            }
        }

        if (Input.GetKey(KeyCode.S)){
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 180;
            }else{
                rotation += rotation > 180? -45:45;
            }
        }

        if (Input.GetKey(KeyCode.D)){
            if( rotation == -1 ){
                // no other movement btn were clicked
                rotation = 90;
            }else{
                rotation += rotation > 90? -45:45;
            }
        }

        m_PlayerNetData= new PlayerNetData{
            Rotation = rotation,
            Position = this.transform.position
        };
        SentUpdateDataToHostServerRpc(m_PlayerNetData);
    }

    

    [ServerRpc]
    private void SentUpdateDataToHostServerRpc(PlayerNetData playerData){
        // host will do
        // usually call by client 
        UpdatePlayerDataClientRpc(playerData);

    }

    [ServerRpc]
    private void UpdatBallPosServerRpc(BallData ballData){
        UpdatBallPosClientRpc( ballData );
    }    

    [ClientRpc]
    private void UpdatePlayerDataClientRpc(PlayerNetData playerData){
        // host will tell all client to do 
        // Only host can use this method

        m_PlayerNetData = playerData;
    }

    
    
    [ClientRpc]
    private void UpdatBallPosClientRpc(BallData ballData){
        if(Vector3.Distance(ballData.Position , FootBallInGameManager.GetInstacne().GetBallData().Position)>0.5f){
            FootBallInGameManager.GetInstacne().UpdateBall(ballData);
        }
    }


}
