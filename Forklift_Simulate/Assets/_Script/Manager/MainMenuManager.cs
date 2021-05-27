using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{

    static MainMenuManager m_Instance;
    public static MainMenuManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType(typeof(MainMenuManager)) as MainMenuManager;

                if (m_Instance == null)
                {
                    var gameObject = new GameObject(typeof(MainMenuManager).Name);
                    m_Instance = gameObject.AddComponent(typeof(MainMenuManager)) as MainMenuManager;
                }
            }
            return m_Instance;
        }
    }

    [SerializeField]
    GameObject MainMenuUICanvas;

    GameObject mainMenuUICanvases;
    public GameObject MainMenuUICanvases
    {
        get { return mainMenuUICanvases; }
        set { mainMenuUICanvases = value; }
    }

    /// <summary>
    /// 生成主選單初始物件
    /// </summary>
    public void InstantiateInitObject()
    {
        mainMenuUICanvases = Instantiate(MainMenuUICanvas);
    }
}
