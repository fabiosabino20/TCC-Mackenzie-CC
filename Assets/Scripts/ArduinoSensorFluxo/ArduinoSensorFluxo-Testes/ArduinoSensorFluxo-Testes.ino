byte statusLed = 13; // LED para controle
byte sensorInterrupt = 0; // Este é o pino de entrada do sensor no Arduino (0 = digital pin 2)
byte sensorPin = 2;

float flowRate = 0; // Este é o valor que pretende-se calcular
float calibrationFactor = 4.8; // O sensor tem aproximadamente 4.8 pulsos a cada L/min de fluxo

// Este número precisa ser setado como volátil para garantir que ele seja atualizado corretamente durante o processo de interrupção
// Conta o numero de pulsos durante o período de interrupção
volatile byte pulseCount;

unsigned long oldTime;

void setup() {
  Serial.begin(9600); //Inicia o Serial

  pinMode(statusLed, OUTPUT);
  digitalWrite(statusLed, HIGH);

  pinMode(sensorPin, INPUT); //Seta o pino de entrada
  digitalWrite(sensorPin, HIGH);

  pulseCount = 0;
  flowRate = 0.0;
  oldTime = 0;

  //Configura o interruptor 0 (pino digital 2 no Arduino Uno) para rodar a função "pulseCounter"
  attachInterrupt(sensorInterrupt, pulseCounter, RISING);
}

void loop() {
  if((millis() - oldTime) > 1000)
  {
    // Desabilitando a interrupção enquanto calculamos o fluxo
    detachInterrupt(sensorInterrupt);

    flowRate = ((1000.0 / (millis() - oldTime)) * pulseCount) / calibrationFactor;

    oldTime = millis();

    unsigned int frac;

    // Valor do fluxo em L/min
    Serial.print("Flow rate: ");
    Serial.print(int(flowRate));
    Serial.print("L/min");
    Serial.print("\n");

    // Reiniciar o contador para começar a proxima contagem 
    pulseCount = 0;
    
    // Ativa as interrupções novamente para a próxima contagem
    attachInterrupt(sensorInterrupt, pulseCounter, FALLING);
  }
}

// Conta os pulsos do sensor de fluxo
void pulseCounter()
{
  pulseCount++;
}