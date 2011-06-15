#define FORWARD 17
#define BACKWARD 18
#define LEFT 20
#define RIGHT 24

#define RAISE 33
#define LOWER 34
#define CAMERA_ON 36
#define CAMERA_OFF 40

#define STATUS_CHECK 64
#define STATUS_OK 65
#define STATUS_ERROR 66

#define COMMAND_MASK 15
#define CAR_PREFIX 16
#define CAMERA_PREFIX 32
#define STATUS_PREFIX 64

#define FORWARD_PIN 2
#define BACKWARD_PIN 3
#define LEFT_PIN 4
#define RIGHT_PIN 5

#define CAMERA_POWER_PIN 6
#define STEPPER_START_PIN 7
#define STEPPER_END_PIN 10

#define IS_FULLY_LOWERED_PIN 11
#define IS_FULLY_EXTENDED_PIN 12
#define REAR_SENSOR_PIN 1
#define SENSOR_TOLERANCE 300

#define STEPPER_DELAY 10

boolean isMotorRunning;

void setup() 
{
  Serial.begin(9600);
  pinMode(FORWARD_PIN, OUTPUT);
  pinMode(BACKWARD_PIN, OUTPUT);
  pinMode(LEFT_PIN, OUTPUT);
  pinMode(RIGHT_PIN, OUTPUT);
  
  pinMode(IS_FULLY_LOWERED_PIN, INPUT);
  pinMode(IS_FULLY_EXTENDED_PIN, INPUT);
  pinMode(CAMERA_POWER_PIN, OUTPUT);
  pinMode(REAR_SENSOR_PIN, INPUT);
  
  //stepper motor pins
  for(int a = STEPPER_START_PIN; a <= STEPPER_END_PIN; a++)
  {
    pinMode(a, OUTPUT);
  }
  isMotorRunning = false;
}

void loop() 
{

  byte command = 0;

  if (Serial.available() == 1) {
    command = Serial.read();
    executeCommand(command);
    Serial.flush();
  } else {
    Serial.flush();
    defaultState();
  }
}

void defaultState()
{
  digitalWrite(FORWARD_PIN, LOW);
  digitalWrite(BACKWARD_PIN, LOW);
  digitalWrite(LEFT_PIN, LOW);
  digitalWrite(RIGHT_PIN, LOW);
}

void executeCommand(byte command)
{
  byte movement;
  
  movement = COMMAND_MASK & command;
  if (CAR_PREFIX & command) {
    if (FORWARD & movement) {
      moveForward();
    } else if (BACKWARD & movement) {
      moveBackward(); 
    }
    if (LEFT & movement) {
      moveLeft(); 
    } else if (RIGHT & movement) {
      moveRight();
    }
    delay(250);
  }
  
  if (CAMERA_PREFIX & command) {
    if(RAISE & movement) {
      if (!isMotorRunning) {
        for (int a = 0; a < 20; a++) {
          raiseCamera();
        }
      }
    } else if (LOWER & movement) {
      if (!isMotorRunning) {
        for (int a = 0; a < 20; a++) {
          lowerCamera();
        }
      }
    }
    
    if(CAMERA_ON & movement) {
      powerOnCamera();
    } else if (CAMERA_OFF & movement) {
      powerOffCamera();
    }
  }
  
  if (STATUS_PREFIX & command) {
     //unused
  }
  
}

void moveForward()
{
  digitalWrite(FORWARD_PIN, HIGH);
  digitalWrite(BACKWARD_PIN, LOW);
}

void moveBackward()
{
  if (!isRearBlocked()) {
    digitalWrite(FORWARD_PIN, LOW);
    digitalWrite(BACKWARD_PIN, HIGH);
  }
}

void moveLeft()
{
  digitalWrite(LEFT_PIN, HIGH);
  digitalWrite(RIGHT_PIN, LOW);
}

void moveRight()
{
  digitalWrite(LEFT_PIN, LOW);
  digitalWrite(RIGHT_PIN, HIGH);
}

boolean isRearBlocked()
{
   if (analogRead(REAR_SENSOR_PIN) > SENSOR_TOLERANCE) {
     return true;
   } else {
     return false;  
   }
}

void powerOnCamera()
{
  digitalWrite(CAMERA_POWER_PIN, LOW);
}

void powerOffCamera()
{
  digitalWrite(CAMERA_POWER_PIN, HIGH);
}

void raiseCamera()
{
  if (digitalRead(IS_FULLY_EXTENDED_PIN) == LOW 
      || isMotorRunning) {
     return;
  }

  isMotorRunning = true;  
  
  int a;
  for (a = STEPPER_START_PIN; a < STEPPER_END_PIN; a++) {
    digitalWrite(a, HIGH);
    delay(STEPPER_DELAY);
    digitalWrite(a + 1, HIGH);
    delay(STEPPER_DELAY);
    digitalWrite(a, LOW);
  }
  digitalWrite(STEPPER_START_PIN, HIGH);
  delay(STEPPER_DELAY);
  digitalWrite(STEPPER_START_PIN, LOW);
  digitalWrite(a, LOW);
  
  isMotorRunning = false;
}

void lowerCamera()
{
  if (digitalRead(IS_FULLY_LOWERED_PIN) == LOW
      || isMotorRunning) { 
     return;
  }
  
  isMotorRunning = true;
  
  int a;
  for (a = STEPPER_END_PIN; a > STEPPER_START_PIN; a--) { 
    digitalWrite(a, HIGH);
    delay(STEPPER_DELAY);
    digitalWrite(a-1, HIGH);
    delay(STEPPER_DELAY);  
    digitalWrite(a, LOW);
  }
  digitalWrite(STEPPER_END_PIN, HIGH);
  delay(STEPPER_DELAY);
  digitalWrite(STEPPER_END_PIN, LOW);
  digitalWrite(a, LOW);

  isMotorRunning = false;
}
