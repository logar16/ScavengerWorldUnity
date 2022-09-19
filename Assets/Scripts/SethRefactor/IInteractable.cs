using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScavengerWorld
{
    public interface IInteractable
    {
        public Transform Transform { get; }

        public void InitActions();
    }
}