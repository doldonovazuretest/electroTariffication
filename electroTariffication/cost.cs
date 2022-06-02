namespace electroTariffication
{
    public record cost
    {
        public cost(string _tariff, decimal _cost)
        {
            tariff = _tariff;
            annualCost = _cost;   
        }
        public string? tariff { get; set; } 
        public decimal annualCost { get; set; }
    }
}
