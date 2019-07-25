using UnityEngine;
using System.Collections;

public class Easing
{

    public enum EaseType
    {
        easeInQuad,
        easeOutQuad,
        easeInOutQuad,
        easeInCubic,
        easeOutCubic,
        easeInOutCubic,
        easeInQuart,
        easeOutQuart,
        easeInOutQuart,
        easeInQuint,
        easeOutQuint,
        easeInOutQuint,
        easeInSine,
        easeOutSine,
        easeInOutSine,
        easeInExpo,
        easeOutExpo,
        easeInOutExpo,
        easeInCirc,
        easeOutCirc,
        easeInOutCirc,
        linear,
        spring,
        easeInBounce,
        easeOutBounce,
        easeInOutBounce,
        easeInBack,
        easeOutBack,
        easeInOutBack,
        easeInElastic,
        easeOutElastic,
        easeInOutElastic,
        /*punch*/
    }

    /// <summary>
    /// The type of the lopp.
    /// </summary>
    public enum LoopType
    {
        none,
        loop,
        pingPong
    }

    public delegate float EasingFunction(float start, float end, float Value);

    /// <summary>
    /// <summary>
    /// The type of the loop.
    /// </summary>
    [Tooltip("The type of the loop")] public LoopType loop;

    [System.NonSerialized] bool m_IsRunning = false;
    [System.NonSerialized] bool m_Finished = false;
    [System.NonSerialized] bool m_Reverse = false;
    [System.NonSerialized] float m_Timer = 0f;

    [System.NonSerialized]
    //protected EasingFunction m_EaseFunction;
    //[System.NonSerialized]
    protected float m_Percentage = 0f;

    [System.NonSerialized] protected float[] m_From, m_To, m_Result;


    public static EasingFunction GetEasingFunction(EaseType easeType)
    {
        EasingFunction mEaseFunction;
        switch (easeType)
        {
            case EaseType.easeInQuad:
                mEaseFunction = new EasingFunction(EaseInQuad);
                break;
            case EaseType.easeOutQuad:
                mEaseFunction = new EasingFunction(EaseOutQuad);
                break;
            case EaseType.easeInOutQuad:
                mEaseFunction = new EasingFunction(EaseInOutQuad);
                break;
            case EaseType.easeInCubic:
                mEaseFunction = new EasingFunction(EaseInCubic);
                break;
            case EaseType.easeOutCubic:
                mEaseFunction = new EasingFunction(EaseOutCubic);
                break;
            case EaseType.easeInOutCubic:
                mEaseFunction = new EasingFunction(EaseInOutCubic);
                break;
            case EaseType.easeInQuart:
                mEaseFunction = new EasingFunction(EaseInQuart);
                break;
            case EaseType.easeOutQuart:
                mEaseFunction = new EasingFunction(EaseOutQuart);
                break;
            case EaseType.easeInOutQuart:
                mEaseFunction = new EasingFunction(EaseInOutQuart);
                break;
            case EaseType.easeInQuint:
                mEaseFunction = new EasingFunction(EaseInQuint);
                break;
            case EaseType.easeOutQuint:
                mEaseFunction = new EasingFunction(EaseOutQuint);
                break;
            case EaseType.easeInOutQuint:
                mEaseFunction = new EasingFunction(EaseInOutQuint);
                break;
            case EaseType.easeInSine:
                mEaseFunction = new EasingFunction(EaseInSine);
                break;
            case EaseType.easeOutSine:
                mEaseFunction = new EasingFunction(EaseOutSine);
                break;
            case EaseType.easeInOutSine:
                mEaseFunction = new EasingFunction(EaseInOutSine);
                break;
            case EaseType.easeInExpo:
                mEaseFunction = new EasingFunction(EaseInExpo);
                break;
            case EaseType.easeOutExpo:
                mEaseFunction = new EasingFunction(EaseOutExpo);
                break;
            case EaseType.easeInOutExpo:
                mEaseFunction = new EasingFunction(EaseInOutExpo);
                break;
            case EaseType.easeInCirc:
                mEaseFunction = new EasingFunction(EaseInCirc);
                break;
            case EaseType.easeOutCirc:
                mEaseFunction = new EasingFunction(EaseOutCirc);
                break;
            case EaseType.easeInOutCirc:
                mEaseFunction = new EasingFunction(EaseInOutCirc);
                break;
            case EaseType.linear:
                mEaseFunction = new EasingFunction(Linear);
                break;
            case EaseType.spring:
                mEaseFunction = new EasingFunction(Spring);
                break;
            case EaseType.easeInBounce:
                mEaseFunction = new EasingFunction(EaseInBounce);
                break;
            case EaseType.easeOutBounce:
                mEaseFunction = new EasingFunction(EaseOutBounce);
                break;
            case EaseType.easeInOutBounce:
                mEaseFunction = new EasingFunction(EaseInOutBounce);
                break;
            case EaseType.easeInBack:
                mEaseFunction = new EasingFunction(EaseInBack);
                break;
            case EaseType.easeOutBack:
                mEaseFunction = new EasingFunction(EaseOutBack);
                break;
            case EaseType.easeInOutBack:
                mEaseFunction = new EasingFunction(EaseInOutBack);
                break;
            case EaseType.easeInElastic:
                mEaseFunction = new EasingFunction(EaseInElastic);
                break;
            case EaseType.easeOutElastic:
                mEaseFunction = new EasingFunction(EaseOutElastic);
                break;
            case EaseType.easeInOutElastic:
                mEaseFunction = new EasingFunction(EaseInOutElastic);
                break;
            default:
                mEaseFunction = Linear;
                break;
        }

        return mEaseFunction;
    }


