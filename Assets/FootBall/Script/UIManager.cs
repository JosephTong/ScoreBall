using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Button m_ServerBtn ;
    [SerializeField] private Button m_HostBtn ;
    [SerializeField] private Button m_ClientBtn ;
    
    private void Start() {
        m_ServerBtn.onClick.AddListener(()=>NetworkManager.Singleton.StartServer());
        m_HostBtn.onClick.AddListener(()=>NetworkManager.Singleton.StartHost());
        m_ClientBtn.onClick.AddListener(()=>NetworkManager.Singleton.StartClient());
    }
}
