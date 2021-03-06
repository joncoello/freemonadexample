using System;

namespace conole001
{
    class Program
    {
        static void Main(string[] args)
        {

            // var io1 = new CheckCompaniesHouseCmd("anz").ToIO<CheckCompaniesHouseCmd, ICheckCompaniesHouseResult>() as IO<CheckCompaniesHouseCmd, ICheckCompaniesHouseResult, ICheckCompaniesHouseResult>;
            // io1.Next(new CheckCompaniesHousePassed("anz"));

            var expr1 =
                from checkCompaniesHouseResult in new CheckCompaniesHouseCmd("anz").ToIO<CheckCompaniesHouseCmd, ICheckCompaniesHouseResult>()
                let checkCompaniesHousePassed = checkCompaniesHouseResult as CheckCompaniesHousePassed
                from createCustomerResult in new CreateCustomerCmd(checkCompaniesHousePassed.Name).ToIO<CreateCustomerCmd, ICreateCustomerResult>()
                select createCustomerResult;

            var expr2 =
                new CheckCompaniesHouseCmd("anz").ToIO<CheckCompaniesHouseCmd, ICheckCompaniesHouseResult>()
                .SelectMany(chr =>
                new CreateCustomerCmd((chr as CheckCompaniesHousePassed).Name).ToIO<CreateCustomerCmd, ICreateCustomerResult>(),
                (chr, ccr) => ccr);

            var expr = expr1;

            var result = Interpreter.Interpret(expr);

            Console.WriteLine("Customer Created - " + ((result as Return<ICreateCustomerResult>).Value as CreateCustomerSuceeded).Name);
            Console.ReadKey();
        }
    }

    class CheckCompaniesHouseCmd
    {
        public string Name { get; set; }
        public CheckCompaniesHouseCmd(string name) => Name = name;
    }
    interface ICheckCompaniesHouseResult { }
    class CheckCompaniesHousePassed : ICheckCompaniesHouseResult
    {
        public string Name { get; set; }
        public CheckCompaniesHousePassed(string name) => Name = name;
    }
    class CreateCustomerCmd
    {
        public string Name { get; set; }
        public CreateCustomerCmd(string name) => Name = name;
    }
    interface ICreateCustomerResult { }
    class CreateCustomerSuceeded : ICreateCustomerResult
    {
        public string Name { get; set; }
        public CreateCustomerSuceeded(string name) => Name = name;
    }

}
