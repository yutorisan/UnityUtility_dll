using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Rendering;
using Unity;

namespace UnityUtility.Modules
{
    [Serializable]
    public class SerializeInterface<TInterface>
    {
        [SerializeField]
        private GameObject m_gameobject;

        private TInterface m_interface;

        public TInterface Interface
        {
            get
            {
                if(m_interface == null)
                {
                    m_interface = m_gameobject.GetComponent<TInterface>();
                    if (m_interface == null)
                    {
                        throw new Exception($"GameObject\"{m_gameobject.name}\"は{typeof(TInterface).Name}を実装したコンポーネントをアタッチしていません");
                    }
                }
                return m_interface;
            }
        }
    }

    [Serializable]
    public class SerializeInterfaceCollection<TInterface>
    {
        [SerializeField]
        private List<GameObject> m_gameobjects;

        private IReadOnlyList<TInterface> m_interfaceCollection;

        public IReadOnlyList<TInterface> InterfaceCollection
        {
            get
            {
                if(m_interfaceCollection == null)
                {
                    var interfaces = new List<TInterface>();
                    foreach (var gameobj in m_gameobjects)
                    {
                        var @interface = gameobj.GetComponent<TInterface>();
                        if (@interface == null)
                        {
                            Debug.LogError($"GameObject\"{gameobj.name}\"は{typeof(TInterface).Name}を実装したコンポーネントをアタッチしていません");
                        }
                        else
                        {
                            interfaces.Add(@interface);
                        }
                    }
                    m_interfaceCollection = interfaces;
                }
                return m_interfaceCollection;
            }
        }
    }
}