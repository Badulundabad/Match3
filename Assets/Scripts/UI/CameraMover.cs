using UnityEngine;
using Zenject;

namespace Scripts.UI
{
    public class CameraMover : MonoBehaviour
    {
        public static CameraMover Instance { get; private set; } // need to replace by zenject

        private bool isGoingUp;
        private float speed = 0;
        private float maxSpeed = 2;
        private Camera camera;
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
            camera = GetComponent<Camera>();
            camera.transform.position = bottomPoint.position;
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
            if (Vector3.Distance(camera.transform.position, point.position) < 0.5f)
            {
                camera.transform.position = point.position;
                camera.transform.rotation = point.rotation;
            }
            else
            {
                Vector3 pos = Vector3.Slerp(camera.transform.position, point.position, speed * Time.deltaTime);
                Quaternion rot = Quaternion.Slerp(camera.transform.rotation, point.rotation, speed * Time.deltaTime);
                camera.transform.position = pos;
                camera.transform.rotation = rot;
            }
            return camera.transform.position == point.position;
        }
    }
}