using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.Vehicles
{
    [RequireComponent(typeof(WSMVehicleController))]
    public class WSMVehiclePlayerInput : MonoBehaviour
    {
        LogtichControl logtichControl;

        public bool enablePlayerInput = true;
        public WSMVehicleInputSettings inputSettings;
        public UnityEvent[] customEvents;

        private WSMVehicleController _vehicleController;
        private HeavyMachinery.ForkliftController _forkliftController;

        private float _acceleration = 0f;
        private float _backFront = 0f;
        private float _steering = 0f;
        private float _forksVerticalSpeed = 0;

        bool isChageControlMode = false;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _forkliftController = GetComponent<HeavyMachinery.ForkliftController>();

            _vehicleController = GetComponent<WSMVehicleController>();

            //一開始先存速度
            _forksVerticalSpeed = _forkliftController.forksVerticalSpeed;

            logtichControl = GetComponent<LogtichControl>();

        }

        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                isChageControlMode = !isChageControlMode;
            }


            if (isChageControlMode)
            {
                OnEditTest();
            }
            else
            {
                if (enablePlayerInput)
                {
                    LogichUse();
                }
            }

      
        }


        void LogichUse()
        {
            if (inputSettings == null) return;

            #region Vehicle Controls

            //前進後退檔控制+-
            Debug.Log("logtichControl.BackMove" + logtichControl.BackMove);
            if (logtichControl.BackMove) _backFront = -1;
            if (!logtichControl.FrontMove && !logtichControl.BackMove) _backFront = 0;
            if (logtichControl.FrontMove) _backFront = 1;
            _vehicleController.BackFrontInput = _backFront;

            //控制油門
            _vehicleController.AccelerationInput = logtichControl.LogitchGasRotation;

            //方向
            _steering = 0f;
            _vehicleController.SteeringInput = logtichControl.LogitchSteelRotation;

            //煞車
            _vehicleController.BrakesInput = logtichControl.LogitchBreakRotation;

            if (Input.GetKey(inputSettings.handbrakeOn)) _vehicleController.HandBrakeInput = 1;
            if (Input.GetKey(inputSettings.handbrakeOff)) _vehicleController.HandBrakeInput = 0;

            //吋動踏板
            _vehicleController.ClutchInput = logtichControl.LogitchCluthRotation;
            if (logtichControl.LogitchCluthRotation > 10) _forkliftController.forksVerticalSpeed = _forksVerticalSpeed * 2;
            else _forkliftController.forksVerticalSpeed = _forksVerticalSpeed;



            if (Input.GetKeyDown(inputSettings.toggleEngine))
                _vehicleController.IsEngineOn = !_vehicleController.IsEngineOn;

            if (Input.GetKeyDown(inputSettings.horn))
                _vehicleController.Horn();

            if (Input.GetKeyDown(inputSettings.headlights))
                _vehicleController.HeadlightsOn = !_vehicleController.HeadlightsOn;

            if (Input.GetKeyDown(inputSettings.interiorLights))
                _vehicleController.InteriorLightsOn = !_vehicleController.InteriorLightsOn;

            if (Input.GetKeyDown(inputSettings.leftSignalLights))
                _vehicleController.LeftSinalLightsOn = !_vehicleController.LeftSinalLightsOn;

            if (Input.GetKeyDown(inputSettings.rightSignalLights))
                _vehicleController.RightSinalLightsOn = !_vehicleController.RightSinalLightsOn;

            if (Input.GetKey(inputSettings.cameraLookDown))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Down;
            else if (Input.GetKey(inputSettings.cameraLookBack))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Backwards;
            else if (Input.GetKey(inputSettings.cameraLookRight))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Right;
            else if (Input.GetKey(inputSettings.cameraLookLeft))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Left;
            else if (Input.GetKey(inputSettings.cameraLookUp))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Up;
            else
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Forward;

            if (Input.GetKeyDown(inputSettings.toggleCamera))
                _vehicleController.CameraToggleRequested = true;

            #endregion

            #region Player Custom Events

            for (int i = 0; i < inputSettings.customEventTriggers.Length; i++)
            {
                if (Input.GetKeyDown(inputSettings.customEventTriggers[i]))
                {
                    if (customEvents.Length > i)
                        customEvents[i].Invoke();
                }
            }

            #endregion
        }


        void OnEditTest()
        {

            //前進後退檔控制+-
            if (Input.GetKey(inputSettings.backMove)) _backFront = 1;
            if (Input.GetKey(inputSettings.nullMove)) _backFront = 0;
            if (Input.GetKey(inputSettings.frontMove)) _backFront = -1;
            _vehicleController.BackFrontInput = _backFront;

            //控制油門
            _acceleration = Input.GetKey(inputSettings.acceleration) ? 1f : 0;
            _acceleration = Input.GetKey(inputSettings.reverse) ? _acceleration - 1 : _acceleration; //倒退改打檔
            _vehicleController.AccelerationInput = _acceleration;

            //方向
            _steering = 0f;
            _steering = Input.GetKey(inputSettings.turnRight) ? _steering + 450 : _steering;
            _steering = Input.GetKey(inputSettings.turnLeft) ? _steering - 450 : _steering;
            _vehicleController.SteeringInput = _steering;

            //煞車
            _vehicleController.BrakesInput = Input.GetKey(inputSettings.brakes) ? 25f : 0f;

            if (Input.GetKey(inputSettings.handbrakeOn)) _vehicleController.HandBrakeInput = 1;
            if (Input.GetKey(inputSettings.handbrakeOff)) _vehicleController.HandBrakeInput = 0;

            //吋動踏板
            _vehicleController.ClutchInput = Input.GetKey(inputSettings.clutch) ? 25f : 0f;
            if (Input.GetKey(inputSettings.clutch)) _forkliftController.forksVerticalSpeed = _forksVerticalSpeed * 2;
            else _forkliftController.forksVerticalSpeed = _forksVerticalSpeed;



            if (Input.GetKeyDown(inputSettings.toggleEngine))
                _vehicleController.IsEngineOn = !_vehicleController.IsEngineOn;

            if (Input.GetKeyDown(inputSettings.horn))
                _vehicleController.Horn();

            if (Input.GetKeyDown(inputSettings.headlights))
                _vehicleController.HeadlightsOn = !_vehicleController.HeadlightsOn;

            if (Input.GetKeyDown(inputSettings.interiorLights))
                _vehicleController.InteriorLightsOn = !_vehicleController.InteriorLightsOn;

            if (Input.GetKeyDown(inputSettings.leftSignalLights))
                _vehicleController.LeftSinalLightsOn = !_vehicleController.LeftSinalLightsOn;

            if (Input.GetKeyDown(inputSettings.rightSignalLights))
                _vehicleController.RightSinalLightsOn = !_vehicleController.RightSinalLightsOn;

            if (Input.GetKey(inputSettings.cameraLookDown))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Down;
            else if (Input.GetKey(inputSettings.cameraLookBack))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Backwards;
            else if (Input.GetKey(inputSettings.cameraLookRight))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Right;
            else if (Input.GetKey(inputSettings.cameraLookLeft))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Left;
            else if (Input.GetKey(inputSettings.cameraLookUp))
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Up;
            else
                _vehicleController.CamLookDirection = WSMVehicleCameraLookDirection.Forward;

            if (Input.GetKeyDown(inputSettings.toggleCamera))
                _vehicleController.CameraToggleRequested = true;


            #region Player Custom Events

            for (int i = 0; i < inputSettings.customEventTriggers.Length; i++)
            {
                if (Input.GetKeyDown(inputSettings.customEventTriggers[i]))
                {
                    if (customEvents.Length > i)
                        customEvents[i].Invoke();
                }
            }

            #endregion
        }

    } 
}
