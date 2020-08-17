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

        public TInterface Interface
        {
            get
            {
                var @interface = m_gameobject.GetComponent<TInterface>();
                if (@interface == null)
                {
                    throw new Exception($"GameObject\"{m_gameobject.name}\"は{typeof(TInterface).Name}を実装したコンポーネントをアタッチしていません");
                }
                return @interface;
            }
        }
    }

    [Serializable]
    public class SerializeInterfaceCollection<TInterface>
    {
        [SerializeField]
        private List<GameObject> m_gameobjects;

        public List<TInterface> InterfaceCollection
        {
            get
            {
                var interfaceList = new List<TInterface>();
                foreach (var gameobj in m_gameobjects)
                {
                    var @interface = gameobj.GetComponent<TInterface>();
                    if (@interface == null)
                    {
                        UnityEngine.Debug.Log($"GameObject\"{gameobj.name}\"は{typeof(TInterface).Name}を実装したコンポーネントをアタッチしていません");
                    }
                    else
                    {
                        interfaceList.Add(@interface);
                    }
                }
                return interfaceList;
            }
        }
    }
}