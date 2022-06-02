namespace electroTariffication
{
    public class tariff
    {
        tarifParameters _tarifParameters;
        string _name;

        // passing formula to tariff object from external component allows for flexibility of updating formula for a given tariff at a given point in time
        // but it is all a question of solution architecture

        // one interesting idea would be to have a web app for users to define calculation parameters for a tariff and then use expression trees to generate 
        // lambdas dynamically and update tarrifs 

        Func<Tuple<int, tarifParameters>, decimal> _calculate;

        public tariff(decimal baseRate, decimal additionalFee, decimal threshold, string name, Func<Tuple<int, tarifParameters>, decimal> calculationFormula)
        {
            _tarifParameters = new tarifParameters() { baseRate = baseRate, additionalFee = additionalFee, threshhold = threshold };
            _name = name;
            _calculate = calculationFormula;
        }

        public tariffCalculatedCost getCostData(int consulmtion) => new tariffCalculatedCost(_name, _calculate.Invoke(new Tuple<int, tarifParameters>(consulmtion, _tarifParameters)));

        public decimal baseRate => _tarifParameters.baseRate;
        public decimal additionalFee => _tarifParameters.additionalFee;
        public decimal threshhold => _tarifParameters.threshhold;
        public string name => _name;
    }
}
