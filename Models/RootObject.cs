namespace Paises.Models
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    public class RootObject
    {
        public string Name { get; set; } // vai pra janela
        public List<string> TopLevelDomain { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public List<string> CallingCodes { get; set; }
        public string Capital { get; set; } // vai pra janela
        public List<object> AltSpellings { get; set; }
        public string Region { get; set; } // vai pra janela
        public string Subregion { get; set; } // vai pra janela
        public int Population { get; set; } // vai pra janela
        public List<object> Latlng { get; set; }
        public string Demonym { get; set; }
        public double Area { get; set; }
        public double Gini { get; set; } // vai pra janela
        public List<string> Timezones { get; set; }
        public List<object> Borders { get; set; }
        public string NativeName { get; set; }
        public string NumericCode { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<Language> Languages { get; set; }
        public Translations Translations { get; set; }
        public string Flag { get; set; } // vai pra janela, se nao houver, imagem X 
        public List<object> RegionalBlocs { get; set; }
        public string Cioc { get; set; }
    }
}
