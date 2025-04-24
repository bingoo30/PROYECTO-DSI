using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using ChartLibrary;
using System.Threading.Tasks;
using System;

namespace UIToolkitDemo
{
    // Shows the player's total experience level (the sum of all four character levels)
    public class LevelMeterView : UIView
    {

        // time to show radial progress bar
        const float k_LerpTime = 1f;

        RadialCounter m_LevelMeterCounter;
        Label m_LevelMeterNumber;

        public LevelMeterView(VisualElement topElement): base(topElement)
        {
            // Listen for character level changes
            CharEvents.LevelUpdated += OnLevelUpdated;
        }

        public override void Dispose()
        {
            base.Dispose();
            CharEvents.LevelUpdated -= OnLevelUpdated;
        }


        protected override void SetVisualElements()
        {
            base.SetVisualElements();

            m_LevelMeterCounter = m_TopElement.Q<RadialCounter>("level-meter__counter");
            m_LevelMeterNumber = m_TopElement.Q<Label>("level-meter__number");
        }

        // Level Meter
        void OnLevelUpdated(float progress)
        {

            // Fire and forget, discard the resulting Task
            _ = UpdateLevelAsync(progress, k_LerpTime);

        }

        // Using async await instead of coroutines to update the progress bar for non-MonoBehaviour
        async Task UpdateLevelAsync(float targetValue, float lerpTime)
        {
            try
            {
                float t = 0;
                float originalValue = m_LevelMeterCounter.progress;
                float tolerance = 0.05f;
                m_LevelMeterNumber.text = targetValue.ToString();

                // Use a stopwatch to calculate elapsed time
                System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                while (Mathf.Abs(m_LevelMeterCounter.progress - targetValue) > tolerance)
                {
                    // Calculate elapsed time using stopwatch
                    float elapsedTime = (float)stopwatch.Elapsed.TotalSeconds;

                    // Calculate t based on the elapsed time and lerpTime
                    t = elapsedTime / lerpTime;

                    m_LevelMeterCounter.progress = Mathf.Lerp(originalValue, targetValue, t);

                    // Pause and yield control back to the Unity's main thread scheduler to allow other tasks to update
                    await Task.Yield();
                }

                m_LevelMeterCounter.progress = targetValue;
                stopwatch.Stop();
            }
            catch (Exception ex)
            {
                Debug.LogError($"[LevelMeterView] UpdateLevelAsync error: {ex.Message}");
            }
        }

    }
}
