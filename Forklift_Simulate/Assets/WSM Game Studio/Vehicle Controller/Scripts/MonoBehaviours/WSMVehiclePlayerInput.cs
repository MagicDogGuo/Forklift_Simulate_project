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
        private float _forksHorizontalSpeed = 0;
        private float _mastTiltSpeed = 0;
        float _maxSpeed = 0;

        float onGasOnlyInFrontBackSpeedLimit = 2f;//沒踩油門入檔的最高速 2022/04/29降速，2022/07/01升速



        bool isChageControlMode = false;


        bool _CheckSeatBeltBool = false;

        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _forkliftController = GetComponent<HeavyMachinery.ForkliftController>();

            _vehicleController = GetComponent<WSMVehicleController>();

            //一開始先存速度
            _forksVerticalSpeed = _forkliftController.forksVerticalSpeed;
            _forksHorizontalSpeed = _forkliftController.forksHorizontalSpeed;
            _mastTiltSpeed = _forkliftController.mastTiltSpeed;

            //行駛最大速度
            _maxSpeed = _vehicleController.MaxSpeed;

            logtichControl = GetComponent<LogtichControl>();

            //安全帶
            _vehicleController.seatBelts[0].SetActive(true);
            _vehicleController.seatBelts[1].SetActive(false);

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
            //Debug.Log("logtichControl.BackMove" + logtichControl.BackMove);
            //if (logtichControl.BackMove) _backFront = -1;
            //if (!logtichControl.FrontMove && !logtichControl.BackMove) _backFront = 0;
            //if (logtichControl.FrontMove) _backFront = 1;
            //if (Input.GetKey(inputSettings.backMove)) _backFront = -1;
            //if (Input.GetKey(inputSettings.nullMove)) _backFront = 0;
            //if (Input.GetKey(inputSettings.frontMove)) _backFront = 1;
            if (GetAllJoysEvent.FrontBar_btn6 ||Input.GetKey(inputSettings.backMove)) _backFront = -1;
            if (!GetAllJoysEvent.FrontBar_btn5 && !GetAllJoysEvent.FrontBar_btn6) _backFront = 0;
            if (GetAllJoysEvent.FrontBar_btn5|| Input.GetKey(inputSettings.frontMove)) _backFront = 1;/////////////////////////////////


            _vehicleController.BackFrontInput = _backFront;

            //方向
            _steering = 0f;
            _vehicleController.SteeringInput = logtichControl.LogitchSteelRotation;

            //煞車
            _vehicleController.BrakesInput = logtichControl.LogitchBreakRotation;

            //手煞車
            //if (Input.GetKey(inputSettings.handbrakeOn)) _vehicleController.HandBrakeInput = 1;
            //if (Input.GetKey(inputSettings.handbrakeOff)) _vehicleController.HandBrakeInput = 0;
            if (GetAllJoysEvent.HandBrake_btn4) _vehicleController.HandBrakeInput = 0;
            else if (!GetAllJoysEvent.HandBrake_btn4) _vehicleController.HandBrakeInput = 1;




            //控制油門(前進檔自動滑動)
            if (_backFront == 1 || _backFront == -1)
            {
                //是否踩油門
                if (logtichControl.LogitchGasRotation > 10)
                {
                    //踩油門
                    _vehicleController.AccelerationInput = logtichControl.LogitchGasRotation;
                    //_vehicleController.MaxSpeed = _maxSpeed;
                }
                else
                {

                    //入檔時的速度
                    if (_vehicleController.CurrentSpeed > onGasOnlyInFrontBackSpeedLimit)
                    {
                        _vehicleController.AccelerationInput = 0f;
                    }
                    else
                    {
                        //沒踩油門時由這來控致剎車，踩煞車不同角度有不同的加速度
                        if (logtichControl.LogitchBreakRotation < 5)
                        {
                            _vehicleController.BrakesInput = 0;
                            _vehicleController.AccelerationInput = 0.3f;
                        }
                        else if (logtichControl.LogitchBreakRotation < 10)
                        {
                            _vehicleController.BrakesInput = 0;
                            _vehicleController.AccelerationInput = 0.05f;
                        }
                        //else if (logtichControl.LogitchBreakRotation >= 10)
                        //{
                        //    _vehicleController.BrakesInput = logtichControl.LogitchBreakRotation;
                        //}
                    }

                    //if (logtichControl.LogitchBreakRotation > 5)
                    //{
                    //    _vehicleController.MaxSpeed = 1;

                    //}
                }
            }
            else
            {
                _vehicleController.AccelerationInput = logtichControl.LogitchGasRotation;
            }




            //吋動踏板(煞車連動)
            //Debug.Log("logtichControl.LogitchGasRotation:" + logtichControl.LogitchCluthRotation);

            _vehicleController.ClutchInput = logtichControl.LogitchCluthRotation;
            if (logtichControl.LogitchCluthRotation > 5)
            {
                _vehicleController.BrakesInput = logtichControl.LogitchCluthRotation;

                //採吋動+踩油門
                if (logtichControl.LogitchGasRotation > 10)
                {
                    _forkliftController.forksVerticalSpeed = _forksVerticalSpeed * 2;
                    _forkliftController.forksHorizontalSpeed = _forksHorizontalSpeed * 2;
                    _forkliftController.mastTiltSpeed = _mastTiltSpeed * 2;
                }
                else
                {
                    _forkliftController.forksVerticalSpeed = _forksVerticalSpeed;
                    _forkliftController.forksHorizontalSpeed = _forksHorizontalSpeed;
                    _forkliftController.mastTiltSpeed = _mastTiltSpeed;
                }

            }
            else
            {
                _forkliftController.forksVerticalSpeed = _forksVerticalSpeed;
                _forkliftController.forksHorizontalSpeed = _forksHorizontalSpeed;
                _forkliftController.mastTiltSpeed = _mastTiltSpeed;

            }



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


            if (logtichControl.CheckSeatBelt && _CheckSeatBeltBool == false)
            {
                _CheckSeatBeltBool = true;

                _vehicleController.seatBelts[0].SetActive(!_vehicleController.seatBelts[0].active);
                _vehicleController.seatBelts[1].SetActive(!_vehicleController.seatBelts[1].active);

            }

            if (!logtichControl.CheckSeatBelt)
            {
                _CheckSeatBeltBool = false;

            }

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

            ////控制油門
            //_acceleration = Input.GetKey(inputSettings.acceleration) ? 25f : 0;
            //_acceleration = Input.GetKey(inputSettings.reverse) ? _acceleration - 1 : _acceleration; //倒退改打檔
            //_vehicleController.AccelerationInput = _acceleration;

            //方向
            _steering = 0f;
            _steering = Input.GetKey(inputSettings.turnRight) ? _steering + 450 : _steering;
            _steering = Input.GetKey(inputSettings.turnLeft) ? _steering - 450 : _steering;
            _vehicleController.SteeringInput = _steering;

            //煞車
            _vehicleController.BrakesInput = Input.GetKey(inputSettings.brakes) ? 25f : 0f;

            //手煞車
            if (Input.GetKey(inputSettings.handbrakeOn)) _vehicleController.HandBrakeInput = 1;
            if (Input.GetKey(inputSettings.handbrakeOff)) _vehicleController.HandBrakeInput = 0;

            //吋動踏板(煞車連動)
            _vehicleController.ClutchInput = Input.GetKey(inputSettings.clutch) ? 25f : 0f;
            if (Input.GetKey(inputSettings.clutch))
            {
                _vehicleController.BrakesInput = Input.GetKey(inputSettings.clutch) ? 25f : 0f;

                //採吋動+踩油門
                if (Input.GetKey(inputSettings.acceleration)) {
                    _forkliftController.forksVerticalSpeed = _forksVerticalSpeed * 2;
                    _forkliftController.forksHorizontalSpeed = _forksHorizontalSpeed * 2;
                    _forkliftController.mastTiltSpeed = _mastTiltSpeed * 2;
                }
                else{
                    _forkliftController.forksVerticalSpeed = _forksVerticalSpeed;
                    _forkliftController.forksHorizontalSpeed = _forksHorizontalSpeed;
                    _forkliftController.mastTiltSpeed = _mastTiltSpeed;

                }

            }



            //控制油門(前進檔自動滑動)
            if (_backFront == 1 || _backFront == -1)
            {
                //沒踩油門且沒踩煞車時由這來控致剎車
                if (!Input.GetKey(inputSettings.acceleration))
                {
                    if (!Input.GetKey(inputSettings.brakes))
                    {
                        _vehicleController.BrakesInput = 0;
                        _vehicleController.AccelerationInput = 0.3f;
                        //入檔時的速度
                        if (_vehicleController.CurrentSpeed > onGasOnlyInFrontBackSpeedLimit)
                        {
                            //Debug.Log("=========_vehicleController.CurrentSpeed" + _vehicleController.CurrentSpeed);
                            _vehicleController.AccelerationInput = 0f;
                        }

                    }
                }
                else
                {
                    //控制油門
                    _acceleration = Input.GetKey(inputSettings.acceleration) ? 25f : 0;
                    _acceleration = Input.GetKey(inputSettings.reverse) ? _acceleration - 1 : _acceleration; //倒退改打檔
                    _vehicleController.AccelerationInput = _acceleration;
                }
            }
          


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
