using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAppTempoAgora.Models
{
    public class Tempo
    {
        public string? Title { get; set; }//Titulo
        public string? Temperature { get; set; }//Temperatura
        public string? Wind { get; set; }//Ventos
        public string? Humidity { get; set; }//Umidade
        public string? Visibility { get; set; }//Visibilidade
        public string? Sunrise { get; set; }//Nascer do Sol
        public string? Sunset { get; set; }//Por do Sol
        public string? Weather {  get; set; } //Clima
        public string? WeatherDescription {  get; set; }//Descrição do Clima

        public string? Latitude { get; set; }//Latitude
        public string? Longitude { get; set; }//Longitude

    }
}
