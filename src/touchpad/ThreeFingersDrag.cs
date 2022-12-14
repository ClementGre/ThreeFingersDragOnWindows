using System;
using System.Timers;
using Microsoft.VisualBasic.Devices;
using ThreeFingersDragOnWindows.src.utils;

namespace ThreeFingersDragOnWindows.src.touchpad;

public class ThreeFingersDrag {
    private readonly Timer _dragEndTimer = new(50);
    private readonly Timer _oneFingerTimer = new(20);

    private bool _isDragging;

    private ThreeFingersPoints _firstOneFingerPoints = ThreeFingersPoints.Empty;
    private bool _isFirstOneFingerInput;
    private long _firstOneFingerContact;
    private long _lastOneFingerContact;

    
    private ThreeFingersPoints _lastThreeFingersPoints = ThreeFingersPoints.Empty;
    private bool _isFirstThreeFingersInput;
    private long _firstThreeFingersContact;
    private long _lastThreeFingersContact;
    private IntMousePoint _lastDragMousePoint = new IntMousePoint(0, 0);

    public ThreeFingersDrag(){
        // Setup timer
        _dragEndTimer.Elapsed += (_, _) => CheckDragEnd();
        _oneFingerTimer.Elapsed += (_, _) => {
            if (Ctms() - _lastThreeFingersContact > 250){
                stopDrag();
            }
        };
        _dragEndTimer.AutoReset = false;
        _oneFingerTimer.AutoReset = false;
    }

    public void OnTouchpadContact(TouchpadContact[] contacts){
        ThreeFingersPoints points = new(contacts);
        
        if(contacts.Length == 3){
            _isFirstOneFingerInput = true;
            if(_isFirstThreeFingersInput){
                _firstThreeFingersContact = Ctms();
                _isFirstThreeFingersInput = false;
            }
            
            if(!_isDragging){
                if (Ctms() - _firstThreeFingersContact > 100){ // When placing four fingers, a three fingers input can be detected for less than 100ms
                    _isDragging = true;
                    MouseOperations.LeftClick(true);
                }
            }else{
                var dist2d = points.GetLongestDist2D(_lastThreeFingersPoints);
                //var dist2d = new MousePoint(_lastThreeFingersPoints.x1 - points.x1, _lastThreeFingersPoints.y1 - points.y1);
                    
                if (App.Prefs.ThreeFingersMove // Three Fingers drag enabled in preferences
                    && _lastThreeFingersPoints != ThreeFingersPoints.Empty // Last contact is a three fingers drag contact
                    && Math.Sqrt(Math.Pow(dist2d.x, 2) + Math.Pow(dist2d.y, 2)) <= 30){ // Fingers can be released and replaced without catching any one/two finger contact. This makes sure the fingers haven't been replaced on the touchpad
                    
                    float elapsed = _lastThreeFingersContact == 0 ? 0 : Ctms() - _lastThreeFingersContact;
                    
                    // Apply the Mouse Speed preference
                    dist2d.Multiply(App.Prefs.MouseSpeed / 60);

                    // Calculate the mouse velocity
                    var mouseVelocity = (float)Math.Max(0.2, Math.Min(dist2d.Length() / elapsed, 20));
                    if (float.IsNaN(mouseVelocity) || float.IsInfinity(mouseVelocity)) mouseVelocity = 1;

                    // Calculate the pointer velocity in function of the mouse velocity and the preference
                    var pointerVelocity = (float)(App.Prefs.MouseAcceleration / 10 * Math.Pow(mouseVelocity, 2) +
                                                  0.4 * mouseVelocity);
                    pointerVelocity = (float)Math.Max(0.4, Math.Min(pointerVelocity, 1.6)); // Clamp
                    if (App.Prefs.MouseAcceleration == 0) pointerVelocity = 1; // Disable acceleration
                    // Apply acceleration
                    dist2d.Multiply(pointerVelocity);

                    MouseOperations.ShiftCursorPosition(dist2d.x, dist2d.y);
                    
                }

                _lastDragMousePoint = MouseOperations.GetCursorPosition();
                _dragEndTimer.Stop();
                _dragEndTimer.Interval = GetReleaseDelay();
                _dragEndTimer.Start();
            }
            _lastThreeFingersContact = Ctms();
            _lastThreeFingersPoints = points; 
        }
        else{
            _isFirstThreeFingersInput = true;
            if (!_isDragging){
                _isFirstOneFingerInput = true;
                return;
            }

            if(_isFirstOneFingerInput || points.Length != _firstOneFingerPoints.Length || Ctms() - _lastOneFingerContact > 50){
                _isFirstOneFingerInput = false;
                _firstOneFingerContact = Ctms();
                _firstOneFingerPoints = points;
            }
            
            if (!App.Prefs.AllowReleaseAndRestart) stopDrag();
            else if (_firstOneFingerPoints.GetLongestDist(points) > 30){
                // When RELEASING and then REPLACING the fingers, one finger or two can be detected and send some events.
                stopDrag();
            }
            else{
                MouseOperations.SetCursorPosition(_lastDragMousePoint.x, _lastDragMousePoint.y);
            }

            _lastOneFingerContact = Ctms();
            _lastThreeFingersPoints = ThreeFingersPoints.Empty;
        }
    }

    private void CheckDragEnd(){
        // minus 15 to avoid bugs when the timer ends before the time elapsed
        if (_isDragging && Ctms() - _lastThreeFingersContact >= GetReleaseDelay() - 15){
            stopDrag();
        }
    }

    private void stopDrag(){
        _isDragging = false;
        MouseOperations.LeftClick(false);
    }

    private int GetReleaseDelay(){
        return App.Prefs.AllowReleaseAndRestart ? Math.Max(App.Prefs.ReleaseDelay, 50) : 50;
    }

    private long Ctms(){
        return new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
    }

    private float MouseSpeedFactor(){
        return App.Prefs.MouseSpeed / 30;
    }
}