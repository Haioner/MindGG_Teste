using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class EmployeeTimeCanvas : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;
    [Space][SerializeField] private UnityEvent OnSliderStart;
    [Space][SerializeField] private UnityEvent OnSliderComplete;
    private Employee _employee;

    private void Awake()
    {
        _employee = GetComponentInParent<Employee>();
        _employee.OnTaskTimeChanged += UpdateTime;
    }

    private void OnDisable()
    {
        _employee.OnTaskTimeChanged -= UpdateTime;
    }

    private void UpdateTime(float taskTime, float timeMultiplier)
    {
        OnSliderStart?.Invoke();
        timeSlider.maxValue = taskTime;
        timeSlider.value = 0;
        StartCoroutine(UpdateSlider(taskTime, timeMultiplier));
    }

    private IEnumerator UpdateSlider(float taskTime, float timeMultiplier)
    {
        while (timeSlider.value < taskTime)
        {
            timeSlider.value += Time.deltaTime * timeMultiplier;
            yield return null;
        }

        OnSliderComplete?.Invoke();
    }
}
