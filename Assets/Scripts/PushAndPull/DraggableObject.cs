using System;
using UnityEngine;

namespace PushAndPull
{
    public interface IDraggableObjectInterface
    {
        public GameObject GetDraggableObject();
    }

    public class DraggableObject : MonoBehaviour, IDraggableObjectInterface
    {
        private GameObject _draggableObject;
        private void Awake()
        {
            _draggableObject = gameObject;
        }

        public GameObject GetDraggableObject()
        {
            return _draggableObject;
        }
    }
}