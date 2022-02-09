using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MetaBallUI : MonoBehaviour
{
    [SerializeField] MetaBallRenderer render = null;
    [SerializeField] GameObject playButton = null;
    [SerializeField] GameObject stopButton = null;
    [SerializeField] LabeledSlider countSlider = null;
    [SerializeField] LabeledSlider distanceSlider = null;
    [SerializeField] LabeledSlider thicknessSlider = null;
    [SerializeField] Toggle shadedToggle = null;

    private void Start()
    {
        render.SpawnBalls();
        render.Render();

        playButton.SetActive(!render.isPlaying);
        stopButton.SetActive(render.isPlaying);

        countSlider.SetValueWithoutNotify(render.ballCount);
        shadedToggle.SetIsOnWithoutNotify(render.shaded);
        distanceSlider.SetValueWithoutNotify(render.lineDistance);
        thicknessSlider.SetValueWithoutNotify(render.lineThickness);
    }

    public void OnPlayClicked() {
        if (!render.isPlaying) {
            playButton.SetActive(false);
            stopButton.SetActive(true);
            render.Play(true);
        }
    }

    public void OnStopClicked() {
        if (render.isPlaying) {
            playButton.SetActive(true);
            stopButton.SetActive(false);
            render.Play(false);
        }
    }

    public void OnCountSlider() {
        render.SetBallCount((int)countSlider.value);
        if (!render.isPlaying) render.Render();
    }

    public void OnLineDistanceSlider() {
        render.lineDistance = distanceSlider.value;
        render.UpdateShaderSettings();
        if (!render.isPlaying) render.Render();
    }

    public void OnLineThicknessSlider() {
        render.lineThickness = thicknessSlider.value;
        render.UpdateShaderSettings();
        if (!render.isPlaying) render.Render();
    }

    public void OnShadedToggled() {
        render.shaded = shadedToggle.isOn;
        render.UpdateShaderSettings();
        if (!render.isPlaying) render.Render();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
