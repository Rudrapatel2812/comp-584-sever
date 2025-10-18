namespace comp_584_sever.DTOs
{
    public class CountryPopulation
    {
        public required int Id { get; set; }

        public required string Name { get; set; } 

        public required string Iso2 { get; set; }

        public required string Iso3 { get; set; }

        public decimal Population { get; set; }
        
    }
}
