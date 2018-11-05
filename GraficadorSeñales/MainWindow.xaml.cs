using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GraficadorSeñales
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        double amplitudMaxima = 1;
        Señal señal;

        Señal señalResultado;

        public MainWindow()
        {
            InitializeComponent();

            
            
        }

        private void btnGraficar_Click(object sender, RoutedEventArgs e)
        {
            


            double tiempoInicial =
                double.Parse(txtTiempoInicial.Text);
            double tiempoFinal =
                double.Parse(txtTiempoFinal.Text);
            double frecuenciaMuestreo =
                double.Parse(txtFrecuenciaMuestreo.Text);

            

            switch (cbTipoSeñal.SelectedIndex)
            {
                //Senoidal
                case 0:
                    double amplitud =
                        double.Parse(
                        ((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtAmplitud.Text);
                    double fase =
                        double.Parse(
                        ((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtFase.Text);
                    double frecuencia =
                        double.Parse(
                        ((ConfiguracionSeñalSenoidal)panelConfiguracion.Children[0]).txtFrecuencia.Text);
                    señal =
                        new SeñalSenoidal(amplitud, fase, frecuencia);
                    break;
                //Rampa
                case 1:
                    señal = new SeñalRampa();
                        break;
                case 2:
                    double alpha =
                        double.Parse(
                        ((ConfiguracionSeñalExponencial)panelConfiguracion.Children[0]).txtAlpha.Text);
                    señal =
                        new SeñalExponencial(alpha);
                    break;
                case 3:
                    señal = new SeñalRectangular();
                    break;
                default:
                    señal = null;
                    break;
            }

            

            señal.TiempoInicial = tiempoInicial;
            señal.TiempoFinal = tiempoFinal;
            señal.FrecuenciaMuestreo = frecuenciaMuestreo;

    

            señal.construirSeñalDigital();
 

            if ((bool)cbEscalar.IsChecked)
                señal.escalar(float.Parse(txtFactorEscala.Text));
            if ((bool)cbDesplazar.IsChecked)
                señal.desplazar(float.Parse(txtMagnitudDesplazamiento.Text));
            if ((bool)cbTruncar.IsChecked)
                señal.truncar(float.Parse(txtUmbral.Text));

            

            señal.actualizarAmplitudMaxima();

            amplitudMaxima = señal.AmplitudMaxima;
            

            plnGrafica.Points.Clear();
            

            lblAmplitudMaximaY.Text =
               amplitudMaxima.ToString("F");
            lblAmplitudMaximaNegativaY.Text =
                "-" + amplitudMaxima.ToString("F");

            if ( señal != null)
            {
                //Recorrer una coleccion o arreglo
                foreach (Muestra muestra in señal.Muestras)
                {
                    plnGrafica.Points.Add(
                        new Point((muestra.X - tiempoInicial) * scrContenedor.Width
                        , (muestra.Y / amplitudMaxima * ((scrContenedor.Height / 2.0) - 30) * -1)
                        + (scrContenedor.Height / 2))
                        );
                }
 
            }
           

            plnEjeX.Points.Clear();
            //Punto del principio
            plnEjeX.Points.Add(
                new Point(0,                          //Coordenada X punto inicial
                (scrContenedor.Height / 2)));         //Coordenada Y punto inicial
            //Punto del fin
            plnEjeX.Points.Add(
                new Point((tiempoFinal - tiempoInicial) * scrContenedor.Width,
                     // x final
                 (scrContenedor.Height / 2)));                         // y final

            plnEjeY.Points.Clear();
            //Punto del principio
            plnEjeY.Points.Add(
                new Point((0 - tiempoInicial) * scrContenedor.Width,  //Coordenada X punto inicial

                (                //Coordenada Y punto inicial
                ((scrContenedor.Height / 2.0) - 30) * -1)
                    + (scrContenedor.Height / 2)));         
            //Punto del fin
            plnEjeY.Points.Add(
                new Point((0 - tiempoInicial) * scrContenedor.Width,  //x final
                (-1 * ((scrContenedor.Height / 2.0) - 30) * -1) //y final
                    + (scrContenedor.Height / 2)));

            
        }

      

        private void cbTipoSeñal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            panelConfiguracion.Children.Clear();
            switch (cbTipoSeñal.SelectedIndex)
            {
                case 0: // Senoidal
                    panelConfiguracion.Children.Add(
                        new ConfiguracionSeñalSenoidal()
                        );
                    break;
                case 1:
                    break;
                case 2:
                    panelConfiguracion.Children.Add(
                        new ConfiguracionSeñalExponencial()
                        );
                    break;
                case 3:
                    break;
                default:
                    break;
            }
        }

        private void cbEscalar_Click(object sender, RoutedEventArgs e)
        {
            txtFactorEscala.IsEnabled = (bool)cbEscalar.IsChecked;
        }

        private void cbDesplazar_Click(object sender, RoutedEventArgs e)
        {
            txtMagnitudDesplazamiento.IsEnabled = (bool)cbDesplazar.IsChecked;
        }

        private void cbTruncar_Click(object sender, RoutedEventArgs e)
        {
            txtUmbral.IsEnabled = (bool)cbTruncar.IsChecked;
        }


    }
}
