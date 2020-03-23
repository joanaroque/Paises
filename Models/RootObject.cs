namespace Countries.Models
{
    using System.Collections.Generic;
    public class RootObject
    {
        public string Name { get; set; } 
        public List<string> TopLevelDomain { get; set; }
        public string Alpha2Code { get; set; }
        public string Alpha3Code { get; set; }
        public List<string> CallingCodes { get; set; }
        public string Capital { get; set; } 
        public List<object> AltSpellings { get; set; }
        public string Region { get; set; } 
        public string Subregion { get; set; } 
        public int Population { get; set; } 
        public List<object> Latlng { get; set; }
        public string Demonym { get; set; }
        public string Area { get; set; }
        public double Gini { get; set; } 
        // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing
        public List<string> Timezones { get; set; }
        public List<object> Borders { get; set; }
        public string NativeName { get; set; }
        public string NumericCode { get; set; }
        public List<Currency> Currencies { get; set; }
        public List<Language> Languages { get; set; }
        public Translations Translations { get; set; }
        public string Flag { get; set; }
        public List<object> RegionalBlocs { get; set; }
        public string Cioc { get; set; }
    }
}
