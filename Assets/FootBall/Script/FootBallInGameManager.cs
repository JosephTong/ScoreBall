using System.Collections;
using System.Collections.Generic;
using FootBallNameSpace;
using PlayerNetStruckNamespace;
using UnityEngine;


namespace FootBallNameSpace
{   
    [System.Serializable]
    public class FootBallHostData
    {
        public Transform m_Ball;
        public Transform m_MainPlayer;
        public Transform m_MainCamera;
        public Vector3 m_CameraOffset;
    }

}

public class FootBallInGameManager : MonoBehaviour
{
    private static FootBallInGameManager m_Instance;
    [SerializeField] private FootBallHostData m_Data;

    public static FootBallInGameManager GetInstacne(){
        if(!m_Instance){
            m_Instance = new GameObject("FootBallInGameManager").AddComponent<FootBallInGameManager>();
        }
        return m_Instance;
    }

    private void Awake() {
        if(m_Instance){
            Destroy(this);
        }else{
            m_Instance = this;
        }
    }

    private void FixedUpdate() {
        CameraFollowPlayer();
    }

    public void CameraFollowPlayer(){
        if(m_Data.m_MainPlayer){
            m_Data.m_MainCamera.transform.position = m_Data.m_MainPlayer.position + m_Data.m_CameraOffset;
        }
    }

    public void SetMainPlayer(Transform player){
        m_Data.m_MainPlayer = player;
    }


    private void OnDestroy() {
        m_Instance = null;
    }

    public BallData GetBallData(){
        return new BallData{
            Position = m_Data.m_Ball.position,
            Rotation = m_Data.m_Ball.eulerAngles,
            Velocity = m_Data.m_Ball.GetComponent<Rigidbody>().velocity,
            AngularVelocity = m_Data.m_Ball.GetComponent<Rigidbody>().angularVelocity
        };
    }

    public void UpdateBall(BallData ballData){
        m_Data.m_Ball.position = ballData.Position;
        m_Data.m_Ball.rotation = Quaternion.Euler(ballData.Rotation);
        m_Data.m_Ball.TryGetComponent<Rigidbody>(out var ballRigidbody);
        ballRigidbody.velocity = ballData.Velocity;
        ballRigidbody.angularVelocity = ballData.AngularVelocity;
    }
}
