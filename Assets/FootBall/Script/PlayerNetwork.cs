using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerNetwork : NetworkBehaviour {

    private NetworkVariable<Vector3> m_NetPos = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    //private NetworkVariable<Vector3> m_NetVelocity = new NetworkVariable<Vector3>(writePerm: NetworkVariableWritePermission.Owner);
    private NetworkVariable<Quaternion> m_NetRot = new NetworkVariable<Quaternion>(writePerm: NetworkVariableWritePermission.Owner);

    private void Update() {
        if(IsOwner){
            m_NetPos.Value = this.transform.position;
            m_NetRot.Value = this.transform.rotation;
        }else{
            this.transform.position = m_NetPos.Value;
            this.transform.rotation = m_NetRot.Value;
        }
    }

}
