using System.Collections;
using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _movingSpeed;
    [SerializeField] private float _turningSpeed;
    [SerializeField] private float _reverseDistance;    

    public IEnumerator MoveTo(Transform target)
    {
        float distance = Vector3.Distance(transform.position, target.position);
        yield return StartCoroutine(TurnTo(target));
        yield return StartCoroutine(MoveForward(distance));
    }

    public IEnumerator MoveReverse()
    {
        yield return StartCoroutine(Move(Vector3.back, _reverseDistance));
    }

    private IEnumerator MoveForward(float distance)
    {
        yield return StartCoroutine(Move(Vector3.forward, distance));
    }

    private IEnumerator Move(Vector3 direction, float distance)
    {

        while (distance > 0)
        {
            float deltaDistance = _movingSpeed * Time.deltaTime;
            transform.Translate(direction * deltaDistance);
            distance -= deltaDistance;
            yield return null;
        }
    }

    private IEnumerator TurnTo(Transform target)
    {
        float targetYRotation = Quaternion.LookRotation(target.position - transform.position).eulerAngles.y;
        float currentYRotation = transform.rotation.eulerAngles.y;

        while (currentYRotation != targetYRotation)
        {
            currentYRotation = Mathf.MoveTowardsAngle(currentYRotation, targetYRotation, _turningSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, currentYRotation, transform.rotation.eulerAngles.z);
            yield return null;
        }        
    }
}
