using System;

[Serializable]
public struct RangedNum<T>
{
    #region Variables

    public T Min;
    public T Max;

    #endregion

    #region Constructor

    public RangedNum(T min, T max)
    {
        Min = min;
        Max = max;
    }

    #endregion
}