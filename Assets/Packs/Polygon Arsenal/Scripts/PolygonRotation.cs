using UnityEngine;
using System.Collections;

namespace PolygonArsenal
{
    public class PolygonRotation : MonoBehaviour
    {

        //[Header("Rotate axises by degrees per second")]
        //public Vector3 rotateVector = Vector3.zero;

        //public enum spaceEnum { Local, World };
        //public spaceEnum rotateSpace;

        // Use this for initialization
        public float speed;
        public Vector3 dir;
        void Start()
        {
            StartCoroutine(transIE());
        }

        // Update is called once per frame
        void Update()
        {
            //if (rotateSpace == spaceEnum.Local)
            //    transform.Rotate(rotateVector * Time.deltaTime);
            //if (rotateSpace == spaceEnum.World)
            //    transform.Rotate(rotateVector * Time.deltaTime, Space.World);
            gameObject.transform.Translate(dir * Time.deltaTime * speed);
        }
        IEnumerator transIE()
        {
            while (true)
            {
                yield return new WaitForSeconds(2);
                dir = new Vector3(Random.Range(0,1f),0,Random.Range(0,1f));
             
            }
        }
    }
}