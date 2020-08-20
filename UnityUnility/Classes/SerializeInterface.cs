using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UnityUtility
{
    [Serializable]
    public class SerializeInterface<TInterface>
    {
        [SerializeField]
        private GameObject m_gameobject;

        private static TInterface component;

        public TInterface Interface
        {
            get
            {
                if(component == null)
                {
                    component = m_gameobject.GetComponent<TInterface>();
                    if (component == null)
                    {
                        throw new Exception($"GameObject\"{m_gameobject.name}\"は{typeof(TInterface).Name}を実装したコンポーネントをアタッチしていません");
                    }
                }
                return component;
            }
        }
    }

    [Serializable]
    public class SerializeInterfaceCollection<TInterface>
    {
        [SerializeField]
        private List<GameObject> m_gameobjects;

        private static IReadOnlyList<TInterface> components;

        public IReadOnlyList<TInterface> InterfaceCollection
        {
            get
            {
                if(components == null)
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
                    components = interfaces;
                }
                return components;
            }
        }
    }
}