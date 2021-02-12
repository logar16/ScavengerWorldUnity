using UnityEngine;
using System.Collections;

// converted to unity c# by http://unitycoder.com/blog
// original source: http://www.openprocessing.org/sketch/1920

public class HaltonSequence
{
    private Vector3 CurrentPos = new Vector3(0.0f, 0.0f, 0.0f);
    long Base2 = 0;
    long Base3 = 0;
    long Base5 = 0;

    public Vector3 Increment()
    {
        float fOneOver3 = 1.0f / 3.0f;
        float fOneOver5 = 1.0f / 5.0f;

        long oldBase2 = Base2;
        Base2++;
        long diff = Base2 ^ oldBase2;

        float s = 0.5f;

        do
        {
            if ((oldBase2 & 1) == 1)
                CurrentPos.x -= s;
            else
                CurrentPos.x += s;

            s *= 0.5f;

            diff >>= 1;
            oldBase2 >>= 1;
        }
        while (diff > 0);

        long bitmask = 0x3;
        long bitadd = 0x1;
        s = fOneOver3;

        Base3++;

        while (true)
        {
            if ((Base3 & bitmask) == bitmask)
            {
                Base3 += bitadd;
                CurrentPos.y -= 2 * s;

                bitmask <<= 2;
                bitadd <<= 2;

                s *= fOneOver3;
            }
            else
            {
                CurrentPos.y += s;
                break;
            }
        };
        bitmask = 0x7;
        bitadd = 0x3;
        long dmax = 0x5;

        s = fOneOver5;

        Base5++;

        while (true)
        {
            if ((Base5 & bitmask) == dmax)
            {
                Base5 += bitadd;
                CurrentPos.z -= 4 * s;

                bitmask <<= 3;
                dmax <<= 3;
                bitadd <<= 3;

                s *= fOneOver5;
            }
            else
            {
                CurrentPos.z += s;
                break;
            }
        };

        return CurrentPos;
    }

    public Vector3[] GenerateSequence(int length)
    {
        Reset();
        var array = new Vector3[length];
        for (int i = 0; i < length; i++)
        {
            array[i] = Increment();
        }

        return array;
    }

    public void Reset()
    {
        CurrentPos = Vector3.zero;
        Base2 = 0;
        Base3 = 0;
        Base5 = 0;
    }
}