using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionData", menuName = "QuestionData", order = 0)]
public class LectureData : ScriptableObject
{
    public string Question;
    public string[] Answers;
    public int Correct;
}
