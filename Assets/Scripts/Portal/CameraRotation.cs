using UnityEngine;

namespace Portal
{
    public class CameraRotation : MonoBehaviour
    {
        private Transform _playerCamera;

        [SerializeField] private string portalName, otherPortalName;

        private GameObject _portal, _otherPortal;

        private bool _isPortalsFound;

        private void OnEnable()
        {
            MagicDoorSceneLoader.LoadScene += OnSceneLoad;
        }

        private void OnDisable()
        {
            MagicDoorSceneLoader.LoadScene -= OnSceneLoad;
        }

        private void OnSceneLoad()
        {
            FindPortals();
        }
        
        public void FindPortals()
        {
            _playerCamera = Camera.main.transform;
            Debug.Log($"{name}: Try Find Portals");
            _portal = GameObject.Find(portalName);
            Debug.Log($"{name}: {_portal.name}");
            _otherPortal = GameObject.Find(otherPortalName);
            Debug.Log($"{name}: {_otherPortal.name}");
            _isPortalsFound = true;
        }

        void Update()
        {
            if(!_isPortalsFound) return;
            Vector3 playerOffsetPortal = _playerCamera.position - _otherPortal.transform.position;
            transform.position = _portal.transform.position + playerOffsetPortal;

            float angularDifferenceBetweenPortalRotations = Quaternion.Angle(_portal.transform.rotation, _otherPortal.transform.rotation);

            Quaternion portalRotationalDifference =
                Quaternion.AngleAxis(angularDifferenceBetweenPortalRotations, Vector3.up);
            Vector3 newCameraDirection = portalRotationalDifference * _playerCamera.forward;
            transform.rotation = Quaternion.LookRotation(newCameraDirection, Vector3.up);
        }
    }
}
