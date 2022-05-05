using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagiMaker
{
    public class PassiveEvent_UseSkill : MonoBehaviour
    {
        public delegate void PassiveCallback(EventObject eventObj, int extraInfo);

        public void RegisterPassive(GameObject target, PassiveCallback inventoryPassiveFunc)
        {
            //target.GetComponent<>().event.AddListener(() => inventoryPassiveFunc(this, extraInfo));
        }
    }
}
