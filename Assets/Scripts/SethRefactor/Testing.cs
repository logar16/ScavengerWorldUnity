using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public class Testing : MonoBehaviour
    {
        public ActionRunner actionRunner;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnAttackEnemyButton()
        {
            actionRunner.SetCurrentAction(ActionType.attackenemy);
        }

        public void OnAttackStorageButton()
        {
            actionRunner.SetCurrentAction(ActionType.attackstorage);
        }

        public void OnGatherButton()
        {
            actionRunner.SetCurrentAction(ActionType.gather);
        }

        public void OnDropoffButton()
        {
            actionRunner.SetCurrentAction(ActionType.dropoff);
        }

        public void OnMoveButton()
        {
            actionRunner.SetCurrentAction(ActionType.move);
        }

        public void OnIdleButton()
        {
            actionRunner.SetCurrentAction(ActionType.none);
        }
    }
}