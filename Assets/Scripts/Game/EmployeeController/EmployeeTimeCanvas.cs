using System.Collections;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine;

public class EmployeeTimeCanvas : MonoBehaviour
{
    [SerializeField] private Slider timeSlider;
    [Space][SerializeField] private UnityEvent OnSliderStart;
    [Space][SerializeField] private UnityEvent OnSliderComplete;

    private Coroutine _sliderCoroutine;
    private Employee _employee;

    private void Awake()
    {
        _employee = GetComponentInParent<Employee>();

        if (_employee != null)
            _employee.OnTaskTimeChanged += UpdateTime;
    }

    private void OnDisable()
    {
        if (_employee != null)
            _employee.OnTaskTimeChanged -= UpdateTime;

        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);
    }

    private void UpdateTime(float taskTime, float timeMultiplier)
    {
        OnSliderStart?.Invoke();
        timeSlider.maxValue = taskTime;
        timeSlider.value = 0;

        if (_sliderCoroutine != null)
            StopCoroutine(_sliderCoroutine);

        _sliderCoroutine = StartCoroutine(UpdateSlider(taskTime, timeMultiplier));
    }

    private IEnumerator UpdateSlider(float taskTime, float timeMultiplier)
    {
        const float updateInterval = 0.1f;
        float elapsedTime = 0f;

        while (elapsedTime < taskTime)
        {
            elapsedTime += updateInterval * timeMultiplier;
            timeSlider.value = Mathf.Clamp(elapsedTime, 0, taskTime);
            yield return new WaitForSeconds(updateInterval);
        }

        timeSlider.value = taskTime;
        OnSliderComplete?.Invoke();
        _sliderCoroutine = null;
    }
}
