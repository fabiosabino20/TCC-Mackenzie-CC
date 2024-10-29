byte statusLed = 13; // LED para controle
byte sensorInterrupt = 0; // Este é o pino de entrada do sensor no Arduino (0 = digital pin 2)
byte sensorPin = 2;

float flowRate = 0;    // Este é o valor do fluxo (que será calculado)
float calibrationFactor = 8.6; // O sensor tem aproximadamente 4.8 pulsos a cada L/min de fluxo - o valor será calibrado após os testes
volatile int pulseCount; //Este número precisa ser setado como volátil para garantir que ele seja atualizado corretamente durante o processo de interrupção

void setup() {
  Serial.begin(9600); //Inicia o Serial

  pinMode(statusLed, OUTPUT);
  digitalWrite(statusLed, HIGH);

  pinMode(sensorPin, INPUT); //Seta o pino de entrada
  digitalWrite(sensorPin, HIGH);

  //Configura o interruptor 0 (pino digital 2 no Arduino Uno) para rodar a função "pulseCounter"
  attachInterrupt(sensorInterrupt, pulseCounter, FALLING);
}

void loop() {
  pulseCount = 0; // Reseta o contador para iniciarmos a contagem em 0 novamente
  interrupts();   // Habilita o interrupção no Arduino
  delay (1000);   // Espera 1 segundo
  noInterrupts(); // Desabilita o interrupção no Arduino
   
  // Cálculo do fluxo
  flowRate = (pulseCount / calibrationFactor);

  // Valor do fluxo em L/min
  Serial.print("Flow rate: ");
  Serial.print(int(flowRate));
  Serial.print("L/min");
  Serial.print("\n");
}
 
void pulseCounter()
{
  pulseCount++; //Quando essa função é chamada, soma-se 1 a variável "count" 
}