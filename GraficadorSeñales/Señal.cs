﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraficadorSeñales
{
    abstract class Señal
    {
        public List<Muestra> Muestras { get; set; }
        public double AmplitudMaxima { get; set; }
        public double TiempoInicial { get; set; }
        public double TiempoFinal { get; set; }
        public double FrecuenciaMuestreo { get; set; }

        public abstract double evaluar(double tiempo);

        public void construirSeñalDigital()
        {
            double periodoMuestreo = 1 / FrecuenciaMuestreo;
            for (double i = TiempoInicial; 
                i <= TiempoFinal; i += periodoMuestreo)
            {
                double valorMuestra =
                    evaluar(i);

                if (Math.Abs(valorMuestra) >
                    AmplitudMaxima)
                {
                    AmplitudMaxima =
                        Math.Abs(valorMuestra);
                }

                Muestras.Add(
                    new Muestra(i, valorMuestra));

            }
        }

        public void actualizarAmplitudMaxima()
        {
            AmplitudMaxima = 1;
            foreach (Muestra muestra in Muestras)
            {
                if (Math.Abs(muestra.Y) > AmplitudMaxima)
                {
                   AmplitudMaxima = Math.Abs(muestra.Y);
                }
                
            }
        }

        public void desplazar(float magnitudDesplazamiento)
        {
            foreach (Muestra muestra in Muestras)
            {
                muestra.Y += magnitudDesplazamiento;
            }
        }

        public void escalar(float factorEscala)
        {
            foreach (Muestra muestra in Muestras)
            {
                muestra.Y *= factorEscala;
            }
        }

        public void truncar(float umbral)
        {
            foreach(Muestra muestra in Muestras)
            {
                if (muestra.Y > umbral)
                {
                    muestra.Y = umbral;
                } else if (muestra.Y < -umbral)
                {
                    muestra.Y = -umbral;
                }
            }
        }

        public static Señal sumar(Señal sumando1, Señal sumando2)
        {
            SeñalPersonalizada resultado =
                new SeñalPersonalizada();

            resultado.TiempoInicial =
                sumando1.TiempoInicial;
            resultado.TiempoFinal =
                sumando1.TiempoFinal;
            resultado.FrecuenciaMuestreo =
                sumando1.FrecuenciaMuestreo;

            int indice = 0;
            foreach(Muestra muestra in sumando1.Muestras)
            {
                Muestra muestraResultado = new Muestra();
                muestraResultado.X =
                    muestra.X;
                muestraResultado.Y =
                    muestra.Y + sumando2.Muestras[indice].Y;
                indice++;
                resultado.Muestras.Add(muestraResultado);
            }


            return resultado;
        }

        public static Señal convolucionar
            (Señal operando1, Señal operando2)
        {
            SeñalPersonalizada resultado =
                new SeñalPersonalizada();

            resultado.TiempoInicial =
                operando1.TiempoInicial +
                operando2.TiempoInicial;

            resultado.TiempoFinal =
                operando1.TiempoFinal +
                operando2.TiempoFinal;

            resultado.FrecuenciaMuestreo =
                operando1.FrecuenciaMuestreo;

            double periodoMuestreo =
                1 / resultado.FrecuenciaMuestreo;
            double duracionSeñal =
                resultado.TiempoFinal - resultado.TiempoInicial;
            double cantindadMuestrasResultado =
                duracionSeñal * resultado.FrecuenciaMuestreo;

            double instanteActual =
                resultado.TiempoInicial;
            for (int n = 0;
                n < cantindadMuestrasResultado; 
                n++)
            {
                double valorMuestra = 0;
                for (int k = 0; 
                    k < operando2.Muestras.Count;
                    k++)
                {
                    if ((n-k) >= 0 &&
                        (n-k) < operando2.Muestras.Count)
                    {
                        valorMuestra +=
                            operando1.Muestras[k].Y
                                * operando2.Muestras[n - k].Y;
                    }
                    
                }
                valorMuestra /= resultado.FrecuenciaMuestreo;
                
                Muestra muestra =
                    new Muestra(instanteActual, valorMuestra);
                resultado.Muestras.Add(muestra);
                instanteActual +=
                    periodoMuestreo;
            }





            return resultado;
        }
    
    }
}
