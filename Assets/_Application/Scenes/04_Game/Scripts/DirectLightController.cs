using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

public class DirectLightController : MonoBehaviour
{
    const float MIN_ANGLE = 20.0f;
    const float MAX_ANGLE = 40.0f;

    float angle = MIN_ANGLE;

    float rotspeed = 1.75f;

    enum MoveMode
    {
        Up,
        Down
    }

    MoveMode mode = MoveMode.Up;

    // Start is called before the first frame update
    void Start()
    {
        this.UpdateAsObservable()
            .Where(_ => mode == MoveMode.Up)
            .Subscribe(_ =>
            {
                angle += Time.deltaTime * rotspeed;
                if (angle > MAX_ANGLE)
                {
                    angle = MAX_ANGLE;
                    mode = MoveMode.Down;
                }
                this.transform.localRotation = Quaternion.Euler(angle, this.transform.localEulerAngles.y, 0);
            })
            .AddTo(this);

        this.UpdateAsObservable()
            .Where(_ => mode == MoveMode.Down)
            .Subscribe(_ =>
            {
                angle -= Time.deltaTime * rotspeed;
                if (angle < MIN_ANGLE)
                {
                    angle = MIN_ANGLE;
                    mode = MoveMode.Up;
                }
                this.transform.localRotation = Quaternion.Euler(angle, this.transform.localEulerAngles.y, 0);
            })
            .AddTo(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
