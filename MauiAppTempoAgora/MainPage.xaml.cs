using MauiAppTempoAgora.Models;
using MauiAppTempoAgora.Service;
using System.Diagnostics;

namespace MauiAppTempoAgora
{
    public partial class MainPage : ContentPage
    {
        CancellationTokenSource _cancelTokenSource;
        bool _isCheckingLocation;//Verifica se está verificando a localização

        string? cidade;

        public MainPage()
        {
            InitializeComponent();
        }

        // https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key
        // https://stackoverflow.com/questions/75174113/maui-windows-platform-cant-access-location


        private async void Button_Clicked(object sender, EventArgs e)//Botão de clique
        {
            try
            {
                _cancelTokenSource = new CancellationTokenSource();//Cancela a fonte do token

                GeolocationRequest request = 
                    new GeolocationRequest(GeolocationAccuracy.Medium, //Precisão da Geolocalização
                    TimeSpan.FromSeconds(10));

                Location? location = await Geolocation.Default.GetLocationAsync(//Obtém a localização
                    request, _cancelTokenSource.Token);

                //Se a localização não for nula
                if (location != null)
                {
                    lbl_latitude.Text = location.Latitude.ToString();//Exibe a latitude
                    lbl_longitude.Text = location.Longitude.ToString();//Exibe a longitude


                    Debug.WriteLine("---------------------------------------");//Exibe a localização
                    Debug.WriteLine(location);                    
                    Debug.WriteLine("---------------------------------------");
                }

            }
            catch (FeatureNotSupportedException fnsEx)//Se não for suportado
            {               
                await DisplayAlert("Erro: Dispositivo não Suporta", //Exibe um alerta
                    fnsEx.Message, "OK");
            }
            catch (FeatureNotEnabledException fneEx)//Se não estiver habilitado
            {                
                await DisplayAlert("Erro: Localização Desabilitada", //Exibe um alerta
                    fneEx.Message, "OK");
            }
            catch (PermissionException pEx)//Se não tiver permissão
            {                
                await DisplayAlert("Erro: Permissão", pEx.Message, "OK");
            }
            catch (Exception ex)//Se houver uma exceção
            {                
                await DisplayAlert("Erro: ", ex.Message, "OK");
            }
        } // Fecha método

        private async Task<string> GetGeocodeReverseData(
            double latitude = 47.673988, double longitude = -122.121513)
        {
            IEnumerable<Placemark> placemarks = 
                await Geocoding.Default.GetPlacemarksAsync(
                    latitude, longitude);

            Placemark? placemark = placemarks?.FirstOrDefault();

            Debug.WriteLine("-------------------------------------------");
            Debug.WriteLine(placemark?.Locality);
            Debug.WriteLine("-------------------------------------------");

            if (placemark != null)
            {
                cidade = placemark.Locality;

                return
                    $"AdminArea:       {placemark.AdminArea}\n" +//Área Administrativa
                    $"CountryCode:     {placemark.CountryCode}\n" +//Código do País
                    $"CountryName:     {placemark.CountryName}\n" +//Nome do País
                    $"FeatureName:     {placemark.FeatureName}\n" +//Nome do Recurso
                    $"Locality:        {placemark.Locality}\n" +//Localidade
                    $"PostalCode:      {placemark.PostalCode}\n" +//Código Postal
                    $"SubAdminArea:    {placemark.SubAdminArea}\n" +//Subárea Administrativa
                    $"SubLocality:     {placemark.SubLocality}\n" +//Sublocalidade
                    $"SubThoroughfare: {placemark.SubThoroughfare}\n" +//Sublogradouro
                    $"Thoroughfare:    {placemark.Thoroughfare}\n";//Logradouro
            }

            return "Nada";
        }

        private async void Button_Clicked_1(object sender, EventArgs e)//Botão de clique
        {
            double latitude = Convert.ToDouble(lbl_latitude.Text);//Converte a latitude
            double longitude = Convert.ToDouble(lbl_longitude.Text);//Converte a longitude

            lbl_reverso.Text = await GetGeocodeReverseData(latitude, longitude);//Exibe a localização
        }

        private async void Button_Clicked_2(object sender, EventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cidade))//Se a cidade não for nula
                {
                    Tempo? previsao = await DataService.GetPrevisaoDoTempo(cidade);//Obtém a previsão do tempo

                    string dados_previsao = "";

                    if (previsao != null)
                    {
                        dados_previsao = $"Humidade: {previsao.Humidity} \n" +
                                         $"Nascer do Sol: {previsao.Sunrise} \n " +
                                         $"Pôr do Sol: {previsao.Sunset} \n" +
                                         $"Temperatura: {previsao.Temperature} \n" +
                                         $"Titulo: {previsao.Title} \n" +
                                         $"Visibilidade: {previsao.Visibility} \n" +
                                         $"Vento: {previsao.Wind} \n" +
                                         $"Previsão: {previsao.Weather} \n" +
                                         $"Descrição: {previsao.WeatherDescription} \n" +
                                         $"Latitude: {previsao.Latitude} \n" +
                                         $"Longitude: {previsao.Longitude}";



                        string url_mapa = $"https://embed.windy.com/embed.html" +
                                           $"?type=map&location=coordinates&metricRain=mm" +
                                           $"&metricTemp=°C&metricWind=km/h&zoom=5&overlay=wind" +
                                           $"&product=ecmwf&level=surface" +
                                           $"&lat={previsao.Latitude}&lon={previsao.Longitude}";


                        Debug.WriteLine(url_mapa);
                        mapa.Source = url_mapa;                                        
                    }
                    else
                    {
                        dados_previsao = $"Sem dados, previsão nula.";
                    }

                    Debug.WriteLine("-------------------------------------------");
                    Debug.WriteLine(dados_previsao);
                    Debug.WriteLine("-------------------------------------------");

                    lbl_previsao.Text = dados_previsao;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro ", ex.Message, "OK");
            }
        }
    }

}
