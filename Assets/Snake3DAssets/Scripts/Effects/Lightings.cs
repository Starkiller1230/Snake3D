using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Lightings : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private Snake _target;
    [Header("Color")]
    [SerializeField] private Color _walk;
    [SerializeField] private Color _run;
    [SerializeField] private float _speedChangeColor;
    [Header("Trail")]
    [SerializeField] private float _speedChangeTrailLifeTime;
    [SerializeField] private float _trailLifeTimeOnRun;
    [Header("Velociti orbital")]
    [SerializeField] private float _velocityOrbitalY;

    private ParticleSystem.VelocityOverLifetimeModule _velocityOverLifeTimeModule;
    private ParticleSystem.TrailModule _trailModule;
    private ParticleSystem.MainModule _mainModule;
    private Color _defaultColor;
    private float _defaultTrailLifeTime;
    private float _defaultVelocityOrbitalY;
    private float _normalizeRunValueTrail = 0;
    private float _normalizeColorValue = 0;

    private void Start()
    {
        if (_effect != null)
        {
            _mainModule = _effect.main;
            _defaultColor = _mainModule.startColor.color;
            _walk.a = _defaultColor.a;
            _run.a = _defaultColor.a;

            if (_effect.trails.enabled == true)
            {
                _trailModule = _effect.trails;
                _defaultTrailLifeTime = _effect.trails.lifetime.constant;
            }

            if (_effect.velocityOverLifetime.enabled == true)
            {
                _defaultVelocityOrbitalY = _effect.velocityOverLifetime.orbitalY.constant;
                _velocityOverLifeTimeModule = _effect.velocityOverLifetime;
            }
        }
    }

    private float CheckValue(ref float value)
    {
        if (value > 1 || value < 0)
            value = value > 1 ? 1 : 0;
        return value;
    }

    private void FixedUpdate()
    {
        if (_target != null)
        {
            _normalizeRunValueTrail += _speedChangeTrailLifeTime * (_target.MoveStatus == MoveStatus.Run ? Time.deltaTime : -Time.deltaTime);
            _normalizeColorValue += _speedChangeColor * (_target.MoveStatus == MoveStatus.Run ? Time.deltaTime : -Time.deltaTime);

            _trailModule.lifetime = Mathf.Lerp(_defaultTrailLifeTime, _trailLifeTimeOnRun, CheckValue(ref _normalizeRunValueTrail));
            _mainModule.startColor = Color.Lerp(_walk, _run, CheckValue(ref _normalizeColorValue));

            _velocityOverLifeTimeModule.orbitalY = _target.MoveStatus == MoveStatus.Run ? _velocityOrbitalY : _defaultVelocityOrbitalY;

        }
    }
}
