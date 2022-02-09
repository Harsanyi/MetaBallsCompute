using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaBall
{
    public float mass { get; private set; } = 0f;
    public float radious { get; private set; } = 0f;
    public Vector2 pos = default;
    public Vector2 velocity = default;

    public MetaBall() {
        AddMass(mass);
    }

    public void AddMass(float amount) {
        mass += amount;

        //needs to recalculate radious which is calculated as a halfSphere;
        radious = Mathf.Pow((6f * mass) / (4 * Mathf.PI), 0.5f);
    }

    public static explicit operator BallData(MetaBall ball) {
        return new BallData() {
            pos =ball.pos, radious=ball.radious
        };
    }

    public void Move(float time) {
        pos += time * velocity;

        if (pos.x < 0 || pos.x > Screen.width) velocity.x *= -1;
        if (pos.y < 0 || pos.y > Screen.height) velocity.y *= -1;
    }
}

public struct BallData
{
    public Vector2 pos;
    public float radious;

    public static int size => sizeof(float)*3;
}
