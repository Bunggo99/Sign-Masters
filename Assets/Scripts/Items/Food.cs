using UnityEngine;

public class Food : MonoBehaviour
{
    #region Variables

    [SerializeField] private RangedNum<int> points;
    [SerializeField] private AudioClip eatSound1;
    [SerializeField] private AudioClip eatSound2;

    #endregion

    #region Properties

    public RangedNum<int> Points => points;
    public AudioClip EatSound1 => eatSound1;
    public AudioClip EatSound2 => eatSound2;

    #endregion
}
