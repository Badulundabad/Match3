using UnityEngine;
using Zenject;

namespace Match3.UI
{
    public class CameraMover : MonoBehaviour
    {
        public static CameraMover Instance { get; private set; } // need to replace by zenject

        private bool isGoingUp;
        private float speed = 0;
        private float maxSpeed = 2;
        private Camera mainCamera;
        private Transform bottomPoint;
        private Transform upperPoint;

        public bool IsReady { get; private set; }

        [Inject]
        private void Construct([Inject(Id = "bottom")] Transform bottomPoint, [Inject(Id = "upper")] Transform upperPoint)
        {
            this.bottomPoint = bottomPoint;
            this.upperPoint = upperPoint;
        }

        private void Start()
        {
            Instance = this;
            mainCamera = GetComponent<Camera>();
            mainCamera.transform.position = bottomPoint.position;
        }

        void Update()
        {
            if (IsReady) return;

            speed += Time.deltaTime;
            speed = Mathf.Clamp(speed, 0, maxSpeed);

            IsReady = isGoingUp ? GoToPoint(upperPoint) : GoToPoint(bottomPoint);
        }

        public void GoUp()
        {
            speed = 0;
            isGoingUp = true;
            IsReady = false;
        }

        public void GoDown()
        {
            speed = 0;
            isGoingUp = false;
            IsReady = false;
        }

        private bool GoToPoint(Transform point)
        {
            if (Vector3.Distance(mainCamera.transform.position, point.position) < 0.5f)
            {
                mainCamera.transform.position = point.position;
                mainCamera.transform.rotation = point.rotation;
            }
            else
            {
                Vector3 pos = Vector3.Slerp(mainCamera.transform.position, point.position, speed * Time.deltaTime);
                Quaternion rot = Quaternion.Slerp(mainCamera.transform.rotation, point.rotation, speed * Time.deltaTime);
                mainCamera.transform.position = pos;
                mainCamera.transform.rotation = rot;
            }
            return mainCamera.transform.position == point.position;
        }
    }
}