using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour {

    private NetworkVariable<PlayerNetworkData> m_NetData = new NetworkVariable<PlayerNetworkData>(writePerm: NetworkVariableWritePermission.Owner);
    //private NetworkVariable<Vector3> m_NetVelocity = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);

    private void Update() {
        if(IsOwner){
            m_NetData.Value = new PlayerNetworkData{
                Pos = this.transform.position,
                Rot = this.transform.rotation
            }; 
        }else{
            this.transform.position = m_NetData.Value.Pos;
            this.transform.rotation = m_NetData.Value.Rot;
        }
    }

    public struct PlayerNetworkData : INetworkSerializable {
        public Vector3 Pos;
        public Quaternion Rot;

        void INetworkSerializable.NetworkSerialize<T>(BufferSerializer<T> serializer)
        {
            serializer.SerializeValue(ref Pos);
            serializer.SerializeValue(ref Rot);
        }
    }

}
