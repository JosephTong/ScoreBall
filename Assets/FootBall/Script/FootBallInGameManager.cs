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


    private void OnDestroy() {
        m_Instance = null;
    }

    public void UpdateBall(BallNetData ballData){
        m_Data.m_Ball.position = ballData.Position;
        m_Data.m_Ball.rotation = ballData.Rotation;
        m_Data.m_Ball.GetComponent<Rigidbody>().velocity = ballData.Velocity;
    }

    public BallNetData getBallData(){
        return new BallNetData{
            Position = m_Data.m_Ball.position,
            Rotation = m_Data.m_Ball.rotation,
            Velocity = m_Data.m_Ball.GetComponent<Rigidbody>().velocity
        };
    }
}