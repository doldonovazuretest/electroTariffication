namespace electroTariffication
{
    public record tariffCalculatedCost
    {
        public tariffCalculatedCost(string _tariff, decimal _cost)
        {
            tariff = _tariff;
            cost = _cost;   
        }
        public string? tariff { get; set; } 
        public decimal cost { get; set; }
    }
}
