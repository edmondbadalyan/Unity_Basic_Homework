using UnityEngine;

namespace Scipts
{
    public class FriendOrEnemy : MonoBehaviour
    {
        public MeshRenderer meshRenderer;
        public bool IsEnemy;
        
        private void Update()
        {
            meshRenderer.material.color = IsEnemy ? Color.red : Color.blue;
        }
    }
}