    #region Easing Functions

    /*
    TERMS OF USE - EASING EQUATIONS
    Open source under the BSD License.
    Copyright (c)2001 Robert Penner
    All rights reserved.
    Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
    Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
    Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
    Neither the name of the author nor the names of contributors may be used to endorse or promote products derived from this software without specific prior written permission.
    THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
    */

    public static float Linear(float start, float end, float value)
    {
        return Mathf.Lerp(start, end, value);
    }

    public static float Clerp(float start, float end, float value)
    {
        float min = 0.0f;
        float max = 360.0f;
        float half = Mathf.Abs((max - min) * 0.5f);
        float retval = 0.0f;
        float diff = 0.0f;
        if ((end - start) < -half)
        {
            diff = ((max - start) + end) * value;
            retval = start + diff;
        }
        else if ((end - start) > half)
        {
            diff = -((max - end) + start) * value;
            retval = start + diff;
        }
        else retval = start + (end - start) * value;

        return retval;
    }

    public static float Spring(float start, float end, float value)
    {
        value = Mathf.Clamp01(value);
        value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) +
                 value) * (1f + (1.2f * (1f - value)));
        return start + (end - start) * value;
    }

    public static float EaseInQuad(float start, float end, float value)
    {
        end -= start;
        return end * value * value + start;
    }

    public static float EaseOutQuad(float start, float end, float value)
    {
        end -= start;
        return -end * value * (value - 2) + start;
    }

    public static float EaseInOutQuad(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value + start;
        value--;
        return -end * 0.5f * (value * (value - 2) - 1) + start;
    }

    public static float EaseInCubic(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value + start;
    }

    public static float EaseOutCubic(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value + 1) + start;
    }

    public static float EaseInOutCubic(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value + 2) + start;
    }

    public static float EaseInQuart(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value + start;
    }

    public static float EaseOutQuart(float start, float end, float value)
    {
        value--;
        end -= start;
        return -end * (value * value * value * value - 1) + start;
    }

    public static float EaseInOutQuart(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value * value + start;
        value -= 2;
        return -end * 0.5f * (value * value * value * value - 2) + start;
    }

    public static float EaseInQuint(float start, float end, float value)
    {
        end -= start;
        return end * value * value * value * value * value + start;
    }

    public static float EaseOutQuint(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * (value * value * value * value * value + 1) + start;
    }

    public static float EaseInOutQuint(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * value * value * value * value * value + start;
        value -= 2;
        return end * 0.5f * (value * value * value * value * value + 2) + start;
    }

    public static float EaseInSine(float start, float end, float value)
    {
        end -= start;
        return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
    }

    public static float EaseOutSine(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
    }

    public static float EaseInOutSine(float start, float end, float value)
    {
        end -= start;
        return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
    }

    public static float EaseInExpo(float start, float end, float value)
    {
        end -= start;
        return end * Mathf.Pow(2, 10 * (value - 1)) + start;
    }

    public static float EaseOutExpo(float start, float end, float value)
    {
        end -= start;
        return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
    }

    public static float EaseInOutExpo(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
        value--;
        return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
    }

    public static float EaseInCirc(float start, float end, float value)
    {
        end -= start;
        return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
    }

    public static float EaseOutCirc(float start, float end, float value)
    {
        value--;
        end -= start;
        return end * Mathf.Sqrt(1 - value * value) + start;
    }

    public static float EaseInOutCirc(float start, float end, float value)
    {
        value /= .5f;
        end -= start;
        if (value < 1) return -end * 0.5f * (Mathf.Sqrt(1 - value * value) - 1) + start;
        value -= 2;
        return end * 0.5f * (Mathf.Sqrt(1 - value * value) + 1) + start;
    }

    public static float EaseInBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        return end - EaseOutBounce(0, end, d - value) + start;
    }

    public static float EaseOutBounce(float start, float end, float value)
    {
        value /= 1f;
        end -= start;
        if (value < (1 / 2.75f))
        {
            return end * (7.5625f * value * value) + start;
        }
        else if (value < (2 / 2.75f))
        {
            value -= (1.5f / 2.75f);
            return end * (7.5625f * (value) * value + .75f) + start;
        }
        else if (value < (2.5 / 2.75))
        {
            value -= (2.25f / 2.75f);
            return end * (7.5625f * (value) * value + .9375f) + start;
        }
        else
        {
            value -= (2.625f / 2.75f);
            return end * (7.5625f * (value) * value + .984375f) + start;
        }
    }

    public static float EaseInOutBounce(float start, float end, float value)
    {
        end -= start;
        float d = 1f;
        if (value < d * 0.5f) return EaseInBounce(0, end, value * 2) * 0.5f + start;
        else return EaseOutBounce(0, end, value * 2 - d) * 0.5f + end * 0.5f + start;
    }

    public static float EaseInBack(float start, float end, float value)
    {
        end -= start;
        value /= 1;
        float s = 1.70158f;
        return end * (value) * value * ((s + 1) * value - s) + start;
    }

    public static float EaseOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value = (value) - 1;
        return end * ((value) * value * ((s + 1) * value + s) + 1) + start;
    }

    public static float EaseInOutBack(float start, float end, float value)
    {
        float s = 1.70158f;
        end -= start;
        value /= .5f;
        if ((value) < 1)
        {
            s *= (1.525f);
            return end * 0.5f * (value * value * (((s) + 1) * value - s)) + start;
        }

        value -= 2;
        s *= (1.525f);
        return end * 0.5f * ((value) * value * (((s) + 1) * value + s) + 2) + start;
    }

    public static float Punch(float amplitude, float value)
    {
        float s;
        if (value == 0)
        {
            return 0;
        }
        else if (value == 1)
        {
            return 0;
        }

        float period = 1 * 0.3f;
        s = period / (2 * Mathf.PI) * Mathf.Asin(0);
        return (amplitude * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * 1 - s) * (2 * Mathf.PI) / period));
    }

    public static float EaseInElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return -(a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) + start;
    }

    public static float EaseOutElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d) == 1) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p * 0.25f;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        return (a * Mathf.Pow(2, -10 * value) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) + end + start);
    }

    public static float EaseInOutElastic(float start, float end, float value)
    {
        end -= start;

        float d = 1f;
        float p = d * .3f;
        float s = 0;
        float a = 0;

        if (value == 0) return start;

        if ((value /= d * 0.5f) == 2) return start + end;

        if (a == 0f || a < Mathf.Abs(end))
        {
            a = end;
            s = p / 4;
        }
        else
        {
            s = p / (2 * Mathf.PI) * Mathf.Asin(end / a);
        }

        if (value < 1)
            return -0.5f * (a * Mathf.Pow(2, 10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p)) +
                   start;
        return a * Mathf.Pow(2, -10 * (value -= 1)) * Mathf.Sin((value * d - s) * (2 * Mathf.PI) / p) * 0.5f + end +
               start;
    }

    #endregion Easing Functions


    #region Apply Functions

    protected void ApplyPunch()
    {
        // x
        if (m_To[0] > 0)
        {
            m_Result[0] = Punch(m_To[0], m_Percentage);
        }
        else if (m_To[0] < 0)
        {
            m_Result[0] = -Punch(Mathf.Abs(m_To[0]), m_Percentage);
        }

        // y
        if (m_To[1] > 0)
        {
            m_Result[1] = Punch(m_To[1], m_Percentage);
        }
        else if (m_To[1] < 0)
        {
            m_Result[1] = -Punch(Mathf.Abs(m_To[1]), m_Percentage);
        }

        // z
        if (m_To[2] > 0)
        {
            m_Result[2] = Punch(m_To[2], m_Percentage);
        }
        else if (m_To[2] < 0)
        {
            m_Result[2] = -Punch(Mathf.Abs(m_To[2]), m_Percentage);
        }
    }

    protected void ApplyShake()
    {
        float diminishingControl = 1 - m_Percentage;
        m_Result[0] = UnityEngine.Random.Range(-m_To[0] * diminishingControl, m_To[0] * diminishingControl);
        m_Result[1] = UnityEngine.Random.Range(-m_To[1] * diminishingControl, m_To[1] * diminishingControl);
        m_Result[2] = UnityEngine.Random.Range(-m_To[2] * diminishingControl, m_To[2] * diminishingControl);
    }

    #endregion Apply Functions
}