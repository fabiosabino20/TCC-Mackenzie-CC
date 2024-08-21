#include "TP4/STP4.h"

STP4* port;

int flowPin = 2;    //Este é o pino de entrada no Arduino
float flowRate = 0;    //Este é o valor que pretende-se calcular
int flow = 1;
volatile int count; //Este número precisa ser setado como volátil para garantir que ele seja atualizado corretamente durante o processo de interrupção

void setup() {
  pinMode(flowPin, INPUT); //Seta o pino de entrada
  attachInterrupt(0, Flow, RISING);  //Configura o interruptor 0 (pino 2 no Arduino Uno) para rodar a função "Flow"
  port = new STP4(9600);
  Serial.begin(9600); //Inicia o Serial
  //interrupts();
}

void loop() {
  count = 0;      // Reseta o contador para iniciarmos a contagem em 0 novamente
  interrupts();   //Habilita o interrupção no Arduino
  delay (1000);   //Espera 1 segundo
  noInterrupts(); //Desabilita o interrupção no Arduino
   
  //Cálculos matemáticos
  flowRate = (count / 4.8);        //Conta os pulsos no último segundo e divide por 4.8
  flowRate = flowRate * 60;      //Converte segundos em minutos, tornando a unidade de medida mL/min
  flowRate = flowRate / 1000;    //Converte mL em litros, tornando a unidade de medida L/min

  port->BeginPack();
  port->PushToPack(&flowRate);
  port->SendPack();

  //Serial.println(flowRate);           //Imprime a variável flowRate no Serial
}
 
void Flow()
{
  count++; //Quando essa função é chamada, soma-se 1 a variável "count" 
  //port->BeginPack();
  //port->PushToPack(&flow);
  //port->PushToPack(&flowRate);
  //port->SendPack();
}