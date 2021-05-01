﻿using UnityEngine;
using UnityEngine.Events;

namespace WSMGameStudio.HeavyMachinery
{
    [RequireComponent(typeof(ForkliftController))]
    public class ForkliftPlayerInput : MonoBehaviour
    {
        public bool enablePlayerInput = true;
        public ForkliftInputSettings inputSettings;
        public UnityEvent[] customEvents;

        private ForkliftController _forkliftController;
        private Vehicles.WSMVehicleController _WSMVehicleController;


        private int _mastTilt = 0;
        private int _forksVertical = 0;
        //private int _forksHorizontal = 0;
        private int _backFront = 0;
        private int _changeLight = 0;



        /// <summary>
        /// Initializing references
        /// </summary>
        void Start()
        {
            _forkliftController = GetComponent<ForkliftController>();

            _WSMVehicleController = GetComponent<Vehicles.WSMVehicleController>();
        }

        /// <summary>
        /// Handling player input
        /// </summary>
        void Update()
        {
            if (enablePlayerInput)
            {
                if (inputSettings == null) return;

                #region Forklift Controls

                if (Input.GetKeyDown(inputSettings.toggleEngine))
                    _forkliftController.IsEngineOn = !_forkliftController.IsEngineOn;

                _mastTilt = Input.GetKey(inputSettings.mastTiltForwards) ? 1 : (Input.GetKey(inputSettings.mastTiltBackwards) ? -1 : 0);
                _forksVertical = Input.GetKey(inputSettings.forksUp) ? 1 : (Input.GetKey(inputSettings.forksDown) ? -1 : 0);
                //_forksHorizontal = Input.GetKey(inputSettings.forksRight) ? 1 : (Input.GetKey(inputSettings.forksLeft) ? -1 : 0);
                if (Input.GetKey(inputSettings.backMove)) _backFront = -1;
                if (Input.GetKey(inputSettings.nullMove)) _backFront = 0;
                if (Input.GetKey(inputSettings.frontMove)) _backFront = 1;

                if (Input.GetKey(inputSettings.rightLight) && _changeLight!= 1) _changeLight = 1;
                if (Input.GetKey(inputSettings.nullLight) && _changeLight != 0) _changeLight = 0;
                if (Input.GetKey(inputSettings.leftLight) && _changeLight != -1) _changeLight = -1;

                //_backFront = Input.GetKey(inputSettings.backMove) ? 1 : (Input.GetKey(inputSettings.frontMove) ? -1 : (Input.GetKey(inputSettings.nullMove) ?0:0));
                //_changePower = Input.GetKey(inputSettings.rightLight) ? 1 : (Input.GetKey(inputSettings.leftLight) ?2 : 1);


                _forkliftController.RotateMast(_mastTilt);//貨插旋轉
                _forkliftController.MoveForksVertically(_forksVertical);//貨插升降
                //更換燈
                _WSMVehicleController.CurrenLightControl(_changeLight);

                //_forkliftController.MoveForksHorizontally(_forksHorizontal);

                //操作桿做動
                _forkliftController.UpdateLevers(_forksVertical, _mastTilt, _backFront, _changeLight);//(_forksVertical, _forksHorizontal, _mastTilt)

                
            
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
        }
    }
}
