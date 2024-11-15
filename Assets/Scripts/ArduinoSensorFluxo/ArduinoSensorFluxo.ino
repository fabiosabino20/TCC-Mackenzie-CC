byte statusLed = 13; // LED para controle
byte sensorInterrupt = 0; // Este é o pino de entrada do sensor no Arduino (0 = digital pin 2)
byte sensorPin = 2;

// Variáveis do sensor
float calibrationFactor = 4.8;
volatile int pulseCount = 0;

// Variáveis para as métricas de fluxo e volume
float flowRate = 0;
float flowArray[200] = {0};
int index = 0;
float medianFlow = 0;
float FVC = 0;
float FEV1 = 0;
float PEF = 0;
float FEF2575 = 0;
float relacaoFEV1FVC = 0;
float flowTime = 0;
float oldTime = 0;
bool canPrint = false;

void setup() {
  Serial.begin(9600); //Inicia a comunicação Serial

  pinMode(statusLed, OUTPUT);
  digitalWrite(statusLed, HIGH);

  pinMode(sensorPin, INPUT);
  digitalWrite(sensorPin, HIGH);

  attachInterrupt(sensorInterrupt, pulseCounter, RISING);
  interrupts();
}

void loop() {
  if(pulseCount > 0) {
    float start = millis();
    delay(100);
    noInterrupts();
    
    oldTime = millis();
    flowTime += (millis() - start) / 1000;
    flowRate = ((pulseCount / calibrationFactor) / 60) * 10;
    flowArray[index] = flowRate;
    index++;

    sendMetrics();

    pulseCount = 0;
    canPrint = true;
    interrupts();
  }
  else if (millis() - oldTime >= 2000 && canPrint) {
    noInterrupts();
    
    calcFEV1();
    calcPEF();
    calcFVC();
    medianFlow = FVC / flowTime;
    relacaoFEV1FVC = (FEV1 / FVC) * 100;
    calcFEF2575();

    sendMetrics();
    resetMetrics();

    interrupts();
  }
}

void pulseCounter() {
  pulseCount++;
}

void calcFEV1() {
  FEV1 = 0;
  for (int i=0; i<10; i++){
    FEV1 += flowArray[i];
  }
  FEV1 /= 10;
}

void calcPEF() {
  for (int i=0; i<index; i++) {
    if (flowArray[i] > PEF) {
      PEF = flowArray[i];
    }
  }
}

void calcFVC() {
  int count = 0;
  float auxFVC = 0;

  for (int i=0; i<index; i++){
    count++;
    auxFVC += flowArray[i];

    if (count == 10) {
      count = 0;
      auxFVC /= 10;
      FVC += auxFVC;
    }
  }

  if (count != 0) {
    auxFVC /= count;
    FVC += (auxFVC * (count / 10));
  }
}

void calcFEF2575() {
  float FVC25 = FVC * 0.25;
  float FVC75 = FVC * 0.75;
  float aux = 0;
  float volumeAcumulado = 0;
  int count = 0;

  for (int i=0; i<index; i++) {
    aux = flowArray[i];
    aux *= 0.1;
    volumeAcumulado += aux;

    if (volumeAcumulado >= FVC25) {
      FEF2575 += flowArray[i];
      count++;
    }
    if (volumeAcumulado > FVC75) {
      FEF2575 /= count;
      break;
    }
  }
}

void resetMetrics() {
  for (int i=0; i<index; i++) {
    flowArray[i] = 0;
  }
  flowRate = 0;
  index = 0;
  medianFlow = 0;
  FVC = 0;
  FEV1 = 0;
  PEF = 0;
  FEF2575 = 0;
  relacaoFEV1FVC = 0;
  flowTime = 0;
  oldTime = 0;
  canPrint = false;
}

void sendMetrics() {
  // Envia os dados por Serial para Unity
  Serial.println(flowRate);
  Serial.println(FVC);
  Serial.println(FEV1);
  Serial.println(PEF);
  Serial.println(relacaoFEV1FVC);
  Serial.println(FEF2575);
}
