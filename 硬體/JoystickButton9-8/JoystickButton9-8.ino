// Simple example application that shows how to read four Arduino
// digital pins and map them to the USB Joystick library.
//
// Ground digital pins 9, 10, 11, and 12 to press the joystick 
// buttons 0, 1, 2, and 3.
//
// NOTE: This sketch file is for use with Arduino Leonardo and
//       Arduino Micro only.
//
// by Matthew Heironimus
// 2015-11-20
//--------------------------------------------------------------------

#include <Joystick.h>

Joystick_ Joystick;

void setup() {
  // Initialize Button Pins
  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  pinMode(4, INPUT_PULLUP);
  pinMode(5, INPUT_PULLUP);
  pinMode(6, INPUT_PULLUP);
  pinMode(7, INPUT_PULLUP);
  pinMode(8, INPUT_PULLUP);
  pinMode(9, INPUT_PULLUP);
  pinMode(10, INPUT_PULLUP);
  pinMode(11, INPUT_PULLUP);
  pinMode(12, INPUT_PULLUP);
  pinMode(A0, INPUT);
  pinMode(A1, INPUT);
  // Initialize Joystick Library
  Joystick.begin();
}

// Constant that maps the phyical pin to the joystick button.
const int pinToButtonMap = 2;

// Last state of the button
int lastButtonState[11] = {0,0,0,0,0,0,0,0,0,0,0};

void loop() {
  int pot1 = analogRead(A0);
  int pot2 = analogRead(A1);
 
  //int mapped1 = map(pot,0,1023,-217,127);
 // {Joystick.setXAxis(pot1);}
  {Joystick.setRyAxis(pot1);}
  {Joystick.setRzAxis(pot2);}
  //Serial.println(mapped); 
 
  // Read pin values
  for (int index = 0; index < 11; index++)
  {
    int currentButtonState = !digitalRead(index + pinToButtonMap );
    if (currentButtonState != lastButtonState[index])
    {
      Joystick.setButton(index, currentButtonState);
      lastButtonState[index] = currentButtonState;
    }
  }
 
  //delay(50);
}
